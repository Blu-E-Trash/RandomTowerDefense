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
        //Tip. 적의 채력이 딜만큼 감소해서 죽을 상황일 때 여러 타워의 공격을 동시에 받으면
        //enemy.OnDie()함수가 여러 번 실행될 수 있음
        //현재 저그이 사태가 사망 상태이면 아래 코드를 실행하지 않음
        if (isDie) return;
        
        //현재 채력을 damage만큼 감소
        currentHP -= damage;

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        //피가 0 이하 = 적 사망
        if(currentHP <= 0)
        {
            isDie = true;
            //적 사망
            enemy.OnDie(EnemyDestroyType.kill);
        }
    }
    private IEnumerator HitAlphaAnimation()
    {
        //현재 적의 색상을 color 변수에 저장
        Color color = spriteRenderer.color;
        //적의 투명도를 40%로 설정
        color.a = 0.4f;
        spriteRenderer.color = color;
        //0.05초동안 대기
        yield return new WaitForSeconds(0.05f);

        //적의 투명도를 100%로 설정
        color.a = 1.0f;
        spriteRenderer.color = color;
    }
}