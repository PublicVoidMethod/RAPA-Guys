using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObstacle : MonoBehaviour
{
    public float rotateSpeed = 10.0f;

    GameObject go;

    void Start()
    {
        go = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        ObstacleRotate();
    }

    void ObstacleRotate()
    {
        go.transform.Rotate(new Vector3(0, 1, 0) * rotateSpeed * Time.deltaTime);
    }
}
