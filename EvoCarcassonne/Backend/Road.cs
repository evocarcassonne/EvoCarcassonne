using System;
using System.Collections.Generic;
using System.Linq;
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

        private CardinalDirection _whereToGoAfterEndOfRoadFound;
       
        private List<IFigure> FiguresOnTiles { get; set; } = new List<IFigure>();

        public Road()
        {
        }

        public void calculate(BoardTile currentTile, bool gameover)
        {
            int result = 0;
            //CheckFigureOnTile(currentTile);
            Gameover = gameover;
        
            if (gameover)
            {
                FirstTile = currentTile;
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
                FirstTile = currentTile;
                for (int i = 0; i < 4; i++)
                {
                    if (IsEndOfRoad(currentTile) && currentTile.BackendTile.Directions[i].Landscape is Road)
                    {
                        result += CalculateWithDirections(currentTile, (CardinalDirection)i);
                    }
                    else if (!IsEndOfRoad(currentTile) && currentTile.BackendTile.Directions[i].Landscape is Road 
                                                       && SearchEndOfRoadTileInGivenDirection(currentTile, (CardinalDirection)i) != null)
                    {
                        FirstTile = SearchEndOfRoadTileInGivenDirection(currentTile, (CardinalDirection)i);
                        result += CalculateWithDirections(SearchEndOfRoadTileInGivenDirection(currentTile, (CardinalDirection)i), _whereToGoAfterEndOfRoadFound);
                        break;
                    }
                    CheckFigureOnTile(FirstTile);
                    /*If the road is not finished, then result should be 0*/
                    if (!IsRoadFinished)
                    {
                        result = 0;
                    }
                }
                /*If the road does not end with the same tile its started, then increase the result*/
                if (!(FirstTile.Coordinates.X == LastTile.Coordinates.X &&
                    FirstTile.Coordinates.Y == LastTile.Coordinates.Y) && IsRoadFinished)
                {
                    result += 1;
                }
               
            }

            Console.WriteLine(@"Figures found:    " + FiguresOnTiles.Count);
            if (FiguresOnTiles.Count != 0)
            {
                DistributePoints(result);
            }
        }


        /**
         * Calculate a road's length and gives back the points earned from finishing it. Sets IsRoadFinished false if the road is not finished.
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
            if (IsEndOfRoad(neighborTile) || neighborTile.Coordinates.Equals(FirstTile.Coordinates))
            {
                CheckFigureOnTile(neighborTile, (int)Utils.GetOppositeDirection(whereToGo));
                LastTile = neighborTile;
                Console.WriteLine(neighborTile);
                return result;
            }
            return SearchInTilesSides(result, neighborTile, (int)Utils.GetOppositeDirection(whereToGo));
        }

        public override bool Equals(object obj)
        {
            return obj is Road;
        }

        /// <summary>
        /// Checks whether the given tile has any figure on it and is road.
        /// </summary>
        /// <param name="currentTile">The examined tile</param>
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

        //TODO: Hogyha nem endofroad-al van meghívva a caluculate és a figura az eggyel mellette lévő tilera van téve, valamiért nem találja meg. Ezt kéne leellenőrizni hogy miért. 
        
        /// <summary>
        /// Checks if the given tile has any figure on it, but only on the given side
        /// </summary>
        /// <param name="currentTile">The examined tile</param>
        /// <param name="onlySideToCheck">The side of tile to be examined</param>
        private void CheckFigureOnTile(BoardTile currentTile, int onlySideToCheck)
        {
            if (currentTile.BackendTile.Directions[onlySideToCheck].Figure != null)
            {
                FiguresOnTiles.Add(currentTile.BackendTile.Directions[onlySideToCheck].Figure);
            }
        }

        /// <summary>
        /// Search the given tile's sides for roads, and if it find one it will cal calculation on it
        /// </summary>
        /// <param name="result">Gets the current number of road tiles</param>
        /// <param name="neighborTile">The tile to be examined</param>
        /// <param name="sideNumber">The integer value of that CardinalDirection where we came from</param>
        /// <returns>The number of tiles found following the road</returns>
        private int SearchInTilesSides(int result, BoardTile neighborTile, int sideNumber)
        {
            for (int i = 0; i < 4; i++)
            {
                if (neighborTile.BackendTile.Directions[i].Landscape is Road && !IsEndOfRoad(neighborTile))
                {
                    CheckFigureOnTile(neighborTile, i);
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
        /// <param name="b"></param>
        /// <returns>The end of road tile found in the given direction. Null if there is no end of road tile</returns>
        private BoardTile SearchEndOfRoadTileInGivenDirection(BoardTile currentTile, CardinalDirection whereToGo)
        {
            BoardTile neighborTile = Utils.GetNeighborTile(Utils.GetSurroundingTiles(currentTile), whereToGo);
            if (neighborTile == null || IsEndOfRoad(neighborTile) || FirstTile.Coordinates.Equals(neighborTile.Coordinates))
            {
                _whereToGoAfterEndOfRoadFound = Utils.GetOppositeDirection(whereToGo);
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
        
        private void DistributePoints(int result)
        {
            var points = new List<int>();
            var players = new List<IOwner>();
            var playersToGetPoints = new List<IOwner>();
            foreach (var i in FiguresOnTiles)
            {
                if (!players.Contains(i.Owner))
                {
                    players.Add(i.Owner);
                }
            }
            int maxIndex = 0;
            for (int i = 0; i < players.Count; i++)
            {
                int currentCount = 0;
                for (int j = 0; j < FiguresOnTiles.Count; j++)
                {
                    if (players[i].Equals(FiguresOnTiles[i].Owner))
                    {
                        currentCount++;
                    }
                }
                points.Add(currentCount);
                if (points[i] > points[maxIndex])
                {
                    maxIndex = i;
                }
            }
            playersToGetPoints.Add(players[maxIndex]);
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i] == points[maxIndex] && i != maxIndex)
                {
                    playersToGetPoints.Add(players[i]);
                }
            }

            foreach (IOwner i in playersToGetPoints)
            {
                i.Points += result / playersToGetPoints.Count;
            }
        }
    }
}
