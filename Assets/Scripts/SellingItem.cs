using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

//�Ǹ��ϴ� ������ ������ ���� ��ũ��Ʈ

public class SellingItem : MonoBehaviour
{

    public GameObject info;
    public TMP_Text itemName;
    public TMP_Text itemPrice;
    
    private List<GameObject> itemList;//�Ǹ� ������ ������ ����


    public int thisItemIndex;
    string thisItemName;
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
        thisItemIndex = Random.Range(0, itemList.Count);
        GameObject thisSlotItem = itemList[thisItemIndex];

        GetComponent<SpriteRenderer>().sprite = thisSlotItem.GetComponent<SpriteRenderer>().sprite;
        thisItemName=thisSlotItem.GetComponent<ItemInfo>().selectItemName.ToString();
        thisItemPrice = thisSlotItem.GetComponent<ItemInfo>().price;
        
        
        itemName.text = thisItemName;
        itemPrice.text = thisItemPrice.ToString();
    }



    public void BuyItem()
    {
        int cost = thisItemPrice;

        if (Player.instance.playerStats.coin >= cost)
        {
            Player.instance.playerStats.coin -= cost;
            Player.instance.playerStats.item = thisItemName;

            //���� ������ �ִ� ������ ���
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
