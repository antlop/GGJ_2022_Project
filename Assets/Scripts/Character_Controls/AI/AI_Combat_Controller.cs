using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Combat_Controller : MonoBehaviour
{
    MeleeAttack meleeAttack_Basic;

    public bool CloseEnoughToMeleeAttack = false;
    public float MeleeAttackSpeed = 1.75f;
    [SerializeField] float MeleeAttackBucket = 0f;

    private void Start()
    {
        meleeAttack_Basic = GetComponent<MeleeAttack>();
    }

    private void FixedUpdate()
    {
        if (MeleeAttackBucket < MeleeAttackSpeed && CloseEnoughToMeleeAttack)
        {
            MeleeAttackBucket += Time.deltaTime;
        }

        if( CloseEnoughToMeleeAttack && MeleeAttackBucket >= MeleeAttackSpeed)
        {
            meleeAttack_Basic.MakeAttack();
            GetComponent<Animator>().SetTrigger("MeleeAttack");
            MeleeAttackBucket = 0;
        }
    }
}
