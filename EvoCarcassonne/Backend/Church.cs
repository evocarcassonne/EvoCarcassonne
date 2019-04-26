using System.Collections.Generic;
using EvoCarcassonne.Model;

namespace EvoCarcassonne.Backend
{
    public class Church : ITile, ILandscape
    {
        public int TileID { get; set; }
        public List<IDirection> Directions { get; set; }
        public Speciality Speciality { get; set; }

        public Church(int tileId, List<IDirection> directions, Speciality speciality)
        {
            TileID = tileId;
            Directions = directions;
            Speciality = speciality;
        }

        public void Rotate(int direction)
        {
            throw new System.NotImplementedException();
        }

        public IDirection getTileSideByCardinalDirection(CardinalDirection side)
        {
            throw new System.NotImplementedException();
        }

        public int calculate(BoardTile currentTile, CardinalDirection whereToGo, bool firstCall)
        {
            return 0;
        }
    }
}
