using System;
using System.Collections.Generic;
using System.Windows.Documents;
using EvoCarcassonne.Controller;
using EvoCarcassonne.Model;

namespace EvoCarcassonne.Backend
{
    public class Road : ILandscape
    {
        private BoardTile FirstTile { get; set; }
        private BoardTile LastTile { get; set; }
        private bool Gameover { get; set; }
        private bool IsRoadFinished { get; set; } = true;

        private CardinalDirection WhereToGoAfterEndOfRoadFound;
       
        private List<IFigure> FiguresOnTiles { get; set; } = new List<IFigure>();

        public Road()
        {
        }

        public void calculate(BoardTile currentTile, bool gameover)
        {
            int result = 0;
            CheckFigureOnTile(currentTile);
            Gameover = gameover;
            /*if (!Gameover && currentTile.Coordinates.X == FirstTile.Coordinates.X && 
                currentTile.Coordinates.Y == FirstTile.Coordinates.Y)
            {
                LastTile = currentTile;
                result = 0;
            }*/
            FirstTile = currentTile;
        
            if (gameover)
            {
                for (var i = 0; i < currentTile.BackendTile.Directions.Count; i++)
                {
                    if (currentTile.BackendTile.Directions[i].Landscape is Road)
                    {
                        result += CalculateWithDirections(currentTile, (CardinalDirection) i);
                    }
                }
                if (FirstTile.Coordinates.X != LastTile.Coordinates.X && FirstTile.Coordinates.Y != LastTile.Coordinates.Y)
                {
                    result += 1;
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    if (IsEndOfRoad(currentTile) && currentTile.BackendTile.Directions[i].Landscape is Road)
                    {
                        result += CalculateWithDirections(currentTile, (CardinalDirection)i);
                    }
                    else if (!IsEndOfRoad(currentTile) && currentTile.BackendTile.Directions[i].Landscape is Road 
                                                       && SearchEndOfRoadTileInGivenDirection(currentTile, (CardinalDirection)i) != null)
                    {
                        result += CalculateWithDirections(SearchEndOfRoadTileInGivenDirection(currentTile, (CardinalDirection)i), WhereToGoAfterEndOfRoadFound);
                    }
                }
                /*If the road does not end with the same tile its started, then increase the result*/
                if (FirstTile.Coordinates.X != LastTile.Coordinates.X &&
                    FirstTile.Coordinates.Y != LastTile.Coordinates.Y)
                {
                    result += 1;
                }
                
                /*If the road is not finished, then result should be 0*/
                if (!IsRoadFinished)
                {
                    result = 0;
                }
            }

            Console.WriteLine(@"Figures found:    "+FiguresOnTiles.Count);
            if(FiguresOnTiles.Count != 0)
            {
                FiguresOnTiles[0].Owner.Points += result;
            }
        }


        /**
         * Calculate a road's length and gives back the points earned from finishing it. Returns 0 if the road is not finished.
         */
        private int CalculateWithDirections(BoardTile currentTile, CardinalDirection whereToGo)
        {
            Console.WriteLine(currentTile);
            int result = 1;
            Dictionary<CardinalDirection, BoardTile> tilesNextToTheGivenTile = Utils.GetSurroundingTiles(currentTile);
            BoardTile neighborTile = Utils.GetNeighborTile(tilesNextToTheGivenTile, whereToGo);
            
            /*If the neighbor tile does not exist then return result and set current tile as the last tile and sets IsRoadFinished false*/
            if (neighborTile == null)
            {
                LastTile = currentTile;
                IsRoadFinished = false;
                return result;
            }
            if (IsEndOfRoad(neighborTile))
            {
                LastTile = neighborTile;
                Console.WriteLine(neighborTile);
                return result;
            }
            result = SearchInTilesSides(result, neighborTile, (int)Utils.GetOppositeDirection(whereToGo));
            
            return result;
        }

        public override bool Equals(object obj)
        {
            return obj is Road;
        }

        /// <summary>
        /// Checks whether the given tile has any figure on it and is road.
        /// </summary>
        /// <param name="currentTile"></param>
        private void CheckFigureOnTile(BoardTile currentTile)
        {
            foreach (var direction in currentTile.BackendTile.Directions)
            {
                if (direction.Figure != null && direction.Landscape is Road)
                {
                    FiguresOnTiles.Add(direction.Figure);
                }
            }
        }

        private void CheckFigureOnTile(BoardTile currentTile, int onlySideToCheck)
        {
            if (currentTile.BackendTile.Directions[onlySideToCheck].Figure != null)
            {
                FiguresOnTiles.Add(currentTile.BackendTile.Directions[onlySideToCheck].Figure);
            }
        }

        private int SearchInTilesSides(int result, BoardTile neighborTile, int sideNumber)
        {
            for (int i = 0; i < 4; i++)
            {
                if (neighborTile.BackendTile.Directions[i].Landscape is Road)
                {
                    CheckFigureOnTile(neighborTile);
                }
                if (neighborTile.BackendTile.Directions[i].Landscape is Road && i != sideNumber)
                {
                    result += CalculateWithDirections(neighborTile, neighborTile.BackendTile.GetCardinalDirectionByIndex(i));
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// It is supposed to be called after IsFinishedRoad has at least one true value. With the parameters it is going to go, and find the end of road, which is just put down.
        /// </summary>
        /// <param name="currentTile">The tile which has just been put down</param>
        /// <param name="whereToGo">The direction where to search for end of road tile</param>
        /// <returns>The end of road tile found in the given direction. Null if there is no end of road tile</returns>
        private BoardTile SearchEndOfRoadTileInGivenDirection(BoardTile currentTile, CardinalDirection whereToGo)
        {
            BoardTile neighborTile = Utils.GetNeighborTile(Utils.GetSurroundingTiles(currentTile), whereToGo);
            if (neighborTile == null || IsEndOfRoad(neighborTile))
            {
                WhereToGoAfterEndOfRoadFound = Utils.GetOppositeDirection(whereToGo);
                return neighborTile;
            }
            for (var i = 0; i < 4; i++)
            {
                if (neighborTile.BackendTile.Directions[i].Landscape is Road && i != (int)Utils.GetOppositeDirection(whereToGo))
                {
                    return SearchEndOfRoadTileInGivenDirection(neighborTile, neighborTile.BackendTile.GetCardinalDirectionByIndex(i));
                }
            }
            return null;
        }

        /// <summary>
        /// Checks whether the given tile is end of a road or not
        /// </summary>
        /// <param name="currentTile">The examined tile</param>
        /// <returns>True if the given tile is end of road, false if not</returns>
        public bool IsEndOfRoad(BoardTile currentTile)
        {
            return currentTile.BackendTile.Speciality.Contains(Speciality.EndOfRoad);
        }
    }
}
