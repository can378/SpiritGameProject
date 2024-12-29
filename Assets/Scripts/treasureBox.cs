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

        // new�� �����Ѱ� ���� �������ϳ�?
        WeightRandom<SelectItemRating> weightRandom = new WeightRandom<SelectItemRating>();

        // ������ ����ġ�� ����ġ�������� �ִ´�.
        for (int i = 0; i < (int)SelectItemRating.END; ++i)
        {
            weightRandom.Add((SelectItemRating)i, RatingWeight[i]);
        }

        // ���ڿ� ����ִ� ������ ������ ����
        int ItemNum = Random.Range(1,4);

        for(int i = 0; i <ItemNum; ++i)
        {
            // ����ġ�� ���� �������� �̴´�.
            SelectItemRating Rating = weightRandom.GetRandomItem();
            print(Rating);
            // ���� ������ �����Ͽ� ���ӿ�����Ʈ�� �����´�.
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

