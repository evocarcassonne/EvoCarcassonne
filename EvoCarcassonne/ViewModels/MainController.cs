using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EvoCarcassonne.Backend;
using EvoCarcassonne.Models;
using Newtonsoft.Json;

namespace EvoCarcassonne.ViewModels
{
    public class MainController : ObservableObject, IViewModel
    {

        #region Public Properties

        /// <summary>
        /// Contains the currently placed tiles on the board. When putting down a tile that tile should be added to list as well.
        /// </summary>
        [JsonProperty]
        public ObservableCollection<BoardTile> PlacedBoardTiles { get; set; } = new ObservableCollection<BoardTile>();
        public ObservableCollection<BoardTile> BoardTiles { get; set; }
        

        /// <summary>
        /// Contains the players
        /// </summary>
        public ObservableCollection<Player> Players { get; set; } = new ObservableCollection<Player>();

       
        /// <summary>
        /// Gets or sets the current round number
        /// </summary>
        public int CurrentRound { get; set; } = 1;

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
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// The current player
        /// </summary>
        public Player CurrentPlayer
        {
            get => _currentPlayer;
            set
            {
                if (_currentPlayer != value)
                {
                    _currentPlayer = value;
                    OnPropertyChanged();
                }
            }
        }

        
        #endregion

        #region Private Members

