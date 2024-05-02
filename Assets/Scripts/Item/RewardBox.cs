using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardBox : NPCbasic
{
    [field: SerializeField] bool isOpen = false;
    [field: SerializeField] bool isLock;
    [field: SerializeField] public List<int> items {get; set; }

    Stats stats;
    SpriteRenderer sprite;

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

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        stats = GetComponent<Stats>();
    }

    public void UnLock()
    {
        if(!isLock)
            return;
        
        if(Player.instance.stats.key < 1)
            return;

        Player.instance.stats.key--;
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerAttack")
        {
            PlayerAttack(other.gameObject);
        }
    }

    public void PlayerAttack(GameObject attacker)
    {
        //Damaged
        HitDetection hitDetection = attacker.GetComponent<HitDetection>();

        AudioManager.instance.SFXPlay("Hit_SFX");

        Damaged(hitDetection.damage, hitDetection.critical, hitDetection.criticalDamage);

    }

    public void Damaged(float damage, float critical = 0, float criticalDamage = 0)
    {
        bool criticalHit = Random.Range(0, 100) < critical * 100 ? true : false;
        damage = criticalHit ? damage * criticalDamage : damage;

        print(this.name +" damaged : " + (1 - stats.defensivePower) * damage);
        stats.HP -= (1 - stats.defensivePower) * damage;

        sprite.color = 0 < (1 - stats.defensivePower) * damage ? Color.red : Color.green;

        Invoke("DamagedOut", 0.05f);
        if (stats.HP <= 0f)
        {
            Player.instance.stats.exp++;
            MapUIManager.instance.UpdateExpUI();
            Break();
        }
    }

    void DamagedOut()
    {
        sprite.color = Color.white;
    }
}
