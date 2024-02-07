using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SubWeaponType { Guard, Parry, Teleport }

public class SubWeapon : SelectItem
{
    [field: SerializeField] public int SubweaponCode { get; private set; }

    [field: SerializeField] public SubWeaponType subWeaponType { get; private set; }
    [field: SerializeField] public WeaponRating weaponRating { get; private set; }

    [field: SerializeField] public float preDelay { get; private set; }     // 선딜레이
    [field: SerializeField] public float rate { get; private set; }         // 사용 시간

    [field: SerializeField] public float coolTime { get; private set; }     // 다음 사용까지 대기 시간

    [field: SerializeField] public float ratio { get; private set; }     // 수치 가드 : 피해 감소율      반격 : 적 스턴 시간     텔레포트 : 거리
}
