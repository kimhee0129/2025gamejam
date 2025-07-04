using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Transform earthTransform;

    public DisasterManager DM;

    public float earthRadius;
    public int test = 5;

    public int playerHealth; // 플레이어의 초기 체력
    public bool isGameOver = false; // 게임오버 상태 확인


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

    void Start()
    {
        playerHealth = CL.Get<int>("PlayerInitialHealth");
        DM.InitDisaster(); // 재난 초기화 시작

    }


    void Update()
    {
    }

    

    public void PlayerTakeDamage(int damage)
    {
        if (isGameOver) return; // 게임오버 상태면 아무것도 하지 않음

        playerHealth -= damage;
        Debug.Log("플레이어 체력: " + playerHealth); // 체력 감소 확인용 로그

        if (playerHealth <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        isGameOver = true;
        Debug.Log("게임 오버!");

    }
}