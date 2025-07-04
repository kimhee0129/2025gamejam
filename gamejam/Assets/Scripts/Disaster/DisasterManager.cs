using UnityEngine;
using System.Collections;

public class DisasterManager : MonoBehaviour
{
    private FireSpawner FS;
    private WindSpawner WS;
    private FloodSpawner fls;
    private VirusSpawner vs;

    void Start()
    {
        FS = GetComponentInChildren<FireSpawner>();
        WS = GetComponentInChildren<WindSpawner>();
        fls = GetComponentInChildren<FloodSpawner>();
        vs = GetComponentInChildren<VirusSpawner>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitDisaster()
    {
        StartCoroutine(Disaster());
    }

    IEnumerator Disaster()
    {
        while (true)
        {
            vs.Spawn();
            yield return new WaitForSeconds(5f);

            /*
             if (Random.value > 0.5f)
             {
                 FS.Spawn();
             }
             else
             {
                 WS.Spawn();
             }

            yield return new WaitForSeconds(5f);
            */
        }
    }
}
