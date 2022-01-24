using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Combat_Controller : MonoBehaviour
{
    MeleeAttack meleeAttack_Basic;

    public bool CloseEnoughToMeleeAttack = false;
    public float MeleeAttackSpeed = 1.75f;
    float MeleeAttackBucket = 0f;

    private void Start()
    {
        meleeAttack_Basic = GetComponent<MeleeAttack>();
    }

    private void FixedUpdate()
    {
        if (MeleeAttackBucket < MeleeAttackSpeed)
        {
            MeleeAttackBucket += Time.deltaTime;
        }

        if( CloseEnoughToMeleeAttack )
        {
            meleeAttack_Basic.MakeAttack();
        }
    }
}
