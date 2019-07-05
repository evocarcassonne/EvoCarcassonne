using System;
using System.Collections.Generic;
using EvoCarcassonne.Models;

namespace EvoCarcassonne.Backend
{
    public class Road : ILandscape
    {
        private BoardTile FirstTile { get; set; }
        private BoardTile LastTile { get; set; }
        private bool Gameover { get; set; }
        private bool IsRoadFinished { get; set; } = true;

        private Utils _utils;

        private CardinalDirection _whereToGoAfterEndOfRoadFound;
       
        private List<IFigure> FiguresOnTiles { get; set; } = new List<IFigure>();

        public Road()
        {
        }

        public void calculate(BoardTile currentTile, bool gameover, Utils utils)
        {
            _utils = utils;
            int result = 0;
            Gameover = gameover;
            FirstTile = currentTile;
            if (gameover)
            {
                for (var i = 0; i < currentTile.BackendTile.Directions.Count; i++)
                {
                    if (currentTile.BackendTile.Directions[i].Landscape is Road)
                    {
                        CheckFigureOnTile(currentTile, i);
                        result += CalculateWithDirections(currentTile, (CardinalDirection) i);
                    }
                    if (IsEndOfRoad(currentTile))
                    {
                        if (!(FirstTile.Coordinates.X == LastTile.Coordinates.X &&
                              FirstTile.Coordinates.Y == LastTile.Coordinates.Y) && IsRoadFinished)
                        {
                            result++;
                        }
                        
                        RemoveFiguresFromFinishedRoad(currentTile, (CardinalDirection)i, true);
                        _utils.DistributePoints(result, FiguresOnTiles);
                        result = 0;
                        FiguresOnTiles = new List<IFigure>();
                    }
                }
                if (FirstTile.Coordinates.X != LastTile.Coordinates.X && FirstTile.Coordinates.Y != LastTile.Coordinates.Y)
                {
                    result += 1;
                }
                _utils.DistributePoints(result, FiguresOnTiles);
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    if (IsEndOfRoad(currentTile) && currentTile.BackendTile.Directions[i].Landscape is Road)
                    {
                        CheckFigureOnTile(currentTile, i);
                        result += CalculateWithDirections(currentTile, (CardinalDirection)i);
                        if (!(FirstTile.Coordinates.X == LastTile.Coordinates.X &&
                              FirstTile.Coordinates.Y == LastTile.Coordinates.Y) && IsRoadFinished)
                        {
                            result++;
                        }
                        if (!IsRoadFinished)
                        {
                            result = 0;
                        }
                        else
                        {
                            RemoveFiguresFromFinishedRoad(currentTile, (CardinalDirection) i, true);
                        }
                        IsRoadFinished = true;
                        _utils.DistributePoints(result, FiguresOnTiles);
                        LastTile = null;
                        FiguresOnTiles = new List<IFigure>();
                        result = 0;
                    }
                    else if (!IsEndOfRoad(currentTile) && currentTile.BackendTile.Directions[i].Landscape is Road 
                                                       && SearchEndOfRoadTileInGivenDirection(currentTile, (CardinalDirection)i) != null)
                    {
                        FirstTile = SearchEndOfRoadTileInGivenDirection(currentTile, (CardinalDirection)i);
                        CheckFigureOnTile(FirstTile, (int)_whereToGoAfterEndOfRoadFound);
                        result += CalculateWithDirections(SearchEndOfRoadTileInGivenDirection(currentTile, (CardinalDirection)i), _whereToGoAfterEndOfRoadFound);
                        /*If the road does not end with the same tile its started, then increase the result*/
                        if (LastTile != null && !(FirstTile.Coordinates.X == LastTile.Coordinates.X &&
                                                  FirstTile.Coordinates.Y == LastTile.Coordinates.Y) && IsRoadFinished)
                        {
                            result++;
                        }
                        /*If the road is not finished, then result should be 0*/
                        if (!IsRoadFinished)
                        {
                            result = 0;
                        }
                        else
                        {
                            RemoveFiguresFromFinishedRoad(FirstTile, _whereToGoAfterEndOfRoadFound, true);
                        }
                        _utils.DistributePoints(result, FiguresOnTiles);
                        break;
                    }   
                }
            }
            FiguresOnTiles = new List<IFigure>();
            FirstTile = null;
            LastTile = null;
            IsRoadFinished = true;
        }




