using System.Collections.Generic;
using DotNetCoreWebApi.Backend.Model;
using DotNetCoreWebApi.Backend.Utils;

namespace DotNetCoreWebApi.Backend.services.impl
{

    class RoadCalculatorService
    {
        private ITile _firstTile { get; set; }
        private ITile _lastTile { get; set; }
        private bool _gameover { get; set; }
        private bool _isRoadFinished { get; set; } = true;
        private Dictionary<CardinalDirection, int> _result { get; set; } = new Dictionary<CardinalDirection, int>();
        private CardinalDirection _whereToGoAfterEndOfRoadFound;
        public Dictionary<CardinalDirection, int> calculate(ITile currentTile, bool gameover)
        {
            int result = 0;
            _gameover = gameover;
            _firstTile = currentTile;
            _result = new Dictionary<CardinalDirection, int>();
            if (gameover)
            {
                _lastTile = new Tile(new List<IDirection>(), new List<Speciality>());
                for (var i = 0; i < currentTile.Directions.Count; i++)
                {
                    if (currentTile.Directions[i].Landscape == Landscape.Road)
                    {
                        result += CalculateWithDirections(currentTile, (CardinalDirection)i);
                        _result.Add((CardinalDirection)i, result);
                    }
                    if (IsEndOfRoad(currentTile) && currentTile.Directions[i].Landscape == Landscape.Road)
                    {
                        if (!(_firstTile.Position.X == _lastTile.Position.X &&
                              _firstTile.Position.Y == _lastTile.Position.Y) && _isRoadFinished)
                        {
                            result++;
                        }
                        if (_result.ContainsKey((CardinalDirection)i)) { _result.Remove((CardinalDirection)i); };
                        _result.Add((CardinalDirection)i, result);
                        result = 0;
                    }
                }
                if (_firstTile.Position.X != _lastTile.Position.X && _firstTile.Position.Y != _lastTile.Position.Y)
                {
                    result += 1;
                }

            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    if (IsEndOfRoad(currentTile) && currentTile.Directions[i].Landscape == Landscape.Road)
                    {
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
                        if (result != 0)
                        {
                            _result.Add((CardinalDirection)i, result);
                        }
                        _isRoadFinished = true;
                        _lastTile = null;
                        result = 0;
                    }
                    else if (!IsEndOfRoad(currentTile) && currentTile.Directions[i].Landscape == Landscape.Road
                                                       && SearchEndOfRoadTileInGivenDirection(currentTile, (CardinalDirection)i) != null)
                    {
                        _firstTile = SearchEndOfRoadTileInGivenDirection(currentTile, (CardinalDirection)i);
                        result += CalculateWithDirections(_firstTile, _whereToGoAfterEndOfRoadFound);
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
                        _result.Add((CardinalDirection)i, result);
                        break;
                    }
                }
            }
            _firstTile = null;
            _lastTile = null;
            _isRoadFinished = true;
            return _result;
        }

        /**
         * Calculate a road's length and gives back the points earned from finishing it. Sets _isRoadFinished false if the road is not finished.
         */
        private int CalculateWithDirections(ITile currentTile, CardinalDirection whereToGo)
        {
            int result = 1;
            Dictionary<CardinalDirection, ITile> tilesNextToTheGivenTile = TileUtils.GetSurroundingTiles(currentTile);
            ITile neighborTile = TileUtils.GetNeighborTile(tilesNextToTheGivenTile, whereToGo);

            /*If the neighbor tile does not exist then return result and set current tile as the last tile and sets _isRoadFinished false*/
            if (neighborTile == null)
            {
                _lastTile = currentTile;
                _isRoadFinished = false;
                return result;
            }

            if (IsEndOfRoad(neighborTile) || neighborTile.Position.Equals(_firstTile.Position))
            {
                _lastTile = neighborTile;
                return result;
            }
            return SearchInTilesSides(result, neighborTile, (int)TileUtils.GetOppositeDirection(whereToGo));
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

        /// <summary>
        /// Checks whether the given tile is end of a road or not
        /// </summary>
        /// <param name="currentTile">The examined tile</param>
        /// <returns>True if the given tile is end of road, false if not</returns>
        public bool IsEndOfRoad(ITile currentTile)
        {
            return currentTile.Speciality.Contains(Speciality.EndOfRoad);
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
                if (neighborTile.Directions[i].Landscape == Landscape.Road && i != sideNumber)
                {
                    result += CalculateWithDirections(neighborTile, neighborTile.GetCardinalDirectionByIndex(i));
                    break;
                }
            }
            return result;
        }
    }
}