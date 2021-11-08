using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helm_Item : MonoBehaviour
{
    public Item_Base Helm_Base;
    public string ItemsName = "Default_Helm";
    public GameObject VisualObject;

    private void Awake()
    {
        if(Helm_Base == null)
        {
            Helm_Base = new Item_Base(ItemsName);
        }
    }

}
