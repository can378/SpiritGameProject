using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActiveSkillName 
{ RotateSlash, FireBall, PoisonCloud, Explosion, Invincible, Thunderbolt  };


public class SkillActive : MonoBehaviour
{
    [field: SerializeField] public ActiveSkillName ASkillName { get; private set; }


    void Start()
    {
        
    }



}
