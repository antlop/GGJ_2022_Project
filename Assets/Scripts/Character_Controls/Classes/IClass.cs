using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClassTypes { Combatant, Mage, Crusader, Ranger, Gun_Slinger, Gladiator }

public interface IClass
{
    public ClassTypes ClassType { get; set; }

    public GameObject MainAbility { get; set; }
    public GameObject AbilityOne { get; set; }
    public GameObject AbilityTwo { get; set; }
}
