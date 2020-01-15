using System.Collections.Generic;

namespace DotNetCoreWebApi.Backend.Model
{
    public interface ITile
    {
        List<IDirection> Directions { get; set; }
        List<Speciality> Speciality { get; set; }
        Coordinates Position { get; set; }
        int Rotation { get; set; }
        void Rotate(int direction);
        IDirection GetTileSideByCardinalDirection(CardinalDirection side);
        CardinalDirection GetCardinalDirectionByIndex(int index);
        IFigure CenterFigure { get; set; }
        string PropertiesAsString { get; set; }
    }
}
