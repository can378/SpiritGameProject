using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Consumable : SelectItem
{
    [field: SerializeField] public bool finishUse { get; private set; }
    [field :SerializeField] public bool throwItem {get; private set; }

    public abstract void UseItem(Player user);
}
