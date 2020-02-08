using System.Collections.Generic;
using System.Linq;
using DotNetCoreWebApi.Backend.Model;
using DotNetCoreWebApi.Backend.Utils;

namespace DotNetCoreWebApi.Backend.services.impl
{
    class ChurchCalculatorService
    {
        public int calculate(ITile currentTile, bool gameover, out List<IFigure> figuresToGiveBack)
        {
            int points = 0;
            List<ITile> surroundingTiles = TileUtils.GetAllSurroundingTiles(currentTile);
            figuresToGiveBack = new List<IFigure>();

            var churchTile = (Church)currentTile;

            if (gameover)
            {
                points = surroundingTiles.Count;
                figuresToGiveBack.Add(churchTile.CenterFigure);
                churchTile.CenterFigure = null;
            }
            else
            {
                if (surroundingTiles.Count == 8 && surroundingTiles.All(e => e != null))
                {
                    if (churchTile.CenterFigure != null)
                    {
                        points = 9;
                        figuresToGiveBack.Add(churchTile.CenterFigure);
                        churchTile.CenterFigure = null;
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