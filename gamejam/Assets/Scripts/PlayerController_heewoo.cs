using System;
using System.Collections;
using UnityEngine;

public class PlayerController_heewoo : MonoBehaviour
{
    private Rigidbody2D rb;
    private float angle = Mathf.PI / 2f;
    private static float radius = 1.89f;
    private float current_radius;
    private float angleSpeed = 1.2f;

    private float jump_time = 0.6f;
    private float jump_d = 1f;
    private bool is_jumping = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        current_radius = radius;
        rb = GetComponent<Rigidbody2D>();

        /* test */
        int test = CL.Get<int>("hello");
        Debug.Log(test);
        Debug.Log(GameManager.instance.test);
        /* */
    }

    void FixedUpdate()
    {
        //rb.linearVelocity = Vector2.zero;
        float xpos = Mathf.Cos(angle) * current_radius;
        float ypos = Mathf.Sin(angle) * current_radius;
        rb.MovePosition(new Vector2(xpos, ypos));
    }
    // Update is called once per frame
    void Update()
    {
        float a = angle * Mathf.Rad2Deg + 90f;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, a));
        if (Input.GetKey(KeyCode.A))
        {
            angle += angleSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            angle += -angleSpeed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !is_jumping)
        {
            StartCoroutine(Jump());
        }
    }

    IEnumerator Jump()
    {
        is_jumping = true;
        float elapsed_time = 0f;
        while (elapsed_time < jump_time / 2)
        {
            elapsed_time += Time.deltaTime;
            current_radius = radius + scalarLerp(0, jump_d, elapsed_time / (jump_time / 2));
            yield return null;
        }

        elapsed_time = 0f;
        while (elapsed_time < jump_time)
        {
            elapsed_time += Time.deltaTime;
            current_radius = radius + scalarLerp(jump_d, 0, elapsed_time / (jump_time / 2));
            yield return null;
        }
        is_jumping = false;
    }

    private float scalarLerp(float s_a, float s_b, float percent)
    {
        Vector2 result = Vector2.Lerp(new Vector2(s_a, 0f), new Vector2(s_b, 0f), percent);
        return result.x;
    }
}
