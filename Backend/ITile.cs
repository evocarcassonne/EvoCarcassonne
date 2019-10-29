﻿using System.Collections.Generic;

namespace Backend
{
    public interface ITile
    {
        List<IDirection> Directions { get; set; }
        List<Speciality> Speciality { get; set; }
        Coordinates Position { get; set; }
        void Rotate(int direction);
        IDirection getTileSideByCardinalDirection(CardinalDirection side);
        CardinalDirection GetCardinalDirectionByIndex(int index);

        IFigure CenterFigure { get; set; }
    }
}