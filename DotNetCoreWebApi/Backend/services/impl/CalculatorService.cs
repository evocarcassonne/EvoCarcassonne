using System.Collections.Generic;
using DotNetCoreWebApi.Backend.Model;
using DotNetCoreWebApi.Backend.Utils;

namespace DotNetCoreWebApi.Backend.services.impl
{
    class CalculatorService : ICalculateService
    {
        public ChurchCalculatorService ChurchCalculatorService { get; set; }
        public CastleCalculatorService CastleCalculatorService { get; set; }
        public RoadCalculatorService RoadCalculatorService { get; set; }
        public IFigureService FigureService { get; set; }

        public CalculatorService(ChurchCalculatorService churchCalculatorService, CastleCalculatorService castleCalculatorService, RoadCalculatorService roadCalculatorService, IFigureService figureService)
        {
            ChurchCalculatorService = churchCalculatorService;
            CastleCalculatorService = castleCalculatorService;
            RoadCalculatorService = roadCalculatorService;
            FigureService = figureService;
        }

        public void Calculate(ITile currentTile, bool gameover, ref List<Player> players)
        {
            int points = 0;
            var allSurrTiles = TileUtils.GetAllSurroundingTiles(currentTile);
            foreach (ITile tile in allSurrTiles)
            {
                if (tile is Church)
                {
                    var church = (Church)tile;
                    List<IFigure> figures;
                    points += ChurchCalculatorService.calculate(tile, gameover, out figures);
                    if (points != 0)
                    {
                        figures = FigureService.GetFiguresToGiveBack(currentTile, 0, true);
                        CalculateUtils.DistributePoints(points, figures);
                        foreach (var figure in figures)
                        {
                            CalculateUtils.GiveBackFigureToOwner(figure, ref players);
                        }
                    }
                }
            }

            //Searching for castle sides, paying attention to be called only once
            for (int i = 0; i < 4; i++)
            {
                if (currentTile.Directions[i].Landscape == Landscape.Castle)
                {
                    List<IFigure> figures;
                    points += CastleCalculatorService.calculate(currentTile, gameover);
                    if (points != 0)
                    {
                        figures = FigureService.GetFiguresToGiveBack(currentTile, (CardinalDirection)i, true);
                        CalculateUtils.DistributePoints(points, figures);
                        foreach (var figure in figures)
                        {
                            CalculateUtils.GiveBackFigureToOwner(figure, ref players);
                        }
                    }
                    break;
                }
            }

            //Searching for road sides, paying attention to be called only once
            for (int i = 0; i < 4; i++)
            {
                if (currentTile.Directions[i].Landscape == Landscape.Road)
                {
                    List<IFigure> figures;
                    points += RoadCalculatorService.calculate(currentTile, gameover);
                    if (points != 0)
                    {
                        figures = FigureService.GetFiguresToGiveBack(currentTile, (CardinalDirection)i, true);
                        CalculateUtils.DistributePoints(points, figures);
                        foreach (var figure in figures)
                        {
                            CalculateUtils.GiveBackFigureToOwner(figure, ref players);
                        }
                    }
                    break;
                }
            }
        }

    }

}