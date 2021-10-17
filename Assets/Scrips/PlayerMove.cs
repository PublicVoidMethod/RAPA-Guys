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

        // ���� ���͸� ī�޶��� ������ �������� ���� �Ѵ�.
        dir = Camera.main.transform.TransformDirection(dir);

        PlayerJump();

        // �߷� ���� �����Ѵ�.
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        // �����̰� �Ѵ�.
        cc.Move(dir * playerSpeed * Time.deltaTime);
    }

    void PlayerJump()
    {
        if(cc.collisionFlags == CollisionFlags.Below)
        {
            jumpCount = 1;
        }

        // ������ �ϱ� ���� �����̽����� �Է��� �޴´ٸ�
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            // ���� �������� �������� �߰��Ѵ�.
            yVelocity = jumpPower;
            jumpCount--;
        }
    }
}
