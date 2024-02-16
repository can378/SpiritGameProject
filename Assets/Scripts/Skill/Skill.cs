using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillLimit {None, Melee, Shot}

public abstract class Skill : SelectItem
{
    [field: SerializeField] public string skillName { get; private set; }
    [field: SerializeField] public string skillID { get; private set; }
    [field: SerializeField] public SkillLimit skillLimit { get; private set; }

    [field: SerializeField] public float preDelay { get; private set; }                 //스킬 사용 전 대기 시간
    [field: SerializeField] public float rate { get; private set; }                     //스킬 사용 시간
    [field: SerializeField] public float postDelay { get; private set; }                //스킬 사용 후 대기 시간

    [field: SerializeField] public float skillDefalutCoolTime { get; private set; }     //기본 대기 시간
    [field: SerializeField] public float skillCoolTime { get; set; }                    //현재 대기 시간

    [field: SerializeField] public GameObject user { get; set; }                        //사용자

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
