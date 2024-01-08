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
            Destroy(collision.gameObject); //���� ������Ʈ ����
            userData.coin++;
            UIManager.instance.UpdateCoinUI();

            if (merchantStore != null)
            {
                int itemCost = merchantStore.recovery;
                BuyItem(itemCost);   // ���� ����, ������ ȹ��
            }
            else
            {
                Debug.LogWarning("MerchantStore not found!");
            }
        }

        if (collision.gameObject.tag == "Key")
        {
            Destroy(collision.gameObject); //Ű ������Ʈ ����
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
            // �߰�: ����� ������ ���� �� ó��
            Debug.LogWarning("Not enough coins to buy the item!");
        }
    }
}
