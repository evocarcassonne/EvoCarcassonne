
namespace Backend
{
    public class Field : ILandscape
    {
        public override bool Equals(object obj)
        {
            return obj is Field;
        }

        public void calculate(ITile currentTile, bool gameover, Utils utils)
        {
            
        }

        public bool CanPlaceFigure(ITile currentTile, CardinalDirection whereToGo, Utils utils, bool firstCall)
        {
            return false;
        }
    }
}
