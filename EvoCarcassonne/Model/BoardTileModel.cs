using System.ComponentModel;
using EvoCarcassonne.Backend;
using EvoCarcassonne.ViewModel;

namespace EvoCarcassonne.Model
{
    public class BoardTileModel {}

    public class BoardTile : BaseViewModel
    {
        private Coordinates _coordinates;
        private string _tag;
        private string _image;
        private double _angle;
        private Tile _backendTile;

        public Coordinates Coordinates
        {
            get { return _coordinates; }
            set { SetProperty(ref _coordinates, value); }
        }

        public string Tag
        {
            get { return _tag; }
            set { SetProperty(ref _tag, value); }
        }

        public string Image
        {
            get { return _image; }
            set { SetProperty(ref _image, value); }
        }

        public double Angle
        {
            get { return _angle; }
            set { SetProperty(ref _angle, value); }
        }

        public Tile BackendTile
        {
            get { return _backendTile; }
            set { SetProperty(ref _backendTile, value); }
        }
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
