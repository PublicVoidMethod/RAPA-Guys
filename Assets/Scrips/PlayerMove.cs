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
        // 캐릭터콘트롤러 캐싱
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
        // 좌우의 입력값을 만들고
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 좌우의 방향을 넣어주고
        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();

        // 방향 벡터를 카메라의 방향을 기준으로 재계산 한다.
        dir = Camera.main.transform.TransformDirection(dir);

        dir.y = 0;

        // 3d모델의 회전  ???????????????????????????????????????????????????
        //playerModel.forward = Vector3.Lerp(playerModel.forward, dir, 0.5f);
        // 방향의 길이값이 있을 때(0보다 클 때)
        if (dir.sqrMagnitude != 0)
        {
            // 모델의 회전값 = 모델의 본래의 회전값, 바라보고자 하는 회전값
            playerModel.rotation = Quaternion.Lerp(playerModel.rotation, Quaternion.LookRotation(dir),
                rotateSpeed * Time.deltaTime);
        }

        PlayerJump();

        // 중력 값을 누적한다.
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        // 움직이게 한다.
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

        // 점프를 하기 위해 스페이스바의 입력을 받는다면
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            // 위쪽 방향으로 점프력을 추가한다.
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

            // 현재위치의 각도를 구한다. @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            float theta = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

            // 한프레임 뒤의 위치를 예상한다.
            float nextTheta = theta - alpha;
            float nextPosX = Mathf.Cos(nextTheta * Mathf.Deg2Rad) * radius;
            //float nextPosY = transform.position.y;
            float nextPosZ = Mathf.Sin(nextTheta * Mathf.Deg2Rad) * radius;

            Vector3 nextPos = new Vector3(nextPosX, 0, nextPosZ);

            Vector3 finalDir = nextPos - dir;

            //print("theta : " + theta + "/ alpha : " + alpha + " /Dir : " + dir + "/ next Pos : " + nextPos);

            acceleVec = finalDir;

            // 현재 위치의 좌표는 (dir.x, dir.z)
            // 다음 위치의 좌표는 ()
            // 다음 프레임의 좌표를 예상하기 위해서는 현재 좌표의 세타를 구해야하고
            // 현재 좌표의 세타는 arctan(dir.z / dir.x)
        }
        // SpinObstacle이라는 태그에 닿지 않았다면
        else
        {
            // 관성의 힘을 0으로 만든다.
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
