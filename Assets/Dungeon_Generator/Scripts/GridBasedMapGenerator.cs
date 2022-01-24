using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

enum CardinalDirections { North, East, South, West, Max };

struct ChosenDirection
{
    public CardinalDirections DirectionChosen;
    public Vector2Int MapCoordinatesDelta;
}

public class GridBasedMapGenerator : MonoBehaviour
{
    Map2D Grid;
    public Vector2Int Depth; //x is Min y is Max
    bool Spawning = true;
    public Vector2Int CurrentMapCoordinates = Vector2Int.zero;

    private int CurrentDepth = 0;
    private TileSpawnerBasedOnGrid Spawner;

    private bool hasMapBeenProcessed = false;

    // Start is called before the first frame update
    void Start()
    {
        Spawner = GetComponent<TileSpawnerBasedOnGrid>();
        Grid = GetComponent<Map2D>();
        Spawner.AddMapCoordToPath(CurrentMapCoordinates);
        Grid.FillCoordinatesInMap(CurrentMapCoordinates.x, CurrentMapCoordinates.y, (int)MapType.FILLED);
    }

    void ProcessMapGridIntoRooms()
    {
        Vector2Int End = Spawner.Path[Spawner.Path.Count - 1];
        Grid.FillCoordinatesInMap(End.x, End.y, (int)MapType.UN_MERGEABLE);


        int currentRoomID = (int)MapType.MAPTYPE_MAX;

        foreach(Vector2Int gridSpot in Spawner.Path)
        {
            currentRoomID++;

            if (Grid.GetMapTypeAtCoords(gridSpot) == (int)MapType.UN_MERGEABLE) { continue; }

            Vector2Int leftOffset = new Vector2Int(-1,0);
            Vector2Int rightOffset = new Vector2Int(1,0);
            Vector2Int UpOffset = new Vector2Int(0,1);
            Vector2Int DownOffset = new Vector2Int(0,-1);

            // check left

            bool HasValidLeft = (Grid.GetMapTypeAtCoords(gridSpot + leftOffset) == (int)MapType.FILLED
                                    || Grid.GetMapTypeAtCoords(gridSpot + leftOffset) == currentRoomID);


            // check right
            bool HasValidRight = (Grid.GetMapTypeAtCoords(gridSpot + rightOffset) == (int)MapType.FILLED
                                    || Grid.GetMapTypeAtCoords(gridSpot + rightOffset) == currentRoomID);

            // check up
            bool HasValidUp = (Grid.GetMapTypeAtCoords(gridSpot + UpOffset) == (int)MapType.FILLED
                                    || Grid.GetMapTypeAtCoords(gridSpot + UpOffset) == currentRoomID);

            // check down
            bool HasValidDown = (Grid.GetMapTypeAtCoords(gridSpot + DownOffset) == (int)MapType.FILLED
                                    || Grid.GetMapTypeAtCoords(gridSpot + DownOffset) == currentRoomID);



            Vector2Int N = gridSpot + UpOffset;
            Vector2Int S = gridSpot + DownOffset;
            Vector2Int E = gridSpot + rightOffset;
            Vector2Int W = gridSpot + leftOffset;

            if ( HasValidLeft && HasValidUp )
            {
                // check North East
                Vector2Int NE = gridSpot + UpOffset + rightOffset;

                if (Grid.GetMapTypeAtCoords(NE) != (int)MapType.EMPTY
                                    && Grid.GetMapTypeAtCoords(NE) != (int)MapType.UN_MERGEABLE)
                {
                    // if valid; set map type to currentRoomID
                    Grid.FillCoordinatesInMap(NE.x, NE.y, currentRoomID);
                    Grid.FillCoordinatesInMap(N.x, N.y, currentRoomID);
                    Grid.FillCoordinatesInMap(E.x, E.y, currentRoomID);
                    Grid.FillCoordinatesInMap(gridSpot.x, gridSpot.y, currentRoomID);
                }
            }

            if( HasValidLeft && HasValidDown)
            {
                // check South East
                Vector2Int SE = gridSpot + DownOffset + rightOffset;

                if (Grid.GetMapTypeAtCoords(SE) != (int)MapType.EMPTY
                                    && Grid.GetMapTypeAtCoords(SE) != (int)MapType.UN_MERGEABLE)
                {
                    // if valid; set map type to currentRoomID
                    Grid.FillCoordinatesInMap(SE.x, SE.y, currentRoomID);
                    Grid.FillCoordinatesInMap(S.x, S.y, currentRoomID);
                    Grid.FillCoordinatesInMap(E.x, E.y, currentRoomID);
                    Grid.FillCoordinatesInMap(gridSpot.x, gridSpot.y, currentRoomID);
                }
            }

            if( HasValidRight && HasValidUp )
            {
                // check North West
                Vector2Int NW = gridSpot + UpOffset + leftOffset;

                if (Grid.GetMapTypeAtCoords(NW) != (int)MapType.EMPTY
                                    && Grid.GetMapTypeAtCoords(NW) != (int)MapType.UN_MERGEABLE)
                {
                    // if valid; set map type to currentRoomID
                    Grid.FillCoordinatesInMap(NW.x, NW.y, currentRoomID);
                    Grid.FillCoordinatesInMap(N.x, N.y, currentRoomID);
                    Grid.FillCoordinatesInMap(W.x, W.y, currentRoomID);
                    Grid.FillCoordinatesInMap(gridSpot.x, gridSpot.y, currentRoomID);
                }
            }

            if( HasValidRight && HasValidDown )
            {
                // check South West
                Vector2Int SW = gridSpot + DownOffset + leftOffset;

                if (Grid.GetMapTypeAtCoords(SW) == (int)MapType.EMPTY
                                    && Grid.GetMapTypeAtCoords(SW) != (int)MapType.UN_MERGEABLE)
                {
                    // if valid; set map type to currentRoomID
                    Grid.FillCoordinatesInMap(SW.x, SW.y, currentRoomID);
                    Grid.FillCoordinatesInMap(S.x, S.y, currentRoomID);
                    Grid.FillCoordinatesInMap(W.x, W.y, currentRoomID);
                    Grid.FillCoordinatesInMap(gridSpot.x, gridSpot.y, currentRoomID);
                }
            }

            //Debug.Log(Grid.ToString());
        }

    }

