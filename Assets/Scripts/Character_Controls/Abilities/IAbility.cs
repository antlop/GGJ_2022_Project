using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IAbility
{
    public Sprite AbilityIcon { get; set; }

    public void SetActivationKey(KeyCode key);

    public void SetCooldownSlider(Slider _slider);
}
