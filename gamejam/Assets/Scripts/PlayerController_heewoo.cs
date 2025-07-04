using System;
using System.Collections;
using UnityEngine;

public class PlayerController_heewoo : MonoBehaviour
{
    private Rigidbody2D rb;
    private float angle = Mathf.PI / 2f;

    private float radius;
    private float current_radius;
    private float angleSpeed;
    private float jump_time;
    private float jump_d;
    private float invincibility_time;


    private bool is_jumping = false;
    private bool isInvincible = false;
    private bool isStunned = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        radius = CL.Get<float>("EarthRadius");
        angleSpeed = CL.Get<float>("PlayerAngleSpeed");
        jump_time = CL.Get<float>("PlayerJumpTime");
        jump_d = CL.Get<float>("PlayerJumpDistance");
        invincibility_time = CL.Get<float>("PlayerInvincibilityTime");

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
        if (!isStunned)
        { // ���� ������ ���� �Է��� �����մϴ�.
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Fire �±׿� �浹�ߴ��� Ȯ��
        if (other.CompareTag("Fire"))
        {
            // ���� ���°� �ƴ� ���� �������� �Խ��ϴ�.
            if (!isInvincible)
            {
                GameManager.instance.PlayerTakeDamage(1);
                StartCoroutine(InvincibilityRoutine());
            }
        }
        // 2. Wind �±׿� �浹�ߴ��� Ȯ��
        else if (other.CompareTag("Wind"))
        {
            // ���� ���°� �ƴ� ���� ���Ͽ� �ɸ��ϴ�.
            if (!isStunned)
            {
                StartCoroutine(StunRoutine(1.0f));
            }

            // Wind�� �÷��̾�� �ε����� ��� ������ϴ�.
            ObjPoolManager.instance.Release(other.gameObject, "Wind");
        }
    }

    IEnumerator InvincibilityRoutine()
    {
        // ��� ���� ���·� ����
        isInvincible = true;

        // (���� ����) ���� ���¸� �ð������� �����ֱ� ���� �÷��̾ �������ϰ� ����ϴ�.
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = new Color(1f, 1f, 1f, 0.5f);

        // 0.1�� ���� ���
        yield return new WaitForSeconds(invincibility_time);

        // 0.1�� �� ���� ���¸� �����ϰ� ���� ������ ���ƿɴϴ�.
        if (sr != null) sr.color = Color.white; // �Ǵ� ���� �����ص� ����
        isInvincible = false;
    }


    private IEnumerator StunRoutine(float duration)
    {
        isStunned = true;
        Debug.Log("�÷��̾� ����! (1�ʰ� �̵� �Ұ�)");

        // (���� ����) ���� ���¸� �ð������� ǥ�� (��: ���� ����)
        // SpriteRenderer sr = GetComponent<SpriteRenderer>();
        // if (sr != null) sr.color = Color.gray;

        yield return new WaitForSeconds(duration);

        // if (sr != null) sr.color = Color.white;
        isStunned = false;
        Debug.Log("���� ����.");
    }
}
