using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int test = 5;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("two GameManagers..");
            Destroy(gameObject);
        }
    }
}