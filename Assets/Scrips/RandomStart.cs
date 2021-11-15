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
        
    //    // 나중에 AI가 생긴다면 중복된 위치에 생기지 않도록 랜덤값으로 생긴 startPoints[]을 삭제? 한다.
    //}

    public void StartPositionSet(GameObject target)
    {
        int random = Random.Range(0, 12);
        target.transform.position = startPoints[random].transform.position;
    }
}
