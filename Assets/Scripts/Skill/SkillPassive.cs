using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PassiveSkillName { a,s };

public class SkillPassive : MonoBehaviour
{
    [field: SerializeField] public PassiveSkillName PSkillName { get; private set; }
    void Start()
    {
        
    }


}
