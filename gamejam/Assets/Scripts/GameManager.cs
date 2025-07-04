using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Transform earthTransform;

    public DisasterManager DM;

    public float earthRadius;
    public int test = 5;

    public int playerHealth; // �÷��̾��� �ʱ� ü��
    public bool isGameOver = false; // ���ӿ��� ���� Ȯ��


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
        DM.InitDisaster(); // �糭 �ʱ�ȭ ����

    }


    void Update()
    {
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

    }
}