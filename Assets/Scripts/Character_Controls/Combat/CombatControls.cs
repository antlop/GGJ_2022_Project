using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatControls : MonoBehaviour
{
    MeleeAttack meleeAttack_Basic;
    public LayerMask ClickToMoveLayer;

    public GameObject AbilityOne;
    public KeyCode AbilityOneKey;
    public GameObject AbilityTwo;
    public KeyCode AbilityTwoKey;

    public Animator AnimController;

    private void Start()
    {
        meleeAttack_Basic = GetComponent<MeleeAttack>();

        UpdateKeyActivationForAbilityOne(AbilityOneKey);
        UpdateKeyActivationForAbilityTwo(AbilityTwoKey);

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

            AnimController.SetTrigger("MeleeAttack");
        }
    }

    public void UpdateKeyActivationForAbilityOne(KeyCode key)
    {
        AbilityOne.GetComponent<IAbility>().SetActivationKey(key);
    }

    public void UpdateKeyActivationForAbilityTwo(KeyCode key)
    {
        AbilityTwo.GetComponent<IAbility>().SetActivationKey(key);
    }
}
