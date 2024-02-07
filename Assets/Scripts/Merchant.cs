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
                int itemI = touchObj.GetComponent<SellingItem>().thisItemIndex;

                print(DataManager.instance.gameData.selectItemList[itemI].name);
            }
            yield return null;
        }
        
    }



    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            checkTemp.SetActive(true);
            Player.instance.status.isAttackable = false;
            StartCoroutine(storeActivate());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            checkTemp.SetActive(false);
            Player.instance.status.isAttackable = true;
            StopAllCoroutines();
        }
    }






}
