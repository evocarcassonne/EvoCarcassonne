using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EvoCarcassonne.View
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView
    {
        public MainView()
        {
            InitializeComponent();

            var random = new Random();

            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    Tile.Tiles.Add(new Tile
                    {
                        Tag = $"{i};{j}",
                        Background = new SolidColorBrush(Color.FromArgb(255, (byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)))
                    });
                }
            }

            Board.ItemsSource = Tile.Tiles;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button b)
            {
                b.Content = b.Tag.ToString();
                var coords = b.Tag.ToString().Split(';').Select(int.Parse).ToArray();
                //MessageBox.Show(coords[0] +" " + coords[1]);
                var random = new Random();

                b.Background = new SolidColorBrush(Color.FromArgb(255, (byte)random.Next(256), (byte)random.Next(256),
                    (byte)random.Next(256)));
                //var x = Tile.GetTile(coords[0], coords[1]);
                //MessageBox.Show(x.Tag.ToString());
            }
        }

        public class Tile
        {
            public static readonly List<Tile> Tiles = new List<Tile>(100);

            public string Tag { get; set; }
            public SolidColorBrush Background { get; set; }

            public static Tile GetTile(int x, int y)
            {
                return Tiles[x * 10 + y];
            }
        }
    }
}
