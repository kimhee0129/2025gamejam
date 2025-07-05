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
        // ���� ������ ��� �ڷ�ƾ�� ��������ִ� ���Ҹ� �մϴ�.
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        float initialSpreadAngle = 10f;

        // 1. ���� ������ ����
        float randomAngle = Random.Range(0f, 360f);

        Vector3 baseDirection = Quaternion.Euler(0, 0, randomAngle) * Vector3.up;

        // 2. ����(�ݽð�)�� ������(�ð�) ���� ����� ��ġ ���
        Vector3 rightDirection = Quaternion.Euler(0, 0, -initialSpreadAngle) * baseDirection;
        Vector3 leftDirection = Quaternion.Euler(0, 0, initialSpreadAngle) * baseDirection;

        Vector3 rightSpawnPos = earthTransform.position + rightDirection * earthRadius;
        Vector3 leftSpawnPos = earthTransform.position + leftDirection * earthRadius;

        // --- 2. '�ܻ�' ���� ---
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

        // --- 3. ���� �ð���ŭ ��� ---
        yield return new WaitForSeconds(CL.Get<float>("FirePreviewTime"));

        // --- 4. '�ܻ�' ���� ---
        if (preview_R != null) ObjPoolManager.instance.Release(preview_R, "Preview_Fire");
        if (preview_L != null) ObjPoolManager.instance.Release(preview_L, "Preview_Fire");


        // 3. ������Ʈ Ǯ���� Fire ����
        GameObject obstacle_R = ObjPoolManager.instance.InstantiateFromPool("Fire");
        GameObject obstacle_L = ObjPoolManager.instance.InstantiateFromPool("Fire");

        // 4. ������ Fire �ʱ�ȭ �� ��ġ/���� ����
        if (obstacle_R != null)
        {
            obstacle_R.transform.position = rightSpawnPos;
            obstacle_R.transform.up = rightDirection;
            // ������ Fire�� �ð����(1)���� �������� ����
            obstacle_R.GetComponent<Fire>().Initialize(1,earthTransform);
        }

        if (obstacle_L != null)
        {
            obstacle_L.transform.position = leftSpawnPos;
            obstacle_L.transform.up = leftDirection;
            // ���� Fire�� �ݽð����(0)���� �������� ����
            obstacle_L.GetComponent<Fire>().Initialize(0,earthTransform);
        }
    }

}
