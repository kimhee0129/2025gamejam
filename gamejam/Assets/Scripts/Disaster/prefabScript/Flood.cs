using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flood : MonoBehaviour
{
    public bool is_start = false;
    public Dictionary<string, float> flood_inf = new Dictionary<string, float>() { { "activated", 0f } };
    private float elapsed_time = 0f;
    private Animator anim;

    public void Init(float apos)
    {
        elapsed_time = 0f;
        is_start = false;
        anim = GetComponent<Animator>();
        flood_inf["center"] = apos;
        flood_inf["start"] = apos;
        flood_inf["end"] = apos;
        flood_inf["activated"] = 0;

        /* Offset 적용 코드 */
        float radius = CL.Get<float>("EarthRadius");
        float offsetX = CL.Get<float>("FloodOffsetX");
        float offsetY = CL.Get<float>("FloodOffsetY");

        // 기준 벡터: (radius + offsetX, radius + offsetY) at angle = π/2
        Vector2 offsetAtTop = new Vector2(offsetX, offsetY);

        // 회전 각도: 현재 각도 - π/2
        float theta = apos - Mathf.PI / 2f;
        float cos = Mathf.Cos(theta);
        float sin = Mathf.Sin(theta);

        // 회전된 offset
        Vector2 rotatedOffset = new Vector2(
            offsetAtTop.x * cos - offsetAtTop.y * sin,
            offsetAtTop.x * sin + offsetAtTop.y * cos
        );

        // 현재 위치 (radius 거리 만큼)
        Vector2 unitDir = new Vector2(Mathf.Cos(apos), Mathf.Sin(apos));
        Vector2 basePos = unitDir * radius;

        // 최종 위치 = 기본 위치 + 회전된 보정값
        transform.position = basePos + rotatedOffset;

        /* 끝 */


        float a = Mathf.Rad2Deg * apos;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, a - 90f));

        StartCoroutine(Warn());
    }

    // Update is called once per frame
    void Update()
    {
        if (is_start)
        {
            elapsed_time += Time.deltaTime;
            float p = Mathf.Min(1f, elapsed_time / CL.Get<float>("FloodTime")); // 중요 ! floodtime === framerate 일치하는지 확인하기!!!!!
            float delta = GameManager.instance.scalarLerp(0f, 1.19f, p);
            flood_inf["start"] = flood_inf["center"] - delta; // 반시계 방향 기준
            flood_inf["end"] = flood_inf["center"] + delta;
        }
    }

    IEnumerator Warn()
    {
        yield return new WaitForSeconds(CL.Get<float>("FloodWarnTime"));
        is_start = true;
        anim.SetTrigger("start_flood");
        flood_inf["activated"] = 1;
        StartCoroutine(Remain());
    }

    IEnumerator Remain()
    {
        yield return new WaitForSeconds(CL.Get<float>("FloodTime") + CL.Get<float>("FloodRemainTime"));
        flood_inf["activated"] = 0;
        ObjPoolManager.instance.Release(gameObject, "Flood");
    }
}
