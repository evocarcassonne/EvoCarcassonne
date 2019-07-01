using EvoCarcassonne.Backend;

namespace EvoCarcassonne.Models
{
    public class BoardTile : ObservableObject
    {
        public Coordinates Coordinates { get; set; }
        public string Tag { get; set; }
        public string Image { get; set; }
        private double _angle;
        private bool _canPlaceFigure;
        private ITile _backendTile;

        public double Angle
        {
            get => _angle;
            set
            {
                if (_angle != value)
                {
                    _angle = value;
                    OnPropertyChanged();
                }
            }
        }

        public ITile BackendTile
        {
            get => _backendTile;
            set
            {
                if (_backendTile != value)
                {
                    _backendTile = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool CanPlaceFigure
        {
            get => _canPlaceFigure;
            set
            {
                if (_canPlaceFigure != value)
                {
                    _canPlaceFigure = value;
                    OnPropertyChanged();
                }
            }
        }

        public override string ToString()
        {
            return "Coordinates    " + Coordinates +
                   "BackEndTile    " + BackendTile;
        }

        public BoardTile(double angle, Coordinates coordinates, string tag, string image, ITile backendTile)
        {
            _angle = angle;
            Coordinates = coordinates;
            Tag = tag;
            Image = image;
            BackendTile = backendTile;
        }

        public Player Player { get; set; }

        public BoardTile() { }
    }
}
