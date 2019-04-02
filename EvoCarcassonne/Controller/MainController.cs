using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using EvoCarcassonne.Model;

namespace EvoCarcassonne.Controller
{
    public class MainController
    {
        public MainController()
        {
            LoadTiles();
        }

        public static ObservableCollection<BoardTile> BoardTiles { get; set; }

        private void LoadTiles()
        {
            var boardTiles = new ObservableCollection<BoardTile>();
            var random = new Random();

            for (var x = 0; x < 10; x++)
            {
                for (var y = 0; y < 10; y++)
                {
                    boardTiles.Add(new BoardTile
                    {
                        Tag = $"{x};{y}",
                        Background = GenerateRandomBackground(random),
                        Coordinates = new Coordinates(x, y)
                    });
                }
            }

            BoardTiles = boardTiles;
        }

        public static BoardTile GetTile(int x, int y)
        {
            if (BoardTiles.Count > x * 10 + y)
            {
                return BoardTiles[x * 10 + y];
            }
            throw new IndexOutOfRangeException();
        }

        public static BoardTile GetTile(int[] coordinates)
        {
            if (BoardTiles.Count > coordinates[0] * 10 + coordinates[1])
            {
                return BoardTiles[coordinates[0] * 10 + coordinates[1]];
            }
            throw new IndexOutOfRangeException();
        }

        private SolidColorBrush GenerateRandomBackground(Random random)
        {
            return new SolidColorBrush(Color.FromArgb(255,
                (byte)random.Next(256),
                (byte)random.Next(256),
                (byte)random.Next(256)));
        }
    }
}
