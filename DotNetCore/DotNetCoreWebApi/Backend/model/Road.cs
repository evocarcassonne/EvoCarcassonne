using System.Collections.Generic;

namespace DotNetCoreWebApi.Backend.Model
{
    public class Road : ILandscape
    {
        private ITile _firstTile { get; set; }
        private ITile _lastTile { get; set; }
        private bool _gameover { get; set; }
        private bool _isRoadFinished { get; set; } = true;
        private bool _canPlaceFigure = true;

        private CardinalDirection _whereToGoAfterEndOfRoadFound;
        private List<IFigure> _figuresOnTiles { get; set; } = new List<IFigure>();

        public Road()
        {
        }

        public void calculate(ITile currentTile, bool gameover, out List<IFigure> figuresToGiveBack)
        {
            int result = 0;
            _gameover = gameover;
            _firstTile = currentTile;
            if (gameover)
            {
                for (var i = 0; i < currentTile.Directions.Count; i++)
                {
                    if (currentTile.Directions[i].Landscape is Road)
                    {
                        CheckFigureOnTile(currentTile, i);
                        result += CalculateWithDirections(currentTile, (CardinalDirection)i);
                    }
                    if (IsEndOfRoad(currentTile) && currentTile.Directions[i].Landscape is Road)
                    {
                        if (!(_firstTile.Position.X == _lastTile.Position.X &&
                              _firstTile.Position.Y == _lastTile.Position.Y) && _isRoadFinished)
                        {
                            result++;
                        }

                        figuresToGiveBack = _figuresOnTiles;
                        Utils.DistributePoints(result, _figuresOnTiles);
                        result = 0;
                        _figuresOnTiles = new List<IFigure>();
                    }
                    RemoveFiguresFromFinishedRoad(currentTile, (CardinalDirection)i, true);
                }
                if (_firstTile.Position.X != _lastTile.Position.X && _firstTile.Position.Y != _lastTile.Position.Y)
                {
                    result += 1;
                }

                figuresToGiveBack = _figuresOnTiles;
                Utils.DistributePoints(result, _figuresOnTiles);
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    if (IsEndOfRoad(currentTile) && currentTile.Directions[i].Landscape is Road)
                    {
                        CheckFigureOnTile(currentTile, i);
                        result += CalculateWithDirections(currentTile, (CardinalDirection)i);
                        if (!(_firstTile.Position.X == _lastTile.Position.X &&
                              _firstTile.Position.Y == _lastTile.Position.Y) && _isRoadFinished)
                        {
                            result++;
                        }
                        if (!_isRoadFinished)
                        {
                            result = 0;
                        }
                        else
                        {
                            RemoveFiguresFromFinishedRoad(currentTile, (CardinalDirection)i, true);
                        }
                        _isRoadFinished = true;
                        figuresToGiveBack = _figuresOnTiles;
                        Utils.DistributePoints(result, _figuresOnTiles);
                        _lastTile = null;
                        _figuresOnTiles = new List<IFigure>();
                        result = 0;
                    }
                    else if (!IsEndOfRoad(currentTile) && currentTile.Directions[i].Landscape is Road
                                                       && SearchEndOfRoadTileInGivenDirection(currentTile, (CardinalDirection)i) != null)
                    {
                        _firstTile = SearchEndOfRoadTileInGivenDirection(currentTile, (CardinalDirection)i);
                        CheckFigureOnTile(_firstTile, (int)_whereToGoAfterEndOfRoadFound);
                        result += CalculateWithDirections(SearchEndOfRoadTileInGivenDirection(currentTile, (CardinalDirection)i), _whereToGoAfterEndOfRoadFound);
                        /*If the road does not end with the same tile its started, then increase the result*/
                        if (_lastTile != null && !(_firstTile.Position.X == _lastTile.Position.X &&
                                                  _firstTile.Position.Y == _lastTile.Position.Y) && _isRoadFinished)
                        {
                            result++;
                        }
                        /*If the road is not finished, then result should be 0*/
                        if (!_isRoadFinished)
                        {
                            result = 0;
                        }
                        else
                        {
                            RemoveFiguresFromFinishedRoad(_firstTile, _whereToGoAfterEndOfRoadFound, true);
                        }
                        figuresToGiveBack = _figuresOnTiles;
                        Utils.DistributePoints(result, _figuresOnTiles);
                        break;
                    }
                }
            }
            _figuresOnTiles = figuresToGiveBack = new List<IFigure>();
            _firstTile = null;
            _lastTile = null;
            _isRoadFinished = true;
        }


