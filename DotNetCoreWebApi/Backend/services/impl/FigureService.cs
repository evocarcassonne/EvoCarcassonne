using System.Collections.Generic;
using System.Linq;
using DotNetCoreWebApi.Backend.Model;
using DotNetCoreWebApi.Backend.Utils;


namespace DotNetCoreWebApi.Backend.services.impl
{
    public class FigureService : IFigureService
    {
        private ITile _currentITile;
        private CardinalDirection _whereToGoAfterEndOfRoadFound;
        private ITile _firstTile;
        private readonly List<IFigure> _figuresOnTiles = new List<IFigure>();
        private List<ITile> CheckedTiles { get; set; } = new List<ITile>();
        private bool _canPlaceFigure { get; set; } = true;
        public bool CanPlaceFigure(ITile currentTile, CardinalDirection whereToGo, bool firstCall)
        {
            _figuresOnTiles.Clear();
            CheckedTiles.Clear();
            _canPlaceFigure = true;

            switch (currentTile.Directions[(int)whereToGo].Landscape)
            {
                case Landscape.Castle: return CanPlaceFigureOnCastle(currentTile, whereToGo, firstCall);
                case Landscape.Field: return true;
                case Landscape.Road: return CanPlaceFigureOnRoad(currentTile, whereToGo, firstCall);
                default: return true;
            }
        }

        public List<IFigure> GetFiguresToGiveBack(ITile currentTile, CardinalDirection whereToGo, bool firstCall)
        {
            _figuresOnTiles.Clear();
            if (currentTile.Speciality.Contains(Speciality.Colostor) || currentTile is Church)
            {
                if (currentTile.CenterFigure != null)
                {
                    _figuresOnTiles.Add(currentTile.CenterFigure);
                }
            }
            else
            {
                if (currentTile.GetTileSideByCardinalDirection(whereToGo).Landscape == Landscape.Castle)
                {
                    SearchFiguresOnCastle(currentTile, whereToGo, true);
                    _firstTile = currentTile;
                    _currentITile = currentTile;
                    _firstTile = currentTile;
                    CheckedTiles.Clear();
                    CheckCastle(currentTile);
                }
                if (currentTile.GetTileSideByCardinalDirection(whereToGo).Landscape == Landscape.Road)
                {
                    SearchFiguresOnRoad(currentTile, whereToGo, true);
                }
            }

            return _figuresOnTiles;
        }

        private void SearchFiguresOnCastle(ITile currentTile, CardinalDirection whereToGo, bool firstCall)
        {
            if (firstCall)
            {
                if (currentTile.GetTileSideByCardinalDirection(whereToGo).Landscape == Landscape.Castle)
                {
                    _firstTile = currentTile;
                    CheckFigureOnTile(currentTile, (int)whereToGo);
                    SearchFiguresOnCastle(currentTile, whereToGo, false);
                }
            }
            else
            {
                var neighborTile = TileUtils.GetNeighborTile(TileUtils.GetSurroundingTiles(currentTile), whereToGo);

                if (neighborTile == null)
                {
                    _figuresOnTiles.Clear();
                    return;
                }
                if (CheckEndOfCastle(neighborTile) || neighborTile.Position.Equals(_firstTile.Position) || CheckedTiles.Contains(neighborTile))
                {
                    CheckFigureOnTile(neighborTile, (int)TileUtils.GetOppositeDirection(whereToGo));
                    return;
                }

                SearchInTilesSides(neighborTile, (int)TileUtils.GetOppositeDirection(whereToGo), Landscape.Castle);
            }
        }

        private void SearchFiguresOnRoad(ITile currentTile, CardinalDirection whereToGo, bool firstCall)
        {
            if (firstCall)
            {
                if (IsEndOfRoad(currentTile) && currentTile.GetTileSideByCardinalDirection(whereToGo).Landscape == Landscape.Road)
                {
                    _firstTile = currentTile;
                    CheckFigureOnTile(currentTile, (int)whereToGo);
                    SearchFiguresOnRoad(currentTile, whereToGo, false);
                }
                else if (!IsEndOfRoad(currentTile) && currentTile.GetTileSideByCardinalDirection(whereToGo).Landscape == Landscape.Road
                            && SearchEndOfRoadTileInGivenDirection(currentTile, whereToGo) != null)
                {
                    var firstTile = SearchEndOfRoadTileInGivenDirection(currentTile, whereToGo);
                    _firstTile = firstTile;
                    CheckFigureOnTile(currentTile, (int)whereToGo);
                    SearchFiguresOnRoad(firstTile, _whereToGoAfterEndOfRoadFound, false);
                }
            }
            else
            {
                var neighborTile = TileUtils.GetNeighborTile(TileUtils.GetSurroundingTiles(currentTile), whereToGo);

                if (neighborTile == null)
                {
                    _figuresOnTiles.Clear();
                    return;
                }
                if (IsEndOfRoad(neighborTile) || neighborTile.Position.Equals(_firstTile.Position))
                {
                    CheckFigureOnTile(neighborTile, (int)TileUtils.GetOppositeDirection(whereToGo));
                    return;
                }
                SearchInTilesSides(neighborTile, (int)TileUtils.GetOppositeDirection(whereToGo), Landscape.Road);
            }

        }

