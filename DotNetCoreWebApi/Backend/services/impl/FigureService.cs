using System.Collections.Generic;
using System.Linq;
using DotNetCoreWebApi.Backend.Model;

namespace DotNetCoreWebApi.Backend.services.impl
{
    public class FigureService : IFigureService
    {
        private ITile _currentITile;
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
                this.CanPlaceFigure(currentTile, whereToGo, true);
            }

            return _figuresOnTiles;
        }


        #region Castle Specific
        private bool CanPlaceFigureOnCastle(ITile currentTile, CardinalDirection whereToGo, bool firstCall)
        {
            ITile neighborTile = Utils.GetNeighborTile(Utils.GetSurroundingTiles(currentTile), whereToGo);
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
                CheckFigureOnTile(neighborTile, (int)Utils.GetOppositeDirection(whereToGo));
                return (neighborTile.Directions[(int)Utils.GetOppositeDirection(whereToGo)].Figure == null && _canPlaceFigure);

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
                && i != (int)Utils.GetOppositeDirection(whereToGo))
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
            ITile neighborTile = Utils.GetNeighborTile(Utils.GetSurroundingTiles(currentTile), whereToGo);

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
                CheckFigureOnTile(neighborTile, (int)Utils.GetOppositeDirection(whereToGo));
                return (neighborTile.Directions[(int)Utils.GetOppositeDirection(whereToGo)].Figure == null && _canPlaceFigure);
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
                if (neighborTile.Directions[i].Landscape == Landscape.Road && i != (int)Utils.GetOppositeDirection(whereToGo))
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

        private void CheckFigureOnTile(ITile currentTile, int onlySideToCheck)
        {
            if (currentTile.Directions[onlySideToCheck].Figure != null)
            {
                _figuresOnTiles.Add(currentTile.Directions[onlySideToCheck].Figure);
                currentTile.Directions[onlySideToCheck].Figure = null;
            }
        }
    }
}