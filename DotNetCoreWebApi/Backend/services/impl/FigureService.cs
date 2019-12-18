using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
            return null;
        }

        #region Castle Specific
        private bool CanPlaceFigureOnCastle(ITile currentTile, CardinalDirection whereToGo, bool firstCall)
        {
            ITile neighborTile = Utils.GetNeighborTile(Utils.GetSurroundingTiles(currentTile), whereToGo);
            if(firstCall){
                _currentITile = currentTile;
                CheckedTiles.Clear();
                _figuresOnTiles.Clear();
                _canPlaceFigure = true;
            }

            if (_currentITile == null)
            {
                _canPlaceFigure = false;
                return false;
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
                            results.Add(CanPlaceFigureOnCastle(currentTile, (CardinalDirection)i,false));
                        }
                    }
                    return results.All(e => e);
                }
            }
            
            CheckedTiles.Add(currentTile);
            if (neighborTile == null)
            {
                return _canPlaceFigure;
            }

            if (CheckEndOfCastle(neighborTile))
            {
                return neighborTile.Directions[(int) Utils.GetOppositeDirection(whereToGo)].Figure == null;
            }

            for (int i = 0; i < 4; i++)
            {
                if (neighborTile.Directions[i].Figure != null && neighborTile.Directions[i].Landscape == Landscape.Castle)
                {
                    _canPlaceFigure = false;
                    return false;
                }
            }
            var resultOfPlacementChecking = new List<bool>();
            for (var i = 0; i < 4; i++)
            {
                if (neighborTile.Directions[i].Landscape == Landscape.Castle
                && i != (int)Utils.GetOppositeDirection(whereToGo))
                {
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
                if(neighborTile.Directions[i].Figure != null &&
                    neighborTile.Directions[i].Landscape == Landscape.Road)
                {
                    _canPlaceFigure = false;
                    return false;
                }
            }

            if (IsEndOfRoad(currentTile))
            {
                for (var i = 0; i < 4; i++)
                {
                    if (neighborTile.Directions[i].Landscape == Landscape.Road && i != (int)Utils.GetOppositeDirection(whereToGo))
                    {
                        return CanPlaceFigure(neighborTile, neighborTile.GetCardinalDirectionByIndex(i), false);
                    }
                }
            }
            else
            {
                for (var i = 0; i < 4; i++)
                {
                    if (neighborTile.Directions[i].Landscape == Landscape.Road && i != (int)Utils.GetOppositeDirection(whereToGo))
                    {
                        return CanPlaceFigure(neighborTile, neighborTile.GetCardinalDirectionByIndex(i), false);
                    }
                }
            }

            return true;
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