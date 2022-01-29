using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusader : MonoBehaviour, IClass
{
    public ClassTypes Type;
    public GameObject BlessingOfCelerity;
    public GameObject Heal;
    public GameObject GuidedStrike;


    public ClassTypes ClassType { get { return Type; } set { Type = value; } }
    public GameObject MainAbility { get { return GuidedStrike; } set { GuidedStrike = value; } }
    public GameObject AbilityOne { get { return BlessingOfCelerity; } set { BlessingOfCelerity = value; } }
    public GameObject AbilityTwo { get { return Heal; } set { Heal = value; } }
}
