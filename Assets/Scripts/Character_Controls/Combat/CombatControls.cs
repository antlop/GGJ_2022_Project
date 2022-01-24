using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatControls : MonoBehaviour
{
    MeleeAttack meleeAttack_Basic;

    private void Start()
    {
        meleeAttack_Basic = GetComponent<MeleeAttack>();
    }

    private void FixedUpdate()
    {
        if(Input.GetMouseButton(1))
        {
            meleeAttack_Basic.MakeAttack();
        }
    }
}
