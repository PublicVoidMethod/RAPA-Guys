using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHammer : MonoBehaviour
{
    public float hammerSpeed = 10.0f;

    void Update()
    {
        HammerRotate();
    }

    void HammerRotate()
    {
        transform.Rotate(new Vector3(-1, 0, 0) * hammerSpeed * Time.deltaTime);
    }
}
