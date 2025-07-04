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

    public FloodSpawner flood_spawner;
    public Transform deb;


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

        // flood에 의한 이동 구현
        angle += flood_spawner.cal_move(angle) * Time.deltaTime;

        /*deb.position = new Vector2(
            Mathf.Cos(flood_spawner.current_flood.flood_inf["end"]) * CL.Get<float>("EarthRadius"),
            Mathf.Sin(flood_spawner.current_flood.flood_inf["end"]) * CL.Get<float>("EarthRadius")
        );*/
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
        // 무적 상태가 아니면서, 부딪힌 상대방이 "Fire"일 때만 데미지를 입습니다.
        if (!isInvincible && other.CompareTag("Fire"))
        {
            // 1. GameManager에 데미지를 입었다고 알립니다.
            GameManager.instance.PlayerTakeDamage(1);

            // 2. 무적 상태를 활성화하는 코루틴을 시작합니다.
            StartCoroutine(InvincibilityRoutine());

            // 3. Fire를 없애는 코드는 삭제합니다. 이제 Fire는 사라지지 않습니다.
        }
    }

    IEnumerator InvincibilityRoutine()
    {
        // 즉시 무적 상태로 변경
        isInvincible = true;

        // (선택 사항) 무적 상태를 시각적으로 보여주기 위해 플레이어를 반투명하게 만듭니다.
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = new Color(1f, 1f, 1f, 0.5f);

        // 0.1초 동안 대기
        yield return new WaitForSeconds(invincibility_time);

        // 0.1초 후 무적 상태를 해제하고 원래 색으로 돌아옵니다.
        if (sr != null) sr.color = Color.white; // 또는 원래 저장해둔 색상
        isInvincible = false;
    }

}
