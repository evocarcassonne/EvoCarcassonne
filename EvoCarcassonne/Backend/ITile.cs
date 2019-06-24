using System.Collections.Generic;

namespace EvoCarcassonne.Backend
{
    public interface ITile
    {
        List<IDirection> Directions { get; set; }
        List<Speciality> Speciality { get; set; }
        void Rotate(int direction);
        IDirection getTileSideByCardinalDirection(CardinalDirection side);
        CardinalDirection GetCardinalDirectionByIndex(int index);

        IFigure CenterFigure { get; set; }
    }
}
