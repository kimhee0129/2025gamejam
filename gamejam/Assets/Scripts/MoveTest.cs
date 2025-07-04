using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class MoveTest : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        Vector2 move = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
            move += new Vector2(0f, 1f);
        if (Input.GetKey(KeyCode.S))
            move += Vector2.down;
        if (Input.GetKey(KeyCode.A))
            move += Vector2.left;
        if (Input.GetKey(KeyCode.D))
            move += Vector2.right;

        transform.Translate(move * moveSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("충돌 발생: " + collision.gameObject.name);
    }
}