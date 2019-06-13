using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using EvoCarcassonne.Backend;

namespace EvoCarcassonne.Model
{
    public class Player
    {
        public string Name { get; set; }

        public SolidColorBrush Color { get; set; }

        public IOwner BackendOwner { get; set; }

        public ObservableCollection<IFigure> Figures { get; set; }

        public const int FigureCount = 7;

        public Player(int index, string name, SolidColorBrush color)
        {
            Name = name;
            Color = color;
            BackendOwner = new Owner(index, name);
            Figures = new ObservableCollection<IFigure>();

            for (int i = 0; i <= FigureCount; i++)
            {
                Figures.Add(new Figure(i, BackendOwner));
            }
        }
    }
}
