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
            
        }

        public bool CanPlaceFigure(BoardTile currentTile, CardinalDirection whereToGo, Utils utils, bool firstCall)
        {
            return false;
        }
    }
}
