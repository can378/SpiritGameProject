using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GroupOrder : MonoBehaviour
{
    SortingGroup sortingG;

    // Start is called before the first frame update
    void Start()
    {
        sortingG = GetComponent<SortingGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        sortingG.sortingOrder = Mathf.RoundToInt(transform.position.y) * -1;
    }
}
