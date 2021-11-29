using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
//using System;


[System.Serializable]
public struct WFC_Tile
{
    public int TileID;
    public string MeshName;
    public int MeshRotation;
    public int constrain_to;
    public int constrains_from;
    public int weight;
    public WFC_Socket[] Sockets;

    public WFC_Tile(int id, string mesh, int rot, int constrainTo, int constrainFrom, int weight, int socketsCount)
    {
        TileID = id;
        MeshName = mesh;
        MeshRotation = rot;
        constrain_to = constrainTo;
        constrains_from = constrainFrom;
        this.weight = weight;
        Sockets = new WFC_Socket[socketsCount];
        for(int i = 0; i < socketsCount; i++)
        {
            Sockets[i] = new WFC_Socket(Vector2.zero, null);
        }
    }
}

[System.Serializable]
public struct WFC_Socket
{
    public Vector2 socket_Position;
    public List<int> valid_neighbours;

    public WFC_Socket(Vector2 position, List<int> neighbours)
    {
        socket_Position = position;
        valid_neighbours = neighbours;

        if (neighbours == null)
        {
            valid_neighbours = new List<int>();
        }
    }
}

[System.Serializable]
public struct WallRestrictions {

    public int[] LeftWallRestrictions;
    public int[] RightWallRestrictions;
    public int[] TopWallRestrictions;
    public int[] BottomWallRestrictions;
}


public class WFC_Tiles : MonoBehaviour
{
    public string Seed;
    public Vector2 PlacementOffset;

    public WFC_Tile[] prototypeTiles;
    public Vector2Int MapSize;
    public WFC_Tile[,] _map;
    public WallRestrictions Restrictions;

    public List<int> mapIndiciesStillInASuperposition;

    public List<GameObject> prefabs;

    bool _startingNewMap = true;
    GameObject parentToMap;

    private void Awake()
    {

        prefabs = new List<GameObject>();
        foreach (Transform obj in transform.GetComponentInChildren<Transform>())
        {
            prefabs.Add(obj.gameObject);
        }
    }

    bool Load()
    {
        string location = Application.persistentDataPath + "/WFC_Tiles";
        string jsonDB = System.IO.File.ReadAllText(location);
        if (jsonDB.Length > 0)
        {
            prototypeTiles = JsonConvert.DeserializeObject<WFC_Tile[]>(jsonDB);
            return true;
        }

        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!Load())
        {
            string json = JsonConvert.SerializeObject(prototypeTiles);
            string location = Application.persistentDataPath + "/WFC_Tiles";
            System.IO.File.WriteAllText(location, json);
        }



        // ****** TESTING *******
        
        for (int i = 0; i < 4; i++) {

            prototypeTiles[0].Sockets[i].valid_neighbours = new List<int>();
            prototypeTiles[0].Sockets[i].valid_neighbours.Add(0);
            prototypeTiles[0].Sockets[i].valid_neighbours.Add(1);
            prototypeTiles[0].Sockets[i].valid_neighbours.Add(2);
            prototypeTiles[0].Sockets[i].valid_neighbours.Add(3);
            prototypeTiles[0].Sockets[i].valid_neighbours.Add(4);
            prototypeTiles[0].Sockets[i].valid_neighbours.Add(5);
            prototypeTiles[0].Sockets[i].valid_neighbours.Add(6);
            prototypeTiles[0].Sockets[i].valid_neighbours.Add(7);
            prototypeTiles[0].Sockets[i].valid_neighbours.Add(8);
          prototypeTiles[0].Sockets[i].valid_neighbours.Add(9);
          prototypeTiles[0].Sockets[i].valid_neighbours.Add(10);
          prototypeTiles[0].Sockets[i].valid_neighbours.Add(11);
          prototypeTiles[0].Sockets[i].valid_neighbours.Add(12);
        }

        // **********************


        GenerateAMap(Seed, MapSize);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateAMap(string seed, Vector2Int size)
    {
        MapSize = size;
        Seed = seed;

        GeneratingANewMap();
    }

