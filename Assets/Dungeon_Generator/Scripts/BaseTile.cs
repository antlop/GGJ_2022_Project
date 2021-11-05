using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileSpawn
{
    public int TileID;
    [Range(1,100)]
    public float SpawnChance = 50f;
}

public class BaseTile : MonoBehaviour
{
    public TileSpawn[] TileIDSpawnPossibilities;
    [HideInInspector]
    public List<Transform> AnchorPoints;
    public Vector2 TileSize = new Vector2(1, 1);
    private bool HasBeenInitialized = false;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        if(HasBeenInitialized) { return; }

        AnchorPoints = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeInHierarchy && transform.GetChild(i).tag == "Anchor")
            {
                AnchorPoints.Add(transform.GetChild(i));
            }
        }
        HasBeenInitialized = true;
    }
}
