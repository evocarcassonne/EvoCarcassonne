using System.Collections.Generic;
using EvoCarcassonne.Model;

namespace EvoCarcassonne.Backend
{
    public class Church : ITile, ILandscape
    {
        public int TileID { get; set; }
        public List<IDirection> Directions { get; set; }
        public List<Speciality> Speciality { get; set; }

        public Church(int tileId, List<IDirection> directions, List<Speciality> speciality)
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

        public CardinalDirection GetCardinalDirectionByIndex(int index)
        {
            switch (index)
            {
                case 0: return CardinalDirection.North;
                case 1: return CardinalDirection.East;
                case 2: return CardinalDirection.South;
                case 3: return CardinalDirection.West;
                default: return CardinalDirection.North;
            }
        }
    }
}
