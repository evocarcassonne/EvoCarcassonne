using System;
using System.Collections.Generic;
using System.Windows.Documents;
using EvoCarcassonne.Model;

namespace EvoCarcassonne.Backend
{
    public class Road : ILandscape
    {
        private BoardTile FirstTile { get; set; }
        private BoardTile LastTile { get; set; }
        private bool Gameover { get; set; }

        public Road()
        {
        }


        /**
         * Calculate a finished road's length and gives back the points earned from finishing it.
         */
        public int calculate(BoardTile currentTile, CardinalDirection whereToGo, bool firstCall, bool gameover)
        {
            Gameover = gameover;
            Console.WriteLine(currentTile);
            int result = 1;
            Dictionary<CardinalDirection, BoardTile> tilesNextToTheGivenTile =
                new Dictionary<CardinalDirection, BoardTile>();
            tilesNextToTheGivenTile = Utils.GetSurroundingTiles(currentTile);
            BoardTile neighborTile = Utils.getNeighborTile(tilesNextToTheGivenTile, whereToGo);
            if (!gameover)
            {
                if (firstCall)
                {
                    FirstTile = currentTile;
                }
                if (currentTile.Coordinates.X == FirstTile.Coordinates.X && currentTile.Coordinates.Y == FirstTile.Coordinates.Y && !firstCall)
                {
                    LastTile = currentTile;
                    return 0;
                }
            }
            else
            {
                if (firstCall)
                {
                    FirstTile = currentTile;
                    int localResult = 0;
                    int numberOfRoadSides = 0;
                    for (int i = 0; i < currentTile.BackendTile.Directions.Count; i++)
                    {
                        if (currentTile.BackendTile.Directions[i].Landscape is Road)
                        {
                            localResult += calculate(currentTile, (CardinalDirection) i, false, true);
                            numberOfRoadSides++;
                        }
                    }
                    result = localResult;
                    if (FirstTile.Coordinates.X != LastTile.Coordinates.X && FirstTile.Coordinates.Y != LastTile.Coordinates.Y)
                    {
                        result += 1;
                    }
                    return result;
                }
            }
            if (neighborTile == null)
            {
                LastTile = currentTile;
                return result;
            }

            if (!currentTile.BackendTile.Directions[(int)whereToGo].Landscape.Equals(neighborTile.BackendTile.Directions[(int)Utils.getOppositeDirection(whereToGo)].Landscape) && IsEndOfRoad(neighborTile))
            {
                LastTile = currentTile;
                Console.WriteLine(currentTile);
                return result;
            }
            if (IsEndOfRoad(neighborTile))
            {
                LastTile = neighborTile;
                Console.WriteLine(neighborTile);
                return result;
            }
            result = searchInTilesSides(result, neighborTile, (int)Utils.getOppositeDirection(whereToGo));
            if (firstCall && FirstTile.Coordinates.X != LastTile.Coordinates.X && FirstTile.Coordinates.Y != LastTile.Coordinates.Y)
            {
                result += 1;
            }
            return result;
        }

        private bool IsEndOfRoad(BoardTile neighborTile)
        {
            foreach (var tile in neighborTile.BackendTile.Speciality)
            {
                if (tile == Speciality.EndOfRoad)
                {
                    return true;
                }
            }
            return false;
        }
        
        private int searchInTilesSides(int result, BoardTile neighborTile, int sideNumber)
        {
            for (int i = 0; i < 4; i++)
            {
                if (neighborTile.BackendTile.Directions[i].Landscape is Road && i != sideNumber)
                {
                    result += calculate(neighborTile, neighborTile.BackendTile.GetCardinalDirectionByIndex(i), false, Gameover);
                    break;
                }
            }

            return result;
        }
        
        public override bool Equals(object obj)
        {
            return obj is Road;
        }
    }
}
