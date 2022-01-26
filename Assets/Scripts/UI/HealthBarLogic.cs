using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarLogic : MonoBehaviour
{
    public Character_Core_Manager PlayerCoreManager;
    public Image HealthBar;
    // Start is called before the first frame update
    void Start()
    {
        PlayerCoreManager = GameObject.FindGameObjectWithTag("Player").GetComponent<Character_Core_Manager>();
    }

    private void FixedUpdate()
    {
        float fillamount = (float)PlayerCoreManager.BaseStats.Health_current / (float)PlayerCoreManager.BaseStats.Health_max;
        Debug.Log("Fill " + fillamount*100 + "%");
        HealthBar.fillAmount = fillamount;
    }
}
