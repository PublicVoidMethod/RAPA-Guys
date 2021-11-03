using System;
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
    public float divingSpeed = 7.0f;
    public float hammerPower = 10.0f;

    float yVelocity = 0;
    int jumpCount = 1;
    int divingCount = 1;
    int jumpZero = 0;

    public Transform playerModel;
    public Transform divingPoint;
    public Vector3 acceleVec;

    CharacterController cc;
    Animator anim;
    
    void Start()
    {
        // ĳ������Ʈ�ѷ� ĳ��
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
            // �¿��� �Է°��� �����
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // �¿��� ������ �־��ְ�
            dir = new Vector3(h, 0, v);
        }
        else
        {
            dir = Vector3.zero;
        }

        // dir���� �ӵ��� 1���� Ŭ ���� dir���� ����ȭ ���ش�(GetAxis�� -1~0, 0~1 �� ���� �����ش�)
        if(dir.magnitude >= 1)
        {
            dir.Normalize();
        }

        // �Է°��� ������ ���ϸ��̼� float���� ��
        anim.SetFloat("Move", dir.magnitude);

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

        if(acceleVec.y > 0)
        {
            acceleVec = acceleVec * 0.98f;
        }

        // �����̰� �Ѵ�.
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
            if(pState != PlayerState.Be_Hit && pState != PlayerState.Diving)
            {
                // ������ ���� 0���� �����.
                acceleVec = Vector3.zero;
            }
            else
            {
                // ������ ���� ���װ��� �ش�(�ڽ��� ���� ���� ��ġ�� ���Ѵ�.)
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
            gameObject.SetActive(false);
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
        // ���⼭ ������ ����ϴµ� ��� �ؾ��ϳ�.....  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //acceleVec = dir * Time.deltaTime;  // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //yield return new WaitForSeconds(2.5f);
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Be_Hit"));
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f);
        pState = PlayerState.Normal;
    }
}
