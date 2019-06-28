using EvoCarcassonne.Models;

namespace EvoCarcassonne.Backend
{
    public class Castle : ILandscape
    {
        public Castle()
        {
        }

        public override bool Equals(object obj)
        {
            return obj is Castle;
        }

        public void calculate(BoardTile currentTile, bool gameover, Utils utils)
        {
            throw new System.NotImplementedException();
        }
    }
}
