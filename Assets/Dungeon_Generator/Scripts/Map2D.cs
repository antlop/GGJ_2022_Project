using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MapType { EMPTY, FILLED };

public class Map2D : MonoBehaviour
{
    public bool OutputMap = false;
    public Vector2 baseSize;
    public bool FixedSize = false;

    int[,] Map;
    bool Initialized = false;

    // Start is called before the first frame update
    void Start()
    {
        InitializeMap();
    }

    public void InitializeMap()
    {
        if( Initialized == true ) { return; }

        Map = new int[(int)baseSize.x, (int)baseSize.y];
        for (int x = 0; x < baseSize.x; x++)
        {
            for (int y = 0; y < baseSize.y; y++)
            {
                Map[x, y] = (int)MapType.EMPTY;
            }
        }
        Initialized = true;
    }

    public bool CheckIfCoordinatesFilled(int x, int y)
    {
        if( Map == null) { InitializeMap(); }

        if (x >= 0 &&
            y >= 0 &&
            x < baseSize.x &&
            y < baseSize.y)
        {
            if( Map[x,y] == (int)MapType.EMPTY)
            {
                return true;
            } else
            {
                return false;
            }
        }
        return false;
    }

    public void FillCoordinatesInMap(int x, int y)
    {
        if (Map == null) { InitializeMap(); }

        if (x >= 0 &&
            y >= 0 &&
            x < baseSize.x &&
            y < baseSize.y)
        {
            if( Map[x,y] == (int)MapType.FILLED)
            {
                Debug.Log("Duplicate at: (" + x + "," + y + ")");
            }

            Map[x, y] = (int)MapType.FILLED;
        }
    }

    private void Update()
    {
        if( OutputMap )
        {
            OutputMap = false;
            Debug.Log(this.ToString());
        }
    }

    public override string ToString()
    {
        string returnstring = "";

        for (int x = 0; x < baseSize.x; x++)
        {
            for (int y = 0; y < baseSize.y; y++)
            {
                if ((int)Map[x, y] == 0)
                {
                    returnstring += "[  ]";
                }
                else
                {
                    returnstring += "[x]";
                }
            }
            returnstring += "\n";
        }
        return returnstring;
    }
}