    void GeneratingANewMap()
    {
        mapIndiciesStillInASuperposition = new List<int>();
        for (int i = 0; i < MapSize.x * MapSize.y; i++)
        {
            mapIndiciesStillInASuperposition.Add(i);
        }

        _map = new WFC_Tile[MapSize.x, MapSize.y];

        for (int x = 0; x < MapSize.x; x++)
        {
            for (int y = 0; y < MapSize.y; y++)
            {
                _map[x, y] = new WFC_Tile(x + y * x, "", 0, 0, 0, 0, 1);
                for (int i = 0; i < prototypeTiles.Length; i++)
                {
                    _map[x, y].Sockets[0].valid_neighbours.Add(i);
                    _map[x, y].Sockets[0].socket_Position = new Vector2(x, y);
                }
            }
        }

        if (!Seed.Equals(""))
        {
            int totalValue = 0;
            foreach (char c in Seed)
            {
                totalValue += System.Convert.ToInt32(c);
            }
            Random.InitState(totalValue);
        }

        if (parentToMap)
        {
            DestroyImmediate(parentToMap);
        }
        
        parentToMap = new GameObject("WFC_Tiles_Parent");

        int randx = Random.Range(0, MapSize.x);
        int randy = Random.Range(0, MapSize.y);

        AssignPrototypeToMapCell(new Vector2(randx, randy), Random.Range(0, prototypeTiles.Length));
        StartTheGenerationProcess(new Vector2(randx, randy));
    }

    void StartTheGenerationProcess(Vector2 startingCoord)
    {
        if(_startingNewMap )
        {
            _startingNewMap = false;
            GeneratingANewMap();
        }


        Queue<Vector2> tilesStack = new Queue<Vector2>();

        tilesStack.Enqueue(startingCoord);
        while(tilesStack.Count > 0)
        {
            Vector2 currCoords = tilesStack.Dequeue();

            foreach(Vector2 dir in GetValidDirections(currCoords))
            {
                // --
                Vector2 adjustedCoords = currCoords + dir;

                List<int> indiciesToMaintain = new List<int>();

                foreach(int neighbour in _map[(int)adjustedCoords.x, (int)adjustedCoords.y].Sockets[0].valid_neighbours)
                {
                    foreach (int prototypeIndex in _map[(int)currCoords.x, (int)currCoords.y].Sockets[0].valid_neighbours)
                    {
                        WFC_Tile prototypeTile = prototypeTiles[prototypeIndex];
                        int socketIndex = GetSocketIndexFromDirection(dir);

                        foreach (int validNeighbour in prototypeTile.Sockets[socketIndex].valid_neighbours)
                        {
                            if (!CheckWallRestrictions(adjustedCoords,neighbour) && neighbour == validNeighbour && !indiciesToMaintain.Contains(validNeighbour))
                            {
                                indiciesToMaintain.Add(validNeighbour);
                                
                            }
                        }

                        if (indiciesToMaintain.Count < 1)
                        {
                            Debug.Log("Issue found at: " + adjustedCoords);
                        }
                    }
                }

                if(indiciesToMaintain.Count == 1)
                {
                    AssignPrototypeToMapCell(adjustedCoords, indiciesToMaintain[0]);
                } else
                {
                    if( indiciesToMaintain.Count != _map[(int)adjustedCoords.x, (int)adjustedCoords.y].Sockets[0].valid_neighbours.Count)
                    {
                        tilesStack.Enqueue(adjustedCoords);
                    }

                    _map[(int)adjustedCoords.x, (int)adjustedCoords.y].Sockets[0].valid_neighbours.Clear();

                    foreach(int neighbour in indiciesToMaintain)
                    {
                        _map[(int)adjustedCoords.x, (int)adjustedCoords.y].Sockets[0].valid_neighbours.Add(neighbour);
                    }
                }
            }
        }

        if (mapIndiciesStillInASuperposition.Count > 0)
        {
            mapIndiciesStillInASuperposition.Sort((p1, p2) => _map[p1 % MapSize.x, p1 / MapSize.x].Sockets[0].valid_neighbours.Count.CompareTo(_map[p2 % MapSize.x, p2 / MapSize.x].Sockets[0].valid_neighbours.Count));

            Vector2 coords = new Vector2(mapIndiciesStillInASuperposition[0] % MapSize.x, mapIndiciesStillInASuperposition[0] / MapSize.x);
            int randIndex = Random.Range(0, _map[(int)coords.x, (int)coords.y].Sockets[0].valid_neighbours.Count);
            if(_map[(int)coords.x, (int)coords.y].Sockets[0].valid_neighbours.Count < 1)
            {
                Debug.Log("no neighbors found at " + coords);
            }
            int prototypeID = _map[(int)coords.x, (int)coords.y].Sockets[0].valid_neighbours[randIndex];
            AssignPrototypeToMapCell(coords, prototypeID);
            StartTheGenerationProcess(coords);
        }
        else
        {
#if UNITY_EDITOR
            int printTotal = 0;
            foreach(WFC_Tile tile in _map)
            {
                printTotal += tile.TileID;
            }
            Debug.Log("Finished - " + printTotal);
#endif
            SpawnMapGameObjects();
        }
    }

