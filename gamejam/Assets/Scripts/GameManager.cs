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

    public int playerHealth; // �÷��̾��� �ʱ� ü��
    public bool isGameOver = false; // ���ӿ��� ���� Ȯ��

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
        DM.InitDisaster(); // �糭 �ʱ�ȭ ����

    }
    void Update()
    {
        if (!isGameOver)
        {
            survivalTime += Time.deltaTime;
            if (scoreText != null)
            {
                // �Ҽ��� ���� ������ ����ϰ� ǥ��
                scoreText.text = "Score: " + (int)survivalTime;
            }
        }

    }

    public void PlayerTakeDamage(int damage)
    {
        if (isGameOver) return; // ���ӿ��� ���¸� �ƹ��͵� ���� ����

        playerHealth -= damage;
        Debug.Log("�÷��̾� ü��: " + playerHealth); // ü�� ���� Ȯ�ο� �α�

        if (playerHealth <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        isGameOver = true;
        Debug.Log("���� ����!");

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