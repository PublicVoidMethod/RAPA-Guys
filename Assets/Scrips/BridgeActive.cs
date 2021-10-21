using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeActive : MonoBehaviour
{
    bool isPlay = true;

    GameObject go;

    void Start()
    {
        go = transform.GetChild(0).gameObject;
        StartCoroutine(BridgeONOFF(true));
    }

    IEnumerator BridgeONOFF(bool startValue)
    {
        while (isPlay)
        {
            go.SetActive(startValue);

            yield return new WaitForSeconds(2f);

            startValue = !startValue;
            //gameObject.SetActive(startValue);

            //yield return new WaitForSeconds(2f);
        }
    }
}
