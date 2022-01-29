using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AI_HealthBar : MonoBehaviour
{
    public Character_Core_Manager charManager;
    public Slider hpBar;

    private void LateUpdate()
    {
        if (hpBar && charManager && charManager.BaseStats)
        {
            hpBar.value = (float)charManager.BaseStats.Health_current / (float)charManager.BaseStats.Health_max;
        }
    }
}
