
namespace Backend
{
    public interface ILandscape
    {
        void calculate(ITile currentTile, bool gameover, Utils utils);

        bool CanPlaceFigure(ITile currentTile, CardinalDirection whereToGo, Utils utils, bool firstCall);

    }
}
