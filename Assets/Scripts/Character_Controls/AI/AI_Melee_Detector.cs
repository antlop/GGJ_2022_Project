using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Melee_Detector : MonoBehaviour
{
    public GameObject Player;
    public AI_Combat_Controller CombatControls;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        CombatControls = GetComponentInParent<AI_Combat_Controller>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.tag == "Player" )
        {
            CombatControls.CloseEnoughToMeleeAttack = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            CombatControls.CloseEnoughToMeleeAttack = false;
        }
    }
}
