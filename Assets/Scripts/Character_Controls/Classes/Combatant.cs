using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatant : MonoBehaviour, IClass
{
    public ClassTypes Type;
    public GameObject SlamAbility;
    public GameObject RageTornadoAbility;
    public GameObject BashAbility;
        

    public ClassTypes ClassType { get { return Type; } set { Type = value; } }
    public GameObject MainAbility { get {return BashAbility; } set { BashAbility = value; } }
    public GameObject AbilityOne { get { return SlamAbility; } set { SlamAbility = value; } }
    public GameObject AbilityTwo { get { return RageTornadoAbility; } set { RageTornadoAbility = value; } }

}
