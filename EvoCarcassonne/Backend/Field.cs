using System;
using EvoCarcassonne.Models;

namespace EvoCarcassonne.Backend
{
    public class Field : ILandscape
    {
        public override bool Equals(object obj)
        {
            return obj is Field;
        }

        public void calculate(BoardTile currentTile, bool gameover, Utils utils)
        {
            throw new NotImplementedException();
        }
    }
}
