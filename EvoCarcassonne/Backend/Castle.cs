using System.Collections.Generic;
using EvoCarcassonne.Models;
using EvoCarcassonne.ViewModels;

namespace EvoCarcassonne.Backend
{
    public class Castle : ILandscape
    {
        private int _whereToGo { get; set; }
        private BoardTile _firstTile { get; set; }
        private bool _finishedCastle { get; set; } = true;
        private int _points { get; set; }
        private BoardTile _currentBoardTile { get; set; }
        private List<BoardTile> _boardTileList { get; set; } = new List<BoardTile>();
        private bool _firstCall { get; set; } = true;
        private List<IFigure> _figuresOnTiles { get; set; } = new List<IFigure>();
        private CardinalDirection _starterWhereToGo { get; set; }
        private bool _deleteFigures { get; set; } = false;
        private List<BoardTile> _placedCastleTiles { get; set; } = new List<BoardTile>();
        private bool _gameOver { get; set; }
        private bool _outOfRange { get; set; } = false;
        private Utils _utils;
        private int _boardTilesSize { get; set; }

        public Castle()
        {
        }

        public void calculate(BoardTile currentTile, bool gameover, Utils utils)
        {
            _utils = utils;
            _boardTilesSize = _utils.GetBoardTilesSize();
            _gameOver = gameover;
            _placedCastleTiles.Clear();

            int result = 0;
            for (int i = 0; i < 4; i++)
            {
                _deleteFigures = false;
                if (CheckEndOfCastle(currentTile) && currentTile.BackendTile.Directions[i].Landscape is Castle)
                {
                    _firstCall = true;
                    result += CalculateWithDirections(currentTile, (CardinalDirection)i);

                    _firstCall = true;
                    DistributePoints(result);
                    _figuresOnTiles = new List<IFigure>();
                    if (result > 0)
                    {
                        _deleteFigures = true;
                        CalculateWithDirections(currentTile, (CardinalDirection)i);
                    }
                    result = 0;

                }
                else if (!CheckEndOfCastle(currentTile))
                {

                    _firstCall = true;
                    result += CalculateCastle(currentTile, false);

                    _firstCall = true;
                    DistributePoints(result);
                    _figuresOnTiles = new List<IFigure>();
                    if (result > 0)
                    {
                        _deleteFigures = true;
                        CalculateCastle(_firstTile, false);
                    }
                    result = 0;

                    break;
                }
            }
        }


