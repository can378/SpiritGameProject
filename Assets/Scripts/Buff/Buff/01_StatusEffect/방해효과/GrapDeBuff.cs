using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuff", menuName = "Buff/Grap")]

public class GrapDeBuff : BuffData
{
    // ���� �� ��ų, �̵� �Ұ�
    // �ǰ� �� ����

    bool LeftRight;     // False�� Left, True�� Right;

    float Tick = 0.0f;         // 0.1��

    public override void Apply(Buff _Buff)
    {
        Overlap(_Buff);
    }

    public override void Overlap(Buff _Buff)
    {

        if (_Buff.target.tag == "Player")
        {
            // ��ø 
            _Buff.AddStack();

            // ���׿� ���� ���ӽð� ����
            _Buff.curDuration = _Buff.duration = defaultDuration;

            // ȿ�� ����
            _Buff.target.SetFlinch(_Buff.curDuration);
        }
        if (_Buff.target.tag == "Enemy")
        {
            // ��ø 
            _Buff.AddStack();

            // ���׿� ���� ���ӽð� ����
            _Buff.curDuration = _Buff.duration = defaultDuration;


            // ȿ�� ����
            _Buff.target.SetFlinch(_Buff.curDuration);
        }
    }

    public override void Update_Buff(Buff _Buff)
    {
        ObjectBasic GrapOwner = (ObjectBasic)_Buff.CustomData["GO"];      // ��⸦ ������ ���
        Transform GrapPos = (Transform)_Buff.CustomData["GP"];
        int KeyDownCount = (int)_Buff.CustomData["KDC"];
        // ��⸦ ������ ����� ���� �� ���� ���°ų� �׾��ٸ� ����
        // �Ǵ� KeyDownCount�� ������ ������ ��
        if (GrapOwner == null || 0 < GrapOwner.status.isFlinch || !GrapOwner.gameObject.activeSelf || KeyDownCount > 30)
        {
            _Buff.curDuration = 0.0f;
            return;
        }

        // ������ ��ġ
        if (GrapPos != null)
        {
            _Buff.target.ChangeColor(new Vector4(1f, 1f, 1f, 0f));
            _Buff.target.transform.position = new Vector3(GrapPos.position.x, GrapPos.position.y, GrapPos.position.z);
        }


        // ���� ��Ŵ
        _Buff.target.status.isFlinch = Mathf.Max(_Buff.target.status.isFlinch, _Buff.curDuration);

        if (_Buff.target.tag == "Player")
        {
            Player player = _Buff.target.GetComponent<Player>();
            // ���� ������ �� �� ���� ����
            if ((!LeftRight && player.hAxis < 0.0f) || (LeftRight && player.hAxis > 0.0f))
            {
                KeyDownCount += (int)(1 + _Buff.target.stats.SEResist[(int)buffType].Value * 10);
                LeftRight = !LeftRight;
            }
        }
        if (_Buff.target.tag == "Enemy")
        {
            Tick += Time.deltaTime;

            // �ش� Tick���� ���Ͱ� KeyCount�� ����
            // ���� ����
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