        public bool CanPlaceFigure(ITile currentTile, CardinalDirection whereToGo, bool firstCall)
        {
            ITile neighborTile = currentTile.GetTileSideByCardinalDirection(whereToGo).Neighbor;
            if (firstCall)
            {
                if (IsEndOfRoad(currentTile))
                {
                    return CanPlaceFigure(currentTile, whereToGo, false);
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

                    return CanPlaceFigure(currentTile, whereToGo, false) &&
                           CanPlaceFigure(currentTile, otherSide, false);
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
                    return neighborTile.Directions[(int)Utils.GetOppositeDirection(whereToGo)].Figure == null;
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
                            if (neighborTile.Directions[i].Landscape is Road && i != (int)Utils.GetOppositeDirection(whereToGo))
                            {
                                return CanPlaceFigure(neighborTile, neighborTile.GetCardinalDirectionByIndex(i), false);
                            }
                        }
                        if (neighborTile.Directions[i].Landscape is Road && i != (int)Utils.GetOppositeDirection(whereToGo))
                        {
                            return CanPlaceFigure(neighborTile, neighborTile.GetCardinalDirectionByIndex(i), false);
                        }
                    }
                }
                else
                {
                    for (var i = 0; i < 4; i++)
                    {
                        if (neighborTile.Directions[i].Landscape is Road && i != (int)Utils.GetOppositeDirection(whereToGo))
                        {
                            return CanPlaceFigure(neighborTile, neighborTile.GetCardinalDirectionByIndex(i), false);
                        }
                    }
                }

            }


            return true;
        }


        /**
         * Calculate a road's length and gives back the points earned from finishing it. Sets _isRoadFinished false if the road is not finished.
         */
        private int CalculateWithDirections(ITile currentTile, CardinalDirection whereToGo)
        {
            int result = 1;
            Dictionary<CardinalDirection, ITile> tilesNextToTheGivenTile = Utils.GetSurroundingTiles(currentTile);
            ITile neighborTile = Utils.GetNeighborTile(tilesNextToTheGivenTile, whereToGo);

            for (int i = 0; i < 4; i++)
            {
                if (currentTile.Directions[i].Landscape is Road && !_gameover && !IsEndOfRoad(currentTile))
                {
                    CheckFigureOnTile(currentTile, i);
                }
            }
            /*If the neighbor tile does not exist then return result and set current tile as the last tile and sets _isRoadFinished false*/
            if (neighborTile == null)
            {
                _lastTile = currentTile;
                _isRoadFinished = false;
                return result;
            }

            if (IsEndOfRoad(neighborTile) || neighborTile.Position.Equals(_firstTile.Position))
            {
                CheckFigureOnTile(neighborTile, (int)Utils.GetOppositeDirection(whereToGo));
                _lastTile = neighborTile;
                return result;
            }
            return SearchInTilesSides(result, neighborTile, (int)Utils.GetOppositeDirection(whereToGo));
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
                _figuresOnTiles.Add(currentTile.Directions[onlySideToCheck].Figure);
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
            ITile neighborTile = Utils.GetNeighborTile(Utils.GetSurroundingTiles(currentTile), whereToGo);
            if (neighborTile == null || IsEndOfRoad(neighborTile) || _firstTile.Position.Equals(neighborTile.Position))
            {
                _whereToGoAfterEndOfRoadFound = Utils.GetOppositeDirection(whereToGo);
                return neighborTile;
            }

            for (var i = 0; i < 4; i++)
            {
                if (neighborTile.Directions[i].Landscape is Road && i != (int)Utils.GetOppositeDirection(whereToGo))
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
            ITile neighborTile = Utils.GetNeighborTile(Utils.GetSurroundingTiles(currentTile), whereToGo);
            if (firstCall)
            {
                //Utils.GiveBackFigureToOwner(currentTile.Directions[(int)whereToGo].Figure);
                currentTile.Directions[(int)whereToGo].Figure = null;
            }

            if (neighborTile == null)
            {
                return;
            }
            if (IsEndOfRoad(neighborTile) || neighborTile.Position.Equals(_firstTile.Position))
            {
                //Utils.GiveBackFigureToOwner(neighborTile.Directions[(int)Utils.GetOppositeDirection(whereToGo)].Figure);
                neighborTile.Directions[(int)Utils.GetOppositeDirection(whereToGo)].Figure = null;
                return;
            }
            for (int i = 0; i < 4; i++)
            {
                if (neighborTile.Directions[i].Figure != null && neighborTile.Directions[i].Landscape is Road)
                {
                    //Utils.GiveBackFigureToOwner( neighborTile.Directions[i].Figure);
                    neighborTile.Directions[i].Figure = null;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                if (neighborTile.Directions[i].Landscape is Road && i != (int)Utils.GetOppositeDirection(whereToGo))
                {
                    RemoveFiguresFromFinishedRoad(neighborTile, (CardinalDirection)i, false);
                    break;
                }
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
