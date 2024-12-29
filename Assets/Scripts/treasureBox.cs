using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treasureBox : MonoBehaviour
{

    [field: SerializeField] int[] RatingWeight = new int[(int)SelectItemRating.END];

    public GameObject lockObj;

    [field: SerializeField] bool isOpen = false;
    [field: SerializeField] bool isLock;
    [field: SerializeField] List<SelectItem> reward = new List<SelectItem>();

    void Awake()
    {

    }

    private void Start()
    {

        // new로 생성한거 따로 지워야하나?
        WeightRandom<SelectItemRating> weightRandom = new WeightRandom<SelectItemRating>();

        // 설정한 가중치를 가중치무작위에 넣는다.
        for (int i = 0; i < (int)SelectItemRating.END; ++i)
        {
            weightRandom.Add((SelectItemRating)i, RatingWeight[i]);
        }

        // 상자에 들어있는 아이템 개수를 정함
        int ItemNum = Random.Range(1,4);

        for(int i = 0; i <ItemNum; ++i)
        {
            // 가중치에 따라 무작위로 뽑는다.
            SelectItemRating Rating = weightRandom.GetRandomItem();
            print(Rating);
            // 게임 데이터 접근하여 게임오브젝트를 가져온다.
            reward.Add(GameData.instance.SelectItemList[1].DrawRandomItem((int)Rating));
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
        // print("reward interaction2");
        // //Item appear!!
        // randomNum = UnityEngine.Random.Range(1, itemCandidate.Count);
        // Instantiate(itemCandidate[randomNum], rewardPos.transform).GetComponent<SpriteRenderer>().sortingOrder
        //     =GetComponent<SpriteRenderer>().sortingOrder+1;

        foreach(SelectItem item in reward)
        {
            Vector3 pos = transform.position + (Random.insideUnitSphere * 3);
            Instantiate(item, pos, Quaternion.identity);
        }

        isOpen = true;

        this.gameObject.SetActive(false);
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

