using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float playerSpeed = 5.0f;

    CharacterController cc;

    void Start()
    {
        // ĳ������Ʈ�ѷ� ĳ��
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        InputMove();
    }

    void InputMove()
    {
        // �¿��� �Է°��� �����
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // �¿��� ������ �־��ְ�
        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();

        // �����̰� �Ѵ�.
        cc.Move(dir * playerSpeed * Time.deltaTime);
    }

    void PlayerJump()
    {

    }
}
