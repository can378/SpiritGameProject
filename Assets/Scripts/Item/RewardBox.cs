using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardBox : NPCbasic
{
    [field: SerializeField] bool isOpen = false;
    [field: SerializeField] bool isLock;
    [field: SerializeField] public List<int> items {get; set; }

    public override void Conversation()
    {
        base.Conversation();
        UnLock();
        OpenChest();
    }

    public override void ConversationOut()
    {
        base.ConversationOut();
    }

    public void UnLock()
    {
        if(!isLock)
            return;
        
        if(Player.instance.playerStats.key < 1)
            return;

        Player.instance.playerStats.key--;
        isLock = false;
    }

    public void Lock()
    {

    }

    void Break()
    {
        print("강제로 부수어 아이템 일부가 망가졌다.");

        int breakItem = items.Count > 0 ? items.Count/2 + 1 : 0;

        for( int i = 0 ; i < breakItem ; i++)
        {
            items[Random.Range(0,items.Count)] = 0;
        }

        OpenChest();
        Destroy(this.gameObject);
    }

    public void OpenChest()
    {
        if (isLock)
            return;

        if(isOpen)
            return;

        foreach(var item in items)
        {
            Vector3 pos = transform.position + (Random.insideUnitSphere * 5);
            pos.z = 0;

            int itemType = item / 100;
            int itemCode = item % 100;
            
            switch(itemType)
            {
                case 0:
                    break;
                case 1:
                    Instantiate(GameData.instance.selectItemList[itemCode],pos,Quaternion.identity);
                    break;
                case 2:
                    Instantiate(GameData.instance.weaponList[itemCode], pos, Quaternion.identity);
                    break;
                case 3:
                    Instantiate(GameData.instance.equipmentList[itemCode], pos, Quaternion.identity);
                    break;
                case 4:
                    Instantiate(GameData.instance.skillList[itemCode], pos, Quaternion.identity);
                    break;
                default:
                    break;
            }
        }

        isOpen = true;
    }

    public override void Dead()
    {
        base.Dead();
        Break();
    }
}
