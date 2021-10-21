using System.Collections;
using UnityEngine;

public class WallMove : MonoBehaviour
{
    public float progressTime = 2f;

    float elapsedTime;
    bool isPlay = true;

    Vector3 startPos;
    Vector3 endPos;

    void Start()
    {
        startPos = transform.position;
        endPos = transform.position + new Vector3(0, -0.801f, 0);
        StartCoroutine(WallUpandDown());
    }

    IEnumerator WallUpandDown()
    {
        while(isPlay)
        {
            while(elapsedTime <= progressTime)  //  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            {
                elapsedTime += Time.deltaTime;
                float percent = elapsedTime / progressTime;

                transform.position = Vector3.Lerp(startPos, endPos, percent);
                yield return new WaitForEndOfFrame();
                // 한 프레임을 기다린 이유는 Lerp의 진행과정을 그리기 위한 작업이다.
                // 만약 한 프레임을 기다리지 않는다면 Lerp의 진행과정이 한프레임에 모두 반복되므로
                // 순간이동 한 것처럼 보이게 된다.
            }
            elapsedTime = 0;
            yield return new WaitForSeconds(1f);

            while(elapsedTime <= progressTime)
            {
                elapsedTime += Time.deltaTime;
                float percent = elapsedTime / progressTime;

                transform.position = Vector3.Lerp(endPos, startPos, percent);
                yield return new WaitForEndOfFrame();
            }
            elapsedTime = 0;
            yield return new WaitForSeconds(1f);
        }
    }
}
