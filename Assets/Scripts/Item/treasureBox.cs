using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treasureBox : MonoBehaviour, Interactable
{

    [field: SerializeField] int[] TypeWeight = new int[(int)SelectItemType.END];

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
        ItemChoose();

        if (isLock) { lockObj.SetActive(true); }
    }

    public void ItemChoose()
    {
        // ====================================
        // 유형
        // new로 생성한거 따로 지워야하나?
        WeightRandom<SelectItemType> typeRandom = new WeightRandom<SelectItemType>();

        // 설정한 가중치를 가중치 무작위에 넣는다.
        for (int i = 0; i < (int)SelectItemType.END; ++i)
        {
            typeRandom.Add((SelectItemType)i, TypeWeight[i]);
        }

        // ====================================
        // 등급
        WeightRandom<SelectItemRating> weightRandom = new WeightRandom<SelectItemRating>();

        // 설정한 가중치를 가중치 무작위에 넣는다.
        for (int i = 0; i < (int)SelectItemRating.END; ++i)
        {
            weightRandom.Add((SelectItemRating)i, RatingWeight[i]);
        }

        // 조건을 담은 Dictionary 생성
        Dictionary<string, int> ItemCondition = new Dictionary<string, int>();

        // 상자에 들어있는 아이템 개수를 정함
        int ItemNum = Random.Range(1, 4);

        // 아이템 개수만큼 조건에 맞게 무작위 선택
        for (int i = 0; i < ItemNum; ++i)
        {
            // 가중치에 따라 무작위로 뽑는다.
            SelectItemType ItemType = typeRandom.GetRandomItem();
            SelectItemRating ItemRating = weightRandom.GetRandomItem();

            //Debug.Log(ItemType + " : " + ItemRating);

            ItemCondition.Add("selectItemType", (int)ItemType);
            ItemCondition.Add("selectItemRating", (int)ItemRating);

            // 게임 데이터 접근하여 게임오브젝트를 가져온다.
            GameObject selectItem = GameData.instance.DrawRandomItem(ItemCondition);

            if (selectItem != null)
                reward.Add(selectItem.GetComponent<SelectItem>());

            ItemCondition.Clear();
        }
        Debug.Log("뽑기 완료");
    }

    public string GetInteractText()
    {
        return "상자 열기";
    }

    public void Interact()
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

    /*
    void WeightRandomTest()
    {
        // ====================================
        // 유형
        // new로 생성한거 따로 지워야하나?
        WeightRandom<SelectItemType> typeRandom = new WeightRandom<SelectItemType>();

        // 설정한 가중치를 가중치 무작위에 넣는다.
        for (int i = 0; i < (int)SelectItemType.END; ++i)
        {
            typeRandom.Add((SelectItemType)i, TypeWeight[i]);
        }

        // ====================================
        // 등급
        // new로 생성한거 따로 지워야하나?
        WeightRandom<SelectItemRating> ratingRandom = new WeightRandom<SelectItemRating>();

        // 설정한 가중치를 가중치 무작위에 넣는다.
        for (int i = 0; i < (int)SelectItemRating.END; ++i)
        {
            ratingRandom.Add((SelectItemRating)i, RatingWeight[i]);
        }

        int[] TypeP = new int[(int)SelectItemType.END];
        int[] RatingP = new int[(int)SelectItemRating.END]; 


        for (int i = 0; i < 100000; ++i)
        {
            // 가중치에 따라 무작위로 뽑는다.
            SelectItemType ItemType = typeRandom.GetRandomItem();
            SelectItemRating ItemRating = ratingRandom.GetRandomItem();

            ++TypeP[(int)ItemType];
            ++RatingP[(int)ItemRating];
        }

        for(int i = 0 ; i < (int)SelectItemType.END;++i)
        {
            print((SelectItemType)i + " : " + ((float)TypeP[i] / 100000.0f));
        }

        for (int i = 0; i < (int)SelectItemRating.END; ++i)
        {
            print((SelectItemRating)i + " : " + ((float)RatingP[i] / 100000.0f));
        }
    }
    */
    
}

