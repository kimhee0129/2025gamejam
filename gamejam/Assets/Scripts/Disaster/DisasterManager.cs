using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisasterManager : MonoBehaviour
{
    private FireSpawner fs;
    private WindSpawner ws;
    private FloodSpawner fls;
    private VirusSpawner vs;

    private Dictionary<string, float> percent = new Dictionary<string, float>()
    {
        {"fire", 5f},
        {"flood", 5f},
        {"virus", 5f},
        {"wind", 10f}
    };

    private float fireTimer = -1f;
    private float floodTimer = -1f;
    private float virusTimer = -1f;
    private float windTimer = -1f;

    void Start()
    {
        fs = GetComponentInChildren<FireSpawner>();
        ws = GetComponentInChildren<WindSpawner>();
        fls = GetComponentInChildren<FloodSpawner>();
        vs = GetComponentInChildren<VirusSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        update_percent();
        spawn();
    }

    private void update_percent()
    {
        percent["fire"] += 0.5f * Time.deltaTime;
        percent["flood"] += 0.5f * Time.deltaTime;
        percent["virus"] += 0.7f * Time.deltaTime;
        percent["wind"] += 1f * Time.deltaTime;
    }

    private void spawn()
    {
        // fire
        if (fireTimer < 0f)
        {
            if (dice(percent["fire"]))
            {
                fireTimer += 10f;
                fs.Spawn();
            }
        }
        else
        {
            fireTimer -= Time.deltaTime;
        }

        // flood
        if (floodTimer < 0f)
        {
            if (dice(percent["flood"]))
            {
                floodTimer += 8f;
                fls.Spawn();
            }
        }
        else
        {
            floodTimer -= Time.deltaTime;
        }

        // virus
        if (virusTimer < 0f)
        {
            if (dice(percent["virus"]))
            {
                virusTimer += 3f;
                vs.Spawn();
            }
        }
        else
        {
            virusTimer -= Time.deltaTime;
        }
        
        // wind
        if (windTimer < 0f)
        {
            if (dice(percent["wind"]))
            {
                windTimer += 1f;
                ws.Spawn();
            }
        }
        else
        {
            windTimer -= Time.deltaTime;
        }
    }

    private bool dice(float p)
    {
        p *= Time.deltaTime;
        float v = Random.value * 100;
        if (v <= p)
            return true;
        else
            return false;
    }
}
