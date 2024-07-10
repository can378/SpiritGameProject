using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    Stats stats;

    public Slider Hpslider;
    public GameObject BuffUI;

    void Awake()
    {
        stats = GetComponent<Stats>();
    }

    void FixedUpdate()
    {
        UpdateHealthUI();
        UpdateBuffUI();
    }

    public void UpdateHealthUI()
    {
        float normalizedHealth = (stats.HP / stats.HPMax) * 100;

        if( normalizedHealth >= 100f)
        {
            Hpslider.gameObject.SetActive(false);
            return;
        }

        Hpslider.gameObject.SetActive(true);
        Hpslider.value = normalizedHealth;
    }

    public void UpdateBuffUI()
    {
        for(int i = 0 ; i<stats.activeEffects.Count ; i++)
        {
            if(stats.activeEffects[i].transform.parent != BuffUI.transform)
                stats.activeEffects[i].transform.SetParent(BuffUI.transform);
        }
        
    }
}
