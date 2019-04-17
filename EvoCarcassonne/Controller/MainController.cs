using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using EvoCarcassonne.Backend;
using EvoCarcassonne.Model;

namespace EvoCarcassonne.Controller
{
    public class MainController
    {
        #region Public Properties

        public static ObservableCollection<BoardTile> BoardTiles { get; set; }

        /**
         * Contains the currently placed tiles on the board. When putting down a tile that tile should be added to list as well.
         */
        public static ObservableCollection<BoardTile> PlacedBoardTiles { get; set; } 
        
        /// <summary>
        /// The current player can put down this tile
        /// </summary>
        public Tile CurrentTile { get; set; }

        /// <summary>
        /// The current tile's ID
        /// </summary>
        public int CurrentTileID { get; set; } = 0;

        /// <summary>
        /// A flag that indicates if the round is started
        /// </summary>
        public bool IsRoundStarded { get; set; } = false;

        #endregion

        #region Commands

        /// <summary>
        /// The command to get a new Tile
        /// </summary>
        public ICommand GetNewTileCommand { get; set; }

        /// <summary>
        /// The command to rotate the CurrentTile left
        /// </summary>
        public ICommand RotateLeftCommand { get; set; }

        /// <summary>
        /// The command to rotate the CurrentTile right
        /// </summary>
        public ICommand RotateRightCommand { get; set; }


        #endregion

        #region Constructor

        public MainController()
        {
            LoadTiles();

            // Create commands            
            RotateLeftCommand = new RelayCommand(() => CurrentTile.Rotate(-90));
            RotateRightCommand = new RelayCommand(() => CurrentTile.Rotate(90));
        }
        #endregion

        #region Private Methods

        private void LoadTiles()
        {
            var boardTiles = new ObservableCollection<BoardTile>();
            
            PlacedBoardTiles = new ObservableCollection<BoardTile>();
            
            var random = new Random();

            var tilesImageList = Utils.GetResourceNames(@"tiles");

            for (var x = 0; x < 10; x++)
            {
                for (var y = 0; y < 10; y++)
                {
                    boardTiles.Add(new BoardTile
                    {
                        Tag = $"{x};{y}",
                        Coordinates = new Coordinates(x, y),
                        Image = tilesImageList[random.Next(tilesImageList.Count)], // for testing purposes
                        // Image = null,
                        Angle = 0,
                        BackendTile = new Tile(0, null, Speciality.None)

                    });
                }
            }

            BoardTiles = boardTiles;
        }


        #endregion

        #region Public Methods

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

        #endregion

    }
}
