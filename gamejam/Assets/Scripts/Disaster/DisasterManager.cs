using UnityEngine;
using System.Collections;

public class DisasterManager : MonoBehaviour
{
    private FireSpawner FS;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FS=GetComponentInChildren<FireSpawner>();
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
            FS.Spawn();
            yield return new WaitForSeconds(5f);
            
        }
    }
}
