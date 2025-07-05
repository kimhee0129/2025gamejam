using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Blink : MonoBehaviour
{
    private SpriteRenderer sr;
    private float blinkInterval = 1.5f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(blink());
    }

    IEnumerator blink()
    {
        float alpha = 1f;
        bool fadingOut = true;

        while (true)
        {
            // 1. alpha 값 업데이트
            if (fadingOut)
            {
                alpha -= Time.deltaTime / blinkInterval;
                if (alpha <= 0f)
                {
                    alpha = 0f;
                    fadingOut = false;
                }
            }
            else
            {
                alpha += Time.deltaTime / blinkInterval;
                if (alpha >= 1f)
                {
                    alpha = 1f;
                    fadingOut = true;
                }
            }

            // 2. 알파 값 적용
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

            yield return null; // 매 프레임 실행
        }
    }
}
