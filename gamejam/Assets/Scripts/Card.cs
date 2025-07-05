using UnityEngine;

public class Card : MonoBehaviour
{
    public GameObject fire;
    public GameObject flood;
    public GameObject virus;
    public GameObject wind;
    void Start()
    {
        
    }

    public void show(string type)
    {
        if (type == "fire")
        {
            fire.SetActive(true);
        }
        else if (type == "flood")
        {
            flood.SetActive(true);
        }
        else if (type == "virus")
        {
            virus.SetActive(true);
        }
        else
        {
            wind.SetActive(true);
        }
    }
}