        /**
         * Calculate a road's length and gives back the points earned from finishing it. Sets IsRoadFinished false if the road is not finished.
         */
        private int CalculateWithDirections(BoardTile currentTile, CardinalDirection whereToGo)
        {
            int result = 1;
            Dictionary<CardinalDirection, BoardTile> tilesNextToTheGivenTile = _utils.GetSurroundingTiles(currentTile);
            BoardTile neighborTile = _utils.GetNeighborTile(tilesNextToTheGivenTile, whereToGo);
            
            for (int i = 0; i < 4; i++)
            {
                if (currentTile.BackendTile.Directions[i].Landscape is Road && !Gameover && !IsEndOfRoad(currentTile))
                {
                    CheckFigureOnTile(currentTile, i);
                }
            }
            /*If the neighbor tile does not exist then return result and set current tile as the last tile and sets IsRoadFinished false*/
            if (neighborTile == null)
            {
                LastTile = currentTile;
                IsRoadFinished = false;
                return result;
            }

            if (IsEndOfRoad(neighborTile) || neighborTile.Coordinates.Equals(FirstTile.Coordinates))
            {
                CheckFigureOnTile(neighborTile, (int)_utils.GetOppositeDirection(whereToGo));
                LastTile = neighborTile;
                return result;
            }
            return SearchInTilesSides(result, neighborTile, (int)_utils.GetOppositeDirection(whereToGo));
        }

        public override bool Equals(object obj)
        {
            return obj is Road;
        }
        
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
            BoardTile neighborTile = _utils.GetNeighborTile(_utils.GetSurroundingTiles(currentTile), whereToGo);
            if (neighborTile == null || IsEndOfRoad(neighborTile) || FirstTile.Coordinates.Equals(neighborTile.Coordinates))
            {
                _whereToGoAfterEndOfRoadFound = _utils.GetOppositeDirection(whereToGo);
                return neighborTile;
            }

            for (var i = 0; i < 4; i++)
            {
                if (neighborTile.BackendTile.Directions[i].Landscape is Road && i != (int)_utils.GetOppositeDirection(whereToGo))
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
        
        private void RemoveFiguresFromFinishedRoad(BoardTile currentTile, CardinalDirection whereToGo, bool firstCall)
        {
            BoardTile neighborTile = _utils.GetNeighborTile(_utils.GetSurroundingTiles(currentTile), whereToGo);
            if (firstCall)
            {
                _utils.GiveBackFigureToOwner(currentTile.BackendTile.Directions[(int)whereToGo].Figure);
                currentTile.BackendTile.Directions[(int)whereToGo].Figure = null;
            }

            if (neighborTile == null)
            {
                return;
            }
            if (IsEndOfRoad(neighborTile) || neighborTile.Coordinates.Equals(FirstTile.Coordinates))
            {
                _utils.GiveBackFigureToOwner(neighborTile.BackendTile.Directions[(int)_utils.GetOppositeDirection(whereToGo)].Figure);
                neighborTile.BackendTile.Directions[(int)_utils.GetOppositeDirection(whereToGo)].Figure = null;
                return;
            }
            for (int i = 0; i < 4; i++)
            {
                if (neighborTile.BackendTile.Directions[i].Figure != null && neighborTile.BackendTile.Directions[i].Landscape is Road)
                {
                    _utils.GiveBackFigureToOwner( neighborTile.BackendTile.Directions[i].Figure);
                    neighborTile.BackendTile.Directions[i].Figure = null;
                }
            }
            
            for (int i = 0; i < 4; i++)
            {
                if (neighborTile.BackendTile.Directions[i].Landscape is Road && i != (int)_utils.GetOppositeDirection(whereToGo))
                {
                    RemoveFiguresFromFinishedRoad(neighborTile, (CardinalDirection)i, false);
                    break;
                }
            }
        }
    }
}
