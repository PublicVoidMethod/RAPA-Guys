using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoate : MonoBehaviour
{
    public float rotSpeed = 200.0f;

    void Start()
    {
        
    }

    void Update()
    {
        MouseToRotate();
    }

    void MouseToRotate()
    {
        // ���콺�� �巡�� ���� �Է��� �޴´�.
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        // �Է��� �������� ȸ���� ������ ����
        Vector3 dir = new Vector3(-y, x, 0);
        dir.Normalize();

        // ȸ�� �������� ȸ���Ѵ�.
        transform.eulerAngles += dir * rotSpeed * Time.deltaTime;
    }
}
