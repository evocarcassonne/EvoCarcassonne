using EvoCarcassonne.Models;

namespace EvoCarcassonne.Backend
{
    public interface ILandscape
    {
        void calculate(BoardTile currentTile, bool gameover, Utils utils);
    }
}
