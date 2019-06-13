using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EvoCarcassonne.Backend
{
    public class Direction : IDirection
    {
        private IFigure _figure;
        public int Id { get; set; }

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

        public IDirection Neighbor { get; set; }

        public Direction(int id, ILandscape landscape, IFigure figure, IDirection neighbor)
        {
            Id = id;
            Landscape = landscape;
            Figure = figure;
            Neighbor = neighbor;
        }

        public Direction(int id, ILandscape landscape, Figure figure)
        {
            Id = id;
            Landscape = landscape;
            Figure = figure;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
