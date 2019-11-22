using System.Collections.Generic;
using DotNetCoreWebApi.Backend.Model;

namespace DotNetCoreWebApi.Backend.services.impl
{
    class CalculateService : ICalculateService
    {
        public void Calculate(ITile currentTile, bool gameover, out List<IFigure> figuresToGiveBack)
        {
            throw new System.NotImplementedException();
        }

        public bool CanPlaceFigure(ITile currentTile, CardinalDirection whereToGo, bool firstCall)
        {
            throw new System.NotImplementedException();
        }
    }

}