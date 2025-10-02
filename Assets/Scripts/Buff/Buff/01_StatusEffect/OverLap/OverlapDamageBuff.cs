using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "NewBuff", menuName = "Buff/OverlapDamage")]
public class OverlapDamageBuff : BuffData
{
    [field: SerializeField] public float damagePer { get; set; }
    // 최대 중첩 시 피해를 주고 제거되는 디버프
    // 저항에 따라 최대 중첩량 증가
    
    public override void Apply(Buff _Buff)     //추가
    {
        Overlap(_Buff);
    }
    public override void Overlap(Buff _Buff)     //갱신
    {
        ObjectBasic objectBasic = _Buff.target.GetComponent<ObjectBasic>();
        Stats stats = _Buff.target.GetComponent<Stats>();

        int maxStack = DefaultMaxStack + (int)(stats.SEResist[(int)buffType].Value * 10);

        _Buff.AddStack();


        _Buff.curDuration = _Buff.duration = defaultDuration;

        if (_Buff.stack == maxStack)
        {
            Debug.Log(_Buff.target.name +":출혈!");
            // 최대 체력이 1000 이상이라면
            if (objectBasic.stats.HPMax.Value >= 1000)
            {
                objectBasic.Damaged(1000 * damagePer);
            }
            else
            {
                objectBasic.Damaged(objectBasic.stats.HPMax.Value * damagePer);
            }

            GameObject BleedObject = ObjectPoolManager.instance.Get("Bleeding");
            BleedObject.transform.position = objectBasic.CenterPivot.transform.position;
            BleedObject.transform.localScale = Vector3.one * 3;

            _Buff.duration = 0;
        }
    }

    public override void Update_Buff(Buff _Buff)
    {
        
    }
    public override void Remove(Buff _Buff)    //제거
    {
           
    }
}
