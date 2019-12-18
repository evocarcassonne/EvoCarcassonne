using System.Collections.Generic;
using DotNetCoreWebApi.Backend.Model;

namespace DotNetCoreWebApi.Backend.services
{
    public interface IFigureService
    {
        bool CanPlaceFigure(ITile currentTile, CardinalDirection whereToGo, bool firstCall);
        List<IFigure> GetFiguresToGiveBack(ITile currentTile, CardinalDirection whereToGo, bool firstCall);
    }
}