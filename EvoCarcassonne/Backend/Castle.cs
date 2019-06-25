using EvoCarcassonne.Models;

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

        public void calculate(BoardTile currentTile, bool gameover)
        {
            throw new System.NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            return obj is Castle;
        }
    }
}
