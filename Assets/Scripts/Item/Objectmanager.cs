using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Objectmanager : MonoBehaviour
{
    MerchantStore merchantStore;
    UserData userData;

    private void Start()
    {
        merchantStore = FindObjectOfType<MerchantStore>();
        userData = FindObjectOfType<DataManager>().userData;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            Destroy(collision.gameObject); //코인 오브젝트 삭제
            userData.coin++;
            UIManager.instance.UpdateCoinUI();

            if (merchantStore != null)
            {
                int itemCost = merchantStore.recovery;
                BuyItem(itemCost);   // 코인 차감, 아이템 획득
            }
            else
            {
                Debug.LogWarning("MerchantStore not found!");
            }
        }

        if (collision.gameObject.tag == "Key")
        {
            Destroy(collision.gameObject); //키 오브젝트 삭제
            userData.key++;
            UIManager.instance.UpdateKeyUI();
        }
    }

    public void BuyItem(int cost)
    {
        if (userData.coin >= cost)
        {
            userData.coin -= cost;
            UIManager.instance.UpdateCoinUI();
        }
        else
        {
            // 추가: 충분한 코인이 없을 때 처리
            Debug.LogWarning("Not enough coins to buy the item!");
        }
    }
}
