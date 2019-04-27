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
        public int calculate(BoardTile currentTile, CardinalDirection whereToGo, bool firstCall)
        {
            Console.WriteLine(currentTile);
            if (currentTile.BackendTile.Speciality == Speciality.EndOfRoad && !firstCall)
            {
                lastTile = currentTile;
                return 1;
            }

            if (firstCall)
            {
                firstTile = currentTile;
            }

            if (currentTile.BackendTile.Speciality != Speciality.EndOfRoad && firstCall)
            {
                return 0;
            }

            int result = 1;
            Dictionary<CardinalDirection, BoardTile> tilesNextToTheGivenTile =
                new Dictionary<CardinalDirection, BoardTile>();

            tilesNextToTheGivenTile = Utils.GetSurroundingTiles(currentTile);
            BoardTile neighborTile;


            switch (whereToGo)
            {
                case CardinalDirection.East:
                    neighborTile = Utils.getNeighborTile(tilesNextToTheGivenTile, whereToGo);

                    if (IsEndOfRoad(neighborTile))
                    {
                        lastTile = neighborTile;
                        Console.WriteLine(neighborTile);
                        return result;
                    }

                    for (int i = 0; i < 4; i++)
                    {
                       
                        if (neighborTile.BackendTile.Directions[i].Landscape is Road && i != 3)
                        {
                            result += calculate(neighborTile, neighborTile.BackendTile.GetCardinalDirectionByIndex(i), false);
                            break;
                        }
                    }
                    break;

                case CardinalDirection.West:
                    neighborTile = Utils.getNeighborTile(tilesNextToTheGivenTile, whereToGo);

                    if (IsEndOfRoad(neighborTile))
                    {
                        lastTile = neighborTile;
                        Console.WriteLine(neighborTile);
                        return result;
                    }

                    for (int i = 0; i < 4; i++)
                    {
                        
                        if (neighborTile.BackendTile.Directions[i].Landscape is Road && i != 1)
                        {
                            result += calculate(neighborTile, neighborTile.BackendTile.GetCardinalDirectionByIndex(i), false);
                            break;
                            
                        }
                    }

                    break;
                case CardinalDirection.North:

                    neighborTile = Utils.getNeighborTile(tilesNextToTheGivenTile, whereToGo);

                    if (IsEndOfRoad(neighborTile))
                    {
                        lastTile = neighborTile;
                        Console.WriteLine(neighborTile);
                        return result;
                    }

                    for (int i = 0; i < 4; i++)
                    {
                        
                        if (neighborTile.BackendTile.Directions[i].Landscape is Road && i != 2)
                        {
                            result += calculate(neighborTile, neighborTile.BackendTile.GetCardinalDirectionByIndex(i), false);
                            break;
                        }
                    }

                    break;
                case CardinalDirection.South:

                    neighborTile = Utils.getNeighborTile(tilesNextToTheGivenTile, whereToGo);

                    if (IsEndOfRoad(neighborTile))
                    {
                        lastTile = neighborTile;
                        Console.WriteLine(neighborTile);
                        return result;
                    }

                    for (int i = 0; i < 4; i++)
                    {
                        if (neighborTile.BackendTile.Directions[i].Landscape is Road && i != 0)
                        {
                            result += calculate(neighborTile, neighborTile.BackendTile.GetCardinalDirectionByIndex(i), false);
                        }
                    }

                    break;
                default: return 0;
            }

            if (firstCall && firstTile.BackendTile.TileID != lastTile.BackendTile.TileID)
            {
                result += 1;
            }

            return result;
        }


        public override bool Equals(object obj)
        {
            return obj is Road;
        }

        private bool IsEndOfRoad(BoardTile neighborTile)
        {
            if (neighborTile == null || neighborTile.BackendTile.Speciality == Speciality.EndOfRoad)
            {
                return true;
            }
            return false;
        }
    }
}
