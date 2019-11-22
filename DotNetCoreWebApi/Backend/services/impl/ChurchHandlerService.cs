using System.Collections.Generic;
using System.Linq;
using DotNetCoreWebApi.Backend.Model;

namespace DotNetCoreWebApi.Backend.services.impl
{

    class ChurchCalculatorService
    {
        public void calculate(ITile currentTile, bool gameover, out List<IFigure> figuresToGiveBack)
        {
            List<ITile> surroundingTiles = new List<ITile>();
            figuresToGiveBack = new List<IFigure>();
            currentTile.Directions.ForEach(e => surroundingTiles.Add(e.Neighbor));

            var churchTile = (Church)currentTile;
            if (gameover)
            {
                churchTile.CenterFigure.Owner.Points += surroundingTiles.Count;
                churchTile.CenterFigure = null;
            }
            else
            {
                if (surroundingTiles.Count == 8 && surroundingTiles.All(e => e != null))
                {
                    if (churchTile.CenterFigure != null)
                    {
                        churchTile.CenterFigure.Owner.Points += 9;
                        figuresToGiveBack.Add(churchTile.CenterFigure);
                        churchTile.CenterFigure = null;
                    }
                }
            }
        }

        public bool CanPlaceFigure(ITile currentTile, CardinalDirection whereToGo, bool firstCall)
        {
            return true;
        }
    }
}