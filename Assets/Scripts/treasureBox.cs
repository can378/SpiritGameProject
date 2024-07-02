using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RewardItem { equipment, weapon, skill,selectItem}
public class treasureBox : MonoBehaviour
{
    public GameObject lockObj;

    [field: SerializeField] bool isOpen = false;
    [field: SerializeField] bool isLock;
    [field: SerializeField] RewardItem rewardItemIndex;
    [field: SerializeField] GameObject rewardPos;

    private List<GameObject> itemCandidate;
    private int randomNum;

    private void Awake()
    {
        switch (rewardItemIndex)
        {
            case RewardItem.equipment:
                itemCandidate = GameData.instance.equipmentList;
                break;
            case RewardItem.weapon:
                itemCandidate = GameData.instance.weaponList;
                break;
            case RewardItem.skill:
                itemCandidate = GameData.instance.skillList;
                break;
            case RewardItem.selectItem:
                itemCandidate = GameData.instance.selectItemList;
                break;
        }
        if (isLock) { lockObj.SetActive(true); }
    }



    public void Interaction()
    {
        
        if (!isOpen)
        {
            if (isLock)
            {
                if (Player.instance.playerStats.key >= 1)
                {
                    Player.instance.playerStats.key--;
                    isLock = false;
                    lockObj.SetActive(false);
                }

            }
            else { Open(); }
        }
    }

    private void Open()
    {
        print("reward interaction2");
        //Item appear!!
        randomNum = UnityEngine.Random.Range(1, itemCandidate.Count);
        Instantiate(itemCandidate[randomNum], rewardPos.transform).GetComponent<SpriteRenderer>().sortingOrder
            =GetComponent<SpriteRenderer>().sortingOrder+1;
        isOpen = true;
    }

    /*
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&& collision.GetComponent<Player>().iDown)
        {
            print("touch treasure box");
            
            //Interaction();

        }
    }
    */
}
