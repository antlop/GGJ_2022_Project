using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum INV_SLOTS { Weapon, Armor, Neck, Trinket, ITEM_MAX };

public class Inventory : MonoBehaviour
{
    [SerializeField] int BagSize = 10;
    public m_Item_Base[] Bag;
    
    public m_Item_Base[] Equipped;


    // Start is called before the first frame update
    void Start()
    {
        Bag = new m_Item_Base[BagSize];
        Equipped = new m_Item_Base[(int)INV_SLOTS.ITEM_MAX];

        // read in any saved items
    }

    public bool AddItemToBag(m_Item_Base _item)
    {
        for(int i = 0; i < BagSize; ++i)
        {
            if( Bag[i].Initialized == false )
            {
                Bag[i] = _item;
                return true;
            }
        }

        return false;
    }

    public m_Item_Base PopItemFromBag(int slot)
    {
        m_Item_Base returnItem = Bag[slot];
        Bag[slot] = new m_Item_Base(false);
        return returnItem;
    }
}