        private int CalculateWithDirections(BoardTile currentTile, CardinalDirection whereToGo)
        {
            // If it is the first tile, reset the properties...
            if (_firstCall)
            {
                _firstTile = currentTile;
                _currentBoardTile = currentTile;
                _firstTile = currentTile;
                _points = 0;
                _finishedCastle = true;
                _boardTileList.Clear();
                _figuresOnTiles.Clear();
                _starterWhereToGo = whereToGo;
            }


            // Check if the castle is not finished         
            if (!_finishedCastle && !_gameOver)
                return 0;

            if (_outOfRange)
            {
                _outOfRange = false;
                return 0;
            }

            // Get the CurrentTile's coordinate
            var x = currentTile.Coordinates.X;
            var y = currentTile.Coordinates.Y;
            var index = x + (y * 10);

            if (x < 0 || y < 0 || x > 9 || y > 9)
                return 0;



            if (IsChecked(_currentBoardTile, _boardTileList))
                return 0;


            if (_currentBoardTile.BackendTile.Directions == null)
            {
                if (!_gameOver)
                    _finishedCastle = false;
                return 0;
            }

            // If it is an EndOfCastle tile and it isn't the first tile...
            if (CheckEndOfCastle(_currentBoardTile) && _firstTile != _currentBoardTile)
            {
                if (_currentBoardTile.BackendTile.Directions[getFromDirection((int)whereToGo)].Figure != null)
                    _figuresOnTiles.Add(_currentBoardTile.BackendTile.Directions[getFromDirection((int)whereToGo)].Figure);

                if (_deleteFigures && _currentBoardTile.BackendTile.Directions[getFromDirection((int)whereToGo)].Figure != null)
                {
                    _utils.GiveBackFigureToOwner(_currentBoardTile.BackendTile.Directions[getFromDirection((int)whereToGo)].Figure);
                    _currentBoardTile.BackendTile.Directions[getFromDirection((int)whereToGo)].Figure = null;
                }
                    

                _points += 2;
                return 0;
            }




            for (int i = 0; i < 4; i++)
            {
                if (_currentBoardTile.BackendTile.Directions[i].Landscape is Castle)
                {
                    if (_firstCall && i != (int)_starterWhereToGo)
                    {
                    }
                    else
                    {
                        if (_currentBoardTile == _firstTile && (int)_starterWhereToGo == getFromDirection((int)whereToGo) && !_firstCall)
                            return 0;

                        if (_currentBoardTile.BackendTile.Directions[i].Figure != null)
                        {
                            _figuresOnTiles.Add(_currentBoardTile.BackendTile.Directions[i].Figure);
                        }

                        if (_deleteFigures && _currentBoardTile.BackendTile.Directions[i].Figure != null)
                        {
                            _utils.GiveBackFigureToOwner(_currentBoardTile.BackendTile.Directions[i].Figure);
                            _currentBoardTile.BackendTile.Directions[i].Figure = null;
                        }

                        whereToGo = (CardinalDirection)i;

                        _boardTileList.Add(_currentBoardTile);
                        _placedCastleTiles.Add(_currentBoardTile);
                        _firstCall = false;
                        _currentBoardTile = _utils.GetOneNeighborTile(_currentBoardTile, GetIndex(i, index));
                        CalculateWithDirections(_currentBoardTile, (CardinalDirection)i);
                        _currentBoardTile = currentTile;

                        if (_currentBoardTile == _firstTile)
                            break;
                    }


                }
            }


            if (_finishedCastle)
            {
                _points += 2;
                if (CheckShield(_currentBoardTile))
                    _points += 2;
            }
            else
                _points = 0;

            if (_deleteFigures)
                _points = 0;

            if (_firstTile == _currentBoardTile && _points == 1)
                _points = 0;


            return _points;
        }

        private bool CheckShield(BoardTile bt)
        {
            foreach (var item in bt.BackendTile.Speciality)
            {
                if (item == Speciality.Shield)
                    return true;
            }

            return false;
        }