        private BoardTile _currentBoardTile;
        private Player _currentPlayer;
        private bool _tileIsDown;
        private bool _hasCurrentTile;
        private bool _alreadyCalculated;
        private int _currentSideForFigure = -1;
        private bool _figureDown = false;
        /// <summary>
        /// A flag that indicates if the current tile is on the board
        /// </summary>
        private bool TileIsDown
        {
            get => _tileIsDown;
            set
            {
                if (_tileIsDown != value)
                {
                    _tileIsDown = value;
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// A flag that indicates if the player has a tile
        /// </summary>
        private bool HasCurrentTile
        {
            get => _hasCurrentTile;
            set
            {
                if (_hasCurrentTile != value)
                {
                    _hasCurrentTile = value;
                    OnPropertyChanged();
                }
            }
        }
        private Utils Utils { get; set; }
        private ObservableCollection<BoardTile> TileStack { get; set; }
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

        /// <summary>
        /// The command to place the CurrentTile to the board
        /// </summary>
        public ICommand PlaceTileCommand { get; set; }

        /// <summary>
        /// The command to place figure in the CurrentTile
        /// </summary>
        public ICommand PlaceFigureCommand { get; set; }

        #endregion

        #region Constructor

        [JsonConstructor]
        public MainController()
        {
            Utils = new Utils(this);
            // Create commands            
            RotateLeftCommand = new RelayCommand(() => Rotate(-90), CanRotate);
            RotateRightCommand = new RelayCommand(() => Rotate(90), CanRotate);
            GetNewTileCommand = new RelayCommand(GetNewTile, CanGetNewTile);
            EndTurnCommand = new RelayCommand(EndTurn, CanEndTurn);
            PlaceTileCommand = new RelayCommand<Button>(PlaceTile, CanPlaceTile);
            PlaceFigureCommand = new RelayCommand<Button>(PlaceFigure, CanPlaceFigure);
        }

        public MainController(IEnumerable<Player> players) : this()
        {
            LoadTiles();

            foreach (var player in players)
            {
                Players.Add(player);
            }

            CurrentPlayer = Players.First();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Load the board
        /// </summary>
        private void LoadTiles()
        {
            TileStack = new TileParser().TileStack;
            var boardTiles = new ObservableCollection<BoardTile>();

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
                        starterTile.CanPlaceFigure = false;

                        boardTiles.Add(starterTile);
                        PlacedBoardTiles.Add(starterTile);
                    }
                    else
                    {
                        // Put empty tiles to the board
                        var emptyBoardTile = new BoardTile(0, new Coordinates(x, y), $"{x};{y}", null, new Tile(null, nullSpecialty));
                        emptyBoardTile.CanPlaceFigure = false;
                        boardTiles.Add(emptyBoardTile);
                    }
                }
            }

            BoardTiles = boardTiles;
        }

        private void Rotate(int angle)
        {
            CurrentBoardTile.Angle += angle;
            CurrentBoardTile.BackendTile.Rotate(angle);
        }

        private bool CanRotate()
        {
            return HasCurrentTile;
        }

        private void GetNewTile()
        {
            TileIsDown = false;
            HasCurrentTile = true;

            _alreadyCalculated = false;

            var random = new Random();

            CurrentBoardTile = TileStack.RemoveAndGet(random.Next(TileStack.Count));
        }

        private bool CanGetNewTile()
        {
            if (TileStack.Count == 0)
            {
                return false;
            }

            return !HasCurrentTile && !TileIsDown;
        }

        private void EndTurn()
        {
            TileIsDown = false;
            PlacedBoardTiles.Last().CanPlaceFigure = false;

            if (TileStack.Count == 0)
            {
                CalculateGameOver();
                return;
            }

            CallCalculate();
            _figureDown = false;
            CurrentRound++;
            CurrentPlayer = Players[(Players.IndexOf(CurrentPlayer) + 1) % Players.Count];
        }

        private void CallCalculate()
        {
            if (_alreadyCalculated)
            {
                return;
            }

            //Checks whether there is a church nearby.
            var allSurrTiles = Utils.GetAllSurroundingTiles(PlacedBoardTiles.Last());
            allSurrTiles.Add(PlacedBoardTiles.Last());
            foreach (BoardTile tile in allSurrTiles)
            {
                if (tile.BackendTile is Church)
                {
                    var church = (Church)tile.BackendTile;
                    church.calculate(tile, false, Utils);
                }
            }

            //Searching for castle sides, paying attention to be called only once
            foreach (var i in PlacedBoardTiles.Last().BackendTile.Directions)
            {
                if (i.Landscape is Castle)
                {
                    i.Landscape.calculate(PlacedBoardTiles.Last(), false, Utils);
                    break;
                }
            }

            //Searching for road sides, paying attention to be called only once
            foreach (var i in PlacedBoardTiles.Last().BackendTile.Directions)
            {
                if (i.Landscape is Road)
                {
                    i.Landscape.calculate(PlacedBoardTiles.Last(), false, Utils);
                    break;
                }
            }

            _alreadyCalculated = true;
        }

        private bool CanEndTurn()
        {
            return !HasCurrentTile && TileIsDown;
        }

        private void PlaceTile(Button button)
        {
            var x = button.Tag.ToString().Split(';').Select(int.Parse).ToArray();
            var index = x[1] + (x[0] * 10);

            CurrentBoardTile.Player = CurrentPlayer;
            CurrentBoardTile.Tag = (string)button.Tag;
            CurrentBoardTile.Coordinates = new Coordinates(x[1], x[0]);
            CurrentBoardTile.CanPlaceFigure = true;
            if (!Utils.CheckFitOfTile(CurrentBoardTile))
            {
                CurrentBoardTile.Coordinates = null;
                CurrentBoardTile.Tag = null;
                return;
            }

            OnPropertyChanged("CurrentPlayer");
            BoardTiles[index] = CurrentBoardTile;
            PlacedBoardTiles.Add(CurrentBoardTile);
            TileIsDown = true;
            HasCurrentTile = false;

            // Set the current tile's image null
            var nullSpecialty = new List<Speciality> { Speciality.None };
            CurrentBoardTile = new BoardTile(0, null, null, null, new Tile(null, nullSpecialty));
        }
        private bool CanPlaceTile(Button button)
        {
            if (!HasCurrentTile || TileIsDown)
            {
                return false;
            }

            var x = button.Tag.ToString().Split(';').Select(int.Parse).ToArray();
            var index = x[1] + (x[0] * 10);

            if (BoardTiles[index].Image != null)
            {
                return false;
            }

            return true;
        }

        

        /// <summary>
        /// This method processes the front-end button component, and then calls himself with back-end values
        /// </summary>
        /// <param name="button"></param>
        private void PlaceFigure(Button button)
        {
           var directionIndex = int.Parse(button.Tag.ToString());
           var currentTile = PlacedBoardTiles.Last();
           PlaceFigure(currentTile, directionIndex);
        }

        /// <summary>
        /// Places a figure on the tile
        /// </summary>
        /// <param name="currentTile">The tile, to put figure on</param>
        /// <param name="side">Which side of tile should be the figure placed</param>
        private void PlaceFigure(BoardTile currentTile, int side)
        {
            if (_figureDown)
            {
                _figureDown = false;
                _currentSideForFigure = -1;

                if (side == 4 && currentTile.BackendTile is Church)
                {
                    currentTile.BackendTile.CenterFigure = null;
                }
                else
                {
                    currentTile.BackendTile.Directions[side].Figure = null;
                }

                Utils.GiveBackFigureToOwner(new Figure(currentTile.Player.BackendOwner));
            }
            else
            {
                var playerFigure = CurrentPlayer.Figures.RemoveAndGet(0);
                if (side == 4 && currentTile.BackendTile is Church)
                {
                    currentTile.BackendTile.CenterFigure = playerFigure;
                }
                else
                {
                    currentTile.BackendTile.Directions[side].Figure = playerFigure;
                }

                _figureDown = true;
                _currentSideForFigure = side;
            }
        }
        
        private bool CanPlaceFigure(Button button)
        {
            var directionIndex = int.Parse(button.Tag.ToString());
            var tile = PlacedBoardTiles.Last();
            
            if (_figureDown)
            {
                return directionIndex == _currentSideForFigure;
            }
            else{
                if (CurrentPlayer.Figures.Count == 0)
                {
                    return false;
                }
                if (tile.CanPlaceFigure)
                {
                    if (directionIndex == 4)
                    {
                        if (tile.BackendTile is Church backendTile)
                        {
                            return backendTile.CenterFigure == null;
                        }

                        return false;
                    }

                    if (directionIndex != 4)
                    {
                        return tile.BackendTile.Directions[directionIndex].Figure == null && tile.BackendTile.Directions[directionIndex].Landscape.CanPlaceFigure(tile, (CardinalDirection)directionIndex, Utils, true);
                    }
                    return tile.BackendTile.Directions[directionIndex].Figure == null;
                }
                return false;
            }
        }
        
        private void CalculateGameOver()
        {
            foreach (BoardTile tile in PlacedBoardTiles)
            {
                if (tile.BackendTile is Church && tile.BackendTile.CenterFigure != null)
                {
                    var churchTile = (Church)tile.BackendTile;
                    churchTile.calculate(tile, true,Utils);
                }
                for (int i = 0; i < 4; i++)
                {
                    if (tile.BackendTile.Directions[i].Figure != null && !(tile.BackendTile.Directions[i].Landscape is Field))
                    {
                        tile.BackendTile.Directions[i].Landscape.calculate(tile, true, Utils);
                    }
                }
            }
        }
        
        #endregion

    }
}
