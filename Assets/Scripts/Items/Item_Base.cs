using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SLOT
{
    ONE_H_WEAPON,
    SHIELD,
    HELMET,
    CHEST,
    GLOVES,
    unslotted
}

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

public struct Affecter
{
    public string NameModifier;
    public SLOT Slot;
    public bool IsPrefix;
    public List<StatObject> StatModifiers;

    public Affecter(string name, SLOT slot, List<StatObject> stats, bool prefix = false)
    {
        NameModifier = name;
        Slot = slot;
        StatModifiers = stats;
        IsPrefix = prefix;
    }
}

public class Item_Base : MonoBehaviour
{
    private string _name_p = "Default";
    [SerializeField]
    private Queue<Affecter> _affecters_p;
    public SLOT EquipmentSlot = SLOT.unslotted;

    public int ItemPrefab_ID = -1;

    Item_Base(string initialName)
    {
        _name_p = initialName;
    }

    // Start is called before the first frame update
    void Start()
    {
        _name_p = "Greataxe";   // TEMP for testing
        _affecters_p = new Queue<Affecter>();
    }

    public void AddAffector(Affecter affecter)
    {
        _affecters_p.Enqueue(affecter);
    }

    public string GetItemName()
    {
        string name = _name_p;

        foreach (Affecter affecter in _affecters_p)
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

        return name;
    }
}
