using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    int AttackID;
    float SpawnTermTime;
    [SerializeField] float SpawnTerm;     // 생성 시간

    //float TickTermTime;
    //[SerializeField] float TickTerm;     // 생성 시간

    [SerializeField] float LifeTime;     // 유지 시간
    [SerializeField] GameObject Effect;

    List<GameObject> EffectList = new List<GameObject>();

    void Onable()
    {
        SpawnTermTime = SpawnTerm;
        AttackID = Guid.NewGuid().GetHashCode();
        //TickTermTime = TickTerm;
    }

    void Update()
    {
        SpawnTermTime += Time.deltaTime;
        //TickTermTime += Time.deltaTime;

        if(SpawnTermTime > SpawnTerm)
        {
            SpawnTermTime -= SpawnTerm;

            GameObject EffectGameObject = ObjectPoolManager.instance.Get(Effect, transform.position);
            EffectList.Add(EffectGameObject);

            HitDetection hitDetection = EffectGameObject.GetComponent<HitDetection>();
            hitDetection.SetDisableTime(LifeTime, ENABLE_TYPE.Time);
            hitDetection.SetAttackID(AttackID);
        }
        
        /*
        if(TickTermTime > TickTerm)
        {
            TickTermTime -= TickTerm;
            int ID = Guid.NewGuid().GetHashCode();
            for (int index = EffectList.Count - 1; index >= 0; index--)
            {
                if (EffectList[index].activeSelf == false)
                {
                    EffectList.RemoveAt(index);
                    continue;
                }

                HitDetection hitDetection = EffectList[index].GetComponent<HitDetection>();

                hitDetection = EffectList[index].GetComponent<HitDetection>();
                hitDetection.SetAttackID(ID);
            }
        }
        */
    }



}
