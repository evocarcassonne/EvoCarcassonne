using DotNetCoreWebApi.Backend.Model;
using System.Collections.Generic;

namespace DotNetCoreWebApi.Backend.services
{
    public interface ICalculateService
    {
        void Calculate(ITile currentTile, bool gameover, out List<IFigure> figuresToGiveBack);
        bool CanPlaceFigure(ITile currentTile, CardinalDirection whereToGo, bool firstCall);
    }
}