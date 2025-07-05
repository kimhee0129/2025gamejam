using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float angle = Mathf.PI / 2f;
    private SpriteRenderer sr; // 1. 스프라이트 렌더러 변수 추가
    private Animator anim;     // 2. 애니메이터 변수 추가

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
        sr = GetComponent<SpriteRenderer>(); // 3. 컴포넌트 가져오기
        anim = GetComponent<Animator>();     // 4. 컴포넌트 가져오기
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
            shakeTime += Time.deltaTime * CL.Get<float>("WindShakeSpeed"); // 흔들림 속도 조절
            float shake = Mathf.Sin(shakeTime) * shakeIntensity;
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, a + shake));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, a));
            shakeTime = 0f; // 초기화
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
            sr.flipX = true; // 6. A키를 누르면 왼쪽으로 뒤집기
        }
        if (Input.GetKey(KeyCode.D))
        {
            angle += -angleSpeed * Time.deltaTime;
            sr.flipX = false; // 7. D키를 누르면 원래 방향
        }


        if (Input.GetKeyDown(KeyCode.Space) && !is_jumping)
        {
            is_jumping = true;
            StartCoroutine(Jump());
        }

        // flood에 의한 이동 구현
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

            // 위로 갔다가 아래로 내려오는 곡선 (포물선 형태)
            float curved = Mathf.Sin(t * Mathf.PI); // t=0 → 0, t=0.5 → 1, t=1 → 0

            current_radius = radius + jump_d * curved;

            yield return null;
        }

        current_radius = radius; // 정확하게 원래 위치로
        is_jumping = false;
    }

    IEnumerator Protect()
    {
        is_protect = true;

        float elapsed_time = 0f;
        float invincibleTime = CL.Get<float>("PlayerInvincibilityTime");
        float blinkCycle = CL.Get<float>("PlayerBlinkTime"); // 페이드인/아웃 한 사이클 시간
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

                // 0~1 → 1~0 → 0~1 반복
                float alpha = Mathf.PingPong(t * 2f, 1f); // 더 빠르게 깜빡이게 하려면 *2f를 늘리기
                Color c = sr.color;
                c.a = alpha;
                sr.color = c;

                yield return null;
            }
        }

        // 무적 끝나고 확실하게 보이도록
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
