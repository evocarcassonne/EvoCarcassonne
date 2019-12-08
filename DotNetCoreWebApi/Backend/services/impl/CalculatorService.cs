using System.Collections.Generic;
using DotNetCoreWebApi.Backend.Model;

namespace DotNetCoreWebApi.Backend.services.impl
{
    class CalculatorService : ICalculateService
    {
        private ChurchCalculatorService churchCalculatorService;
        private CastleCalculatorService castleCalculatorService;
        private RoadCalculatorService roadCalculatorService;

        public CalculatorService(ChurchCalculatorService churchCalculatorService, CastleCalculatorService castleCalculatorService, RoadCalculatorService roadCalculatorService)
        {
            this.churchCalculatorService = churchCalculatorService;
            this.castleCalculatorService = castleCalculatorService;
            this.roadCalculatorService = roadCalculatorService;
        }

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
                    churchCalculatorService.calculate(tile, gameover, out figures);
                    figuresToGiveBack.AddRange(figures);
                }
            }

            //Searching for castle sides, paying attention to be called only once
            foreach (var i in currentTile.Directions)
            {
                if (i.Landscape == Landscape.Castle)
                {
                    List<IFigure> figures;
                    castleCalculatorService.calculate(currentTile, gameover, out figures);
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
                    roadCalculatorService.calculate(currentTile, gameover, out figures);
                    figuresToGiveBack.AddRange(figures);
                    break;
                }
            }
        }

        public bool CanPlaceFigure(ITile currentTile, CardinalDirection whereToGo, bool firstCall)
        {
            switch (currentTile.Directions[(int)whereToGo].Landscape)
            {
                case Landscape.Castle: return castleCalculatorService.CanPlaceFigure(currentTile, whereToGo, firstCall);
                case Landscape.Field: return true;
                case Landscape.Road: return roadCalculatorService.CanPlaceFigure(currentTile, whereToGo, firstCall);
                default: return true;
            }
        }

    }

}