        private void SearchInTilesSides(ITile neighborTile, int sideNumber, Landscape forWhat)
        {
            for (int i = 0; i < 4; i++)
            {
                if (neighborTile.Directions[i].Landscape == forWhat)
                {
                    CheckFigureOnTile(neighborTile, i);
                }
            }
            CheckedTiles.Add(neighborTile);
            for (int i = 0; i < 4; i++)
            {
                if (forWhat == Landscape.Road && neighborTile.Directions[i].Landscape == Landscape.Road && i != sideNumber)
                {
                    SearchFiguresOnRoad(neighborTile, neighborTile.GetCardinalDirectionByIndex(i), false);
                    break;
                }
                if (forWhat == Landscape.Castle && neighborTile.Directions[i].Landscape == Landscape.Castle && i != sideNumber)
                {
                    SearchFiguresOnRoad(neighborTile, neighborTile.GetCardinalDirectionByIndex(i), false);
                    break;
                }
            }
        }

        private void CheckCastle(ITile currentTile)
        {
            if (IsChecked(_currentITile, CheckedTiles))
                return;


            if (_currentITile == null || _currentITile.Directions == null)
            {
                return;
            }

            // If it is an EndOfCastle tile and it isn't the first tile...
            if (CheckEndOfCastle(_currentITile) && _firstTile != _currentITile)
            {
                return;
            }

            for (int i = 0; i < 4; i++)
            {
                if (_currentITile.Directions[i].Landscape == Landscape.Castle)
                {
                    CheckFigureOnTile(_currentITile, i);
                }
            }
            CheckedTiles.Add(_currentITile);
            for (int i = 0; i < 4; i++)
            {
                if (_currentITile.Directions[i].Landscape == Landscape.Castle)
                {
                    _currentITile = _currentITile.GetTileSideByCardinalDirection((CardinalDirection)i).Neighbor;
                    CheckCastle(_currentITile);
                    _currentITile = currentTile;
                }
            }
        }

        /// <summary>
        /// It is supposed to be called after IsFinishedRoad has at least one true value. With the parameters it is going to go, and find the end of road, which is just put down.
        /// </summary>
        /// <param name="currentTile">The tile which has just been put down</param>
        /// <param name="whereToGo">The direction where to search for end of road tile</param>
        /// <returns>The end of road tile found in the given direction. Null if there is no end of road tile</returns>
        private ITile SearchEndOfRoadTileInGivenDirection(ITile currentTile, CardinalDirection whereToGo)
        {
            ITile neighborTile = TileUtils.GetNeighborTile(TileUtils.GetSurroundingTiles(currentTile), whereToGo);
            if (neighborTile == null || IsEndOfRoad(neighborTile))
            {
                _whereToGoAfterEndOfRoadFound = TileUtils.GetOppositeDirection(whereToGo);
                return neighborTile;
            }

            for (var i = 0; i < 4; i++)
            {
                if (neighborTile.Directions[i].Landscape == Landscape.Road && i != (int)TileUtils.GetOppositeDirection(whereToGo))
                {
                    return SearchEndOfRoadTileInGivenDirection(neighborTile, neighborTile.GetCardinalDirectionByIndex(i));
                }
            }
            return null;
        }

        private void CheckFigureOnTile(ITile currentTile, int onlySideToCheck)
        {
            if (currentTile.Directions[onlySideToCheck].Figure != null)
            {
                _figuresOnTiles.Add(currentTile.Directions[onlySideToCheck].Figure);
                currentTile.Directions[onlySideToCheck].Figure = null;
            }
        }

