using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WFC_Editor : MonoBehaviour
{
    public TMP_InputField MapSizeX;
    public TMP_InputField MapSizeY;
    public TMP_InputField Seed;
     
    public WFC_Tiles WFC;

    public void GenerateNewMap()
    {
        WFC.GenerateAMap(Seed.text, new Vector2Int(System.Convert.ToInt32(MapSizeX.text), System.Convert.ToInt32(MapSizeY.text)));
    }
}
