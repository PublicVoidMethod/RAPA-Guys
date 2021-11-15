using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

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
    public float divingSpeed = 7.0f;
    public float hammerPower = 10.0f;

    float yVelocity = 0;
    int jumpCount = 1;
    int divingCount = 1;

    public Transform playerModel;
    public Transform divingPoint;
    public Vector3 acceleVec;

    CharacterController cc;
    Animator anim;

    void Start()
    {
        // 나의 포지션을 랜덤한 시작 위치에 위치시킨다.
        RandomStart.instance.StartPositionSet(gameObject);

        // 캐릭터콘트롤러 캐싱
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        pState = PlayerState.Idle;
        StartCoroutine(StayIdle());
    }

    void Update()
    {
        switch (pState)
        {
            case PlayerState.Idle:
                Idle();
                break;
            case PlayerState.Normal:
                break;
            case PlayerState.Be_Hit:
                break;
            case PlayerState.Diving:
                //PlayerDiving();
                break;
        }

        InputMove();
    }

    private void Idle()  // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    {
        
    }

    IEnumerator StayIdle()
    {
        yield return new WaitForSeconds(3f);
        pState = PlayerState.Normal;
        
    }

    void InputMove()
    {
        Vector3 dir;
        if (pState == PlayerState.Normal)
        {
            // 전후좌우의 입력값을 만들고
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // 전후좌우의 방향을 넣어주고
            dir = new Vector3(h, 0, v);
        }
        else
        {
            dir = Vector3.zero;
        }

        // dir값이 속도가 1보다 클 때만 dir값을 정규화 해준다(GetAxis의 -1~0, 0~1 의 값을 낮춰준다)
        if(dir.magnitude >= 1)
        {
            dir.Normalize();
        }

        // 입력값을 받으면 에니메이션 float값을 줌
        anim.SetFloat("Move", dir.magnitude);

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

        if(acceleVec.y > 0)
        {
            acceleVec = acceleVec * 0.98f;
        }

        // 움직이게 한다.
        cc.Move((dir * playerSpeed * Time.deltaTime) + acceleVec);
        //transform.position += dir * playerSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftControl) && divingCount > 0)
        {
            divingCount--;
            jumpPower = 0;
            pState = PlayerState.Diving;
            anim.SetTrigger("Dive");
            StartCoroutine(DivingTime());
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
        
    }

    IEnumerator DivingTime()
    {

        //    yVelocity = 0;
        //    Vector3 dir = divingPoint.position - transform.position;
        //    dir.Normalize();
        //    cc.Move(dir * divingSpeed * Time.deltaTime);

        acceleVec = playerModel.forward * divingSpeed * Time.deltaTime;

        yield return new WaitForSeconds(0.5f);
        divingCount = 1;
        jumpPower = 2.5f;

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
            if(pState != PlayerState.Be_Hit && pState != PlayerState.Diving)
            {
                // 관성의 힘을 0으로 만든다.
                acceleVec = Vector3.zero;
            }
            else
            {
                // 관성의 힘에 저항값을 준다(자신의 힘에 저항 수치를 곱한다.)
                acceleVec = acceleVec * 0.98f;
                acceleVec.y = acceleVec.y * 0.3f;
            }
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Stone"))
        {
            Vector3 dir = transform.position - other.transform.position;

            if(pState == PlayerState.Normal) StartCoroutine(FallDown(dir));
        }

        else if (other.gameObject.CompareTag("Hammer"))
        {
            Vector3 dir = transform.position - other.transform.position;
            //Vector3 dir = Vector3.Lerp(transform.position, other.transform.position, hammerPower * Time.deltaTime); // @@@@@@@@@@@@@@

            anim.SetTrigger("Be_Hit");

            if(pState == PlayerState.Normal) StartCoroutine(HammerSmite(dir));
        }
       
        else if (other.gameObject.CompareTag("Pendulum"))
        {
            Vector3 dir = transform.position - other.transform.position;
            //Vector3 dir = Vector3.Lerp(transform.position, other.transform.position, hammerPower * Time.deltaTime); // @@@@@@@@@@@@@@

            dir.y = 0.002f;
            dir = dir * 2f;

            anim.SetTrigger("Be_Hit");

            if (pState == PlayerState.Normal) StartCoroutine(HammerSmite(dir));
        }

        else if (other.gameObject.CompareTag("FinishLine"))
        {
            //gameObject.SetActive(false);
            // 다시 출발선으로 옮겨준다.
            RandomStart.instance.StartPositionSet(gameObject);
        }
    }

    public void StartFallDown(Vector3 dir)
    {
        StartCoroutine(FallDown(dir));
    }

    IEnumerator FallDown(Vector3 dir)
    {
        anim.SetTrigger("Be_Hit");
        pState = PlayerState.Be_Hit;
        acceleVec = dir * Time.deltaTime;
        //yield return new WaitForSeconds(2.7f);
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Be_Hit"));
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f);
        pState = PlayerState.Normal;
    }

    IEnumerator HammerSmite(Vector3 dir)
    {
        pState = PlayerState.Be_Hit;
        acceleVec = (dir + Vector3.up * 0.8f)  * hammerPower * Time.deltaTime;
        // 여기서 러프를 써야하는데 어떻게 해야하나.....  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //acceleVec = dir * Time.deltaTime;  // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //yield return new WaitForSeconds(2.5f);
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Be_Hit"));
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f);
        pState = PlayerState.Normal;
    }
}
