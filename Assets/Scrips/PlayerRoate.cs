using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoate : MonoBehaviour
{
    public float rotSpeed = 200.0f;

    public Transform cameraArm;

    void Start()
    {
        
    }

    void Update()
    {
        MouseToRotate();
    }

    void MouseToRotate()
    {
        #region FPS
        //// 마우스의 드래그 방향 입력을 받는다.
        //float x = Input.GetAxis("Mouse X");
        //float y = Input.GetAxis("Mouse Y");

        //// 입력을 기준으로 회전의 방향을 설정
        //Vector3 dir = new Vector3(-y, x, 0);
        //dir.Normalize();

        //// 회전 방향으로 회전한다.
        //transform.eulerAngles += dir * rotSpeed * Time.deltaTime;
        #endregion
        #region TPS 인터넷
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = (camAngle.x - mouseDelta.y);

        if (x < 100f)
        {
            x = Mathf.Clamp(x, -1f, 15f);
        }
        else
        {
            x = Mathf.Clamp(x, 359f, 361f);
        }
        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x * rotSpeed, camAngle.z);
        #endregion
    }
}
