using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatControls : MonoBehaviour
{
//    MeleeAttack meleeAttack_Basic;
    public GameObject MainAbility;
    public KeyCode MainAbilityKey;
    public Slider MainAbilityCooldownSlider;

    public GameObject AbilityOne;
    public KeyCode AbilityOneKey;
    public Slider AbilityOneCooldownSlider;

    public GameObject AbilityTwo;
    public KeyCode AbilityTwoKey;
    public Slider AbilityTwoCooldownSlider;

    public Animator AnimController;

    public IClass currentClass;

    private void Start()
    {
        currentClass = GameObject.FindGameObjectWithTag("Class_Object").GetComponent<IClass>();

        MainAbility = currentClass.MainAbility;
        AbilityOne = currentClass.AbilityOne;
        AbilityTwo = currentClass.AbilityTwo;

        UpdateKeyActivationForMainAbility(MainAbilityKey);
        UpdateKeyActivationForAbilityOne(AbilityOneKey);
        UpdateKeyActivationForAbilityTwo(AbilityTwoKey);
    }

    private void FixedUpdate()
    {
    }

    public void UpdateKeyActivationForMainAbility(KeyCode key)
    {
        MainAbility.GetComponent<IAbility>().SetActivationKey(key);
        MainAbility.GetComponent<IAbility>().SetCooldownSlider(MainAbilityCooldownSlider);
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
