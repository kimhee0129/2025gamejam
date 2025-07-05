using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour
{
    private int direction = 0;
    private float spreadAngle;
    private Transform earthTransform;
    private float earthRadius;
    private float OriginEarthRadius;
    private float PositionAdujusting;
    private float SpreadTime;
    private float VanishTime;

    void OnEnable()
    {
    }

    public void Initialize(int direction, Transform earth)
    {
        this.direction = direction;
        this.spreadAngle = CL.Get<float>("FireSpreadAngle");
        OriginEarthRadius = CL.Get<float>("EarthRadius");
        PositionAdujusting = CL.Get<float>("FirePositionAdjusting");
        earthRadius = OriginEarthRadius + PositionAdujusting;
        this.earthTransform = earth;
        this.SpreadTime = CL.Get<float>("FireSpreadTime");
        this.VanishTime = CL.Get<float>("FireVanishTime");

        StartCoroutine(SpreadAndVanishRoutine());
    }

    IEnumerator SpreadAndVanishRoutine()
    {
        yield return new WaitForSeconds(SpreadTime);

        float angleToRotate = (direction == 1) ? -spreadAngle : spreadAngle;
        Vector3 newDirection = Quaternion.Euler(0, 0, angleToRotate) * transform.up;
        Vector3 spawnPosition = earthTransform.position + newDirection * earthRadius;

        GameObject nextFire = ObjPoolManager.instance.InstantiateFromPool("Fire");
        if (nextFire != null)
        {
            nextFire.transform.position = spawnPosition;
            nextFire.transform.up = newDirection;


            nextFire.GetComponent<Fire>().Initialize(this.direction,this.earthTransform);
        }

        yield return new WaitForSeconds(VanishTime);

        ObjPoolManager.instance.Release(gameObject,"Fire");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �浹�� ������ �±װ� "Fire"���� Ȯ���մϴ�.
        if (other.CompareTag("Fire"))
        {
            Fire otherFire = other.GetComponent<Fire>();

            if (otherFire != null && this.direction == 0 && otherFire.direction == 1)
            {
                // ���� å���� ��(�ݽð���� Fire)���� �ִ�.
                ObjPoolManager.instance.Release(gameObject, "Fire");
                ObjPoolManager.instance.Release(other.gameObject, "Fire");
            }
        }
    }
}