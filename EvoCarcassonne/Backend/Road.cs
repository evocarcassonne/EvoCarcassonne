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
            #region Init required values
            Console.WriteLine(currentTile);
            int result = 1;
            Dictionary<CardinalDirection, BoardTile> tilesNextToTheGivenTile = Utils.GetSurroundingTiles(currentTile);
            BoardTile neighborTile = Utils.GetNeighborTile(tilesNextToTheGivenTile, whereToGo);
            if (firstCall)
            {
                Gameover = gameover;
                FirstTile = currentTile;
            }
            #endregion
            
            #region GameOverCall
            if (gameover && firstCall)
            {
                int localResult = 0;
                for (int i = 0; i < currentTile.BackendTile.Directions.Count; i++)
                {
                    if (currentTile.BackendTile.Directions[i].Landscape is Road)
                    {
                        localResult += calculate(currentTile, (CardinalDirection) i, false, true);
                    }
                }
                result = localResult;
                if (FirstTile.Coordinates.X != LastTile.Coordinates.X && FirstTile.Coordinates.Y != LastTile.Coordinates.Y)
                {
                    result += 1;
                }
                return result;
            }
            #endregion
            
            #region NormalCall
            if(!gameover && currentTile.Coordinates.X == FirstTile.Coordinates.X && currentTile.Coordinates.Y == FirstTile.Coordinates.Y && !firstCall)
            {
                LastTile = currentTile;
                return 0;
            }
            #endregion
            
            #region Calculation
            ILandscape backEndLandscape = currentTile.BackendTile.Directions[(int)whereToGo].Landscape;
            /*If the neighbor tile is does not exist or (the neighbor tile does not have the same landscape
             on the connecting side and the neighbor tile is end of road) then return result and set current tile as the last tile*/
            if (neighborTile == null || !backEndLandscape.Equals(neighborTile.BackendTile.Directions[(int)Utils.GetOppositeDirection(whereToGo)].Landscape) && Utils.IsEndOfRoad(neighborTile))
            {
                LastTile = currentTile;
                Console.WriteLine(currentTile);
                return result;
            }
            if (Utils.IsEndOfRoad(neighborTile))
            {
                LastTile = neighborTile;
                Console.WriteLine(neighborTile);
                return result;
            }
            result = SearchInTilesSides(result, neighborTile, (int)Utils.GetOppositeDirection(whereToGo));
            /*If the road does not end with the same tile its started, then increase the result*/
            if (firstCall && FirstTile.Coordinates.X != LastTile.Coordinates.X && FirstTile.Coordinates.Y != LastTile.Coordinates.Y)
            {
                result += 1;
            }
            #endregion
            
            return result;
        }
        
        private int SearchInTilesSides(int result, BoardTile neighborTile, int sideNumber)
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
