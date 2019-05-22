using EvoCarcassonne.Model;

namespace EvoCarcassonne.Backend
{
    public class Castle : ILandscape
    {
        public int calculate()
        {
            throw new System.NotImplementedException();
        }

        public Castle()
        {
        }

        public int calculate(BoardTile currentTile, CardinalDirection whereToGo, bool firstCall, bool gameover)
        {
            throw new System.NotImplementedException();
        }


        public override bool Equals(object obj)
        {
            return obj is Castle;
        }
    }
}
