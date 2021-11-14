using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

[System.Serializable]
public class MapTile
{
    public Vector2 TileSize = new Vector2(1, 1);
    public int ID;
    public GameObject WorldObjectPrefab;
}

public class TileDatabase : MonoBehaviour
{
    #region Singleton
    private static TileDatabase instance = null;

    // Game Instance Singleton
    public static TileDatabase Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        if(this.transform.parent == null)
            DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    public MapTile[] MapTiles;
    public Dictionary<Vector2, List<MapTile>> Tiles;
    private bool hasInitialized = false;
    public Material TempMat;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        if(hasInitialized) { return; }

        hasInitialized = true;
        Tiles = new Dictionary<Vector2, List<MapTile>>();

        string[] dir = Directory.GetDirectories(Application.persistentDataPath);




        foreach (MapTile tile in MapTiles)
        {
            if (!Tiles.ContainsKey(tile.TileSize))
            {
                Tiles[tile.TileSize] = new List<MapTile>();
            }
            Tiles[tile.TileSize].Add(tile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetPrefabForObjectID(int ID)
    {
        Initialize();
        foreach (MapTile mt in MapTiles)
        {
            if( mt.ID == ID) { return mt.WorldObjectPrefab; }
        }
        return null;
    }

    public MapTile GetPrefabToFitSpace(Vector2 spaceSize)
    {
        Initialize();
        int firstRand = Random.Range(1, (int)spaceSize.x);
        int secondRand = Random.Range(1, (int)spaceSize.y);
        
        List<Vector2> possibleSizes = GetPossibleSizesToFitSpace(spaceSize);

        MapTile returnTile = new MapTile();
        returnTile.ID = -1;

        List<MapTile> subList = Tiles[possibleSizes[Random.Range(0,possibleSizes.Count)]];

        if ( subList.Count > 0)
        {
            returnTile = subList.First();
            while(returnTile == null || !returnTile.WorldObjectPrefab.activeSelf )
            {
                subList.RemoveAt(0);

                if (subList.Count > 0)
                {
                    returnTile = subList.First();
                }
            }
        } else
        {
            Debug.Log("issue: 123");
        }
        return returnTile;
    }

    private List<Vector2> GetPossibleSizesToFitSpace(Vector2 spaceSize)
    {
        List<Vector2> returnList = new List<Vector2>();

        foreach(Vector2 space in Tiles.Keys)
        {
            if( ( space.x <= spaceSize.x  && space.y <= spaceSize.y) ||
                (space.y <= spaceSize.x && space.x <= spaceSize.y) )
            {
                returnList.Add(space);
            }
        }
        returnList.Sort((a, b) => b.magnitude.CompareTo(a.magnitude));
        return returnList;
    }
}
