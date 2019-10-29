using System.Collections.Generic;

namespace Backend
{
    public class Road : ILandscape
    {
        private ITile FirstTile { get; set; }
        private ITile LastTile { get; set; }
        private bool Gameover { get; set; }
        private bool IsRoadFinished { get; set; } = true;

        private bool _canPlaceFigure = true;
        
        private Utils _utils;

        private CardinalDirection _whereToGoAfterEndOfRoadFound;
       
        private List<IFigure> FiguresOnTiles { get; set; } = new List<IFigure>();

        public Road()
        {
        }

        public void calculate(ITile currentTile, bool gameover, Utils utils)
        {
            _utils = utils;
            int result = 0;
            Gameover = gameover;
            FirstTile = currentTile;
            if (gameover)
            {
                for (var i = 0; i < currentTile.Directions.Count; i++)
                {
                    if (currentTile.Directions[i].Landscape is Road)
                    {
                        CheckFigureOnTile(currentTile, i);
                        result += CalculateWithDirections(currentTile, (CardinalDirection) i);
                    }
                    if (IsEndOfRoad(currentTile) && currentTile.Directions[i].Landscape is Road)
                    {
                        if (!(FirstTile.Position.X == LastTile.Position.X &&
                              FirstTile.Position.Y == LastTile.Position.Y) && IsRoadFinished)
                        {
                            result++;
                        }
                        _utils.DistributePoints(result, FiguresOnTiles);
                        result = 0;
                        FiguresOnTiles = new List<IFigure>();
                    }
                    RemoveFiguresFromFinishedRoad(currentTile, (CardinalDirection)i, true);
                }
                if (FirstTile.Position.X != LastTile.Position.X && FirstTile.Position.Y != LastTile.Position.Y)
                {
                    result += 1;
                }
                _utils.DistributePoints(result, FiguresOnTiles);
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    if (IsEndOfRoad(currentTile) && currentTile.Directions[i].Landscape is Road)
                    {
                        CheckFigureOnTile(currentTile, i);
                        result += CalculateWithDirections(currentTile, (CardinalDirection)i);
                        if (!(FirstTile.Position.X == LastTile.Position.X &&
                              FirstTile.Position.Y == LastTile.Position.Y) && IsRoadFinished)
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
                    else if (!IsEndOfRoad(currentTile) && currentTile.Directions[i].Landscape is Road 
                                                       && SearchEndOfRoadTileInGivenDirection(currentTile, (CardinalDirection)i) != null)
                    {
                        FirstTile = SearchEndOfRoadTileInGivenDirection(currentTile, (CardinalDirection)i);
                        CheckFigureOnTile(FirstTile, (int)_whereToGoAfterEndOfRoadFound);
                        result += CalculateWithDirections(SearchEndOfRoadTileInGivenDirection(currentTile, (CardinalDirection)i), _whereToGoAfterEndOfRoadFound);
                        /*If the road does not end with the same tile its started, then increase the result*/
                        if (LastTile != null && !(FirstTile.Position.X == LastTile.Position.X &&
                                                  FirstTile.Position.Y == LastTile.Position.Y) && IsRoadFinished)
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


        public bool CanPlaceFigure(ITile currentTile, CardinalDirection whereToGo, Utils utils, bool firstCall)
        {
            ITile neighborTile = currentTile.getTileSideByCardinalDirection(whereToGo).Neighbor;
            if (firstCall)
            {
                if (IsEndOfRoad(currentTile))
                {
                    return CanPlaceFigure(currentTile, whereToGo, utils, false);
                }
                else
                {
                    CardinalDirection otherSide = CardinalDirection.North;
                    for (int i = 0; i < 4; i++)
                    {
                        if (currentTile.Directions[i].Landscape is Road && i != (int)whereToGo)
                        {
                            otherSide = (CardinalDirection)i;
                        }
                    }

                    return CanPlaceFigure(currentTile, whereToGo, utils, false) &&
                           CanPlaceFigure(currentTile, otherSide, utils, false);
                }
            }
            else
            {
                if (neighborTile == null)
                {
                    return _canPlaceFigure;
                }

                if (IsEndOfRoad(neighborTile))
                {
                    return neighborTile.Directions[(int)utils.GetOppositeDirection(whereToGo)].Figure == null;
                }

                for (int i = 0; i < 4; i++)
                {
                    if (neighborTile.Directions[i].Figure != null &&
                        neighborTile.Directions[i].Landscape is Road)
                    {
                        _canPlaceFigure = false;
                        return false;
                    }
                }

                if (IsEndOfRoad(currentTile))
                {
                    for (var i = 0; i < 4; i++)
                    {
                        if (currentTile.Speciality.Contains(Speciality.EndOfRoad))
                        {
                            if (neighborTile.Directions[i].Landscape is Road && i != (int)utils.GetOppositeDirection(whereToGo))
                            {
                                return CanPlaceFigure(neighborTile, neighborTile.GetCardinalDirectionByIndex(i), utils, false);
                            }
                        }
                        if (neighborTile.Directions[i].Landscape is Road && i != (int)utils.GetOppositeDirection(whereToGo))
                        {
                            return CanPlaceFigure(neighborTile, neighborTile.GetCardinalDirectionByIndex(i), utils, false);
                        }
                    }
                }
                else
                {
                    for (var i = 0; i < 4; i++)
                    {
                        if (neighborTile.Directions[i].Landscape is Road && i != (int)utils.GetOppositeDirection(whereToGo))
                        {
                            return CanPlaceFigure(neighborTile, neighborTile.GetCardinalDirectionByIndex(i), utils, false);
                        }
                    }
                }

            }
            
           
            return true;
        }


        /**
         * Calculate a road's length and gives back the points earned from finishing it. Sets IsRoadFinished false if the road is not finished.
         */
        private int CalculateWithDirections(ITile currentTile, CardinalDirection whereToGo)
        {
            int result = 1;
            Dictionary<CardinalDirection, ITile> tilesNextToTheGivenTile = _utils.GetSurroundingTiles(currentTile);
            ITile neighborTile = _utils.GetNeighborTile(tilesNextToTheGivenTile, whereToGo);
            
            for (int i = 0; i < 4; i++)
            {
                if (currentTile.Directions[i].Landscape is Road && !Gameover && !IsEndOfRoad(currentTile))
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

            if (IsEndOfRoad(neighborTile) || neighborTile.Position.Equals(FirstTile.Position))
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
        private void CheckFigureOnTile(ITile currentTile, int onlySideToCheck)
        {
            if (currentTile.Directions[onlySideToCheck].Figure != null)
            {
                FiguresOnTiles.Add(currentTile.Directions[onlySideToCheck].Figure);
            }
        }

        /// <summary>
        /// Search the given tile's sides for roads, and if it find one it will cal calculation on it
        /// </summary>
        /// <param name="result">Gets the current number of road tiles</param>
        /// <param name="neighborTile">The tile to be examined</param>
        /// <param name="sideNumber">The integer value of that CardinalDirection where we came from</param>
        /// <returns>The number of tiles found following the road</returns>
        private int SearchInTilesSides(int result, ITile neighborTile, int sideNumber)
        {
            for (int i = 0; i < 4; i++)
            {
                if (neighborTile.Directions[i].Landscape is Road && i != sideNumber)
                {
                    result += CalculateWithDirections(neighborTile, neighborTile.GetCardinalDirectionByIndex(i));
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
        private ITile SearchEndOfRoadTileInGivenDirection(ITile currentTile, CardinalDirection whereToGo)
        {
            ITile neighborTile = _utils.GetNeighborTile(_utils.GetSurroundingTiles(currentTile), whereToGo);
            if (neighborTile == null || IsEndOfRoad(neighborTile) || FirstTile.Position.Equals(neighborTile.Position))
            {
                _whereToGoAfterEndOfRoadFound = _utils.GetOppositeDirection(whereToGo);
                return neighborTile;
            }

            for (var i = 0; i < 4; i++)
            {
                if (neighborTile.Directions[i].Landscape is Road && i != (int)_utils.GetOppositeDirection(whereToGo))
                {
                    return SearchEndOfRoadTileInGivenDirection(neighborTile, neighborTile.GetCardinalDirectionByIndex(i));
                }
            }
            return null;
        }

        /// <summary>
        /// Checks whether the given tile is end of a road or not
        /// </summary>
        /// <param name="currentTile">The examined tile</param>
        /// <returns>True if the given tile is end of road, false if not</returns>
        public bool IsEndOfRoad(ITile currentTile)
        {
            return currentTile.Speciality.Contains(Speciality.EndOfRoad);
        }
        
        private void RemoveFiguresFromFinishedRoad(ITile currentTile, CardinalDirection whereToGo, bool firstCall)
        {
            ITile neighborTile = _utils.GetNeighborTile(_utils.GetSurroundingTiles(currentTile), whereToGo);
            if (firstCall)
            {
                _utils.GiveBackFigureToOwner(currentTile.Directions[(int)whereToGo].Figure);
                currentTile.Directions[(int)whereToGo].Figure = null;
            }

            if (neighborTile == null)
            {
                return;
            }
            if (IsEndOfRoad(neighborTile) || neighborTile.Position.Equals(FirstTile.Position))
            {
                _utils.GiveBackFigureToOwner(neighborTile.Directions[(int)_utils.GetOppositeDirection(whereToGo)].Figure);
                neighborTile.Directions[(int)_utils.GetOppositeDirection(whereToGo)].Figure = null;
                return;
            }
            for (int i = 0; i < 4; i++)
            {
                if (neighborTile.Directions[i].Figure != null && neighborTile.Directions[i].Landscape is Road)
                {
                    _utils.GiveBackFigureToOwner( neighborTile.Directions[i].Figure);
                    neighborTile.Directions[i].Figure = null;
                }
            }
            
            for (int i = 0; i < 4; i++)
            {
                if (neighborTile.Directions[i].Landscape is Road && i != (int)_utils.GetOppositeDirection(whereToGo))
                {
                    RemoveFiguresFromFinishedRoad(neighborTile, (CardinalDirection)i, false);
                    break;
                }
            }
        }   
    }
}
