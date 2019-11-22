using System.Collections.Generic;
using DotNetCoreWebApi.Backend.Model;

namespace DotNetCoreWebApi.Backend.services
{
    interface ICalculateService
    {
        void Calculate(ITile currentTile, bool gameover, out List<IFigure> figuresToGiveBack);
        bool CanPlaceFigure(ITile currentTile, CardinalDirection whereToGo, bool firstCall);
    }
}