using UnityEngine;

public class FloodSpawner : MonoBehaviour
{
    public Flood current_flood;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Spawn()
    {
        Flood f = ObjPoolManager.instance.InstantiateFromPool("Flood").GetComponent<Flood>();
        f.Init(Random.Range(0f, 2 * Mathf.PI));
        current_flood = f;
    }

    float Normalize(float rad)
    {
        rad %= 2 * Mathf.PI;
        if (rad < 0) rad += 2 * Mathf.PI;
        return rad;
    }

    public float cal_move(float player_apos)
    {
        if (current_flood == null)
            return 0f;
        if (!current_flood.is_start || current_flood.flood_inf["activated"] == 0f)
            return 0f;

        float start = Normalize(current_flood.flood_inf["start"]);
        float end = Normalize(current_flood.flood_inf["end"]);
        float center = Normalize(current_flood.flood_inf["center"]);
        player_apos = Normalize(player_apos);

        // 플레이어가 호 안에 있을 때만 이동
        if (IsOnArc(player_apos, start, end))
        {
            // center 기준 왼쪽(반시계 방향)이면 -0.3f, 오른쪽이면 +0.3f
            return IsOnArc(player_apos, start, center) ? -CL.Get<float>("FloodPushSpeed") : CL.Get<float>("FloodPushSpeed");
        }

        return 0f;
    }

    bool IsOnArc(float angle, float start, float end)
    {
        angle = Normalize(angle);
        start = Normalize(start);
        end = Normalize(end);

        float arcLength = (end - start + 2 * Mathf.PI) % (2 * Mathf.PI);
        float angleOffset = (angle - start + 2 * Mathf.PI) % (2 * Mathf.PI);

        return angleOffset <= arcLength;
    }
}
