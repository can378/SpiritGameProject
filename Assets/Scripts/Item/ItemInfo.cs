using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectItemName 
{ 
    Bomb,HPPortion, SpeedPortion, SkillPortion, 
    Insam,Sansam,
    Item, 
    SmallArmor, NormalArmor, LargeArmor, 
    Snack 
};

public class ItemInfo : MonoBehaviour
{
    [field:SerializeField]public SelectItemName selectItemName { get; private set; }
    public int price;
    
}
