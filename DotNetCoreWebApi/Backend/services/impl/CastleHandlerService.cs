using System.Collections.Generic;
using DotNetCoreWebApi.Backend.Model;
using DotNetCoreWebApi.Backend.Utils;

namespace DotNetCoreWebApi.Backend.services.impl
{

    class CastleCalculatorService
    {
        private bool _canPlaceFigure { get; set; }
        private int _whereToGo { get; set; }
        private ITile _firstTile { get; set; }
        private bool _finishedCastle { get; set; } = true;
        private int _points { get; set; }
        private ITile _currentITile { get; set; }
        private List<ITile> _ITileList { get; set; } = new List<ITile>();
        private bool _firstCall { get; set; } = true;
        private CardinalDirection _starterWhereToGo { get; set; }
        private List<ITile> _placedCastleTiles { get; set; } = new List<ITile>();
        private bool _gameOver { get; set; }
        private Dictionary<CardinalDirection, int> _result { get; set; } = new Dictionary<CardinalDirection, int>();

        public Dictionary<CardinalDirection, int> calculate(ITile currentTile, bool gameover)
        {
            _gameOver = gameover;
            _placedCastleTiles.Clear();
            _result.Clear();


            int result = 0;
            for (int i = 0; i < 4; i++)
            {
                if (CheckEndOfCastle(currentTile) && currentTile.Directions[i].Landscape == Landscape.Castle)
                {
                    _firstCall = true;
                    result += CalculateWithDirections(currentTile, (CardinalDirection)i);
                    _result.Add((CardinalDirection)i, _points);
                    result = 0;
                }
                else if (!CheckEndOfCastle(currentTile))
                {
                    _firstCall = true;
                    result += CalculateCastle(currentTile, false);
                    _result.Add((CardinalDirection)i, _points);
                    result = 0;

                    break;
                }
            }

            return _result;
        }


        private int CalculateWithDirections(ITile currentTile, CardinalDirection whereToGo)
        {
            // If it is the first tile, reset the properties...
            if (_firstCall)
            {
                _firstTile = currentTile;
                _currentITile = currentTile;
                _firstTile = currentTile;
                _points = 0;
                _finishedCastle = true;
                _ITileList.Clear();
                _starterWhereToGo = whereToGo;
            }


            // Check if the castle is not finished
            if (!_finishedCastle && !_gameOver)// || _currentITile == null)
                return 0;

            if (IsChecked(_currentITile, _ITileList))
                return 0;

            if (_currentITile == null || _currentITile.Directions == null)
            {
                if (!_gameOver)
                    _finishedCastle = false;
                return 0;
            }

            // If it is an EndOfCastle tile and it isn't the first tile...
            if (CheckEndOfCastle(_currentITile) && _firstTile != _currentITile)
            {
                _points += 2;
                return 0;
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
                            return 0;

                        whereToGo = (CardinalDirection)i;

                        _ITileList.Add(_currentITile);
                        _placedCastleTiles.Add(_currentITile);
                        _firstCall = false;
                        _currentITile = _currentITile.GetTileSideByCardinalDirection((CardinalDirection)i).Neighbor;
                        CalculateWithDirections(_currentITile, (CardinalDirection)i);
                        _currentITile = currentTile;

                        if (_currentITile == _firstTile)
                            break;
                    }
                }
            }

            CheckForPoints();

            return _points;
        }

        private void CheckForPoints()
        {
            if (_finishedCastle)
            {
                _points += 2;
                if (CheckShield(_currentITile))
                    _points += 2;
            }
            else if (!_finishedCastle && _gameOver)
            {
                _points += 2;
                if (CheckShield(_currentITile))
                    _points += 2;
            }
            else
                _points = 0;

            if (_firstTile == _currentITile && _points == 1)
                _points = 0;
        }

        private bool CheckShield(ITile bt)
        {
            foreach (var item in bt.Speciality)
            {
                if (item == Speciality.Shield)
                    return true;
            }

            return false;
        }

        private bool CheckEndOfCastle(ITile bt)
        {
            foreach (var item in bt.Speciality)
            {
                if (item == Speciality.EndOfCastle)
                    return true;
            }

            return false;
        }

        private bool IsChecked(ITile currentTile, List<ITile> btList)
        {
            foreach (var item in btList)
            {
                if (item == currentTile)
                    return true;
            }

            return false;
        }


        // If the current tile doesn't have endofcastle speciality 
        private int CalculateCastle(ITile currentTile, bool gameover)
        {
            // Check if the castle is not finished
            if (!_finishedCastle && !_gameOver)// || _currentITile == null)
                return 0;

            // If it is the first tile, reset the properties...
            if (_firstCall)
            {
                _firstCall = false;
                _firstTile = currentTile;
                _currentITile = currentTile;
                _firstTile = currentTile;
                _points = 0;
                _finishedCastle = true;
                _ITileList.Clear();
            }


            if (IsChecked(_currentITile, _ITileList))
                return 0;


            if (_currentITile == null || _currentITile.Directions == null)
            {
                if (!_gameOver)
                    _finishedCastle = false;

                return 0;
            }


            // If it is an EndOfCastle tile and it isn't the first tile...
            if (CheckEndOfCastle(_currentITile) && _firstTile != _currentITile)
            {
                _points += 2;
                return 0;
            }

            for (int i = 0; i < 4; i++)
            {
                if (_currentITile.Directions[i].Landscape == Landscape.Castle)
                {
                    _whereToGo = i;
                    _ITileList.Add(_currentITile);
                    _currentITile = _currentITile.GetTileSideByCardinalDirection((CardinalDirection)i).Neighbor;
                    //if (_currentITile == null) break;

                    CalculateCastle(_currentITile, false);
                    _currentITile = currentTile;
                }
            }

            CheckForPoints();
            return _points;
        }
    }
}