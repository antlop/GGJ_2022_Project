using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TileSpawnerBasedOnGrid : MonoBehaviour
{
    public GameObject EnemySpawner;
    public List<GameObject> MapTiles;
    public GameObject DoorwayPrefab;
    public List<Vector2Int> Path;
    public float SingleUnitConversion = 10;
    public GameObject BaseTile;

    public GameObject PreviousTile;
    private Vector2Int PreviousCoord;
    private int TileID = 0;
    Map2D Grid;
    private bool UsedInitialTile = false;
    private bool _spawn = false;

    public int EnemySpawnRate = 1;
    public int currentEnemySpawn = 0;

    public bool Spawn {
        get { return _spawn; }
        set {
            _spawn = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Path == null)
        {
            Path = new List<Vector2Int>();
        }
        Grid = GetComponent<Map2D>();
        PreviousCoord = new Vector2Int(-99999, -99999);
    }

    // Update is called once per frame
    void Update()
    {
        if (_spawn)
        {
            if (Path.Count > 0)
            {
                SpawnTileAtCurrentLocation(Path[0]);
                Path.RemoveAt(0);
            } else
            {
                Spawn = false;
                EnableNavMesh();
            }
        }
    }

    private void EnableNavMesh()
    {
        if(BaseTile)
        {
            BaseTile.transform.Find("Plane").GetComponent<NavMeshSurface>().BuildNavMesh();
        }
    }

    public void AddMapCoordToPath(int x, int y)
    {
        AddMapCoordToPath(new Vector2Int(x, y));
    }

    public void AddMapCoordToPath(Vector2Int coord)
    {
        if (Path == null)
        {
            Path = new List<Vector2Int>();
        }

        Path.Add(coord);
    }

    void SpawnTileAtCurrentLocation(Vector2Int coord)
    {
        GameObject tile;
        if (!UsedInitialTile)
        {
            tile = BaseTile;
            UsedInitialTile = true;
        } else
        {
            GameObject tileToSpawn = null;

            if (PreviousTile)
            {
                BaseTile prevBaseTile = PreviousTile.GetComponent<BaseTile>();
                if (prevBaseTile)
                {
                    tileToSpawn = prevBaseTile.GetBaseTileToSpawn();
                }
            }

            if( !tileToSpawn )
            {
                tileToSpawn = MapTiles[0];
            }

            tile = Instantiate(tileToSpawn, transform);
        }

        tile.transform.position = new Vector3(coord.x * SingleUnitConversion, 0, coord.y * SingleUnitConversion);
        float offsetX = Grid.baseSize.x * SingleUnitConversion * -0.5f;
        float offsetZ = Grid.baseSize.y * SingleUnitConversion * -0.5f;
        tile.transform.Translate(new Vector3(offsetX, 0, offsetZ));

        tile.name = TileID.ToString();
        TileID++;

        currentEnemySpawn++;
        if (currentEnemySpawn >= EnemySpawnRate && EnemySpawner)
        {
            GameObject obj = Instantiate(EnemySpawner, tile.transform.Find("Plane").position, Quaternion.identity, tile.transform);
            obj.transform.position += new Vector3(0, 1, 0);
            currentEnemySpawn = 0;
        }

        //Break down walls
        if (PreviousTile && (PreviousCoord.x != -99999 && PreviousCoord.y != -99999))
        {
            float percentToSpawnDoor = Random.Range(0f, 100f);

            //if x == 1 y == 0 SOUTH
            //if x == -1 y == 0 NORTH
            //if x == 0 y == -1 WEST
            //if x == 0 y == 1 EAST
            Vector2Int delta = PreviousCoord - coord;
            string prevWall = "";
            string newWall = "";

            if( delta.x == 1 && delta.y == 0)
            {
                prevWall = "West";
                newWall = "East";
            }
            else if(delta.x == -1 && delta.y == 0)
            {
                prevWall = "East";
                newWall = "West";
            }
            else if( delta.x == 0 && delta.y == 1)
            {
                prevWall = "South";
                newWall = "North";
            }
            else if( delta.x == 0 && delta.y == -1)
            {
                prevWall = "North";
                newWall = "South";
            }

            if(percentToSpawnDoor <= 30.0f)
            {
                GameObject door = Instantiate(DoorwayPrefab, tile.transform.Find("Walls"));
                door.transform.position = tile.transform.Find("Walls").Find(newWall).position;
                door.transform.rotation = tile.transform.Find("Walls").Find(newWall).rotation;
                door.transform.localScale = new Vector3(1, 1, 1);
            }

            Transform prevWalls = PreviousTile.transform.Find("Walls");
            if (prevWalls && prevWalls.Find(prevWall) && prevWalls.Find(prevWall).gameObject)
            {
                DestroyImmediate(PreviousTile.transform.Find("Walls").Find(prevWall).gameObject);
            }
            Transform tileWalls = tile.transform.Find("Walls");
            if (tileWalls && tileWalls.Find(newWall) && tileWalls.Find(newWall).gameObject != null)
            {
                DestroyImmediate(tile.transform.Find("Walls").Find(newWall).gameObject);
            }
            PreviousTile.name += "_" + prevWall;

           
        }

        PreviousTile = tile;
        PreviousCoord = coord;
    }
}
