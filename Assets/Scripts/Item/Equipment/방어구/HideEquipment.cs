using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideEquipment : Equipment
{
    [SerializeField] float hideCoolTime = 10f;
    bool isHide = false;

    void Update()
    {
        Passive();
    }

    void Passive()
    {
        if (user == null )
        {
            return;
        }

        if (user.playerStatus.isAttack || (0 < user.playerStatus.isFlinch))
        {
            HideOut();
            hideCoolTime = 10f;
            return;
        }

        hideCoolTime -= Time.deltaTime;

        if(hideCoolTime <= 0f && !isHide)
        {
            Hide();
        }
    }

    void Hide()
    {
        user.gameObject.layer = LayerMask.NameToLayer("PlayerHide");
        user.GetComponent<ObjectBasic>().ChangeColor(new Color(1, 1, 1, 0.4f));
        isHide = true;
    }

    void HideOut()
    {
        user.gameObject.layer = user.defaultLayer;
        user.GetComponent<ObjectBasic>().ChangeColor(new Color(1, 1, 1, 1f));
        isHide = false;
    }

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = target;
            Debug.Log("10초 동안 전투 중이 아니면 은신");
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            HideOut();
            this.user = null;
        }
    }
}
