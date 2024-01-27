using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Objectmanager : MonoBehaviour
{
    //MerchantStore merchantStore;
    UserData userData;

    private void Start()
    {
        //merchantStore = FindObjectOfType<MerchantStore>();
        userData = FindObjectOfType<DataManager>().userData;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Item")
        {
            Item item = collision.GetComponent<Item>();
            
            if (item.itemClass == ItemClass.Coin)
            {
                Destroy(collision.gameObject); //코인 오브젝트 삭제
                userData.coin++;
                MapUIManager.instance.UpdateCoinUI();

                /*
                if (merchantStore != null)
                {
                    int itemCost = merchantStore.recovery;
                    BuyItem(itemCost);   // 코인 차감, 아이템 획득
                }
                else
                {
                    Debug.LogWarning("MerchantStore not found!");
                }
                */
            }

            if (item.itemClass == ItemClass.Key)
            {
                Destroy(collision.gameObject); //키 오브젝트 삭제
                userData.key++;
                MapUIManager.instance.UpdateKeyUI();
            }
        }
        
    }

    
}
