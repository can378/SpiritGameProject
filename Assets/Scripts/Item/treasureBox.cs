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
        // ����
        // new�� �����Ѱ� ���� �������ϳ�?
        WeightRandom<SelectItemType> typeRandom = new WeightRandom<SelectItemType>();

        // ������ ����ġ�� ����ġ �������� �ִ´�.
        for (int i = 0; i < (int)SelectItemType.END; ++i)
        {
            typeRandom.Add((SelectItemType)i, TypeWeight[i]);
        }

        // ====================================
        // ���
        WeightRandom<SelectItemRating> weightRandom = new WeightRandom<SelectItemRating>();

        // ������ ����ġ�� ����ġ �������� �ִ´�.
        for (int i = 0; i < (int)SelectItemRating.END; ++i)
        {
            weightRandom.Add((SelectItemRating)i, RatingWeight[i]);
        }

        // ������ ���� Dictionary ����
        Dictionary<string, int> ItemCondition = new Dictionary<string, int>();

        // ���ڿ� ����ִ� ������ ������ ����
        int ItemNum = Random.Range(1, 4);

        // ������ ������ŭ ���ǿ� �°� ������ ����
        for (int i = 0; i < ItemNum; ++i)
        {
            // ����ġ�� ���� �������� �̴´�.
            SelectItemType ItemType = typeRandom.GetRandomItem();
            SelectItemRating ItemRating = weightRandom.GetRandomItem();

            //Debug.Log(ItemType + " : " + ItemRating);

            ItemCondition.Add("selectItemType", (int)ItemType);
            ItemCondition.Add("selectItemRating", (int)ItemRating);

            // ���� ������ �����Ͽ� ���ӿ�����Ʈ�� �����´�.
            GameObject selectItem = GameData.instance.DrawRandomItem(ItemCondition);

            if (selectItem != null)
                reward.Add(selectItem.GetComponent<SelectItem>());

            ItemCondition.Clear();
        }
        Debug.Log("�̱� �Ϸ�");
    }

    public string GetInteractText()
    {
        return "���� ����";
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
        // ����
        // new�� �����Ѱ� ���� �������ϳ�?
        WeightRandom<SelectItemType> typeRandom = new WeightRandom<SelectItemType>();

        // ������ ����ġ�� ����ġ �������� �ִ´�.
        for (int i = 0; i < (int)SelectItemType.END; ++i)
        {
            typeRandom.Add((SelectItemType)i, TypeWeight[i]);
        }

        // ====================================
        // ���
        // new�� �����Ѱ� ���� �������ϳ�?
        WeightRandom<SelectItemRating> ratingRandom = new WeightRandom<SelectItemRating>();

        // ������ ����ġ�� ����ġ �������� �ִ´�.
        for (int i = 0; i < (int)SelectItemRating.END; ++i)
        {
            ratingRandom.Add((SelectItemRating)i, RatingWeight[i]);
        }

        int[] TypeP = new int[(int)SelectItemType.END];
        int[] RatingP = new int[(int)SelectItemRating.END]; 


        for (int i = 0; i < 100000; ++i)
        {
            // ����ġ�� ���� �������� �̴´�.
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

