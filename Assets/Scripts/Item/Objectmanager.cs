using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Objectmanager : MonoBehaviour
{
    public TMP_Text coinText;
    public TMP_Text keyText;
    public int coinCount = 0; //초기 코인 개수
    public int keyCount = 0; //초기 키 개수

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            Destroy(collision.gameObject); //코인 오브젝트 삭제
            coinCount++; //코인 개수 증가
            coinText.text = coinCount.ToString(); //텍스트 변환

            MerchantStore merchantStore = FindObjectOfType<MerchantStore>();

            if (merchantStore != null)
            {
                int itemCost = merchantStore.recovery;

                // 코인을 차감하고 아이템을 얻는 동작 수행
                BuyItem(itemCost);
            }
            else
            {
                Debug.LogWarning("MerchantStore not found!");
            }
        }

        if (collision.gameObject.tag == "Key")
        {
            Destroy(collision.gameObject); //키 오브젝트 삭제
            keyCount++; //키 개수 증가
            keyText.text = keyCount.ToString(); //텍스트 변환
        }
    }

    public void BuyItem(int cost)
    {
        if(coinCount >= cost)
        {
            coinCount -= cost;
            coinText.text = coinCount.ToString();
        }
        else
        {
            // 추가: 충분한 코인이 없을 때 처리
            Debug.LogWarning("Not enough coins to buy the item!");
        }
    }
}
