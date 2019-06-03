using System;
using EvoCarcassonne.Model;

namespace EvoCarcassonne.Backend
{
    public class Field : ILandscape
    {
        public int calculate(BoardTile currentTile, CardinalDirection whereToGo, bool firstCall, bool gameover)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            return obj is Field;
        }
    }
}
