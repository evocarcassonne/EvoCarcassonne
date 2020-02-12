using System;
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
        private bool _finishedCastle;
        private CardinalDirection _starterWhereToGo;
        private bool _firstCall = false;
        private int _whereToGo;
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
            bool isChurchWithRoad = false;
            foreach (var side in currentTile.Directions)
            {
                if (side.Landscape == Landscape.Road)
                {
                    isChurchWithRoad = true;
                }
            }

            if ((currentTile.Speciality.Contains(Speciality.Colostor) || currentTile is Church) && isChurchWithRoad)
            {
                if (firstCall)
                {
                    if (currentTile.CenterFigure != null)
                    {
                        _figuresOnTiles.Add(currentTile.CenterFigure);
                        currentTile.CenterFigure = null;
                    }
                }
                else
                {
                    _firstTile = currentTile;
                    if (currentTile.GetTileSideByCardinalDirection(whereToGo).Landscape == Landscape.Castle)
                    {
                        _currentITile = currentTile;
                        CheckedTiles.Clear();
                        _finishedCastle = true;
                        _starterWhereToGo = whereToGo;
                        SearchFiguresOnCastle(currentTile, whereToGo);
                    }
                    if (currentTile.GetTileSideByCardinalDirection(whereToGo).Landscape == Landscape.Road)
                    {
                        SearchFiguresOnRoad(currentTile, whereToGo, true);
                    }
                }
            }
            else if (currentTile.Speciality.Contains(Speciality.Colostor) || currentTile is Church)
            {
                if (currentTile.CenterFigure != null)
                {
                    _figuresOnTiles.Add(currentTile.CenterFigure);
                    currentTile.CenterFigure = null;
                }
            }
            else
            {
                _firstTile = currentTile;
                if (currentTile.GetTileSideByCardinalDirection(whereToGo).Landscape == Landscape.Castle)
                {
                    _currentITile = currentTile;
                    CheckedTiles.Clear();
                    _finishedCastle = true;
                    _starterWhereToGo = whereToGo;
                    SearchFiguresOnCastle(currentTile, whereToGo);
                }
                if (currentTile.GetTileSideByCardinalDirection(whereToGo).Landscape == Landscape.Road)
                {
                    SearchFiguresOnRoad(currentTile, whereToGo, true);
                }
            }

            return _figuresOnTiles;
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

        private void SearchFiguresOnCastle(ITile currentTile, CardinalDirection whereToGo)
        {
            if (CheckEndOfCastle(currentTile))
            {
                if (currentTile.GetTileSideByCardinalDirection(whereToGo).Landscape == Landscape.Castle)
                {
                    CalculateWithDirections(currentTile, whereToGo);
                }
            }
            else
            {
                CalculateCastle(currentTile);
            }
        }

        private void CalculateCastle(ITile currentTile)
        {
            // Check if the castle is not finished
            if (!_finishedCastle)// || _currentITile == null)
                return;

            if (IsChecked(_currentITile, CheckedTiles))
                return;


            if (_currentITile == null || _currentITile.Directions == null)
            {
                return;
            }
            CheckedTiles.Add(_currentITile);
            // If it is an EndOfCastle tile and it isn't the first tile...
            if (CheckEndOfCastle(_currentITile) && _firstTile != _currentITile)
            {
                CheckFigureOnTile(currentTile, (int)TileUtils.GetOppositeDirection((CardinalDirection)_whereToGo));
                return;
            }

            for (int i = 0; i < 4; i++)
            {
                if (_currentITile.Directions[i].Landscape == Landscape.Castle)
                {
                    CheckFigureOnTile(_currentITile, i);
                    _whereToGo = i;
                    _currentITile = _currentITile.GetTileSideByCardinalDirection((CardinalDirection)i).Neighbor;
                    CalculateCastle(_currentITile);
                    _currentITile = currentTile;
                }
            }
        }

        private void CalculateWithDirections(ITile currentTile, CardinalDirection whereToGo)
        {
            // Check if the castle is not finished
            if (!_finishedCastle)// || _currentITile == null)
                return;

            if (IsChecked(_currentITile, CheckedTiles))
                return;


            if (_currentITile == null || _currentITile.Directions == null)
            {
                return;
            }

            // If it is an EndOfCastle tile and it isn't the first tile...
            if (CheckEndOfCastle(_currentITile) && _firstTile != _currentITile)
            {
                CheckFigureOnTile(currentTile, (int)TileUtils.GetOppositeDirection(whereToGo));
                return;
            }

            for (int i = 0; i < 4; i++)
            {
                if (_currentITile.Directions[i].Landscape == Landscape.Castle)
                {
                    if (_firstCall && i != (int)_starterWhereToGo)
                    {
                    }
                    else
                    {
                        if (_currentITile == _firstTile && (int)_starterWhereToGo == (int)TileUtils.GetOppositeDirection(whereToGo) && !_firstCall)
                            return;

                        CheckFigureOnTile(currentTile, i);

                        whereToGo = (CardinalDirection)i;

                        CheckedTiles.Add(_currentITile);
                        _currentITile = _currentITile.GetTileSideByCardinalDirection((CardinalDirection)i).Neighbor;
                        CalculateWithDirections(_currentITile, (CardinalDirection)i);
                        _currentITile = currentTile;

                        if (_currentITile == _firstTile)
                            break;
                    }
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
        private bool IsEndOfRoad(ITile currentTile)
        {
            return currentTile.Speciality.Contains(Speciality.EndOfRoad);
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
            if (neighborTile == null || IsEndOfRoad(neighborTile) || _firstTile.Position.Equals(neighborTile.Position))
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

        private void SearchFiguresOnRoad(ITile currentTile, CardinalDirection whereToGo, bool firstCall)
        {
            if (firstCall)
            {
                _firstTile = currentTile;
                if (IsEndOfRoad(currentTile) && currentTile.GetTileSideByCardinalDirection(whereToGo).Landscape == Landscape.Road)
                {
                    CheckFigureOnTile(currentTile, (int)whereToGo);
                    SearchFiguresOnRoad(currentTile, whereToGo, false);
                }
                else if (!IsEndOfRoad(currentTile) && currentTile.GetTileSideByCardinalDirection(whereToGo).Landscape == Landscape.Road
                            && SearchEndOfRoadTileInGivenDirection(currentTile, whereToGo) != null)
                {
                    var firstTile = SearchEndOfRoadTileInGivenDirection(currentTile, whereToGo);
                    _firstTile = firstTile;
                    CheckFigureOnTile(_firstTile, (int)_whereToGoAfterEndOfRoadFound);
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

                int sideNumber = (int)TileUtils.GetOppositeDirection(whereToGo);

                for (int i = 0; i < 4; i++)
                {
                    if (neighborTile.Directions[i].Landscape == Landscape.Road)
                    {
                        CheckFigureOnTile(neighborTile, i);
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    if (neighborTile.Directions[i].Landscape == Landscape.Road && i != sideNumber)
                    {
                        SearchFiguresOnRoad(neighborTile, neighborTile.GetCardinalDirectionByIndex(i), false);
                        break;
                    }

                }
            }

        }

        #endregion


    }
}