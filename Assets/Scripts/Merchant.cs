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

            yield return null;
        }
        
    }



    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            checkTemp.SetActive(true);
            Player.instance.isAttackable = false;
            //StartCoroutine(storeActivate());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            checkTemp.SetActive(false);
            Player.instance.isAttackable = true;
            //StopAllCoroutines();
        }
    }






}
