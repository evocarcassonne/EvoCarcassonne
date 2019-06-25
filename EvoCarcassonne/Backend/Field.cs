using System;
using EvoCarcassonne.Models;

namespace EvoCarcassonne.Backend
{
    public class Field : ILandscape
    {
        public void calculate(BoardTile currentTile, bool gameover)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            return obj is Field;
        }
    }
}
