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
        // 캐릭터콘트롤러 캐싱
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        InputMove();
        //PlayerDiving();
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

        dir.y = 0;

        // 3d모델의 회전  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
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

        // 점프를 하기 위해 스페이스바의 입력을 받는다면
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            // 위쪽 방향으로 점프력을 추가한다.
            yVelocity = jumpPower;
            jumpCount--;
        }
    }

    //void PlayerDiving()  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    //{
    //    // 왼쪽 컨트롤키를 누른다면
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
        }
    }
}
