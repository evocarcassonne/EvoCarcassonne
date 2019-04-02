using System.ComponentModel;
using System.Windows.Media;

namespace EvoCarcassonne.Model
{
    public class BoardTileModel {}

    public class BoardTile : INotifyPropertyChanged
    {
        private string _tag;
        private SolidColorBrush _background;
        private Coordinates _coordinates;

        public Coordinates Coordinates
        {
            get => _coordinates;
            set
            {
                if (_coordinates != value)
                {
                    _coordinates = value;
                    RaisePropertyChanged("Coordinates");
                }
            }
        }

        public string Tag
        {
            get => _tag;
            set
            {
                if (_tag != value)
                {
                    _tag = value;
                    RaisePropertyChanged("Tag");
                }
            }
        }

        public SolidColorBrush Background
        {
            get => _background;
            set
            {
                if (_background != value)
                {
                    _background = value;
                    RaisePropertyChanged("Background");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

    public class Coordinates
    {
        public int X { get; }
        public int Y { get; }

        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({ X },{ Y })";
        }
    }
}
