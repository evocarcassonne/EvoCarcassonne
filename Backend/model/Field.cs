
using System.Collections.Generic;

namespace Backend.Model
{
    public class Field : ILandscape
    {
        public override bool Equals(object obj)
        {
            return obj is Field;
        }

        public void calculate(ITile currentTile, bool gameover, out List<IFigure> figuresToGiveBack)
        {
            figuresToGiveBack = new List<IFigure>();
        }

        public bool CanPlaceFigure(ITile currentTile, CardinalDirection whereToGo, bool firstCall)
        {
            return false;
        }
    }
}
