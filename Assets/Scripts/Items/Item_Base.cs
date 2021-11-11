using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum SLOT
{
    ONE_H_WEAPON,
    SHIELD,
    HELMET,
    CHEST,
    GLOVES,
    unslotted
}

[System.Serializable]
public struct StatObject
{
    public string StatName; // ie "Health", "Speed", "MinDamage", "GlobalDamage"
    public List<float> Values;

    public StatObject(string name, List<float> values)
    {
        StatName = name;
        Values = values;
    }
    public StatObject(string name, float value)
    {
        StatName = name;
        Values = new List<float>();
        Values.Add(value);
    }

    public override string ToString()
    {
        string retString = StatName + ": " + Values[0];
        if( Values.Count > 1 )
        {
            for (int i = 1; i < Values.Count; i++)
            {
                retString += "," + Values[i];
            }
        }
        return retString;
    }
}

[System.Serializable]
public struct Affecter
{
    public string NameModifier;
    public SLOT Slot;
    public bool IsPrefix;
    public int ID;
    public List<StatObject> StatModifiers;

    public Affecter(string name, SLOT slot, List<StatObject> stats, bool prefix = false, int id = -1)
    {
        NameModifier = name;
        Slot = slot;
        StatModifiers = stats;
        IsPrefix = prefix;
        ID = id;

    }
}

[System.Serializable]
public class Item_Base
{
    public string _name_p;
    public SLOT Item_Slot = SLOT.unslotted;
    public int Item_ID = -1;
    public int Modifier; //weapons = haste
    public int[] Stat_Range; //weapons = damage; armor = rollable armour value
    public int Mesh_ID;

    public List<int> Affecters;

    public Item_Base(string initialName)
    {
        _name_p = initialName;
        //    _affecters_p = new Queue<Affecter>();
        Affecters = new List<int>();
    }

    public void AddAffector(Affecter affecter)
    {
    //    _affecters_p.Enqueue(affecter);
    }

    public void AddAffecter(int affecterID)
    {
        Affecters.Add(affecterID);
    }

    public string GetItemName()
    {
        string name = _name_p;

    /*    foreach (Affecter affecter in _affecters_p)
        {
            if (affecter.IsPrefix)
            {
                name = affecter.NameModifier + " " + name;
            }
            else
            {
                name += " " + affecter.NameModifier;
            }
        }
    */

        for(int i = 0; i < Affecters.Count; i++)
        {
            Affecter affecter = AffectersDatabase.Instance.GetAffecterWithID(Affecters[i]);

            if( affecter.IsPrefix)
            {
                name = affecter.NameModifier + " " + name;
            } else
            {
                name += " " + affecter.NameModifier;
            }
        }

        return name;
    }

   
}
