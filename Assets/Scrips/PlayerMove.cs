using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float playerSpeed = 5.0f;

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

        // 움직이게 한다.
        cc.Move(dir * playerSpeed * Time.deltaTime);
    }

    void PlayerJump()
    {

    }
}
