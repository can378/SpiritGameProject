using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum BuffType { ABILITY , CONTINUOUS_DAMAGE , DISTURBING_EFFECT ,OVERLAP, SPECIAL, Buff }

[System.Serializable]
public class Buff
{
    [field: SerializeField] public BuffData buffData { get; private set; }
    [field: SerializeField] public ObjectBasic target { get; private set; }
    [field: SerializeField] public float duration { get; set; }                              // ���� ���� �ð�
    [field: SerializeField] public float curDuration { get; set; }                          // �ð�
    [field: SerializeField] public int stack { get; set; }                      // ���� ��ø
    [field: SerializeField] public TMP_Text overlapText { get; private set; }

    [field: SerializeField] public Dictionary<string, object> CustomData { get; private set; }

    public Buff(BuffData _BuffData, ObjectBasic _Target)
    {
        buffData = _BuffData;
        target = _Target;
        CustomData = new Dictionary<string, object>();
    }

    void Update()
    {
        buffData.icon.fillAmount = curDuration / duration;
        //if (this.target == null)
        //    Destroy(this.gameObject);
    }

    public void AddStack()
    {
        stack = stack < buffData.DefaultMaxStack ? stack + 1 : buffData.DefaultMaxStack;
        //overlapText.text = stack > 1 ? stack.ToString() : null;

    }

    public void Apply()     //�߰�
    {
        buffData.Apply(this);
    }
    public void Overlap()     //����
    {
        buffData.Overlap(this);
    }
    public void Update_Buff()
    {
        buffData.Update_Buff(this);
    }
    public void Remove()    //����
    {
        buffData.Remove(this);
    }
}
