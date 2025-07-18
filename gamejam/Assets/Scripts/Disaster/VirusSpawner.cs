using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusSpawner : MonoBehaviour
{
    public PlayerController pc;
    public void Spawn()
    {
        StartCoroutine(WarnAndShow());
    }

    IEnumerator WarnAndShow()
    {
        int m = 1;
        if (Random.value < 0.5f)
            m = -1;

        float r = CL.Get<float>("EarthRadius") + CL.Get<float>("VirusWarnOffset");
        Vector2 polar_pos = new Vector2(0f, r * m);

        GameObject warn = ObjPoolManager.instance.InstantiateFromPool("virus_warn");
        warn.transform.position = polar_pos;

        yield return new WaitForSeconds(CL.Get<float>("VirusWarnTime"));
        ObjPoolManager.instance.Release(warn, "virus_warn");

        show(polar_pos);
    }

    public void show(Vector2 polar_pos)
    {

        VirusExplosion ve = ObjPoolManager.instance.InstantiateFromPool("virus_explosion").GetComponent<VirusExplosion>();
        ve.Init(polar_pos);

        // bullet 발사
        List<VirusBullet> vb_list = new List<VirusBullet>();
        List<Vector2> dir_list = new List<Vector2>();

        dir_list.Add(new Vector2(1f, 2f).normalized);
        dir_list.Add(new Vector2(1f, 1.5f).normalized);
        dir_list.Add(new Vector2(1f, 1f).normalized);

        int bend = 1;

        if (pc.transform.position.x < 0)
        {
            for (int i = 0; i < 3; i++)
            {
                dir_list[i] = new Vector2(dir_list[i].x * -1, dir_list[i].y);
            }
        }

        if (polar_pos.y < 0)
        {
            for (int i = 0; i < 3; i++)
            {
                dir_list[i] = new Vector2(dir_list[i].x, dir_list[i].y * -1);
            }
        }

        for (int i = 0; i < 3; i++)
        {
            vb_list.Add(ObjPoolManager.instance.InstantiateFromPool("virus_bullet").GetComponent<VirusBullet>());
        }

        for (int i = 0; i < 3; i++)
        {
            Vector2 offset = pc.transform.position.normalized * -CL.Get<float>("VirusOffsetY");
            vb_list[i].Shoot(polar_pos, (Vector2)pc.transform.position - offset, dir_list[i], bend);
        }
    }
}
