using EvoCarcassonne.Models;

namespace EvoCarcassonne.Backend
{
    public interface ILandscape
    {
        void calculate(BoardTile currentTile, bool gameover, Utils utils);

        bool CanPlaceFigure(BoardTile currentTile, CardinalDirection whereToGo, Utils utils, bool firstCall);

    }
}
