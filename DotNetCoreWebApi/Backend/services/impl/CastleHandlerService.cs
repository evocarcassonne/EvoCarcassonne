using System.Collections.Generic;
using DotNetCoreWebApi.Backend.Model;

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
        private List<IFigure> _figuresOnTiles { get; set; } = new List<IFigure>();
        private CardinalDirection _starterWhereToGo { get; set; }
        private bool _deleteFigures { get; set; } = false;
        private List<ITile> _placedCastleTiles { get; set; } = new List<ITile>();
        private bool _gameOver { get; set; }
        private bool _outOfRange { get; set; } = false;
        private List<IFigure> FiguresToGiveBacktoOwner = new List<IFigure>();

        public void calculate(ITile currentTile, bool gameover, out List<IFigure> figuresToGiveBack)
        {
            _gameOver = gameover;
            _placedCastleTiles.Clear();

            int result = 0;
            for (int i = 0; i < 4; i++)
            {
                _deleteFigures = false;
                if (CheckEndOfCastle(currentTile) && currentTile.Directions[i].Landscape == Landscape.Castle)
                {
                    _firstCall = true;
                    result += CalculateWithDirections(currentTile, (CardinalDirection)i);

                    _firstCall = true;
                    Utils.DistributePoints(result, _figuresOnTiles);
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
                    Utils.DistributePoints(result, _figuresOnTiles);
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

            figuresToGiveBack = FiguresToGiveBacktoOwner;
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
                _figuresOnTiles.Clear();
                FiguresToGiveBacktoOwner.Clear();
                _starterWhereToGo = whereToGo;
            }


            // Check if the castle is not finished         
            if (!_finishedCastle && !_gameOver)
                return 0;

            if (_outOfRange)
            {
                _finishedCastle = false;
                _outOfRange = false;
                return 0;
            }

            if (IsChecked(_currentITile, _ITileList))
                return 0;


            if (_currentITile.Directions == null)
            {
                if (!_gameOver)
                    _finishedCastle = false;
                return 0;
            }

            // If it is an EndOfCastle tile and it isn't the first tile...
            if (CheckEndOfCastle(_currentITile) && _firstTile != _currentITile)
            {
                if (_currentITile.Directions[(int)Utils.GetOppositeDirection(whereToGo)].Figure != null)
                    _figuresOnTiles.Add(_currentITile.Directions[(int)Utils.GetOppositeDirection(whereToGo)].Figure);

                if (_deleteFigures && _currentITile.Directions[(int)Utils.GetOppositeDirection(whereToGo)].Figure != null)
                {
                    FiguresToGiveBacktoOwner.Add(_currentITile.Directions[(int)Utils.GetOppositeDirection(whereToGo)].Figure);
                    _currentITile.Directions[(int)Utils.GetOppositeDirection(whereToGo)].Figure = null;
                }


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
                        if (_currentITile == _firstTile && (int)_starterWhereToGo == (int)Utils.GetOppositeDirection(whereToGo) && !_firstCall)
                            return 0;

                        if (_currentITile.Directions[i].Figure != null)
                        {
                            _figuresOnTiles.Add(_currentITile.Directions[i].Figure);
                        }

                        if (_deleteFigures && _currentITile.Directions[i].Figure != null)
                        {
                            FiguresToGiveBacktoOwner.Add(_currentITile.Directions[i].Figure);
                            _currentITile.Directions[i].Figure = null;
                        }

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

            if (_deleteFigures)
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
            if (!_finishedCastle && !_gameOver)
                return 0;

            if (_outOfRange)
            {
                _finishedCastle = false;
                _outOfRange = false;
                return 0;
            }

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
                _figuresOnTiles.Clear();
                FiguresToGiveBacktoOwner.Clear();
            }


            if (IsChecked(_currentITile, _ITileList))
                return 0;


            if (_currentITile.Directions == null)
            {
                if (!_gameOver)
                    _finishedCastle = false;

                return 0;
            }


            // If it is an EndOfCastle tile and it isn't the first tile...
            if (CheckEndOfCastle(_currentITile) && _firstTile != _currentITile)
            {
                if (_currentITile.Directions[(int)Utils.GetOppositeDirection((CardinalDirection)_whereToGo)].Figure != null)
                    _figuresOnTiles.Add(_currentITile.Directions[(int)Utils.GetOppositeDirection((CardinalDirection)_whereToGo)].Figure);

                if (_deleteFigures && _currentITile.Directions[(int)Utils.GetOppositeDirection((CardinalDirection)_whereToGo)].Figure != null)
                {
                    FiguresToGiveBacktoOwner.Add(_currentITile.Directions[(int)Utils.GetOppositeDirection((CardinalDirection)_whereToGo)].Figure);
                    _currentITile.Directions[(int)Utils.GetOppositeDirection((CardinalDirection)_whereToGo)].Figure = null;
                }

                _points += 2;
                return 0;
            }

            for (int i = 0; i < 4; i++)
            {
                if (_currentITile.Directions[i].Landscape == Landscape.Castle)
                {
                    if (_currentITile.Directions[i].Figure != null)
                    {
                        _figuresOnTiles.Add(_currentITile.Directions[i].Figure);
                    }

                    if (_deleteFigures && _currentITile.Directions[i].Figure != null)
                    {
                        FiguresToGiveBacktoOwner.Add(_currentITile.Directions[i].Figure);
                        _currentITile.Directions[i].Figure = null;
                    }

                    _whereToGo = i;
                    _ITileList.Add(_currentITile);
                    _currentITile = _currentITile.GetTileSideByCardinalDirection((CardinalDirection)i).Neighbor;
                    CalculateCastle(_currentITile, false);
                    _currentITile = currentTile;
                }
            }

            CheckForPoints();
            return _points;
        }

        public bool CanPlaceFigure(ITile currentTile, CardinalDirection whereToGo, bool firstCall)
        {
            _placedCastleTiles.Clear();
            _gameOver = true;

            int result = 0;

            if (CheckEndOfCastle(currentTile) && currentTile.Directions[(int)whereToGo].Landscape == Landscape.Castle)
            {
                _firstCall = true;
                result += CalculateWithDirections(currentTile, whereToGo);

                if (_figuresOnTiles.Count > 0)
                    _canPlaceFigure = false;
                else
                    _canPlaceFigure = true;

                result = 0;

            }
            else if (!CheckEndOfCastle(currentTile))
            {

                _firstCall = true;
                result += CalculateCastle(currentTile, false);

                if (_figuresOnTiles.Count > 0)
                    _canPlaceFigure = false;
                else
                    _canPlaceFigure = true;

                result = 0;

            }


            return _canPlaceFigure;
        }

        public override bool Equals(object obj)
        {
            return (Landscape)obj == Landscape.Castle;
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