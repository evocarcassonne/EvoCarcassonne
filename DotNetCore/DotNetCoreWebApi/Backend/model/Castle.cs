using System.Collections.Generic;

namespace DotNetCoreWebApi.Backend.Model
{
    public class Castle : ILandscape
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
        public Castle()
        {
        }

        public void calculate(ITile currentTile, bool gameover, out List<IFigure> figuresToGiveBack)
        {
            _gameOver = gameover;
            _placedCastleTiles.Clear();

            int result = 0;
            for (int i = 0; i < 4; i++)
            {
                _deleteFigures = false;
                if (CheckEndOfCastle(currentTile) && currentTile.Directions[i].Landscape is Castle)
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

            // Get the CurrentTile's coordinate
            var x = currentTile.Position.X;
            var y = currentTile.Position.Y;
            var index = x + (y * 10);



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
                if (_currentITile.Directions[getFromDirection((int)whereToGo)].Figure != null)
                    _figuresOnTiles.Add(_currentITile.Directions[getFromDirection((int)whereToGo)].Figure);

                if (_deleteFigures && _currentITile.Directions[getFromDirection((int)whereToGo)].Figure != null)
                {
                    FiguresToGiveBacktoOwner.Add(_currentITile.Directions[getFromDirection((int)whereToGo)].Figure);
                    _currentITile.Directions[getFromDirection((int)whereToGo)].Figure = null;
                }
                    

                _points += 2;
                return 0;
            }

            for (int i = 0; i < 4; i++)
            {
                if (_currentITile.Directions[i].Landscape is Castle)
                {
                    if (_firstCall && i != (int)_starterWhereToGo)
                    {
                    }
                    else
                    {
                        if (_currentITile == _firstTile && (int)_starterWhereToGo == getFromDirection((int)whereToGo) && !_firstCall)
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


            return _points;
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

        public override bool Equals(object obj)
        {
            return obj is Castle;
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
                if (_currentITile.Directions[getFromDirection(_whereToGo)].Figure != null)
                    _figuresOnTiles.Add(_currentITile.Directions[getFromDirection(_whereToGo)].Figure);

                if (_deleteFigures && _currentITile.Directions[getFromDirection((int)_whereToGo)].Figure != null)
                {
                    FiguresToGiveBacktoOwner.Add(_currentITile.Directions[getFromDirection((int)_whereToGo)].Figure);
                    _currentITile.Directions[getFromDirection((int)_whereToGo)].Figure = null;
                }

                _points += 2;
                return 0;
            }

            for (int i = 0; i < 4; i++)
            {
                if (_currentITile.Directions[i].Landscape is Castle)
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


            return _points;
        }

        public bool CanPlaceFigure(ITile currentTile, CardinalDirection whereToGo, bool firstCall)
        {
            _placedCastleTiles.Clear();
            _gameOver = true;

            int result = 0;

            if (CheckEndOfCastle(currentTile) && currentTile.Directions[(int)whereToGo].Landscape is Castle)
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

                if ( _figuresOnTiles.Count > 0)
                    _canPlaceFigure = false;
                else
                    _canPlaceFigure = true;

                result = 0;

            }


            return _canPlaceFigure;
        }
    }
}
