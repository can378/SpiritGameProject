using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardBox : MonoBehaviour
{
    bool isOpen=false;
    public bool isLocked;

    private GameObject Reward;
    private List<GameObject> itemList;
    int thisItemIndex;

    void Start()
    {
        itemList = DataManager.instance.gameData.selectItemList;
    }

    private void openBox() 
    {
        thisItemIndex = Random.Range(0, itemList.Count);
        Reward = Instantiate(itemList[thisItemIndex]);
        Reward.transform.position=transform.position+new Vector3(0,0.5f,0);
        isOpen = true;
    }

    IEnumerator checkBox() 
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.F)&& isOpen==false)
            {
                if (isLocked) { }
                else { openBox(); }
            }
            StopCoroutine(checkBox());
            yield return null;
        }
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isOpen==false&&collision.tag=="Player") 
        { StartCoroutine(checkBox()); }
        
    }


}
