using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public enum PlayerState  // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    {
        Idle,
        Normal,
        Be_Hit,
        Diving
    }
    public PlayerState pState;

    public float playerSpeed = 5.0f;
    public float gravity = -20.0f;
    public float jumpPower = 15.0f;
    public float rotateSpeed = 10.0f;
    public float divingSpeed = 5.0f;

    float yVelocity = 0;
    int jumpCount = 1;
    int divingCount = 1;

    public Transform playerModel;
    public Transform divingPoint;
    public Vector3 acceleVec;

    CharacterController cc;
    
    void Start()
    {
        // ĳ������Ʈ�ѷ� ĳ��
        cc = GetComponent<CharacterController>();

        pState = PlayerState.Normal;  // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    }

    void Update()
    {
        switch (pState) // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        {
            case PlayerState.Idle:

                break;
            case PlayerState.Normal:
                InputMove();
                break;
            case PlayerState.Be_Hit:

                break;
            case PlayerState.Diving:
                PlayerDiving();
                break;
        }
    }

    void InputMove()
    {
        // �¿��� �Է°��� �����
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // �¿��� ������ �־��ְ�
        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();

        // ���� ���͸� ī�޶��� ������ �������� ���� �Ѵ�.
        dir = Camera.main.transform.TransformDirection(dir);

        dir.y = 0;

        // 3d���� ȸ��  ???????????????????????????????????????????????????
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
        cc.Move((dir * playerSpeed * Time.deltaTime) + acceleVec);
        //transform.position += dir * playerSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            divingCount--;
            pState = PlayerState.Diving;
        }
    }

    void PlayerJump()
    {
        if (cc.collisionFlags == CollisionFlags.Below)
        {
            jumpCount = 1;
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

    void PlayerDiving()  // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    {
        StartCoroutine(DivingTime());
    }

    IEnumerator DivingTime()
    {
        yVelocity = 0;
        Vector3 dir = divingPoint.position - transform.position;
        dir.Normalize();
        cc.Move(dir * divingSpeed * Time.deltaTime);

        yield return new WaitForSeconds(0.5f);
        divingCount = 1;

        pState = PlayerState.Normal;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("SpinObstacle"))
        {
            RotateObstacle ro = hit.transform.GetComponentInParent<RotateObstacle>();
            float alpha = ro.rotateSpeed * Time.deltaTime;  //  @@@@@@@@@@@@@@@@@@@@

            transform.Rotate(Vector3.up * ro.rotateSpeed * Time.deltaTime);

            Vector3 dir = (transform.position - hit.transform.position);
            dir.y = 0;
            float radius = dir.magnitude;
            //print(dir);

            // ������ġ�� ������ ���Ѵ�. @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            float theta = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

            // �������� ���� ��ġ�� �����Ѵ�.
            float nextTheta = theta - alpha;
            float nextPosX = Mathf.Cos(nextTheta * Mathf.Deg2Rad) * radius;
            //float nextPosY = transform.position.y;
            float nextPosZ = Mathf.Sin(nextTheta * Mathf.Deg2Rad) * radius;

            Vector3 nextPos = new Vector3(nextPosX, 0, nextPosZ);

            Vector3 finalDir = nextPos - dir;

            //print("theta : " + theta + "/ alpha : " + alpha + " /Dir : " + dir + "/ next Pos : " + nextPos);

            acceleVec = finalDir;

            // ���� ��ġ�� ��ǥ�� (dir.x, dir.z)
            // ���� ��ġ�� ��ǥ�� ()
            // ���� �������� ��ǥ�� �����ϱ� ���ؼ��� ���� ��ǥ�� ��Ÿ�� ���ؾ��ϰ�
            // ���� ��ǥ�� ��Ÿ�� arctan(dir.z / dir.x)
        }
        // SpinObstacle�̶�� �±׿� ���� �ʾҴٸ�
        else
        {
            // ������ ���� 0���� �����.
            acceleVec = Vector3.zero;
        }

        if (hit.gameObject.CompareTag("Stone"))
        {
            StartCoroutine(FallDown());
        }
    }

    IEnumerator FallDown()
    {
        pState = PlayerState.Be_Hit;
        yield return new WaitForSeconds(1f);
        pState = PlayerState.Normal;
    }
}
