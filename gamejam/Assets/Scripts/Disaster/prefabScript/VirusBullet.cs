using UnityEngine;
using System;
using System.Collections;

public class VirusBullet : MonoBehaviour
{
    private float t;

    public void Shoot(Vector2 startPos, Vector2 endPos, Vector2 fireDirection, int bend)
    {
        StartCoroutine(MoveAlongCurve(startPos, endPos, fireDirection, bend));
    }

    IEnumerator MoveAlongCurve(Vector2 startPos, Vector2 endPos, Vector2 fireDirection, int bend)
    {
        t = 0f;
        transform.position = startPos;

        Vector2 controlPoint = startPos + fireDirection.normalized * CL.Get<float>("VirusCurve");

        // 부드러운 궤적을 위해 약간 endPos 방향으로 끌어당김
        controlPoint = Vector2.Lerp(controlPoint, endPos, 0.3f);

        // bend == -1 이면 기준선에 대해 대칭시킴
        if (bend == -1)
        {
            controlPoint = ReflectAcrossLine(controlPoint, startPos, endPos);
        }

        // bend == 0 이면 그냥 직선
        if (bend == 0)
        {
            controlPoint = (startPos + endPos) * 0.5f; // 직선 이동이므로 중간점으로
        }

        while (t < 1f)
        {
            t += Time.deltaTime / CL.Get<float>("VirusTime");

            Vector2 newPos = Mathf.Pow(1 - t, 2) * startPos +
                            2 * (1 - t) * t * controlPoint +
                            Mathf.Pow(t, 2) * endPos;

            transform.position = newPos;
            yield return null;
        }

        transform.position = endPos;

        ObjPoolManager.instance.Release(gameObject, "virus_bullet");
    }

    Vector2 ReflectAcrossLine(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
    {
        Vector2 dir = lineEnd - lineStart;
        Vector2 normal = new Vector2(-dir.y, dir.x).normalized;

        Vector2 toPoint = point - lineStart;
        float dist = Vector2.Dot(toPoint, normal);

        return point - 2 * dist * normal;
    }
}