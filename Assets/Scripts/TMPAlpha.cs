using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TMPAlpha : MonoBehaviour
{
    [SerializeField]
    private float lerpTime = 0.5f;
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    public void FadeOut()
    {
        StartCoroutine(AlplphaLerp(1, 0));
    }
    private IEnumerator AlplphaLerp(float start, float end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while(percent < 1)
        {
           //lerpTime 시간동안 반복문 실행
           currentTime += Time.deltaTime;
            percent = currentTime / lerpTime;

            //폰트 투명도를 start -> end로
            Color color = text.color;
            color.a = Mathf.Lerp(start, end, percent);
            text.color = color;

            yield return null;
        }
    }
}
