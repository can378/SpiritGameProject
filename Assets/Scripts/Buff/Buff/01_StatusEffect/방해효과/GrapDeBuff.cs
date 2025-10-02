using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuff", menuName = "Buff/Grap")]

public class GrapDeBuff : BuffData
{
    // 공격 및 스킬, 이동 불가
    // 피격 시 해제

    bool LeftRight;     // False면 Left, True면 Right;

    float Tick = 0.0f;         // 0.1초

    public override void Apply(Buff _Buff)
    {
        Overlap(_Buff);
    }

    public override void Overlap(Buff _Buff)
    {

        if (_Buff.target.tag == "Player")
        {
            // 중첩 
            _Buff.AddStack();

            // 저항에 따른 지속시간 적용
            _Buff.curDuration = _Buff.duration = defaultDuration;

            // 효과 적용
            _Buff.target.SetFlinch(_Buff.curDuration);
        }
        if (_Buff.target.tag == "Enemy")
        {
            // 중첩 
            _Buff.AddStack();

            // 저항에 따른 지속시간 적용
            _Buff.curDuration = _Buff.duration = defaultDuration;


            // 효과 적용
            _Buff.target.SetFlinch(_Buff.curDuration);
        }
    }

    public override void Update_Buff(Buff _Buff)
    {
        ObjectBasic GrapOwner = (ObjectBasic)_Buff.CustomData["GO"];      // 잡기를 시전한 대상
        Transform GrapPos = (Transform)_Buff.CustomData["GP"];
        int KeyDownCount = (int)_Buff.CustomData["KDC"];
        // 잡기를 시전한 대상이 있을 때 경직 상태거나 죽었다면 해제
        // 또는 KeyDownCount가 조건을 만족할 때
        if (GrapOwner == null || 0 < GrapOwner.status.isFlinch || !GrapOwner.gameObject.activeSelf || KeyDownCount > 30)
        {
            _Buff.curDuration = 0.0f;
            return;
        }

        // 붙잡힐 위치
        if (GrapPos != null)
        {
            _Buff.target.ChangeColor(new Vector4(1f, 1f, 1f, 0f));
            _Buff.target.transform.position = new Vector3(GrapPos.position.x, GrapPos.position.y, GrapPos.position.z);
        }


        // 경직 시킴
        _Buff.target.status.isFlinch = Mathf.Max(_Buff.target.status.isFlinch, _Buff.curDuration);

        if (_Buff.target.tag == "Player")
        {
            Player player = _Buff.target.GetComponent<Player>();
            // 왼쪽 눌러야 할 때 왼쪽 누름
            if ((!LeftRight && player.hAxis < 0.0f) || (LeftRight && player.hAxis > 0.0f))
            {
                KeyDownCount += (int)(1 + _Buff.target.stats.SEResist[(int)buffType].Value * 10);
                LeftRight = !LeftRight;
            }
        }
        if (_Buff.target.tag == "Enemy")
        {
            Tick += Time.deltaTime;

            // 해당 Tick마다 몬스터가 KeyCount를 누름
            // 방해 저항
            if (Tick > 0.5f)
            {
                KeyDownCount += (int)(1 + _Buff.target.stats.SEResist[(int)buffType].Value * 10);
                Tick -= 0.5f;
            }
        }
    }

    public override void Remove(Buff _Buff)
    {
        _Buff.target.ChangeColor(new Vector4(1f, 1f, 1f, 1f));
        _Buff.target.ClearFlinch();
    }

    public void SetGrapCustomData(Buff _Buff, ObjectBasic _OB, Transform _Pos)
    {
        _Buff.CustomData.Add("GO", _OB);
        _Buff.CustomData.Add("GP", _Pos);
        _Buff.CustomData.Add("KDC", 0);
    }

}

