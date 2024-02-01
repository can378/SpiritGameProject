using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�Ǹ��ϴ� ������ ������ ���� ��ũ��Ʈ

public class SellingItem : Merchant
{
    public List<GameObject> itemList;//�Ǹ� ������ ������ ����


    int itemIndex;
    string thisItemName;
    int thisItemPrice;

    UserData userData;


    private void Awake()
    {
        userData = DataManager.instance.userData;
        itemList = DataManager.instance.gameVar.itemList;
    }

    void Start()
    {
        setStore();
    }


    private void setStore()
    {
        itemIndex = Random.Range(0, itemList.Count);
        GameObject thisSlotItem = itemList[itemIndex];

        GetComponent<SpriteRenderer>().sprite = thisSlotItem.GetComponent<SpriteRenderer>().sprite;
        thisItemName=thisSlotItem.GetComponent<ItemStatus>().name;
        thisItemPrice = thisSlotItem.GetComponent<ItemStatus>().price;
        //itemName.text = thisSlotItem.GetComponent<ItemStatus>().itemName;
        //itemPrice.text = thisSlotItem.GetComponent<ItemStatus>().price.ToString();
    }



    public void BuyItem()
    {
        int cost = thisItemPrice;

        if (userData.coin >= cost)
        {
            userData.coin -= cost;

            MapUIManager.instance.UpdateCoinUI();
            MapUIManager.instance.updateItemUI(itemList[itemIndex]);

            Player.instance.playerItem = Instantiate(itemList[itemIndex]);
            Player.instance.playerItem.SetActive(false);

            this.gameObject.SetActive(false);
            //userData.playerItem = GameData.instance.itemList[2].name;
        }
        else
        {
            Debug.LogWarning("Not enough coins to buy the item!");
        }
    }
}
