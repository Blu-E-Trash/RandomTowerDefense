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
    public GameObject DeadPanel;
    
    

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
    }
    public void Update()
    {
        if (currentHp <= 0) {
            SetDeadActive();
        }
    }
    public void SetDeadActive()
    {
        if (DeadPanel.activeSelf)
            DeadPanel.SetActive(false);
        else
            DeadPanel.SetActive(true);
    }

    private IEnumerator HitAlphaAnimation()
    {
        //전체화면 크기로 배치된 imageScreen의 색상을 color 변수에 저장
        //imageScreen의 투명도를 40%로 설정
        Color color = imageScreen.color;
        color.a = 0.04f;
        imageScreen.color = color;

        //투명도가 0%가 될때까지 감소
        while(color.a >= 0.0f)
        {
            color.a -= 0.1f*Time.deltaTime;
            imageScreen.color = color;

            yield return null;
        }
    }
}