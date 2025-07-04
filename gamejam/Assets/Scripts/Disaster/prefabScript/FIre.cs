using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour
{
    private int direction = 0;
    private float spreadAngle;

    void OnEnable()
    {
        StartCoroutine(SpreadAndVanishRoutine());
    }

    public void Initialize(int direction)
    {
        this.direction = direction;
        this.spreadAngle = CL.Get<float>("FireSpreadAngle");
    }

    IEnumerator SpreadAndVanishRoutine()
    {
        yield return new WaitForSeconds(0.3f);

        Transform earthTransform = GameManager.instance.earthTransform;
        float earthRadius = GameManager.instance.earthRadius;

        float angleToRotate = (direction == 1) ? -spreadAngle : spreadAngle;
        Vector3 newDirection = Quaternion.Euler(0, 0, angleToRotate) * transform.up;

        Vector3 spawnPosition = earthTransform.position + newDirection * earthRadius;

        GameObject nextFire = ObjPoolManager.instance.InstantiateFromPool("Fire");
        if (nextFire != null)
        {
            nextFire.transform.position = spawnPosition;
            nextFire.transform.up = newDirection;


            nextFire.GetComponent<Fire>().Initialize(this.direction);
        }

        yield return new WaitForSeconds(0.2f);

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