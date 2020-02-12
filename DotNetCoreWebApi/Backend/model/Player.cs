using System;
using System.Collections.Generic;

namespace DotNetCoreWebApi.Backend.Model
{
    public class Player
    {
        public Guid playerId { get; set; }
        private int _numberOfFigures = 7;
        public List<Figure> Figures = new List<Figure>();
        public IOwner Owner;
        public string Color { get; set; } = "#000";
        public Player(IOwner owner)
        {
            Owner = owner;
            for (int i = 0; i < _numberOfFigures; i++)
            {
                Figures.Add(new Figure(owner));
            }
        }
    }
}