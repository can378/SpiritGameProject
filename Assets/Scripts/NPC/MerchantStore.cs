using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class MerchantStore : MonoBehaviour
{
    public List<GameObject> itemList;//판매 가능한 아이템 종류
    private List<GameObject> sellingItem;//이번에 판매할 아이템

    Button button;

    public Image img;
    public TMP_Text itemName;
    public TMP_Text itemPrice;

    private int itemIndex;
    
    UserData userData;

    //public int recovery = 1;
    //public TMP_Text recoveryCoin;

    void Start()
    {
        userData = DataManager.instance.userData;

        setStore();

        button = GetComponent<Button>();
        button.onClick.AddListener(BuyItem);
    }

    private void setStore() 
    {
        itemIndex=Random.Range(0,itemList.Count);
        GameObject thisSlotItem= itemList[itemIndex];

        img.GetComponent<Image>().sprite= thisSlotItem.GetComponent<SpriteRenderer>().sprite;
        itemName.text = thisSlotItem.GetComponent<ItemStatus>().itemName;
        itemPrice.text=thisSlotItem.GetComponent<ItemStatus>().price.ToString();
    }

    
    public void BuyItem()
    {
        int cost=int.Parse(itemPrice.text);

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
