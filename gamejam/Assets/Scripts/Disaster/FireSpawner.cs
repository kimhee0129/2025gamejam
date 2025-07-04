using UnityEngine;

public class FireSpawner : MonoBehaviour
{
    public Transform earthTransform;

    public float earthRadius;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        earthRadius = CL.Get<float>("EarthRadius");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        float initialSpreadAngle = 10f;

        // 1. ���� ������ ����
        float randomAngle = Random.Range(0f, 360f);

        Debug.Log("������ ���� ����: " + randomAngle);
        Vector3 baseDirection = Quaternion.Euler(0, 0, randomAngle) * Vector3.up;

        // 2. ����(�ݽð�)�� ������(�ð�) ���� ����� ��ġ ���
        Vector3 rightDirection = Quaternion.Euler(0, 0, -initialSpreadAngle) * baseDirection;
        Vector3 leftDirection = Quaternion.Euler(0, 0, initialSpreadAngle) * baseDirection;

        Vector3 rightSpawnPos = earthTransform.position + rightDirection * earthRadius;
        Vector3 leftSpawnPos = earthTransform.position + leftDirection * earthRadius;

        // 3. ������Ʈ Ǯ���� Fire ����
        GameObject obstacle_R = ObjPoolManager.instance.InstantiateFromPool("Fire");
        GameObject obstacle_L = ObjPoolManager.instance.InstantiateFromPool("Fire");

        // 4. ������ Fire �ʱ�ȭ �� ��ġ/���� ����
        if (obstacle_R != null)
        {
            obstacle_R.transform.position = rightSpawnPos;
            obstacle_R.transform.up = rightDirection;
            // ������ Fire�� �ð����(1)���� �������� ����
            obstacle_R.GetComponent<Fire>().Initialize(1);
        }

        if (obstacle_L != null)
        {
            obstacle_L.transform.position = leftSpawnPos;
            obstacle_L.transform.up = leftDirection;
            // ���� Fire�� �ݽð����(0)���� �������� ����
            obstacle_L.GetComponent<Fire>().Initialize(0);
        }
    }

}
