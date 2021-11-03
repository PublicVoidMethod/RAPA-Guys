using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickRotate : MonoBehaviour
{
    public float stickSpeed = 5.0f;

    void Update()
    {
        transform.Rotate(Vector3.up * stickSpeed * Time.deltaTime);
    }
}
