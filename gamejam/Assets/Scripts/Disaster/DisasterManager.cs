using UnityEngine;
using System.Collections;

public class DisasterManager : MonoBehaviour
{
    private FireSpawner FS;
    private WindSpawner WS;
    private FloodSpawner fls;

    void Start()
    {
        FS = GetComponentInChildren<FireSpawner>();
        WS = GetComponentInChildren<WindSpawner>();
        fls = GetComponentInChildren<FloodSpawner>();
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
            fls.Spawn();
            yield return new WaitForSeconds(10f);

            
             if (Random.value > 0.5f)
             {
                 FS.Spawn();
             }
             else
             {
                 WS.Spawn();
             }

            yield return new WaitForSeconds(5f);
        }
    }
}
