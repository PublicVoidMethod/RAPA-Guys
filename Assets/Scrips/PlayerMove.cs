using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float playerSpeed = 5.0f;
    public float gravity = -20.0f;
    public float jumpPower = 15.0f;
    public float rotateSpeed = 10.0f;
    public float divingSpeed = 5.0f;

    float yVelocity = 0;
    int jumpCount = 1;
    //int divingCount = 1;

    public Transform playerModel;
    public Transform divingPoint;

    CharacterController cc;
    //Rigidbody rb;
    
    void Start()
    {
        // ĳ������Ʈ�ѷ� ĳ��
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        InputMove();
        //PlayerDiving();
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

        dir.y = 0;

        // 3d���� ȸ��  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //playerModel.forward = Vector3.Lerp(playerModel.forward, dir, 0.5f);
        // ������ ���̰��� ���� ��(0���� Ŭ ��)
        if (dir.sqrMagnitude != 0)
        {
            // ���� ȸ���� = ���� ������ ȸ����, �ٶ󺸰��� �ϴ� ȸ����
            playerModel.rotation = Quaternion.Lerp(playerModel.rotation, Quaternion.LookRotation(dir),
                rotateSpeed * Time.deltaTime);
        }

        PlayerJump();

        // �߷� ���� �����Ѵ�.
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        // �����̰� �Ѵ�.
        cc.Move(dir * playerSpeed * Time.deltaTime);
        //transform.position += dir * playerSpeed * Time.deltaTime;
    }

    void PlayerJump()
    {
        if (cc.collisionFlags == CollisionFlags.Below)
        {
            jumpCount = 1;
            //divingCount = 1;  // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            yVelocity = 0;
        }

        // ������ �ϱ� ���� �����̽����� �Է��� �޴´ٸ�
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            // ���� �������� �������� �߰��Ѵ�.
            yVelocity = jumpPower;
            jumpCount--;
        }
    }

    //void PlayerDiving()  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    //{
    //    // ���� ��Ʈ��Ű�� �����ٸ�
    //    if (Input.GetKeyDown(KeyCode.LeftControl))
    //    {
    //        yVelocity = 0;
    //        Vector3 dir = divingPoint.position - transform.position;
    //        dir.Normalize();
    //        transform.position += dir * divingSpeed * Time.deltaTime;
    //    }
    //}

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("SpinObstacle"))
        {
            RotateObstacle ro = hit.transform.GetComponentInParent<RotateObstacle>();
            float rotSpeed = ro.rotateSpeed;


            Vector3 dir = (transform.position - hit.transform.position);
            dir.y = 0;
            float radius = dir.magnitude;

            // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            float theta = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

            // ���� ��ġ�� ��ǥ�� (dir.x, dir.z)
            // ���� ��ġ�� ��ǥ�� ()
            // ���� �������� ��ǥ�� �����ϱ� ���ؼ��� ���� ��ǥ�� ��Ÿ�� ���ؾ��ϰ�
            // ���� ��ǥ�� ��Ÿ�� arctan(dir.z / dir.x)
        }
    }
}
