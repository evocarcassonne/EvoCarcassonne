using EvoCarcassonne.Models;

namespace EvoCarcassonne.Backend
{
    public class Castle : ILandscape
    {
        private bool _canPlaceFigure { get; set; }
        public Castle()
        {
        }

        public override bool Equals(object obj)
        {
            return obj is Castle;
        }

        public void calculate(BoardTile currentTile, bool gameover, Utils utils)
        {
            throw new System.NotImplementedException();
        }

        public bool CanPlaceFigure(BoardTile currentTile, CardinalDirection whereToGo, Utils utils, bool firstCall)
        {
            _utils = utils;
            _boardTilesSize = _utils.GetBoardTilesSize();
            _placedCastleTiles.Clear();
            _gameOver = true;

            int result = 0;

            if (CheckEndOfCastle(currentTile) && currentTile.BackendTile.Directions[(int)whereToGo].Landscape is Castle)
            {
                _firstCall = true;
                result += CalculateWithDirections(currentTile, whereToGo);

                if (result > 0 && _figuresOnTiles.Count > 0)
                    _canPlaceFigure = false;
                else
                    _canPlaceFigure = true;

                result = 0;

            }
            else if (!CheckEndOfCastle(currentTile))
            {

                _firstCall = true;
                result += CalculateCastle(currentTile, false);

                if (result > 0 && _figuresOnTiles.Count > 0)
                    _canPlaceFigure = false;
                else
                    _canPlaceFigure = true;

                result = 0;

            }


            return _canPlaceFigure;
        }
    }
}
