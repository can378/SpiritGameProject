using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/DodgeAttack")]
public class DodgeAttack : PassiveData
{
    [SerializeField] GameObject dodgeHitBoxPrefab;
    GameObject dodgeHitBox;

    public override void Update_Passive(ObjectBasic _User)
    {
        if (_User == null)
        {
            return;
        }

        Player player = _User.GetComponent<Player>();

        if (!dodgeHitBox.activeSelf && player.playerStatus.isDodge)
        {
            dodgeHitBox.GetComponent<HitDetection>().SetHit_Ratio(10, 100.0f, player.playerStats.DefensivePower);
            dodgeHitBox.SetActive(true);
        }
        else if (dodgeHitBox.activeSelf && !player.playerStatus.isDodge)
        {
            dodgeHitBox.SetActive(false);
        }

    }

    public override void Apply(ObjectBasic _User)
    {
        if (_User.tag == "Player")
        {
            dodgeHitBox = Instantiate(dodgeHitBoxPrefab, _User.transform.position, Quaternion.identity);
            dodgeHitBox.transform.parent = _User.gameObject.transform;
            dodgeHitBox.SetActive(false);
        }
        else
        {
            Debug.Log("Error : Player Only Passive Skill");
        }
    }

    // Update is called once per frame
    public override void Remove(ObjectBasic _User)
    {
        if (_User.tag == "Player")
        {
            Destroy(dodgeHitBox);
        }
    }
}
