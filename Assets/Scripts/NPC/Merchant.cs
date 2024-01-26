using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Merchant : MonoBehaviour
{
    public List<GameObject> itemList;//판매 가능한 아이템 종류
    public List<GameObject> sellingItem;//이번에 판매할 아이템
    public List<GameObject> storePos;

    public GameObject storePanel;

    UserData userData;




    private void Start()
    {
        userData = FindObjectOfType<DataManager>().userData;
        //setStore();

    }


    private void setStore()
    {
        for (int i = 0; i < storePos.Count; i++)
        {
            int randNum = Random.Range(0, itemList.Count);
            GameObject itemObj = Instantiate(itemList[randNum]);
            itemObj.GetComponent<ItemStatus>().obtainable = false;
            itemObj.transform.position = storePos[i].transform.position;

        }
    }
    private void buying(GameObject buyingObj)
    {

        int price = buyingObj.GetComponent<ItemStatus>().price;

        if (userData.coin >= price)
        {
            userData.coin -= price;
            MapUIManager.instance.UpdateCoinUI();
            MapUIManager.instance.updateItemUI(buyingObj);
            buyingObj.SetActive(false);
            buyingObj.GetComponent<ItemStatus>().obtainable = true;
            Player.instance.playerItem = buyingObj;
            //userData.playerItem = GameData.instance.itemList[2].name;
        }
        else { print("no enough coin"); }

    }

    /*
    IEnumerator selectItem()
    {

        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray);


                if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Item"))
                {

                    Debug.Log(hit.collider.gameObject.name);
                    buying(hit.collider.gameObject);
                }
            }

            yield return null;
        }
    }
    */

    IEnumerator storeActivate()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                storePanel.SetActive(!storePanel.activeSelf);
            }
            yield return null;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(storeActivate());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StopAllCoroutines();
            if (storePanel != null && storePanel.activeSelf)
            {
                storePanel.SetActive(false);
            }
        }
    }






}
