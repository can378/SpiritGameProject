using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//판매하는 아이템 각각에 들어가는 스크립트

public class SellingItem : MonoBehaviour
{
    private List<GameObject> itemList;//판매 가능한 아이템 종류


    public int thisItemIndex;
    string thisItemName;
    int thisItemPrice;

    UserData userData;

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        userData = DataManager.instance.userData;
        itemList = DataManager.instance.gameVar.itemList;
    }

    void Start()
    {
        setStore();
    }


    private void setStore()
    {
        thisItemIndex = Random.Range(0, itemList.Count);
        GameObject thisSlotItem = itemList[thisItemIndex];

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
            userData.playerItem = thisItemName;

            //전에 가지고 있던 아이템 드랍
            if (Player.instance.playerItem != null)
            { 
                Player.instance.playerItem.SetActive(true); 
                Player.instance.playerItem.transform.position = Player.instance.transform.position; 
            }

            MapUIManager.instance.UpdateCoinUI();
            MapUIManager.instance.updateItemUI(itemList[thisItemIndex]);

            Player.instance.playerItem = Instantiate(itemList[thisItemIndex]);
            Player.instance.playerItem.SetActive(false);

            this.gameObject.SetActive(false);
            
        }
        else
        {
            Debug.LogWarning("Not enough coins to buy the item!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //show information
        print("name="+thisItemName + " /price="+thisItemPrice);
        //check buying
        StartCoroutine(checkBuying());
    }

    private void OnTriggerExit2D(Collider2D collision) 
    { StopCoroutine(checkBuying()); }


    IEnumerator checkBuying() 
    {
        while (true) 
        { 
            if (Input.GetKeyDown(KeyCode.F))
            {  BuyItem();  }
            yield return null;
        }

    }
    
}
