using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomStart : MonoBehaviour
{
    public static RandomStart instance;

    public GameObject player;
    public GameObject[] startPoints = new GameObject[12];

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //void Start()
    //{
    //    int random = Random.Range(0, 12);
    //    player.transform.position = startPoints[random].transform.position;
        
    //    // ���߿� AI�� ����ٸ� �ߺ��� ��ġ�� ������ �ʵ��� ���������� ���� startPoints[]�� ����? �Ѵ�.
    //}

    public void StartPositionSet(GameObject target)
    {
        int random = Random.Range(0, 12);
        target.transform.position = startPoints[random].transform.position;
    }
}
