﻿namespace Backend
{
    public class Figure : IFigure
    {
        public IOwner Owner { get; set; }

        public Figure(IOwner owner)
        {
            Owner = owner;
        }
    }
}
