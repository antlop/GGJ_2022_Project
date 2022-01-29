using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour, IClass
{
    public ClassTypes Type;
    public GameObject ArcaneBomb;
    public GameObject LightningBeam;
    public GameObject MagicMissile;

    public ClassTypes ClassType { get { return Type; } set { Type = value; } }
    public GameObject MainAbility { get { return MagicMissile; } set { MagicMissile = value; } }
    public GameObject AbilityOne { get { return LightningBeam; } set { LightningBeam = value; } }
    public GameObject AbilityTwo { get { return ArcaneBomb; } set { ArcaneBomb = value; } }
}
