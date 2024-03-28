using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombinationRandom
{
    public static List<int> CombRandom(int length, int min, int max) 
    {
        List<int> list = new List<int>();
        for(int i = 0 ; i < length ; i++)
        {
            while(true)
            {
                int ran = Random.Range(min,max);
                if(list.IndexOf(ran) == -1)
                {
                    list.Add(ran);
                    break;
                }
            }
            
        }
        return list;
    }
}
