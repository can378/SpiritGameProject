using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class MerchantStore : MonoBehaviour
{
    public int recovery = 1;
    public TMP_Text recoveryCoin;

    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(BuyItem);
    }

    public void BuyItem()
    {
        int currentCoins = int.Parse(recoveryCoin.text);

        Objectmanager objectManager = FindObjectOfType<Objectmanager>();

        if (objectManager != null && currentCoins >= recovery)
        {
            currentCoins -= recovery;
            recoveryCoin.text = currentCoins.ToString();

            // Objectmanager�� ���� ������ ���ҽ�Ű�� ���� ����
            objectManager.BuyItem(recovery);
        }
    }
}
