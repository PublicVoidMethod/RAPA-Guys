using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomStart : MonoBehaviour
{
    public GameObject player;
    public GameObject[] startPoints = new GameObject[12];

    

    void Start()
    {
        int random = Random.Range(0, 12);
        player.transform.position = startPoints[random].transform.position;
        
        // ���߿� AI�� ����ٸ� �ߺ��� ��ġ�� ������ �ʵ��� ���������� ���� startPoints[]�� ����? �Ѵ�.
    }

    void Update()
    {
        //int random = Random.Range(0, 12);
        //if (player.transform.position.z < 25 && player.transform.position.y < -1.5f)
        //{
        //    player.transform.position = startPoints[random].transform.position;
        //}
    }
}
