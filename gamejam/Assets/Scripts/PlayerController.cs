using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float angle = Mathf.PI / 2f;
    private SpriteRenderer sr; // 1. ��������Ʈ ������ ���� �߰�
    private Animator anim;     // 2. �ִϸ����� ���� �߰�

    private float radius;
    private float current_radius;
    private float angleSpeed;
    private float jump_time;
    private float jump_d;
    private bool is_jumping = false;
    private bool is_protect = false;
    private bool can_control = true;
    private int wind_count = 0;

    private float shakeTime = 0f;
    private float shakeIntensity;

    public FloodSpawner flood_spawner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        radius = CL.Get<float>("EarthRadius");
        angleSpeed = CL.Get<float>("PlayerAngleSpeed");
        jump_time = CL.Get<float>("PlayerJumpTime");
        jump_d = CL.Get<float>("PlayerJumpDistance");
        shakeIntensity = CL.Get<float>("WindShake");

        current_radius = radius;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>(); // 3. ������Ʈ ��������
        anim = GetComponent<Animator>();     // 4. ������Ʈ ��������
    }

    void FixedUpdate()
    {
        float xpos = Mathf.Cos(angle) * current_radius;
        float ypos = Mathf.Sin(angle) * current_radius;
        rb.MovePosition(new Vector2(xpos, ypos));
    }
    // Update is called once per frame
    void Update()
    {
        float a = angle * Mathf.Rad2Deg - 90f;

        if (!can_control)
        {
            shakeTime += Time.deltaTime * CL.Get<float>("WindShakeSpeed"); // ��鸲 �ӵ� ����
            float shake = Mathf.Sin(shakeTime) * shakeIntensity;
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, a + shake));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, a));
            shakeTime = 0f; // �ʱ�ȭ
        }

        anim.SetBool("can_control", can_control);

        if (!can_control)
        {
            anim.SetBool("is_walking", false);
            return;
        }

        anim.SetBool("is_walking", Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D));

        if (Input.GetKey(KeyCode.A))
        {
            angle += angleSpeed * Time.deltaTime;
            sr.flipX = true; // 6. AŰ�� ������ �������� ������
        }
        if (Input.GetKey(KeyCode.D))
        {
            angle += -angleSpeed * Time.deltaTime;
            sr.flipX = false; // 7. DŰ�� ������ ���� ����
        }


        if (Input.GetKeyDown(KeyCode.Space) && !is_jumping)
        {
            is_jumping = true;
            StartCoroutine(Jump());
        }

        // flood�� ���� �̵� ����
        float flood_eff = flood_spawner.cal_move(angle) * Time.deltaTime;
        if (Mathf.Abs(flood_eff) > 0 && !is_protect)
        {
            StartCoroutine(Protect());
            GameManager.instance.Damage(CL.Get<int>("FloodDamage"));
        }
        angle += flood_eff;
    }

    IEnumerator Jump()
    {
        float elapsed_time = 0f;

        while (elapsed_time < jump_time)
        {
            elapsed_time += Time.deltaTime;
            float t = elapsed_time / jump_time;

            // ���� ���ٰ� �Ʒ��� �������� � (������ ����)
            float curved = Mathf.Sin(t * Mathf.PI); // t=0 �� 0, t=0.5 �� 1, t=1 �� 0

            current_radius = radius + jump_d * curved;

            yield return null;
        }

        current_radius = radius; // ��Ȯ�ϰ� ���� ��ġ��
        is_jumping = false;
    }

    IEnumerator Protect()
    {
        is_protect = true;

        float elapsed_time = 0f;
        float invincibleTime = CL.Get<float>("PlayerInvincibilityTime");
        float blinkCycle = CL.Get<float>("PlayerBlinkTime"); // ���̵���/�ƿ� �� ����Ŭ �ð�
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        sr.color = Color.red;
        yield return new WaitForSeconds(CL.Get<float>("PlayerRedTime"));
        sr.color = Color.white;

        while (elapsed_time < invincibleTime)
        {
            float t = 0f;
            while (t < blinkCycle)
            {
                t += Time.deltaTime;
                elapsed_time += Time.deltaTime;

                // 0~1 �� 1~0 �� 0~1 �ݺ�
                float alpha = Mathf.PingPong(t * 2f, 1f); // �� ������ �����̰� �Ϸ��� *2f�� �ø���
                Color c = sr.color;
                c.a = alpha;
                sr.color = c;

                yield return null;
            }
        }

        // ���� ������ Ȯ���ϰ� ���̵���
        Color finalColor = sr.color;
        finalColor.a = 1f;
        sr.color = finalColor;

        is_protect = false;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wind")
        {
            wind_count++;
            can_control = false;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (is_protect)
            return;

        if (other.tag == "Fire")
        {
            StartCoroutine(Protect());
            GameManager.instance.Damage(CL.Get<int>("FireDamage"));
        }

        if (other.tag == "Wind")
        {
            StartCoroutine(Protect());
            GameManager.instance.Damage(CL.Get<int>("WindDamage"));
        }

        if (other.tag == "Virus")
        {
            StartCoroutine(Protect());
            GameManager.instance.Damage(CL.Get<int>("VirusDamage"));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Wind")
        {
            wind_count--;
            if (wind_count == 0)
                can_control = true;
        }
    }
}
