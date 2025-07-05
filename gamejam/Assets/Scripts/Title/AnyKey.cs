using UnityEngine;
using UnityEngine.SceneManagement;

public class AnyKey : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Ingame");
        }
    }
}
