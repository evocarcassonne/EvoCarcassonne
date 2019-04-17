using System.ComponentModel;
using EvoCarcassonne.Backend;

namespace EvoCarcassonne.Model
{
    public class BoardTileModel {}

    public class BoardTile : INotifyPropertyChanged
    {
        public Coordinates Coordinates { get; set; }
        public string Tag { get; set; }
        public string Image { get; set; }
        private double _angle;
        public double Angle
        {
            get => _angle;
            set
            {
                if (_angle != value)
                {
                    _angle = value;
                    RaisePropertyChanged("Angle");
                }
            }
        }
        public Tile BackendTile { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public override string ToString()
        {
            return "Coordinates    " + Coordinates +
                   "BackEndTile    " + BackendTile;
        }
    }

}