    // Update is called once per frame
    void Update()
    {
        if( Spawning )
        {


            Vector2Int newPotentialCoordinates = CurrentMapCoordinates;

            List<int> possibleDirections = new List<int>{ 0,1,2,3 };
            bool BreakOut = false;
            int loopcount = 0;
            ChosenDirection chosenDirection = new ChosenDirection();
            do
            {
                loopcount++;
                if(loopcount > 10) {
                    BreakOut = true;
                    Debug.Log("Failure");
                    break;
                }


                chosenDirection = GetCoordinatesDeltaForNewPotentialDirection(possibleDirections.ToArray());
                newPotentialCoordinates = CurrentMapCoordinates + chosenDirection.MapCoordinatesDelta;
                possibleDirections.Remove((int)chosenDirection.DirectionChosen);

                if (possibleDirections.Count() == 0 || chosenDirection.DirectionChosen == CardinalDirections.Max)
                {
                    BreakOut = true;
                }

            } while (!Grid.CheckIfCoordinatesFilled(newPotentialCoordinates.x, newPotentialCoordinates.y) || BreakOut);
        
            if( !BreakOut)
            {
                Grid.FillCoordinatesInMap(newPotentialCoordinates.x, newPotentialCoordinates.y, (int)MapType.FILLED);
                CurrentMapCoordinates = newPotentialCoordinates;
                Spawner.AddMapCoordToPath(CurrentMapCoordinates);
            } else
            {
                Spawner.Spawn = true;
                Spawning = false;

            if(!hasMapBeenProcessed) {
                Debug.Log("StartProcessing 1 : " + Spawner.Path.Count);
                ProcessMapGridIntoRooms();
                hasMapBeenProcessed = true;
            }
            }

            CurrentDepth++;
            if (CurrentDepth >= Depth.y)
            {
                Spawner.Spawn = true;
                Spawning = false;

            if(!hasMapBeenProcessed) {
                Debug.Log("StartProcessing 2 : " + Spawner.Path.Count);
                ProcessMapGridIntoRooms();
                hasMapBeenProcessed = true;
            }
            }
        }
    }

    ChosenDirection GetCoordinatesDeltaForNewPotentialDirection(int[] possibleDirections)
    {
        ChosenDirection chosenDirection = new ChosenDirection();

        chosenDirection.DirectionChosen = (CardinalDirections)Random.Range(0, possibleDirections.Count());

        if( possibleDirections.Length <= 0)
        {
            chosenDirection.DirectionChosen = CardinalDirections.Max;
            return chosenDirection;
        }

        switch (possibleDirections[(int)chosenDirection.DirectionChosen])
        {
            case (int)CardinalDirections.North:
                chosenDirection.MapCoordinatesDelta = new Vector2Int(0, 1);
                break;
            case (int)CardinalDirections.East:
                chosenDirection.MapCoordinatesDelta = new Vector2Int(1, 0);
                break;
            case (int)CardinalDirections.South:
                chosenDirection.MapCoordinatesDelta = new Vector2Int(0, -1);
                break;
            case (int)CardinalDirections.West:
                chosenDirection.MapCoordinatesDelta = new Vector2Int(-1, 0);
                break;
        }

        return chosenDirection;
    }

}
