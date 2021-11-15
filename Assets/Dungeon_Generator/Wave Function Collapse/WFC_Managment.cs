using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WFC_SaveObject
{
    public string Seed;
    public Vector2 MapSize;
}

public class WFC_Managment : MonoBehaviour
{
    [SerializeField]
    List<WFC_SaveObject> _savedMaps;

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
