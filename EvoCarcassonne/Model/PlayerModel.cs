﻿using System.Collections.ObjectModel;
using EvoCarcassonne.Backend;

namespace EvoCarcassonne.Model
{
    public class Player
    {
        public string Name { get; set; }

        public System.Windows.Media.Brush Color { get; set; }

        public IOwner BackendOwner { get; set; }

        public ObservableCollection<IFigure> Figures { get; set; }

        public const int FigureCount = 7;

        public Player(int index, string name, System.Windows.Media.Brush color)
        {
            Name = name;
            Color = color;
            BackendOwner = new Owner(index, name);
            Figures = new ObservableCollection<IFigure>();

            for (int i = 0; i < FigureCount; i++)
            {
                Figures.Add(new Figure(i, BackendOwner));
            }
        }
    }
}
