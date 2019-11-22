using System.Collections.Generic;
using DotNetCoreWebApi.Backend.Model;

namespace DotNetCoreWebApi.Backend.services.impl
{
    class CalculatorService : ICalculateService
    {
        public void Calculate(ITile currentTile, bool gameover, out List<IFigure> figuresToGiveBack)
        {
            var allSurrTiles = figuresToGiveBack = new List<IFigure>();
            Utils.GetAllSurroundingTiles(currentTile);
            foreach (ITile tile in allSurrTiles)
            {
                if (tile is Church)
                {
                    var church = (Church)tile;
                    List<IFigure> figures;
                    new ChurchCalculatorService().calculate(tile, gameover, out figures);
                    figuresToGiveBack.AddRange(figures);
                }
            }

            //Searching for castle sides, paying attention to be called only once
            foreach (var i in currentTile.Directions)
            {
                if (i.Landscape == Landscape.Castle)
                {
                    List<IFigure> figures;
                    new CastleCalculatorService().calculate(currentTile, gameover, out figures);
                    figuresToGiveBack.AddRange(figures);
                    break;
                }
            }

            //Searching for road sides, paying attention to be called only once
            foreach (var i in currentTile.Directions)
            {
                if (i.Landscape == Landscape.Road)
                {
                    List<IFigure> figures;
                    new RoadCalculatorService().calculate(currentTile, gameover, out figures);
                    figuresToGiveBack.AddRange(figures);
                    break;
                }
            }
        }

        public bool CanPlaceFigure(ITile currentTile, CardinalDirection whereToGo, bool firstCall)
        {
            switch (currentTile.Directions[(int)whereToGo].Landscape)
            {
                case Landscape.Castle: return new CastleCalculatorService().CanPlaceFigure(currentTile, whereToGo, firstCall);
                case Landscape.Field: return true;
                case Landscape.Road: return new RoadCalculatorService().CanPlaceFigure(currentTile, whereToGo, firstCall);
                default: return true;
            }
        }

    }

}