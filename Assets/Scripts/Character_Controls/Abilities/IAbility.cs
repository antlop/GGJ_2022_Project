using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IAbility
{

    public void SetActivationKey(KeyCode key);

    public void Activate();

    public void SetCooldownSlider(Slider _slider);
}
