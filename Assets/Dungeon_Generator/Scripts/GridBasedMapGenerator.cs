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

    // Start is called before the first frame update
    void Start()
    {
        Spawner = GetComponent<TileSpawnerBasedOnGrid>();
        Grid = GetComponent<Map2D>();
        Spawner.AddMapCoordToPath(CurrentMapCoordinates);
        Grid.FillCoordinatesInMap(CurrentMapCoordinates.x, CurrentMapCoordinates.y);
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
                Grid.FillCoordinatesInMap(newPotentialCoordinates.x, newPotentialCoordinates.y);
                CurrentMapCoordinates = newPotentialCoordinates;
                Spawner.AddMapCoordToPath(CurrentMapCoordinates);
            } else
            {
                Spawner.Spawn = true;
                Spawning = false;
            }

            CurrentDepth++;
            if (CurrentDepth >= Depth.y)
            {
                Spawning = false;
                Spawner.Spawn = true;
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
