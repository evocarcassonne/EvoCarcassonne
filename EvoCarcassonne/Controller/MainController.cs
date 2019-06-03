using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EvoCarcassonne.Backend;
using EvoCarcassonne.Model;

namespace EvoCarcassonne.Controller
{
    public class MainController : BaseViewModel
    {

        #region Public Properties

        /**
         * Contains the currently placed tiles on the board. When putting down a tile that tile should be added to list as well.
         */
        public static ObservableCollection<BoardTile> PlacedBoardTiles { get; set; }

        public static ObservableCollection<BoardTile> BoardTiles { get; set; }

        public static ObservableCollection<BoardTile> TileStack { get; set; }


       
        /// <summary>
        /// The current tile's ID
        /// </summary>
        public int CurrentTileID { get; set; } = 0;

        /// <summary>
        /// A flag that indicates if the round is started
        /// </summary>
        public bool IsRoundStarted { get; set; } = false;

        /// <summary>
        /// A flag that indicates if the player has a tile
        /// </summary>
        public bool HasCurrentTile
        {
            get => _hasCurrentTile;
            set
            {
                if (_hasCurrentTile != value)
                {
                    _hasCurrentTile = value;
                    OnPropertyChanged("HasCurrentTile");
                }
            }
        }

        /// <summary>
        /// The current Tile
        /// </summary>
        public BoardTile CurrentBoardTile
        {
            get => _currentBoardTile;
            set
            {
                if (_currentBoardTile != value)
                {
                    _currentBoardTile = value;
                    OnPropertyChanged("CurrentBoardTile");
                }
            }
        }

        // The players
        public Owner Player1 = new Owner(1, "Pista");
        public Owner Player2 = new Owner(2, "Géza");

        /// <summary>
        /// The current player
        /// </summary>
        public Owner CurrentPlayer
        {
            get => _currentPlayer;
            set
            {
                if (_currentPlayer != value)
                {
                    _currentPlayer = value;
                    OnPropertyChanged("CurrentPlayer");
                }
            }
        }

        /// <summary>
        /// A flag that indicates if the current tile is on the board
        /// </summary>
        public bool TileIsDown
        {
            get => _tileIsDown;
            private set
            {
                if (_tileIsDown != value)
                {
                    _tileIsDown = value;
                    OnPropertyChanged("TileIsDown");
                }
            }
        }

        #endregion

        #region Private Members

        private BoardTile _currentBoardTile;
        private Owner _currentPlayer;
        private bool _tileIsDown;
        private bool _hasCurrentTile;

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

        /// <summary>
        /// The command to end the turn
        /// </summary>
        public ICommand EndTurnCommand { get; set; }

        #endregion

        #region Constructor

        public MainController()
        {
            LoadTiles();

            // Create commands            
            RotateLeftCommand = new RelayCommand(() => CurrentBoardTile.BackendTile.Rotate(-90));
            RotateRightCommand = new RelayCommand(() => CurrentBoardTile.BackendTile.Rotate(90));

            GetNewTileCommand = new RelayCommand(() => GetNewTile());
            EndTurnCommand = new RelayCommand(() => EndTurn());


            CurrentPlayer = Player1;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Load the board
        /// </summary>
        private void LoadTiles()
        {
            TileStack = TileParser.GetTileStack();
            var boardTiles = new ObservableCollection<BoardTile>();
            PlacedBoardTiles = new ObservableCollection<BoardTile>();

            var nullSpecialty = new List<Speciality> { Speciality.None };

            for (var x = 0; x < 10; x++)
            {
                for (var y = 0; y < 10; y++)
                {
                    if (x == 5 && y == 5)
                    {
                        // Put a tile to the middle of the board
                        var starterTile = TileStack.RemoveAndGet(0);
                        starterTile.Tag = $"{x};{y}";
                        starterTile.Coordinates = new Coordinates(x, y);
                        starterTile.BackendTile.TileID = CurrentTileID;

                        boardTiles.Add(starterTile);
                        PlacedBoardTiles.Add(starterTile);

                        CurrentTileID++;
                    }
                    else
                    {
                        // Put empty tiles to the board
                        var emptyBoardTile = new BoardTile(0, new Coordinates(x, y), $"{x};{y}", null, new Tile(0, null, nullSpecialty));
                        boardTiles.Add(emptyBoardTile);
                    }

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

        public void GetNewTile()
        {
            if (HasCurrentTile || TileIsDown)
                return;

            if (TileStack.Count < 1)
            {
                /* TODO */
                throw new NotImplementedException();
            }

            TileIsDown = false;
            HasCurrentTile = true;

            var random = new Random();

            CurrentBoardTile = TileStack.RemoveAndGet(random.Next(TileStack.Count));
            CurrentBoardTile.BackendTile.TileID = CurrentTileID;

            CurrentTileID++;

        }

        public void EndTurn()
        {
            if (!TileIsDown)
                return;

            TileIsDown = false;

            if (CurrentPlayer == Player1)
                CurrentPlayer = Player2;
            else
                CurrentPlayer = Player1;
        }

        public void PutTile(Button b)
        {
            if (TileIsDown)
                return;

            var x = b.Tag.ToString().Split(';').Select(int.Parse).ToArray();
            var index = x[1] + (x[0] * 10);

            if (BoardTiles[index].Image == null)
            {
                CurrentBoardTile.Tag = (string) b.Tag;
                CurrentBoardTile.Coordinates = new Coordinates(x[1], x[0]);
                if (!Utils.CheckFitOfTile(CurrentBoardTile))
                {
                    CurrentBoardTile.Coordinates = null;
                    CurrentBoardTile.Tag = null;
                    return;
                }
                BoardTiles[index] = CurrentBoardTile;
                PlacedBoardTiles.Add(CurrentBoardTile);
                TileIsDown = true;
                HasCurrentTile = false;
            }
            //BoardTiles[index] = new BoardTile(CurrentBoardTile.Angle, new Coordinates(x[0], x[1]), (string) b.Tag, CurrentBoardTile.Image, CurrentBoardTile.BackendTile);
            //BoardTiles[index] = new BoardTile
            //{
            //    Tag = (string) b.Tag,
            //    Coordinates = new Coordinates(x[0], x[1]),
            //    Image = CurrentBoardTile.Image, 
            //    Angle = CurrentBoardTile.Angle,
            //    BackendTile = new Tile(0, null, specialities)

            //};
            else
                return;

            // Set the current tile's image null
            var nullSpecialty = new List<Speciality> { Speciality.None };
            CurrentBoardTile = new BoardTile(0, null, null, null, new Tile(0, null, nullSpecialty));
        }

        #endregion

    }
}
