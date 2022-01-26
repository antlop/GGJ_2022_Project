using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatControls : MonoBehaviour
{
    MeleeAttack meleeAttack_Basic;
    public LayerMask ClickToMoveLayer;

    private void Start()
    {
        meleeAttack_Basic = GetComponent<MeleeAttack>();
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000, ClickToMoveLayer))
            {
                meleeAttack_Basic.MakeAttackTowardMouseClick(hit.point);
            }
            else
            {
                meleeAttack_Basic.MakeAttack();
            }
        }
    }
}
