using System;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace EvoCarcassonne.Controller
{
    public class MainController
    {
        public MainController()
        {
            LoadTiles();
        }

        public ObservableCollection<Tile> Tiles { get; set; }

        private void LoadTiles()
        {
            var tiles = new ObservableCollection<Tile>();
            var random = new Random();

            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    tiles.Add(new Tile
                    {
                        Tag = $"{i};{j}",
                        Background = new SolidColorBrush(Color.FromArgb(255, (byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256))),
                    });
                }
            }

            Tiles = tiles;
        }

    }

    public class Tile
    {
        public string Tag { get; set; }
        public SolidColorBrush Background { get; set; }

        public static Tile GetTile(int x, int y, ObservableCollection<Tile> tiles)
        {
            if (tiles.Count > x * 10 + y)
            {
                return tiles[x * 10 + y];
            }

            throw new IndexOutOfRangeException();
        }
    }
}
