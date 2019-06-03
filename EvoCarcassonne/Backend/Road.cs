using System;
using System.Collections.Generic;
using EvoCarcassonne.Model;

namespace EvoCarcassonne.Backend
{
    public class Road : ILandscape
    {
        private BoardTile firstTile { get; set; }
        private BoardTile lastTile { get; set; }

        public Road()
        {
        }


        /**
         * Calculate a finished road's length and gives back the points earned from finishing it.
         */
        public int calculate(BoardTile currentTile, CardinalDirection whereToGo, bool firstCall, bool gameover)
        {
            if (!gameover)
            {


                Console.WriteLine(currentTile);
                foreach (var tile in currentTile.BackendTile.Speciality)
                {
                    if (tile == Speciality.EndOfRoad && !firstCall)
                    {
                        lastTile = currentTile;
                        return 1;
                    }
                }

                if (firstCall)
                {
                    firstTile = currentTile;
                }

                foreach (var tile in currentTile.BackendTile.Speciality)
                {
                    if (tile != Speciality.EndOfRoad && firstCall)
                    {
                        return 0;
                    }
                }

                int result = 1;
                Dictionary<CardinalDirection, BoardTile> tilesNextToTheGivenTile =
                    new Dictionary<CardinalDirection, BoardTile>();

                tilesNextToTheGivenTile = Utils.GetSurroundingTiles(currentTile);
                BoardTile neighborTile = Utils.getNeighborTile(tilesNextToTheGivenTile, whereToGo);

                if (IsEndOfRoad(neighborTile))
                {
                    lastTile = neighborTile;
                    Console.WriteLine(neighborTile);
                    return result;
                }

                switch (whereToGo)
                {
                    case CardinalDirection.East:
                        result = searchInTilesSides(result, neighborTile, 3);
                        break;
                    case CardinalDirection.West:
                        result = searchInTilesSides(result, neighborTile, 1);
                        break;
                    case CardinalDirection.North:
                        result = searchInTilesSides(result, neighborTile, 2);
                        break;
                    case CardinalDirection.South:
                        result = searchInTilesSides(result, neighborTile, 0);
                        break;
                    default: return 0;
                }

                if (firstCall && firstTile.BackendTile.TileID != lastTile.BackendTile.TileID)
                {
                    result += 1;
                }

                return result;
            }
            else
            {
                return 0;
            }
        }

        

        public override bool Equals(object obj)
        {
            return obj is Road;
        }

        private bool IsEndOfRoad(BoardTile neighborTile)
        {
            foreach (var tile in neighborTile.BackendTile.Speciality)
            {
                if (neighborTile == null || tile == Speciality.EndOfRoad)
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
                    result += calculate(neighborTile, neighborTile.BackendTile.GetCardinalDirectionByIndex(i), false,false);
                    break;
                }
            }

            return result;
        }
        
    }
}
