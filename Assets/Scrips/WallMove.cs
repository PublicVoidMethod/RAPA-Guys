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
                // �� �������� ��ٸ� ������ Lerp�� ��������� �׸��� ���� �۾��̴�.
                // ���� �� �������� ��ٸ��� �ʴ´ٸ� Lerp�� ��������� �������ӿ� ��� �ݺ��ǹǷ�
                // �����̵� �� ��ó�� ���̰� �ȴ�.
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