        private void DistributePoints(int result)
        {
            var points = new List<int>();
            var players = new List<IOwner>();
            var playersToGetPoints = new List<IOwner>();
            foreach (var i in _figuresOnTiles)
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
                for (int j = 0; j < _figuresOnTiles.Count; j++)
                {
                    if (players[i].Equals(_figuresOnTiles[j].Owner))
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

            if (players.Count != 0)
            {
                playersToGetPoints.Add(players[maxIndex]);
            }
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i] == points[maxIndex] && i != maxIndex)
                {
                    playersToGetPoints.Add(players[i]);
                }
            }
            foreach (var i in playersToGetPoints)
            {
                i.Points += result;
            }
        }


        private int getFromDirection(int c)
        {
            switch (c)
            {
                case 0:
                    return 2;
                case 1:
                    return 3;
                case 2:
                    return 0;
                case 3:
                    return 1;
                default:
                    return 0;
            }
        }

        private bool CheckEndOfCastle(BoardTile bt)
        {
            foreach (var item in bt.BackendTile.Speciality)
            {
                if (item == Speciality.EndOfCastle)
                    return true;
            }

            return false;
        }

        private bool IsChecked(BoardTile currentTile, List<BoardTile> btList)
        {
            foreach (var item in btList)
            {
                if (item == currentTile)
                    return true;
            }

            return false;
        }



        private int GetIndex(int side, int index)
        {
            switch (side)
            {
                case 0:
                    if (index - 10 < 0)
                    {
                        _outOfRange = true;
                        return 0;
                    }

                    return index - 10;
                case 1:
                    if (index + 1 > _boardTilesSize)
                    {
                        _outOfRange = true;
                        return 0;
                    }


                    return index + 1;
                case 2:
                    if (index + 10 > _boardTilesSize)
                    {
                        _outOfRange = true;
                        return 0;
                    }


                    return index + 10;
                case 3:
                    if (index - 1 < 0)
                    {
                        _outOfRange = true;
                        return 0;
                    }


                    return index - 1;
                default: return 0;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Castle;
        }

        // If the current tile doesn't have endofcastle speciality 
        private int CalculateCastle(BoardTile currentTile, bool gameover)
        {
            // Check if the castle is not finished         
            if (!_finishedCastle && !_gameOver)
                return 0;

            // Get the CurrentTile's coordinate
            var x = currentTile.Coordinates.X;
            var y = currentTile.Coordinates.Y;
            var index = x + (y * 10);

            if (x < 0 || y < 0 || x > 9 || y > 9)
                return 0;


            // If it is the first tile, reset the properties...
            if (_firstCall)
            {
                _firstCall = false;
                _firstTile = currentTile;
                _currentBoardTile = currentTile;
                _firstTile = currentTile;
                _points = 0;
                _finishedCastle = true;
                _boardTileList.Clear();
                _figuresOnTiles.Clear();

            }


            if (IsChecked(_currentBoardTile, _boardTileList))
                return 0;


            if (_currentBoardTile.BackendTile.Directions == null)
            {
                if (!_gameOver)
                    _finishedCastle = false;

                return 0;
            }


            // If it is an EndOfCastle tile and it isn't the first tile...
            if (CheckEndOfCastle(_currentBoardTile) && _firstTile != _currentBoardTile)
            {
                if (_currentBoardTile.BackendTile.Directions[getFromDirection(_whereToGo)].Figure != null)
                    _figuresOnTiles.Add(_currentBoardTile.BackendTile.Directions[getFromDirection(_whereToGo)].Figure);

                if (_deleteFigures && _currentBoardTile.BackendTile.Directions[getFromDirection((int)_whereToGo)].Figure != null)
                {
                    _utils.GiveBackFigureToOwner(_currentBoardTile.BackendTile.Directions[getFromDirection((int)_whereToGo)].Figure);
                    _currentBoardTile.BackendTile.Directions[getFromDirection((int)_whereToGo)].Figure = null;
                }

                _points += 2;
                return 0;
            }




            for (int i = 0; i < 4; i++)
            {
                if (_currentBoardTile.BackendTile.Directions[i].Landscape is Castle)
                {
                    if (_currentBoardTile.BackendTile.Directions[i].Figure != null)
                    {
                        _figuresOnTiles.Add(_currentBoardTile.BackendTile.Directions[i].Figure);
                    }

                    if (_deleteFigures && _currentBoardTile.BackendTile.Directions[i].Figure != null)
                    {
                        _utils.GiveBackFigureToOwner(_currentBoardTile.BackendTile.Directions[i].Figure);
                        _currentBoardTile.BackendTile.Directions[i].Figure = null;
                    }
                        

                    _whereToGo = i;
                    _boardTileList.Add(_currentBoardTile);
                    _currentBoardTile = _utils.GetOneNeighborTile(_currentBoardTile, GetIndex(i, index));
                    CalculateCastle(_currentBoardTile, false);
                    _currentBoardTile = currentTile;
                }
            }


            if (_finishedCastle)
            {
                _points += 2;
                if (CheckShield(_currentBoardTile))
                    _points += 2;
            }
            else
                _points = 0;

            if (_deleteFigures)
                _points = 0;


            if (_firstTile == _currentBoardTile && _points == 1)
                _points = 0;


            return _points;
        }
    }
}
