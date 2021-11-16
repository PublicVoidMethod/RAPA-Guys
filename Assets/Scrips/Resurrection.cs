using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resurrection : MonoBehaviour
{
    public Vector3 savePoint;

    //PlayerMove pm;
    AgentTest at;

    void Start()
    {
        savePoint = GameObject.Find("StartingPoints").transform.position;
        // pm = GetComponent<PlayerMove>();
        at = GetComponent<AgentTest>();
    }

    void Update()
    {
        if(transform.position.y < -5.5f)
        {
            float randomX = Random.Range(-5.5f, 5.5f);
            transform.position = savePoint + new Vector3(randomX, 0, 0);

            //pm.StartFallDown(Vector3.zero);
            at.StartFallDown(Vector3.zero);

            //transform.position = go.GetComponent<RandomStart>().startPoints[random].transform.position;
        }
    }

    IEnumerator RewurrectoinWaiting()
    {
        yield return new WaitForSeconds(0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SavePoint"))
        {
            savePoint = other.transform.position;
        }
    }
}
