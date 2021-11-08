using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMP_item_ancestry_tester : MonoBehaviour
{
    public int heirloomID;
    public Item_Base Heirloom;
    public TMPro.TextMeshProUGUI NameText;

    // Start is called before the first frame update
    void Start()
    {
        Heirloom = BaseItemDatabase.Instance.GetItemWithID(heirloomID);
    }


    int clickCount = 0;
    public void SetItemNameToUI()
    {
        if( Heirloom != null && NameText)
        {
            NameText.text = Heirloom.GetItemName();

            List<StatObject> statmods = new List<StatObject>();
            statmods.Add(new StatObject("Health", 24));

            switch(clickCount)
            {
                case 0:
                    Heirloom.AddAffector(new Affecter("Barbarian", SLOT.ONE_H_WEAPON, statmods, true));
                    break;
                case 1:
                    Heirloom.AddAffector(new Affecter("Fierce", SLOT.ONE_H_WEAPON, statmods, true));
                    break;
                case 2:
                    Heirloom.AddAffector(new Affecter(", kingslayer", SLOT.ONE_H_WEAPON, statmods, false));
                    break;
                case 4:
            //        Item_Prefab_Database.Instance.DatabaseStruct.BaseItemsDatabaseList.Add(Heirloom);
                    break;
                default:
                    break;
            }

            clickCount++;
        }
    }
}
