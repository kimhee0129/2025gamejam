using UnityEngine;

public class VirusExplosion : MonoBehaviour
{
    public void Init(Vector2 pos)
    {
        transform.position = pos;
    }

    public void terminate()
    {
        ObjPoolManager.instance.Release(gameObject, "virus_explosion");
    }
}
