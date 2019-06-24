using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EvoCarcassonne.Backend
{
    public class Direction : ObservableObject, IDirection
    {
        private IFigure _figure;

        public ILandscape Landscape { get; set; }

        public IFigure Figure
        {
            get => _figure;
            set
            {
                if (_figure != value)
                {
                    _figure = value;
                    OnPropertyChanged();
                }
            }
        }

        public Direction(ILandscape landscape, IFigure figure)
        {
            Landscape = landscape;
            Figure = figure;
        }
    }
}
