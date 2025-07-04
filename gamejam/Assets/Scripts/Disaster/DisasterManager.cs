using UnityEngine;
using System.Collections;

public class DisasterManager : MonoBehaviour
{
    private FloodSpawner fls;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //FS=GetComponentInChildren<FireSpawner>();
        fls = GetComponentInChildren<FloodSpawner>();
    private FireSpawner FS;
    private WindSpawner WS;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FS=GetComponentInChildren<FireSpawner>();
        WS=GetComponentInChildren<WindSpawner>();
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
