using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float playerSpeed = 5.0f;
    public float gravity = -20.0f;
    public float jumpPower = 15.0f;

    float yVelocity = 0;
    int jumpCount = 1;

    CharacterController cc;

    void Start()
    {
        // 캐릭터콘트롤러 캐싱
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        InputMove();
    }

    void InputMove()
    {
        // 좌우의 입력값을 만들고
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 좌우의 방향을 넣어주고
        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();

        // 방향 벡터를 카메라의 방향을 기준으로 재계산 한다.
        dir = Camera.main.transform.TransformDirection(dir);

        PlayerJump();

        // 중력 값을 누적한다.
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        // 움직이게 한다.
        cc.Move(dir * playerSpeed * Time.deltaTime);
    }

    void PlayerJump()
    {
        if(cc.collisionFlags == CollisionFlags.Below)
        {
            jumpCount = 1;
        }

        // 점프를 하기 위해 스페이스바의 입력을 받는다면
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            // 위쪽 방향으로 점프력을 추가한다.
            yVelocity = jumpPower;
            jumpCount--;
        }
    }
}
