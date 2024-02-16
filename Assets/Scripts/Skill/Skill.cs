using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillLimit {None, Melee, Shot}

public abstract class Skill : SelectItem
{
    [field: SerializeField] public string skillName { get; private set; }
    [field: SerializeField] public string skillID { get; private set; }
    [field: SerializeField] public SkillLimit skillLimit { get; private set; }

    [field: SerializeField] public float preDelay { get; private set; }                 //��ų ��� �� ��� �ð�
    [field: SerializeField] public float rate { get; private set; }                     //��ų ��� �ð�
    [field: SerializeField] public float postDelay { get; private set; }                //��ų ��� �� ��� �ð�

    [field: SerializeField] public float skillDefalutCoolTime { get; private set; }     //�⺻ ��� �ð�
    [field: SerializeField] public float skillCoolTime { get; set; }                    //���� ��� �ð�

    [field: SerializeField] public GameObject user { get; set; }                        //�����

    public abstract void Use(GameObject user);
    public IEnumerator CoolDown()
    {
        if(user.tag == "Player")
        {
            Player player = user.GetComponent<Player>();
            skillCoolTime = skillDefalutCoolTime + (skillDefalutCoolTime * player.userData.skillCoolTime);
        }
        
        
        while(skillCoolTime >= 0)
        {
            yield return new WaitForSeconds(0.1f);

            skillCoolTime -= 0.1f;
        }
    }



}
