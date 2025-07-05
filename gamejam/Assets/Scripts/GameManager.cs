using UnityEngine;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Transform earthTransform;
    public HPbar hp_bar;
    public TMP_Text hp;
    public TMP_Text score;

    public DisasterManager DM;

    public float earthRadius;

    private int max_hp;
    private int current_hp;

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
        max_hp = CL.Get<int>("PlayerInitialHealth");
        current_hp = max_hp;

    }
    void Update()
    {
        survivalTime += Time.deltaTime * 10;
        score.text = $"{(int)survivalTime}";

        hp.text = $"{current_hp}/{max_hp}";
        hp_bar.set_value((float)current_hp / (float)max_hp);
    }

    public void Damage(int damage)
    {
        current_hp -= damage;

        if (current_hp <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Time.timeScale = 0f;
    }

    public float scalarLerp(float s_a, float s_b, float percent)
    {
        Vector2 result = Vector2.Lerp(new Vector2(s_a, 0f), new Vector2(s_b, 0f), percent);
        return result.x;
    }
}