        #region Castle Specific
        private bool CanPlaceFigureOnCastle(ITile currentTile, CardinalDirection whereToGo, bool firstCall)
        {
            ITile neighborTile = TileUtils.GetNeighborTile(TileUtils.GetSurroundingTiles(currentTile), whereToGo);
            if (firstCall)
            {
                _currentITile = currentTile;
                CheckedTiles.Clear();
                _figuresOnTiles.Clear();
                _canPlaceFigure = true;
            }

            if (_currentITile == null)
            {
                _canPlaceFigure = false;
            }
            if (IsChecked(_currentITile, CheckedTiles))
            {
                return _canPlaceFigure;
            }

            if (firstCall)
            {
                if (CheckEndOfCastle(currentTile))
                {
                    return CanPlaceFigureOnCastle(currentTile, whereToGo, false);
                }
                else
                {
                    List<bool> results = new List<bool>();
                    for (int i = 0; i < 4; i++)
                    {
                        if (currentTile.Directions[i].Landscape == Landscape.Castle && i != (int)whereToGo)
                        {
                            results.Add(CanPlaceFigureOnCastle(currentTile, (CardinalDirection)i, false));
                        }
                    }
                    return results.All(e => e);
                }
            }

            CheckedTiles.Add(currentTile);
            CheckFigureOnTile(currentTile, (int)whereToGo);
            if (neighborTile == null)
            {
                return _canPlaceFigure;
            }

            if (CheckEndOfCastle(neighborTile))
            {
                CheckFigureOnTile(neighborTile, (int)TileUtils.GetOppositeDirection(whereToGo));
                return (neighborTile.Directions[(int)TileUtils.GetOppositeDirection(whereToGo)].Figure == null && _canPlaceFigure);
            }

            for (int i = 0; i < 4; i++)
            {
                if (neighborTile.Directions[i].Figure != null && neighborTile.Directions[i].Landscape == Landscape.Castle)
                {
                    CheckFigureOnTile(currentTile, i);
                    _canPlaceFigure = false;
                }
            }
            var resultOfPlacementChecking = new List<bool>();
            for (var i = 0; i < 4; i++)
            {
                if (neighborTile.Directions[i].Landscape == Landscape.Castle
                && i != (int)TileUtils.GetOppositeDirection(whereToGo))
                {
                    CheckFigureOnTile(neighborTile, i);
                    resultOfPlacementChecking.Add(CanPlaceFigureOnCastle(neighborTile, neighborTile.GetCardinalDirectionByIndex(i),
                        false));
                }
            }

            return resultOfPlacementChecking.All(e => e);
        }

        private bool CheckEndOfCastle(ITile currentITile)
        {
            return currentITile.Speciality.Contains(Speciality.EndOfCastle);
        }

        private bool IsChecked(ITile currentITile, List<ITile> iTileList)
        {
            return iTileList.Contains(currentITile);
        }

        #endregion

        #region Road Specific
        private bool CanPlaceFigureOnRoad(ITile currentTile, CardinalDirection whereToGo, bool firstCall)
        {
            ITile neighborTile = TileUtils.GetNeighborTile(TileUtils.GetSurroundingTiles(currentTile), whereToGo);
            if (firstCall)
            {
                if (IsEndOfRoad(currentTile))
                {
                    return CanPlaceFigureOnRoad(currentTile, whereToGo, false);
                }
                else
                {
                    CardinalDirection otherSide = CardinalDirection.North;
                    for (int i = 0; i < 4; i++)
                    {
                        if (currentTile.Directions[i].Landscape == Landscape.Road && i != (int)whereToGo)
                        {
                            otherSide = (CardinalDirection)i;
                        }
                    }

                    return CanPlaceFigureOnRoad(currentTile, whereToGo, false) &&
                           CanPlaceFigureOnRoad(currentTile, otherSide, false);
                }
            }

            this.CheckFigureOnTile(currentTile, (int)whereToGo);

            if (neighborTile == null)
            {
                return _canPlaceFigure;
            }

            if (IsEndOfRoad(neighborTile))
            {
                CheckFigureOnTile(neighborTile, (int)TileUtils.GetOppositeDirection(whereToGo));
                return (neighborTile.Directions[(int)TileUtils.GetOppositeDirection(whereToGo)].Figure == null && _canPlaceFigure);
            }

            for (int i = 0; i < 4; i++)
            {
                if (neighborTile.Directions[i].Figure != null &&
                    neighborTile.Directions[i].Landscape == Landscape.Road)
                {
                    this.CheckFigureOnTile(currentTile, i);
                    _canPlaceFigure = false;
                }
            }

            for (var i = 0; i < 4; i++)
            {
                if (neighborTile.Directions[i].Landscape == Landscape.Road && i != (int)TileUtils.GetOppositeDirection(whereToGo))
                {
                    return CanPlaceFigureOnRoad(neighborTile, neighborTile.GetCardinalDirectionByIndex(i), false);
                }
            }

            return _canPlaceFigure;
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
        #endregion


    }
}