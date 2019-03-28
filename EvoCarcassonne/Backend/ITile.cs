using System.Collections.Generic;

namespace EvoCarcassonne.Backend
{
    public interface ITile

    {
        int TileID { get; set; }
        List<IDirection> Directions { get; set; }
        Speciality Speciality { get; set; }
        void Rotate(int direction);
        IDirection getTileSideByCardinalDirection(CardinalDirection side);
    }
}
