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
    public PlayerController pc;
    public GameObject goUI;
    public Card card;
    public TMP_Text bigscore;
    public TMP_Text best;

    public bool playing = true;


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
        if (!PlayerPrefs.HasKey("Best"))
        {
            PlayerPrefs.SetInt("Best", 0);
        }
        max_hp = CL.Get<int>("PlayerInitialHealth");
        current_hp = max_hp;

    }
    void Update()
    {
        survivalTime += Time.deltaTime * 10;
        score.text = $"{(int)survivalTime}";

        hp.text = $"{Mathf.Max(0, current_hp)}/{max_hp}";
        hp_bar.set_value((float)current_hp / (float)max_hp);
    }

    public void Damage(int damage, string src)
    {
        current_hp -= damage;

        if (current_hp <= 0)
        {
            GameOver(src);
        }
    }

    void GameOver(string killer)
    {
        bigscore.text = $"{(int)survivalTime}";

        if ((int)survivalTime >= PlayerPrefs.GetInt("Best"))
        {
            PlayerPrefs.SetInt("Best", (int)survivalTime);
        }

        best.text = $"Best: {PlayerPrefs.GetInt("Best")}";

        playing = false;
        Time.timeScale = 0f;

        StartCoroutine(GameoverUI(killer));
        pc.death_anim();
    }

    public float scalarLerp(float s_a, float s_b, float percent)
    {
        Vector2 result = Vector2.Lerp(new Vector2(s_a, 0f), new Vector2(s_b, 0f), percent);
        return result.x;
    }

    IEnumerator GameoverUI(string killer)
    {
        yield return new WaitForSecondsRealtime(CL.Get<float>("gameoverUITime"));

        hp.gameObject.SetActive(false);
        hp_bar.gameObject.SetActive(false);
        score.gameObject.SetActive(false);

        goUI.SetActive(true);
        card.show(killer);
    }
}