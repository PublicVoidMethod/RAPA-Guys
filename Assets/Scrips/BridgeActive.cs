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
        //go = transform.GetChild(1).gameObject;
        //StartCoroutine(BridgeONOFF(false));
        //go = transform.GetChild(2).gameObject;
        //StartCoroutine(BridgeONOFF(true));
        //go = transform.GetChild(3).gameObject;
        //StartCoroutine(BridgeONOFF(false));

        //if(go == transform.GetChild(0).gameObject && go == transform.GetChild(1))
        //{
        //    StartCoroutine(BridgeONOFF(true));
        //}
        //else
        //{
        //    StartCoroutine(BridgeONOFF(false));
        //}
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
