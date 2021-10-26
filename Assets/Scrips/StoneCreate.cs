using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneCreate : MonoBehaviour
{
    public float createTime = 3.0f;
    [Range(0, 1.0f)]
    public float makeItRandom;
    
    int stonePower;

    public GameObject[] stonePool = new GameObject[10];
    public Transform[] stonePoints = new Transform[3];
    public GameObject stone;
    public Transform stonePocket;

    WaitForSeconds waitSeconds;

    void Start()
    {
        // 오브젝트 풀을 만든다.
        for (int i = 0; i < stonePool.Length; i++)
        {
            stonePool[i] = Instantiate(stone);
            stonePool[i].transform.parent = stonePocket;
            // 만들어진 오브젝트를 껐어
            stonePool[i].SetActive(false);
        }
        waitSeconds = new WaitForSeconds(createTime);
        StartCoroutine(StoneInstantiate());
    }

    //
    GameObject ReturnObject()
    {
        // null이 되는 변수를 하나 선언
        GameObject atciveObj = null;

        // stonePool의 들어가있는 GameObject들을 반복한다.
        foreach(GameObject go in stonePool)
        {
            // 만약 go가 비활성화라면
            if (!go.activeSelf)
            {
                // atciveObj에 go를 넣고
                atciveObj = go;
                // ObjFalse()의 코루틴을 시작한다.
                StartCoroutine(ObjFalse(atciveObj));
                // 포이치문을 나온다
                break;
            }
        }
        // atciveObj에 담는다.
        return atciveObj;
    }
    
    IEnumerator ObjFalse(GameObject go)
    {
        // 5초를 기다린 후
        yield return new WaitForSeconds(5.0f);
        // go를 비활성화 한다.
        go.SetActive(false);
    }

    IEnumerator StoneInstantiate()
    {
        // 무한루프를 돌거야
        while (true)
        {
            // 스톤이 생성되는 위치를 특정한다.
            for (int i = 0; i < stonePoints.Length; i++)
            {
                // 스톤의 힘을 랜덤값으로 받는다
                stonePower = Random.Range(0, 10);
                // 스톤의 생성되는 시간을 랜덤값으로 받는다
                makeItRandom = Random.Range(0, 1.5f);

                // 오브젝트 풀의 오브젝트 하나를 가져온다.
                GameObject go = ReturnObject();
                // go가 null이 아니라면
                if (go != null)
                {
                    // go의 포워드 방향과 힘을 부여해준다.
                    go.GetComponent<Rigidbody>().velocity = stonePoints[i].forward * stonePower;

                    // 스톤의 위치를 특정하고 대기 시간을 준다
                    go.transform.position = stonePoints[i].position;
                    // go를 
                    go.SetActive(true);
                    yield return new WaitForSeconds(makeItRandom);
                }
            }
                    
            yield return waitSeconds;
        }
    }

    #region 인스턴시에이트 방식
    //IEnumerator StoneInstantiate()
    //{
    //    while (true)
    //    {
    //        for (int i = 0; i < stonePoints.Length; i++)
    //        {
    //            stonePower = Random.Range(0, 10);
    //            makeItRandom = Random.Range(0, 1.5f);

    //            GameObject go = Instantiate(stone);
    //            go.GetComponent<Rigidbody>().velocity = stonePoints[i].forward * stonePower;

    //            // 스톤의 위치를 특정하고 대기 시간을 준다
    //            go.transform.position = stonePoints[i].position;

    //            yield return new WaitForSeconds(makeItRandom);
    //        }
    //        yield return waitSeconds;
    //    }
    //}
    #endregion
}
