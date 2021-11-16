using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class AgentTest : Agent
{
    public enum PlayerState  // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    {
        Idle,
        Normal,
        Be_Hit,
        Diving
    }
    public PlayerState pState;

    public float playerSpeed = 2.0f;
    public float gravity = -10.0f;
    public float jumpPower = 2.5f;
    public float rotateSpeed = 10.0f;
    public float divingSpeed = 3.0f;
    public float hammerPower = 5.0f;
    
    float yVelocity = 0;
    int jumpCount = 1;
    int divingCount = 1;
    float lerpHorizontal;
    float lerpVertical;

    public Transform playerModel;
    public Transform divingPoint;
    
    public Vector3 acceleVec;


    CharacterController cc;
    Animator anim;
    
    Vector3 finalDir;

    public override void Initialize()
    {
        // ĳ������Ʈ�ѷ� ĳ��
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        pState = PlayerState.Idle;
        StartCoroutine(StayIdle());
        StartCoroutine(MoveCo());
    }

    public override void OnEpisodeBegin()
    {
        // ���� �������� ������ ���� ��ġ�� ��ġ��Ų��.
        RandomStart.instance.StartPositionSet(gameObject);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //AddReward(-1.0f / MaxStep);

        //if (transform.position.y < 5.5f) AddReward(-3.0f);

        // ����, �¿�, ����, ���̺��� �׼�Ű�� �ޱ�
        float horizontal = actions.DiscreteActions.Array[0] - 1;
        float vertical = actions.DiscreteActions.Array[1] - 1;
        int jump = actions.DiscreteActions.Array[2];
        int diving = actions.DiscreteActions.Array[3];

        lerpHorizontal = Mathf.Lerp(lerpHorizontal, horizontal, 10 * Time.deltaTime);
        lerpVertical = Mathf.Lerp(lerpVertical, vertical, 10 * Time.deltaTime);

        Vector3 dir = new Vector3(lerpHorizontal, 0, lerpVertical);

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

        PlayerJump(jump);

        // �߷� ���� �����Ѵ�.
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        if (acceleVec.y > 0)
        {
            acceleVec = acceleVec * 0.98f;
        }

        // �����̰� �Ѵ�.

        finalDir = dir;

        //transform.position += dir * playerSpeed * Time.deltaTime;

        if (diving != 0 && divingCount > 0)
        {
            divingCount--;
            jumpPower = 0;
            pState = PlayerState.Diving;
            anim.SetTrigger("Dive");
            StartCoroutine(DivingTime());
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        Vector3 dir;
        if (pState == PlayerState.Normal)
        {
            // �����¿��� �Է°��� �����
            float h = Input.GetAxis("Horizontal") + 1;
            float v = Input.GetAxis("Vertical") + 1;

            // �����¿��� ������ �־��ְ�
            dir = new Vector3(h - 1, 0, v - 1);

            actionsOut.DiscreteActions.Array[0] = (int)h;
            actionsOut.DiscreteActions.Array[1] = (int)v;
        }
        else
        {
            dir = Vector3.zero;
            actionsOut.DiscreteActions.Array[0] = 1;
            actionsOut.DiscreteActions.Array[1] = 1;
        }
        // dir���� �ӵ��� 1���� Ŭ ���� dir���� ����ȭ ���ش�(GetAxis�� -1~0, 0~1 �� ���� �����ش�)
        if (dir.magnitude >= 1)
        {
            dir.Normalize();
        }
        if (Input.GetButtonDown("Jump"))
        {
            actionsOut.DiscreteActions.Array[2] = 1;
        }
        else
        {
            actionsOut.DiscreteActions.Array[2] = 0;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            actionsOut.DiscreteActions.Array[3] = 1;
        }
        else
        {
            actionsOut.DiscreteActions.Array[3] = 0;
        }
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

        //InputMove();
    }


    IEnumerator MoveCo()
    {
        while(true)
        {
            cc.Move((finalDir * playerSpeed * Time.deltaTime) + acceleVec);

            yield return null;
        }
    }

    private void Idle()  // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    {

    }

    IEnumerator StayIdle()
    {
        yield return new WaitForSeconds(3f);
        pState = PlayerState.Normal;

    }

    #region �̵��Լ�
    //void InputMove()
    //{
    //    Vector3 dir;
    //    if (pState == PlayerState.Normal)
    //    {
    //        // �����¿��� �Է°��� �����
    //        float h = Input.GetAxis("Horizontal") + 1;
    //        float v = Input.GetAxis("Vertical") + 1;

    //        // �����¿��� ������ �־��ְ�
    //        dir = new Vector3(h - 1, 0, v - 1);
    //    }
    //    else
    //    {
    //        dir = Vector3.zero;
    //    }

    //    // dir���� �ӵ��� 1���� Ŭ ���� dir���� ����ȭ ���ش�(GetAxis�� -1~0, 0~1 �� ���� �����ش�)
    //    if (dir.magnitude >= 1)
    //    {
    //        dir.Normalize();
    //    }

    //    // �Է°��� ������ ���ϸ��̼� float���� ��
    //    anim.SetFloat("Move", dir.magnitude);

    //    // ���� ���͸� ī�޶��� ������ �������� ���� �Ѵ�.
    //    dir = Camera.main.transform.TransformDirection(dir);

    //    dir.y = 0;

    //    // 3d���� ȸ��  ???????????????????????????????????????????????????
    //    //playerModel.forward = Vector3.Lerp(playerModel.forward, dir, 0.5f);
    //    // ������ ���̰��� ���� ��(0���� Ŭ ��)
    //    if (dir.sqrMagnitude != 0)
    //    {
    //        // ���� ȸ���� = ���� ������ ȸ����, �ٶ󺸰��� �ϴ� ȸ����
    //        playerModel.rotation = Quaternion.Lerp(playerModel.rotation, Quaternion.LookRotation(dir),
    //            rotateSpeed * Time.deltaTime);
    //    }

    //    PlayerJump();

    //    // �߷� ���� �����Ѵ�.
    //    yVelocity += gravity * Time.deltaTime;
    //    dir.y = yVelocity;

    //    if (acceleVec.y > 0)
    //    {
    //        acceleVec = acceleVec * 0.98f;
    //    }

    //    // �����̰� �Ѵ�.
    //    cc.Move((dir * playerSpeed * Time.deltaTime) + acceleVec);
    //    //transform.position += dir * playerSpeed * Time.deltaTime;

    //    if (Input.GetKeyDown(KeyCode.LeftControl) && divingCount > 0)
    //    {
    //        divingCount--;
    //        jumpPower = 0;
    //        pState = PlayerState.Diving;
    //        anim.SetTrigger("Dive");
    //        StartCoroutine(DivingTime());
    //    }
    //}
    #endregion

    void PlayerJump(int jumpTrigger)
    {
        if (cc.collisionFlags == CollisionFlags.Below)
        {
            jumpCount = 1;
            yVelocity = 0;
        }

        // ������ �ϱ� ���� �����̽����� �Է��� �޴´ٸ�
        //if (Input.GetButtonDown("Jump") && jumpCount > 0)
        if(jumpTrigger != 0 && jumpCount > 0)
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
            if (pState != PlayerState.Be_Hit && pState != PlayerState.Diving)
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
            //AddReward(-10);

            Vector3 dir = transform.position - other.transform.position;

            if (pState == PlayerState.Normal) StartCoroutine(FallDown(dir));
        }

        else if (other.gameObject.CompareTag("Hammer"))
        {
            //AddReward(-10);

            Vector3 dir = transform.position - other.transform.position;
            //Vector3 dir = Vector3.Lerp(transform.position, other.transform.position, hammerPower * Time.deltaTime); // @@@@@@@@@@@@@@

            anim.SetTrigger("Be_Hit");

            if (pState == PlayerState.Normal) StartCoroutine(HammerSmite(dir));
        }

        else if (other.gameObject.CompareTag("Pendulum"))
        {
            //AddReward(-10);

            Vector3 dir = transform.position - other.transform.position;
            //Vector3 dir = Vector3.Lerp(transform.position, other.transform.position, hammerPower * Time.deltaTime); // @@@@@@@@@@@@@@

            dir.y = 0.002f;
            dir = dir * 2f;

            anim.SetTrigger("Be_Hit");

            if (pState == PlayerState.Normal) StartCoroutine(HammerSmite(dir));
        }

        else if (other.gameObject.CompareTag("FinishLine"))
        {
            //AddReward(100);

            //gameObject.SetActive(false);
            // �ٽ� ��߼����� �Ű��ش�.
            //RandomStart.instance.StartPositionSet(gameObject);
            EndEpisode();
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
        acceleVec = (dir + Vector3.up * 0.8f) * hammerPower * Time.deltaTime;
        // ���⼭ ������ ����ϴµ� ��� �ؾ��ϳ�.....  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //acceleVec = dir * Time.deltaTime;  // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //yield return new WaitForSeconds(2.5f);
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Be_Hit"));
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f);
        pState = PlayerState.Normal;
    }
}
