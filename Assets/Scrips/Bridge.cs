using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    public float progressTime = 3.0f;
    public float waitTime = 2.0f;

    float elapsedTime = 0;

    Vector3 startPos;
    Vector3 endPos;

    void Start()
    {
        startPos = transform.position;
        endPos = transform.position + new Vector3(0, 0, -10.3f);
        StartCoroutine(BridgeRepetition());
    }

    IEnumerator BridgeRepetition()
    {
        yield return new WaitForSeconds(waitTime);

        while (true)
        {
            while (elapsedTime <= progressTime)
            {
                elapsedTime += Time.deltaTime;
                float percent = elapsedTime / progressTime;

                transform.position = Vector3.Lerp(startPos, endPos, percent);
                yield return null;
            }
            yield return new WaitForSeconds(1f - elapsedTime);
            elapsedTime = 0;

            while (elapsedTime <= progressTime)
            {
                elapsedTime += Time.deltaTime;
                float percent = elapsedTime / progressTime;

                transform.position = Vector3.Lerp(endPos, startPos, percent);
                yield return null;
            }
            yield return new WaitForSeconds(3f - elapsedTime);
            elapsedTime = 0;
        }
    }
}
