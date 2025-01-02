using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Merchant : MonoBehaviour
{

    
    public GameObject checkTemp;

    

    

   

    IEnumerator storeActivate()
    {
        while(true) 
        {
            GameObject touchObj= GameManager.instance.touchedObject;

            if (touchObj!=null&&touchObj.tag == "SellingItem")
            {
                print(touchObj.GetComponent<SellingItem>().thisSelectItem);
            }
            yield return null;
        }
        
    }



    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            checkTemp.SetActive(true);
            Player.instance.playerStatus.isAttackable = false;
            StartCoroutine(storeActivate());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            checkTemp.SetActive(false);
            Player.instance.playerStatus.isAttackable = true;
            StopAllCoroutines();
        }
    }






}
