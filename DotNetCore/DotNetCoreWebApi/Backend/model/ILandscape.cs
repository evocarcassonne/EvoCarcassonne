
using System.Collections.Generic;

namespace DotNetCoreWebApi.Backend.Model
{
    public interface ILandscape
    {
        void calculate(ITile currentTile, bool gameover, out List<IFigure> figuresToGiveBack);

        bool CanPlaceFigure(ITile currentTile, CardinalDirection whereToGo, bool firstCall);

    }
}
