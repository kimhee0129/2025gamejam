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

        transform.position = new Vector2(
            Mathf.Cos(apos) * CL.Get<float>("EarthRadius") + CL.Get<float>("FloodOffsetX"),
            Mathf.Sin(apos) * CL.Get<float>("EarthRadius") + CL.Get<float>("FloodOffsetY")
        );

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
