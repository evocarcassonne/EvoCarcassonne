using System.ComponentModel;
using System.Windows.Media;

namespace EvoCarcassonne.Model
{
    public class BoardTileModel {}

    public class BoardTile : INotifyPropertyChanged
    {
        public Coordinates Coordinates { get; set; }
        public string Tag { get; set; }
        public SolidColorBrush Background { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class Coordinates
    {
        private int X { get; }
        private int Y { get; }

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
