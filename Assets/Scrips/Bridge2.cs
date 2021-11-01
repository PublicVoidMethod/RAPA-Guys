using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge2 : MonoBehaviour
{
    public float progressTime = 3.0f;

    float elapsedTime = 0;

    Vector3 startPos;
    Vector3 endPos;

    void Start()
    {
        startPos = transform.position;
        endPos = transform.position + new Vector3(0, 0, 10.3f);
        StartCoroutine(BridgeRepetition());
    }

    IEnumerator BridgeRepetition()
    {
        while (true)
        {
            while (elapsedTime <= progressTime)
            {
                elapsedTime += Time.deltaTime;
                float percent = elapsedTime / progressTime;

                transform.position = Vector3.Lerp(startPos, endPos, percent);
                yield return new WaitForEndOfFrame();
            }
            elapsedTime = 0;
            yield return new WaitForSeconds(2.9f);

            while (elapsedTime <= progressTime)
            {
                elapsedTime += Time.deltaTime;
                float percent = elapsedTime / progressTime;

                transform.position = Vector3.Lerp(endPos, startPos, percent);
                yield return new WaitForEndOfFrame();
            }
            elapsedTime = 0;
            yield return new WaitForSeconds(0.9f);
        }
    }

    void Update()
    {

    }
}
