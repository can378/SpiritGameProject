using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

//판매하는 아이템 각각에 들어가는 스크립트

public class SellingItem : MonoBehaviour
{
    [field: SerializeField] int[] TypeWeight = new int[(int)SelectItemType.END];

    [field: SerializeField] int[] RatingWeight = new int[(int)SelectItemRating.END];

    public GameObject info;
    public TMP_Text itemName;
    public TMP_Text itemPrice;
    public GameObject spriteR;
    

    private List<GameObject> itemList;//판매 가능한 아이템 종류


    public int thisItemIndex;
    string thisItemName;
    int thisItemID;
    int thisItemPrice;


    UserData userData;

    

    private void Awake()
    {
        userData = DataManager.instance.userData;
        itemList = DataManager.instance.gameData.selectItemList;
    }

    void Start()
    {
        setStore();

    }


    private void setStore()
    {
        // ====================================
        // 유형
        // new로 생성한거 따로 지워야하나?
        WeightRandom<SelectItemType> typeRandom = new WeightRandom<SelectItemType>();

        // 설정한 가중치를 가중치무작위에 넣는다.
        for (int i = 0; i < (int)SelectItemType.END; ++i)
        {
            typeRandom.Add((SelectItemType)i, TypeWeight[i]);
        }

        SelectItemType ItemType = typeRandom.GetRandomItem();

        // ====================================
        // 등급
        // new로 생성한거 따로 지워야하나?
        WeightRandom<SelectItemRating> weightRandom = new WeightRandom<SelectItemRating>();

        // 설정한 가중치를 가중치무작위에 넣는다.
        for (int i = 0; i < (int)SelectItemRating.END; ++i)
        {
            weightRandom.Add((SelectItemRating)i, RatingWeight[i]);
        }

        SelectItemRating ItemRating = weightRandom.GetRandomItem();

        // 조건을 담은 Dictionary 생성
        Dictionary<string,int> ItemCondition = new Dictionary<string, int>();
        ItemCondition.Add("selectItemType", (int)ItemType);
        ItemCondition.Add("selectItemRating", (int)ItemRating);
       
        // GameData에서 해당 조건에 맞는 무작위 아이템 반환
        GameObject thisSlotItem = GameData.instance.DrawRandomItem(ItemCondition).GetComponent<GameObject>();

        //GameObject thisSlotImage = Instantiate(thisSlotItem);
        spriteR.GetComponent<SpriteRenderer>().sprite = thisSlotItem.GetComponent<SpriteRenderer>().sprite;
        spriteR.transform.localScale = thisSlotItem.transform.localScale;

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
            //useCoin
            Player.instance.playerStats.coin -= cost;

            /*
            //전에 가지고 있던 아이템 드랍
            if (Player.instance.playerItem != null)
            { 
                Player.instance.playerItem.SetActive(true); 
                Player.instance.playerItem.transform.position = Player.instance.transform.position; 
            }

            MapUIManager.instance.updateItemUI(itemList[thisItemIndex]);
            Player.instance.playerItem = Instantiate(itemList[thisItemIndex]);
            Player.instance.playerItem.SetActive(false);
            */

            Player.instance.playerStats.item = thisItemID;

            //UseItem
            switch (itemList[thisItemIndex].GetComponent<SelectItem>().selectItemType)
            {
                case SelectItemType.Consumable:
                    itemList[thisItemIndex].GetComponent<HPPortion>().
                        UseItem(FindObj.instance.Player.GetComponent<Player>());
                    break;
                default: break;
            }

            //set active false
            gameObject.SetActive(false);
            
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