    List<Vector2> GetValidDirections(Vector2 currCoord)
    {
        List<Vector2> retList = new List<Vector2>();

        Vector2[] newPositions = new Vector2[4] {   new Vector2(1, 0),      // left
                                                    new Vector2(-1, 0),     // right
                                                    new Vector2(0,1),       // up
                                                    new Vector2(0,-1) };    // down

   
        foreach (Vector2 pos in newPositions)
        {
            int newX = (int)(currCoord.x + pos.x);
            int newY = (int)(currCoord.y + pos.y);

            if (newX >= 0 &&
                newX < MapSize.x &&
                newY >= 0 &&
                newY < MapSize.y)
            {

                if (_map[newX, newY].TileID > -1)
                {
                    retList.Add(new Vector2(pos.x, pos.y));
                }
            }
        }

        return retList;
    }

    void AssignPrototypeToMapCell(Vector2 mapIndex, int prototypeIndex)
    {
        if (prototypeIndex >= 0 && prototypeIndex < prototypeTiles.Length &&
            mapIndex.x >= 0 && mapIndex.y >= 0 && mapIndex.x < MapSize.x && mapIndex.y < MapSize.y)
        {
            _map[(int)mapIndex.x, (int)mapIndex.y].MeshName = prototypeTiles[prototypeIndex].MeshName;
            _map[(int)mapIndex.x, (int)mapIndex.y].MeshRotation = prototypeTiles[prototypeIndex].MeshRotation;
            _map[(int)mapIndex.x, (int)mapIndex.y].TileID = prototypeTiles[prototypeIndex].TileID;

            mapIndiciesStillInASuperposition.Remove((int)(mapIndex.x + MapSize.x * mapIndex.y));
        }
    }

    int GetSocketIndexFromDirection(Vector2 dir)
    {
        if(dir.x == 0)
        {
            if( dir.y > 0)
            {
                return 2;
            } else
            {
                return 3;
            }
        } else if( dir.x > 0)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }

    void SpawnMapGameObjects()
    {
        for (int x = 0; x < MapSize.x; x++)
        {
            for(int y = 0; y < MapSize.y; y++)
            {
                WFC_Tile tile = _map[x, y];
                GameObject obj = Instantiate(prefabs[tile.TileID], new Vector3(x * PlacementOffset.x, 0, y * PlacementOffset.y), Quaternion.identity);

               
                
                obj.transform.Rotate(Vector3.up, tile.MeshRotation);
                
                switch(tile.MeshRotation)
                {
                    case 90:
                        obj.transform.Translate(new Vector3(0, 0, -3), Space.World);
                        break;
                    case 180:
                        obj.transform.Translate(new Vector3(-3, 0, -3), Space.World);
                        break;
                    case 270:
                        obj.transform.Translate(new Vector3(-3, 0, 0), Space.World);
                        break;
                }

                obj.transform.parent = parentToMap.transform;
            }
        }
    }

    bool CheckWallRestrictions(Vector2 coord, int tileID)
    {
        if (coord.x == 0) // left wall
        {
            if (Restrictions.LeftWallRestrictions.Length > 0)
            {
                foreach (int id in Restrictions.LeftWallRestrictions)
                {
                    if (tileID.Equals(id))
                    {
                        return true;
                    }
                }
            }
        }
        else if (coord.x == MapSize.x - 1) //right wall 
        {
            if (Restrictions.RightWallRestrictions.Length > 0)
            {
                foreach (int id in Restrictions.RightWallRestrictions)
                {
                    if (tileID.Equals(id))
                    {
                        return true;
                    }
                }
            }
        }

        if (coord.y == 0) // bottom wall
        {
            if (Restrictions.BottomWallRestrictions.Length > 0)
            {
                foreach (int id in Restrictions.BottomWallRestrictions)
                {
                    if (tileID.Equals(id))
                    {
                        return true;
                    }
                }
            }
        }
        else if (coord.y == MapSize.y - 1) // top wall
        {
            if (Restrictions.TopWallRestrictions.Length > 0)
            {
                foreach (int id in Restrictions.TopWallRestrictions)
                {
                    if (tileID.Equals(id))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
