using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public int Health_current = 1;
    public int Health_max = 1;

    public int Brawn = 0;         //strength
    public int Attunement = 0;    //intellegence
    public int Fortitude = 0;     //constitution

    public void setMaxHealth(int value)
    {
        Health_max = value;
        Health_current = Health_max;
    }

    public void AdjustCurrentHealth(int value)
    {
        Health_current += value;
        if( Health_current > Health_max)
        {
            Health_current = Health_max;
        }
    }
}
