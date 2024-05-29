using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

//판매하는 아이템 각각에 들어가는 스크립트

public class SellingItem : MonoBehaviour
{

    public GameObject info;
    public TMP_Text itemName;
    public TMP_Text itemPrice;
    
    private List<GameObject> itemList;//판매 가능한 아이템 종류


    public int thisItemIndex;
    string thisItemName;
    int thisItemID;
    int thisItemPrice;


    UserData userData;

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        userData = DataManager.instance.userData;
        itemList = DataManager.instance.gameData.selectItemList;
    }

    void Start()
    {
        setStore();

    }


    private void setStore()
    {
        thisItemIndex = Random.Range(1, itemList.Count);
        GameObject thisSlotItem = itemList[thisItemIndex];

        //GameObject thisSlotImage = Instantiate(thisSlotItem);
        //GetComponent<SpriteRenderer>().sprite = thisSlotItem.GetComponentInChildren<SpriteRenderer>().sprite;
        thisItemName = thisSlotItem.GetComponent<Consumable>().selectItemName;
        thisItemID = thisSlotItem.GetComponent<Consumable>().selectItemID;
        thisItemPrice = thisSlotItem.GetComponent<Consumable>().price;
        
        
        itemName.text = thisItemName.ToString();
        itemPrice.text = thisItemPrice.ToString();
    }



    public void BuyItem()
    {
        int cost = thisItemPrice;

        if (Player.instance.playerStats.coin >= cost)
        {
            Player.instance.playerStats.coin -= cost;
            Player.instance.playerStats.item = thisItemID;

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
        if (collision.tag == "Player")
        {
            //check buying
            StartCoroutine(checkBuying());

            //show information
            info.SetActive(true);

            
        }
    }

    private void OnTriggerExit2D(Collider2D collision) 
    { 
        if(collision.tag=="Player")
        {
            print("stop check buying");
            StopAllCoroutines();
            info.SetActive(false);
        }
        
    }


    IEnumerator checkBuying() 
    {
        while (true) 
        {
            print("checkBuying");
            if (Input.GetKeyDown(KeyCode.F))
            {  
                BuyItem(); 
                break; 
            }
            yield return null;
        }

    }
    
}
