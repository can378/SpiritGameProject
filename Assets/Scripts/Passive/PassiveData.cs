using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveData : ScriptableObject
{
    [field: SerializeField] public string PName { get; private set; }
    [field: SerializeField, TextArea] public string PDescription { get; private set; }
    [field: SerializeField] public int PID { get; private set; }
    [field: SerializeField] public Sprite PSprite { get; private set; }


    public abstract void Update_Passive(ObjectBasic _User);
    public abstract void Apply(ObjectBasic _User);
    public abstract void Remove(ObjectBasic _User);

    public virtual string Update_Description(Stats _Stats)
    {
        return string.Format(PDescription);
    }
}
