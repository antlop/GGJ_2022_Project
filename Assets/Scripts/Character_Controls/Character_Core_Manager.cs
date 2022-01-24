using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Core_Manager : MonoBehaviour
{
    public bool Alive = true;
    public Stats BaseStats;
    public GameObject DeathScreenCanvas;

    // Start is called before the first frame update
    void Start()
    {
        BaseStats = GetComponent<Stats>();
        if (BaseStats == null)
        {
            BaseStats = new Stats();
            BaseStats.setMaxHealth(10);
        }
    }

    public void TakeDamage(int value)
    {
        BaseStats.AdjustCurrentHealth(value * -1);
        Debug.Log("Took " + value + " damage. Now at HP: " + BaseStats.Health_current);

        if( BaseStats.Health_current <= 0 )
        {
            Alive = false;
            if (gameObject.tag == "Player")
            {
                DeathScreenCanvas.SetActive(true);
            }
            Destroy(gameObject);
        }
    }
}
