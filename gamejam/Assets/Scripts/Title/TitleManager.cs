using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public UnityEngine.UI.Image panel;
    public GameObject ts;
    private float showtime = 1f;
    private float up_d = 2f;
    void Start()
    {
        StartCoroutine(show());
    }

    IEnumerator show()
    {
        float elapsed_time = 0f;
        while (elapsed_time < showtime)
        {
            elapsed_time += Time.deltaTime;
            float p = elapsed_time / showtime;
            panel.color = new Color(0f, 0f, 0f, 1f - 0.1f * p);
            Color c = Color.white;
            c.a = p;
            SpriteRenderer[] sr_array = ts.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sr in sr_array)
            {
                sr.color = c;
            }

            ts.transform.position = new Vector2(0f, -up_d + up_d * p);
            yield return null;
        }
    }


}
