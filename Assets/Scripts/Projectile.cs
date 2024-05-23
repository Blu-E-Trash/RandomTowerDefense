using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Movement2D movement2D;
    private Transform target;
    //private int damage;
    private float damage;

    public void Setup(Transform target,float damage) { 
        movement2D = GetComponent<Movement2D>();
        this.target = target; //Ÿ���� �������� Ÿ��
        this.damage = damage;  // Ÿ���� �������� ��
    }
    public void Update()
    {
        if (target != null) { //target�� �����ϸ�
            //�߻�ü�� target�� ��ġ�� �̵�
            Vector3 direction = (target.position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else//���� ������ target�� �������
        {
            //�߻� ������Ʈ ����
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return; //���� �ƴ� ���� �ε�����
        if (collision.transform != target) return;//���� target�� ���� �ƴ� ��

        collision.GetComponent<EnemyHp>().TakeDamage(damage);
        Destroy(gameObject);
    }
}
