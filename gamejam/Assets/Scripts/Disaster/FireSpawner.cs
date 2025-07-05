using UnityEngine;
using System.Collections;

public class FireSpawner : MonoBehaviour
{
    public Transform earthTransform;
    public float earthRadius;
    private float OriginEarthRadius;
    private float PositionAdujusting;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OriginEarthRadius = CL.Get<float>("EarthRadius");
        PositionAdujusting = CL.Get<float>("FirePositionAdjusting");
        earthRadius = OriginEarthRadius + PositionAdujusting;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        // 실제 로직이 담긴 코루틴을 실행시켜주는 역할만 합니다.
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        float initialSpreadAngle = 10f;

        // 1. 랜덤 기준점 생성
        float randomAngle = Random.Range(0f, 360f);

        Vector3 baseDirection = Quaternion.Euler(0, 0, randomAngle) * Vector3.up;

        // 2. 왼쪽(반시계)과 오른쪽(시계) 불의 방향과 위치 계산
        Vector3 rightDirection = Quaternion.Euler(0, 0, -initialSpreadAngle) * baseDirection;
        Vector3 leftDirection = Quaternion.Euler(0, 0, initialSpreadAngle) * baseDirection;

        Vector3 rightSpawnPos = earthTransform.position + rightDirection * earthRadius;
        Vector3 leftSpawnPos = earthTransform.position + leftDirection * earthRadius;

        // --- 2. '잔상' 생성 ---
        GameObject preview_R = ObjPoolManager.instance.InstantiateFromPool("Preview_Fire");
        GameObject preview_L = ObjPoolManager.instance.InstantiateFromPool("Preview_Fire");

        if (preview_R != null)
        {
            preview_R.transform.position = rightSpawnPos;
            preview_R.transform.up = rightDirection;
        }
        if (preview_L != null)
        {
            preview_L.transform.position = leftSpawnPos;
            preview_L.transform.up = leftDirection;
        }

        // --- 3. 예고 시간만큼 대기 ---
        yield return new WaitForSeconds(CL.Get<float>("FirePreviewTime"));

        // --- 4. '잔상' 제거 ---
        if (preview_R != null) ObjPoolManager.instance.Release(preview_R, "Preview_Fire");
        if (preview_L != null) ObjPoolManager.instance.Release(preview_L, "Preview_Fire");


        // 3. 오브젝트 풀에서 Fire 생성
        GameObject obstacle_R = ObjPoolManager.instance.InstantiateFromPool("Fire");
        GameObject obstacle_L = ObjPoolManager.instance.InstantiateFromPool("Fire");

        // 4. 각각의 Fire 초기화 및 위치/방향 설정
        if (obstacle_R != null)
        {
            obstacle_R.transform.position = rightSpawnPos;
            obstacle_R.transform.up = rightDirection;
            // 오른쪽 Fire는 시계방향(1)으로 퍼지도록 설정
            obstacle_R.GetComponent<Fire>().Initialize(1,earthTransform);
        }

        if (obstacle_L != null)
        {
            obstacle_L.transform.position = leftSpawnPos;
            obstacle_L.transform.up = leftDirection;
            // 왼쪽 Fire는 반시계방향(0)으로 퍼지도록 설정
            obstacle_L.GetComponent<Fire>().Initialize(0,earthTransform);
        }
    }

}
