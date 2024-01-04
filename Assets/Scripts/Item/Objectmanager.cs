using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Objectmanager : MonoBehaviour
{
    public TMP_Text coinText;
    public TMP_Text keyText;
    public int coinCount = 0; //�ʱ� ���� ����
    public int keyCount = 0; //�ʱ� Ű ����

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            Destroy(collision.gameObject); //���� ������Ʈ ����
            coinCount++; //���� ���� ����
            coinText.text = coinCount.ToString(); //�ؽ�Ʈ ��ȯ

            int itemCost = FindObjectOfType<MerchantStore>().recovery;

            // ������ �����ϰ� �������� ��� ���� ����
            BuyItem(itemCost);
        }

        if (collision.gameObject.tag == "Key")
        {
            Destroy(collision.gameObject); //Ű ������Ʈ ����
            keyCount++; //Ű ���� ����
            keyText.text = keyCount.ToString(); //�ؽ�Ʈ ��ȯ
        }
    }

    public void BuyItem(int cost)
    {
        if(coinCount >= cost)
        {
            coinCount -= cost;
            coinText.text = coinCount.ToString();
        }
    }
}
