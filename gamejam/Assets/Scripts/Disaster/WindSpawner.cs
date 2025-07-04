using UnityEngine;
using System.Collections;

public class WindSpawner : MonoBehaviour
{
    public Transform earthTransform;
    public float earthRadius;
    private float OriginEarthRadius;
    private float PositionAdujusting;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OriginEarthRadius = CL.Get<float>("EarthRadius");
        PositionAdujusting = CL.Get<float>("WindPositionAdjusting");
        earthRadius = OriginEarthRadius - PositionAdujusting;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        float randomAngle = Random.Range(0f, 360f);

        Debug.Log("바람 랜덤 각도: " + randomAngle);

        Vector3 baseDirection = Quaternion.Euler(0, 0, randomAngle) * Vector3.up;
        Vector3 SpawnPos = earthTransform.position + baseDirection * earthRadius;


        GameObject preview = ObjPoolManager.instance.InstantiateFromPool("Preview_Wind");

        if (preview != null)
        {
            preview.transform.position = SpawnPos;
            preview.transform.up = baseDirection;
        }

        yield return new WaitForSeconds(0.5f);

        if (preview != null) ObjPoolManager.instance.Release(preview, "Preview_Wind");

        GameObject obstacle = ObjPoolManager.instance.InstantiateFromPool("Wind");

        if (obstacle != null)
        {
            obstacle.transform.position = SpawnPos;
            obstacle.transform.up = baseDirection;
            obstacle.GetComponent<Wind>().Initialize(earthTransform);

        }

    }

}
