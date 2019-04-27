using System.Collections.ObjectModel;
using EvoCarcassonne.Model;

namespace EvoCarcassonne.Backend
{
    public interface ILandscape
    {
        int calculate(BoardTile currentTile, CardinalDirection whereToGo, bool firstCall);
    }
}
