using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EvoCarcassonne
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var tiles = new List<Tile>(100);
            var random = new Random();

            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    tiles.Add(new Tile
                    {
                        Tag = $"{i + 1},{j + 1}",
                        Background = new SolidColorBrush(Color.FromArgb(255, (byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)))
                    });
                }
            }

            Board.ItemsSource = tiles;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button b)
            {
                b.Content = b.Tag.ToString();
            }
        }
    }

    public class Tile
    {
        public string Tag { get; set; }
        public SolidColorBrush Background { get; set; }
    }
}
