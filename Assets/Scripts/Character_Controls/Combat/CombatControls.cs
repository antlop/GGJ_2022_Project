using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatControls : MonoBehaviour
{
    MeleeAttack meleeAttack_Basic;
    public Slider meleeCooldownSlider;
    public LayerMask ClickToMoveLayer;

    public GameObject AbilityOne;
    public KeyCode AbilityOneKey;
    public Slider AbilityOneCooldownSlider;
    public GameObject AbilityTwo;
    public KeyCode AbilityTwoKey;
    public Slider AbilityTwoCooldownSlider;

    public Animator AnimController;

    private void Start()
    {
        meleeAttack_Basic = GetComponent<MeleeAttack>();
        meleeAttack_Basic.SetCooldownSlider(meleeCooldownSlider);

        UpdateKeyActivationForAbilityOne(AbilityOneKey);
        UpdateKeyActivationForAbilityTwo(AbilityTwoKey);

    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(1) && meleeAttack_Basic.IsAttackOffCooldown())
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
        AbilityOne.GetComponent<IAbility>().SetCooldownSlider(AbilityOneCooldownSlider);
    }

    public void UpdateKeyActivationForAbilityTwo(KeyCode key)
    {
        AbilityTwo.GetComponent<IAbility>().SetActivationKey(key);
        AbilityTwo.GetComponent<IAbility>().SetCooldownSlider(AbilityTwoCooldownSlider);
    }
}
