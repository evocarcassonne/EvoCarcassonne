using EvoCarcassonne.Model;

namespace EvoCarcassonne.Backend
{
    public interface ILandscape
    {
        void calculate(BoardTile currentTile, bool gameover);
    }
}
