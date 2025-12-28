using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffEffect : MonoBehaviour
{
    public abstract void Play(in Buff _Buff);
    public abstract void Overlap(in Buff _Buff);
    public abstract void Stop(in Buff _Buff);
}
