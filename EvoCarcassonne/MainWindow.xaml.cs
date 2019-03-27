using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Primitives;
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

            this.DataContext = new Board(10, 10);
        }

        private void UniformGrid_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
        }
    }

    public class Board
    {
        int _rows;
        int _columns;
        List<TileF> _tiles = new List<TileF>();

        public Board(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;
            Random random = new Random();

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    _tiles.Add(new TileF()
                    {
                        Data = string.Format("R{0}C{1}", r, c),
                        Background = new SolidColorBrush(Color.FromArgb(255, (byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)))
                    });
                }
            }
        }

        public int Rows
        {
            get { return _rows; }
            set { _rows = value; }
        }

        public int Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }

        public List<TileF> Tiles
        {
            get { return _tiles; }
            set { _tiles = value; }
        }
    }

    public class TileF
    {
        public string Data { get; set; }
        public SolidColorBrush Background { get; set; }
    }
}
