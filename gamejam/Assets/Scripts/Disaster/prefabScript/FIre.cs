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
        // 충돌한 상대방의 태그가 "Fire"인지 확인합니다.
        if (other.CompareTag("Fire"))
        {
            Fire otherFire = other.GetComponent<Fire>();

            if (otherFire != null && this.direction == 0 && otherFire.direction == 1)
            {
                // 제거 책임은 나(반시계방향 Fire)에게 있다.
                ObjPoolManager.instance.Release(gameObject, "Fire");
                ObjPoolManager.instance.Release(other.gameObject, "Fire");
            }
        }
    }
}