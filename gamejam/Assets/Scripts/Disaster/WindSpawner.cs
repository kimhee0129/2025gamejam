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
        // 3. ������ �ٶ��� ������ ��ġ�� ������ ���
        float randomAngle = Random.Range(0f, 360f);
        Vector3 baseDirection = Quaternion.Euler(0, 0, randomAngle) * Vector3.up;
        Vector3 spawnPos = earthTransform.position + baseDirection * earthRadius;

        // 4. ���� �ٶ� ������ ����ϴ� �ڷ�ƾ�� �����Ŵ
        StartCoroutine(SpawnSingleWindRoutine(spawnPos, baseDirection));

        yield return null;
    }

    private IEnumerator SpawnSingleWindRoutine(Vector3 position, Vector3 direction)
    {
        // --- '�ܻ�' ���� ---
        GameObject preview = ObjPoolManager.instance.InstantiateFromPool("Preview_Wind");
        if (preview != null)
        {
            preview.transform.position = position;
            preview.transform.up = direction;
        }

        // --- ���� �ð���ŭ ��� ---
        yield return new WaitForSeconds(previewDuration);

        // --- '�ܻ�' ���� ---
        if (preview != null) ObjPoolManager.instance.Release(preview, "Preview_Wind");

        // --- '��¥ �ٶ�' ���� ---
        GameObject obstacle = ObjPoolManager.instance.InstantiateFromPool("Wind");
        if (obstacle != null)
        {
            obstacle.transform.position = position;
            obstacle.transform.up = direction;

            obstacle.GetComponent<Wind>().Initialize(earthTransform);
        }
    }

}
