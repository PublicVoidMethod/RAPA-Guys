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
}
