using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumSwing : MonoBehaviour
{
    public float swingSpeed = 10.0f;
    public float limit = 65.0f;
    public float randomTime;

    void Start()
    {
        randomTime = Random.Range(0, 6f);
        //randomTime = Mathf.PI;
    }

    void Update()
    {
        float angle = Mathf.Sin(Time.time * swingSpeed + randomTime) * limit;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
