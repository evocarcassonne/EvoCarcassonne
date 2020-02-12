using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            bool isCalledChurch = false;
            allSurrTiles.Add(currentTile);

            foreach (ITile tile in allSurrTiles)
            {
                if (tile is Church)
                {
                    var church = (Church)tile;
                    List<IFigure> figures;
                    foreach (var side in church.Directions)
                    {
                        if (side.Landscape == Landscape.Road)
                        {
                            isCalledChurch = true;
                        }
                    }

                    points += ChurchCalculatorService.calculate(tile, gameover);
                    if (points != 0)
                    {
                        figures = FigureService.GetFiguresToGiveBack(tile, 0, true);
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

                    var pointDirections = CastleCalculatorService.calculate(currentTile, gameover);
                    foreach (var item in pointDirections)
                    {
                        points = item.Value;
                        Console.WriteLine("");
                        Console.WriteLine("");
                        Console.WriteLine($"Erre megyek: {item.Key}, ennyi pontot találtam: {item.Value}, és CASTLE vagyok");
                        Console.WriteLine("");
                        Console.WriteLine("");
                        if (points != 0)
                        {
                            figures = FigureService.GetFiguresToGiveBack(currentTile, item.Key, true);
                            CalculateUtils.DistributePoints(points, figures);
                            foreach (var figure in figures)
                            {
                                CalculateUtils.GiveBackFigureToOwner(figure, ref players);
                            }
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
                    var pointDirections = RoadCalculatorService.calculate(currentTile, gameover);
                    foreach (var item in pointDirections)
                    {
                        points = item.Value;
                        Console.WriteLine("");
                        Console.WriteLine("");
                        Console.WriteLine($"Erre megyek: {item.Key}, ennyi pontot találtam: {item.Value}, és ROAD vagyok");
                        Console.WriteLine("");
                        Console.WriteLine("");
                        if (points != 0)
                        {
                            figures = FigureService.GetFiguresToGiveBack(currentTile, item.Key, !isCalledChurch);
                            CalculateUtils.DistributePoints(points, figures);
                            foreach (var figure in figures)
                            {
                                CalculateUtils.GiveBackFigureToOwner(figure, ref players);
                            }
                        }
                    }
                    break;
                }
            }
        }

    }

}