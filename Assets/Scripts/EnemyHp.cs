using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    [SerializeField]
    private float maxHP;
    private float currentHP;
    private bool isDie = false;
    private Enemy enemy;
    private SpriteRenderer spriteRenderer;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP;
        enemy = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void TakeDamage(float damage)
    {
        //Tip. ���� ä���� ����ŭ �����ؼ� ���� ��Ȳ�� �� ���� Ÿ���� ������ ���ÿ� ������
        //enemy.OnDie()�Լ��� ���� �� ����� �� ����
        //���� ������ ���°� ��� �����̸� �Ʒ� �ڵ带 �������� ����
        if (isDie) return;
        
        //���� ä���� damage��ŭ ����
        currentHP -= damage;

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        //�ǰ� 0 ���� = �� ���
        if(currentHP <= 0)
        {
            isDie = true;
            //�� ���
            enemy.OnDie(EnemyDestroyType.kill);
        }
    }
    private IEnumerator HitAlphaAnimation()
    {
        //���� ���� ������ color ������ ����
        Color color = spriteRenderer.color;
        //���� ������ 40%�� ����
        color.a = 0.4f;
        spriteRenderer.color = color;
        //0.05�ʵ��� ���
        yield return new WaitForSeconds(0.05f);

        //���� ������ 100%�� ����
        color.a = 1.0f;
        spriteRenderer.color = color;
    }
}