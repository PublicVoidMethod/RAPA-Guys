using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetrunObstacle : MonoBehaviour
{
    public float progressTime = 2.5f;

    float elapsedTime;
    bool isPlay = true;

    public Transform startPos;
    public Transform endPos;

    void Start()
    {
        StartCoroutine(RoundTrip());
    }

    IEnumerator RoundTrip()
    {
        while (isPlay)
        {
            while(elapsedTime <= progressTime)
            {
                elapsedTime += Time.deltaTime;
                float percent = elapsedTime / progressTime;
                transform.position = Vector3.Lerp(startPos.position, endPos.position, percent);
                yield return new WaitForEndOfFrame();
            }
            
            elapsedTime = 0;
            //yield return new WaitForSeconds(0.3f);
            yield return null;

            while (elapsedTime <= progressTime)
            {
                elapsedTime += Time.deltaTime;
                float percent = elapsedTime / progressTime;
                transform.position = Vector3.Lerp(endPos.position, startPos.position, percent);
                yield return new WaitForEndOfFrame();
            }

            elapsedTime = 0;
            //yield return new WaitForSeconds(0.3f);
            yield return null;
        }
    }
}
