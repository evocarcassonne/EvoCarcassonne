using System.Collections.Generic;
using System.Linq;
using DotNetCoreWebApi.Backend.Model;
using DotNetCoreWebApi.Backend.Utils;

namespace DotNetCoreWebApi.Backend.services.impl
{
    class ChurchCalculatorService
    {
        public int calculate(ITile currentTile, bool gameover)
        {
            int points = 0;
            List<ITile> surroundingTiles = TileUtils.GetAllSurroundingTiles(currentTile);

            var churchTile = (Church)currentTile;

            if (gameover)
            {
                points = surroundingTiles.Count;
            }
            else
            {
                if (surroundingTiles.Count == 8 && surroundingTiles.All(e => e != null))
                {
                    if (churchTile.CenterFigure != null)
                    {
                        points = 9;
                    }
                }
            }
            return points;
        }

        public bool CanPlaceFigure(ITile currentTile, CardinalDirection whereToGo, bool firstCall)
        {
            return true;
        }
    }
}