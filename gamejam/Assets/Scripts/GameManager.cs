using UnityEngine;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Transform earthTransform;
    public TextMeshProUGUI scoreText;

    public DisasterManager DM;

    public float earthRadius;
    public int test = 5;

    public int playerHealth; // 플레이어의 초기 체력
    public bool isGameOver = false; // 게임오버 상태 확인

    public float survivalTime;


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
        survivalTime = 0f;
    }

    void Start()
    {
        playerHealth = CL.Get<int>("PlayerInitialHealth");
        DM.InitDisaster(); // 재난 초기화 시작

    }
    void Update()
    {
        if (!isGameOver)
        {
            survivalTime += Time.deltaTime;
            if (scoreText != null)
            {
                // 소수점 없이 정수로 깔끔하게 표시
                scoreText.text = "Score: " + (int)survivalTime;
            }
        }

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

        PlayerController_heewoo player = FindObjectOfType<PlayerController_heewoo>();
        if (player != null)
        {
            player.OnDie();
        }

    }

    public float scalarLerp(float s_a, float s_b, float percent)
    {
        Vector2 result = Vector2.Lerp(new Vector2(s_a, 0f), new Vector2(s_b, 0f), percent);
        return result.x;
    }
}