using System.Collections.Generic;
using UnityEngine;

public class MinimpController : MonoBehaviour
{
    public GameObject mapPanel;
    private void Start()
    {
        //∏  ∫Ò»∞º∫»≠
        mapPanel.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("M key pressed");
            mapPanel.SetActive(!mapPanel.activeSelf);
        }
        if (Input.anyKeyDown)
        {
            Debug.Log("Key pressed");
        }

    }
}