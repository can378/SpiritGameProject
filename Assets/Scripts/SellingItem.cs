using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

//�Ǹ��ϴ� ������ ������ ����? ��ũ��Ʈ

public class SellingItem : MonoBehaviour, Interactable
{
    [field: SerializeField] int[] TypeWeight = new int[(int)SelectItemType.END];

    [field: SerializeField] int[] RatingWeight = new int[(int)SelectItemRating.END];

    public event System.Action InteractEvent;


    public GameObject info;
    //public TMP_Text itemName;
    public TMP_Text itemPrice;

    [field: SerializeField] public SelectItem thisSelectItem {get; private set;}

    void Start()
    {
        SetStore();
    }
    
    private void SetStore()
    {
        // ====================================
        // ����
        // new�� �����Ѱ� ���� �������ϳ�?
        WeightRandom<SelectItemType> typeRandom = new WeightRandom<SelectItemType>();

        // ������ ����ġ�� ����ġ�������� �ִ´�.
        for (int i = 0; i < (int)SelectItemType.END; ++i)
        {
            typeRandom.Add((SelectItemType)i, TypeWeight[i]);
        }

        SelectItemType ItemType = typeRandom.GetRandomItem();

        // ====================================
        // ���?
        // new�� �����Ѱ� ���� �������ϳ�?
        WeightRandom<SelectItemRating> weightRandom = new WeightRandom<SelectItemRating>();

        // ������ ����ġ�� ����ġ�������� �ִ´�.
        for (int i = 0; i < (int)SelectItemRating.END; ++i)
        {
            weightRandom.Add((SelectItemRating)i, RatingWeight[i]);
        }

        SelectItemRating ItemRating = weightRandom.GetRandomItem();

        // ������ ���� Dictionary ����
        Dictionary<string, int> ItemCondition = new Dictionary<string, int>();
        ItemCondition.Add("selectItemType", (int)ItemType);
        ItemCondition.Add("selectItemRating", (int)ItemRating);

        // GameData���� �ش� ���ǿ� �´� ������ ������ ��ȯ
        GameObject thisSlotItem = Instantiate(GameData.instance.DrawRandomItem(ItemCondition), transform.position, Quaternion.identity);
        thisSlotItem.transform.SetParent(this.gameObject.transform);
        thisSlotItem.GetComponent<Collider2D>().enabled = false;

        thisSelectItem = thisSlotItem.GetComponent<SelectItem>();

        //itemName.text = thisSelectItem.itemInstance.itemData.selectItemName;
        itemPrice.text = thisSelectItem.itemInstance.itemData.price.ToString();
    }

    public string GetInteractText()
    {
        return "�����ϱ�";
    }

    public void Interact()
    {
        BuyItem();
        InteractEvent?.Invoke();
    }

    public void BuyItem()
    {
        int cost = thisSelectItem.itemInstance.itemData.price;

        if (Player.instance.playerStats.coin >= cost)
        {
            //useCoin
            Player.instance.playerStats.coin -= cost;
            Player.instance.GainSelectItem(thisSelectItem);

            //set active false
            gameObject.SetActive(false);
            thisSelectItem = null;

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
            //show information
            MapUIManager.instance.toolTipPanel.GetComponent<ToolTipUI>().OpenToolTipUI(thisSelectItem.itemInstance);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            MapUIManager.instance.toolTipPanel.GetComponent<ToolTipUI>().CloseToolTipUI();
        }

    }

    /*
    [field: SerializeField] int[] TypeWeight = new int[(int)SelectItemType.END];

    [field: SerializeField] int[] RatingWeight = new int[(int)SelectItemRating.END];

    public GameObject info;
    public TMP_Text itemName;
    public TMP_Text itemPrice;
    public GameObject spriteR;
    

    private List<GameObject> itemList;//�Ǹ� ������ ������ ����


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
        // ����
        // new�� �����Ѱ� ���� �������ϳ�?
        WeightRandom<SelectItemType> typeRandom = new WeightRandom<SelectItemType>();

        // ������ ����ġ�� ����ġ�������� �ִ´�.
        for (int i = 0; i < (int)SelectItemType.END; ++i)
        {
            typeRandom.Add((SelectItemType)i, TypeWeight[i]);
        }

        SelectItemType ItemType = typeRandom.GetRandomItem();

        // ====================================
        // ���?
        // new�� �����Ѱ� ���� �������ϳ�?
        WeightRandom<SelectItemRating> weightRandom = new WeightRandom<SelectItemRating>();

        // ������ ����ġ�� ����ġ�������� �ִ´�.
        for (int i = 0; i < (int)SelectItemRating.END; ++i)
        {
            weightRandom.Add((SelectItemRating)i, RatingWeight[i]);
        }

        SelectItemRating ItemRating = weightRandom.GetRandomItem();

        // ������ ���� Dictionary ����
        Dictionary<string,int> ItemCondition = new Dictionary<string, int>();
        ItemCondition.Add("selectItemType", (int)ItemType);
        ItemCondition.Add("selectItemRating", (int)ItemRating);
       
        // GameData���� �ش� ���ǿ� �´� ������ ������ ��ȯ
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

            
            //���� ������ �ִ� ������ ���?
            if (Player.instance.playerItem != null)
            { 
                Player.instance.playerItem.SetActive(true); 
                Player.instance.playerItem.transform.position = Player.instance.transform.position; 
            }

            MapUIManager.instance.updateItemUI(itemList[thisItemIndex]);
            Player.instance.playerItem = Instantiate(itemList[thisItemIndex]);
            Player.instance.playerItem.SetActive(false);
            

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

    */

}
