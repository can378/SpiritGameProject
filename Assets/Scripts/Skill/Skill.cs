using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : SelectItem
{
    [field: SerializeField] public string skillName { get; private set; }
    [field: SerializeField] public string skillID { get; private set; }

    [field: SerializeField] public GameObject user { get; set; }

    [field: SerializeField] public float skillMaxCoolTime { get; private set; }
    [field: SerializeField] public float skillCoolTime { get; set; }

    public abstract void Use(GameObject user);
    public IEnumerator CoolDown()
    {
        skillCoolTime = skillMaxCoolTime;
        
        while(skillCoolTime >= 0)
        {
            yield return new WaitForSeconds(0.1f);

            skillCoolTime -= 0.1f;
        }
    }



}
