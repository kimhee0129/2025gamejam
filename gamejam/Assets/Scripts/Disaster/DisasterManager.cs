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
            
        }
    }
}
