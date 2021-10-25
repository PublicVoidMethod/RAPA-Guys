using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeActive : MonoBehaviour
{
    public float time = 2f;
    WaitForSeconds waitSencond;
    void Start()
    {
        waitSencond = new WaitForSeconds(time);
        for (int i = 0; i < transform.childCount; i++)
        {
           StartCoroutine(BridgeONOFF(transform.GetChild(i).gameObject));
        }
    }

    IEnumerator BridgeONOFF(GameObject obj)
    {
        while (true)
        {
            yield return waitSencond;
            //yield return new WaitForSeconds(2f);
            obj.SetActive(!obj.activeSelf);
        }
    }
}
