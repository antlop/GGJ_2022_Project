using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarLogic : MonoBehaviour
{
    public Image HealthBar;
    public Character_Core_Manager coreManager;

    // Update is called once per frame
    void LateUpdate()
    {
        HealthBar.fillAmount = (float)coreManager.BaseStats.Health_current / (float)coreManager.BaseStats.Health_max;
    }
}
