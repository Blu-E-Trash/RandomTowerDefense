using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour
{
    [SerializeField]
    private Image imageScreen;
    [SerializeField]
    private float maxHP = 20;
    private float currentHp;
    
    

    public float MaxHP => maxHP;
    public float CurrentHp => currentHp;

    private void Awake()
    {
        currentHp = maxHP;
        
    }

    public void TakeDamage(float damage)
    {
        
        currentHp -= damage;

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        if (currentHp <= 0) { 
        }
    }

    private IEnumerator HitAlphaAnimation()
    {
        //��üȭ�� ũ��� ��ġ�� imageScreen�� ������ color ������ ����
        //imageScreen�� ������ 40%�� ����
        Color color = imageScreen.color;
        color.a = 0.04f;
        imageScreen.color = color;

        //������ 0%�� �ɶ����� ����
        while(color.a >= 0.0f)
        {
            color.a -= 0.1f*Time.deltaTime;
            imageScreen.color = color;

            yield return null;
        }
    }
}