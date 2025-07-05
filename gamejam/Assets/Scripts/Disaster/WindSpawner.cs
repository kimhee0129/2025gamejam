using UnityEngine;
using System.Collections;

public class WindSpawner : MonoBehaviour
{
    public Transform earthTransform;
    public float earthRadius;
    private float OriginEarthRadius;
    private float PositionAdujusting;
    public float previewDuration;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OriginEarthRadius = CL.Get<float>("EarthRadius");
        PositionAdujusting = CL.Get<float>("WindPositionAdjusting");
        previewDuration = CL.Get<float>("WindPreviewDuration");
        earthRadius = OriginEarthRadius + PositionAdujusting;
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
        // 3. 각각의 바람을 생성할 위치와 방향을 계산
        float randomAngle = Random.Range(0f, 360f);
        Vector3 baseDirection = Quaternion.Euler(0, 0, randomAngle) * Vector3.up;
        Vector3 spawnPos = earthTransform.position + baseDirection * earthRadius;

        // 4. 개별 바람 생성을 담당하는 코루틴을 실행시킴
        StartCoroutine(SpawnSingleWindRoutine(spawnPos, baseDirection));

        yield return null;
    }

    private IEnumerator SpawnSingleWindRoutine(Vector3 position, Vector3 direction)
    {
        // --- '잔상' 생성 ---
        GameObject preview = ObjPoolManager.instance.InstantiateFromPool("Preview_Wind");
        if (preview != null)
        {
            preview.transform.position = position;
            preview.transform.up = direction;
        }

        // --- 예고 시간만큼 대기 ---
        yield return new WaitForSeconds(previewDuration);

        // --- '잔상' 제거 ---
        if (preview != null) ObjPoolManager.instance.Release(preview, "Preview_Wind");

        // --- '진짜 바람' 생성 ---
        GameObject obstacle = ObjPoolManager.instance.InstantiateFromPool("Wind");
        if (obstacle != null)
        {
            obstacle.transform.position = position;
            obstacle.transform.up = direction;

            obstacle.GetComponent<Wind>().Initialize(earthTransform);
        }
    }

}
