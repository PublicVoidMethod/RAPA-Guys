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
        // ������Ʈ Ǯ�� �����.
        for (int i = 0; i < stonePool.Length; i++)
        {
            stonePool[i] = Instantiate(stone);
            stonePool[i].transform.parent = stonePocket;
            // ������� ������Ʈ�� ����
            stonePool[i].SetActive(false);
        }
        waitSeconds = new WaitForSeconds(createTime);
        StartCoroutine(StoneInstantiate());
    }

    //
    GameObject ReturnObject()
    {
        // null�� �Ǵ� ������ �ϳ� ����
        GameObject atciveObj = null;

        // stonePool�� ���ִ� GameObject���� �ݺ��Ѵ�.
        foreach(GameObject go in stonePool)
        {
            // ���� go�� ��Ȱ��ȭ���
            if (!go.activeSelf)
            {
                // atciveObj�� go�� �ְ�
                atciveObj = go;
                // ObjFalse()�� �ڷ�ƾ�� �����Ѵ�.
                StartCoroutine(ObjFalse(atciveObj));
                // ����ġ���� ���´�
                break;
            }
        }
        // atciveObj�� ��´�.
        return atciveObj;
    }
    
    IEnumerator ObjFalse(GameObject go)
    {
        // 5�ʸ� ��ٸ� ��
        yield return new WaitForSeconds(5.0f);
        // go�� ��Ȱ��ȭ �Ѵ�.
        go.SetActive(false);
    }

    IEnumerator StoneInstantiate()
    {
        // ���ѷ����� ���ž�
        while (true)
        {
            // ������ �����Ǵ� ��ġ�� Ư���Ѵ�.
            for (int i = 0; i < stonePoints.Length; i++)
            {
                // ������ ���� ���������� �޴´�
                stonePower = Random.Range(0, 10);
                // ������ �����Ǵ� �ð��� ���������� �޴´�
                makeItRandom = Random.Range(0, 1.5f);

                // ������Ʈ Ǯ�� ������Ʈ �ϳ��� �����´�.
                GameObject go = ReturnObject();
                // go�� null�� �ƴ϶��
                if (go != null)
                {
                    // go�� ������ ����� ���� �ο����ش�.
                    go.GetComponent<Rigidbody>().velocity = stonePoints[i].forward * stonePower;

                    // ������ ��ġ�� Ư���ϰ� ��� �ð��� �ش�
                    go.transform.position = stonePoints[i].position;
                    // go�� 
                    go.SetActive(true);
                    yield return new WaitForSeconds(makeItRandom);
                }
            }
                    
            yield return waitSeconds;
        }
    }

    #region �ν��Ͻÿ���Ʈ ���
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

    //            // ������ ��ġ�� Ư���ϰ� ��� �ð��� �ش�
    //            go.transform.position = stonePoints[i].position;

    //            yield return new WaitForSeconds(makeItRandom);
    //        }
    //        yield return waitSeconds;
    //    }
    //}
    #endregion
}
