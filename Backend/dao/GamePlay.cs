using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Model;
using Newtonsoft.Json;

namespace Backend.dao
{
    public class GamePlay
    {
        #region Public properties

        public Guid Id;
        public List<Player> Players = new List<Player>();
        public int CurrentRound
        {
            get => _currentRound;
            set
            {
                if (_currentRound != value)
                {
                    _currentRound = value;
                }
            }
        }
        
        /// <summary>
        /// The tile that is currently in hand, or on the table
        /// </summary>
        public ITile CurrentTile
        {
            get => _currentTile;
            set
            {
                if (_currentTile != value)
                {
                    _currentTile = value;
                }
            }
        }
        
        public Player CurrentPlayer
        {
            get => _currentPlayer;
            set
            {
                if (_currentPlayer != value)
                {
                    _currentPlayer = value;
                }
            }
        }

        #endregion

        #region Constructor

            [JsonConstructor]
            public GamePlay()
            {
                Id = Guid.NewGuid();
                _randomNumberGenerator = new Random();
            }

            public GamePlay(IEnumerable<Player> players) : this()
            {
                LoadTiles();

                foreach (var player in players)
                {
                    Players.Add(player);
                }

                CurrentPlayer = Players.First();
            }


                #endregion
        
        #region Public methods
        
        public ITile GetNewTile()
        {
            if (TileStack.Count == 0 || HasCurrentTile || TileIsDown)
            {
                return null;
            }

            TileIsDown = false;
            HasCurrentTile = true;
            _alreadyCalculated = false;

            CurrentTile = TileStack.RemoveAndGet(_randomNumberGenerator.Next(TileStack.Count));
            return CurrentTile;
        }

        public bool PlaceTile(Coordinates coordinates)
        {
            bool canPlaceTile = !(!HasCurrentTile || TileIsDown);

            PlacedTiles.ForEach(t => {
                if (!canPlaceTile || t.Position.Equals(coordinates))
                {
                    canPlaceTile = false;
                }
            });
            if (!canPlaceTile)
            {
                return false;
            }
            CurrentPlayer = CurrentPlayer;
            CurrentTile.Position = coordinates;
            CanPlaceFigureProperty = true;
            if (!Utils.CheckFitOfTile(CurrentTile))
            {
                CurrentTile.Position = null;
                return false;
            }

            UpdateTiles();
            PlacedTiles.Add(CurrentTile);
            TileIsDown = true;
            HasCurrentTile = false;

            CurrentTile = new Tile(null, new List<Speciality> { Speciality.None });
            return true;
        }

        private void UpdateTiles()
        {
            foreach (var placedTile in PlacedTiles)
            {
                if (placedTile.Position.X  == CurrentTile.Position.X + 1 && placedTile.Position.Y == CurrentTile.Position.Y)
                {
                    placedTile.Directions[3].Neighbor = CurrentTile;
                }
                if (placedTile.Position.X == CurrentTile.Position.X && placedTile.Position.Y == CurrentTile.Position.Y + 1)
                {
                    placedTile.Directions[0].Neighbor = CurrentTile;
                }
                if (placedTile.Position.X == CurrentTile.Position.X - 1 && placedTile.Position.Y == CurrentTile.Position.Y)
                {
                    placedTile.Directions[1].Neighbor = CurrentTile;
                }
                if (placedTile.Position.X == CurrentTile.Position.X && placedTile.Position.Y == CurrentTile.Position.Y - 1)
                {
                    placedTile.Directions[2].Neighbor = CurrentTile;
                }
            }
        }

        /// <summary>
        /// Places a figure on the tile
        /// </summary>
        /// <param name="currentTile">The tile, to put figure on</param>
        /// <param name="side">Which side of tile should be the figure placed</param>
        public bool PlaceFigure(int side)
        {
            if (!CanPlaceFigure(CurrentTile, side))
            {
                return false;
            }
            IFigure figureToGetdown = null;
            if (_figureDown)
            {
                _figureDown = false;
                _currentSideForFigure = -1;

                if (side == 4 && CurrentTile is Church)
                {
                    figureToGetdown = CurrentTile.CenterFigure;
                    CurrentTile.CenterFigure = null;
                }
                else
                {
                    figureToGetdown = CurrentTile.Directions[side].Figure;
                    CurrentTile.Directions[side].Figure = null;
                }

                if (figureToGetdown != null)
                {
                    Utils.GiveBackFigureToOwner(figureToGetdown, ref Players);   
                }
            }
            else
            {
                var playerFigure = CurrentPlayer.Figures.RemoveAndGet(0);
                if (side == 4 && CurrentTile is Church)
                {
                    CurrentTile.CenterFigure = playerFigure;
                }
                else
                {
                    CurrentTile.Directions[side].Figure = playerFigure;
                }

                _figureDown = true;
                _currentSideForFigure = side;
            }

            return true;
        }
        public void EndTurn()
        {
            TileIsDown = false;
            CanPlaceFigureProperty = false;

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
        #endregion

        #region Private methods
        private void LoadTiles()
        {
            TileStack = new TileParser().TileStack;
            for (var x = 0; x < 10; x++)
            {
                for (var y = 0; y < 10; y++)
                {
                    if (x == 5 && y == 5)
                    {
                        // Put a tile to the middle of the board
                        var starterTile = TileStack.RemoveAndGet(0);
                        starterTile.Position = new Coordinates(x, y);
                        CanPlaceFigureProperty = false;
                        PlacedTiles.Add(starterTile);
                    }
                }
            }
        }
        private bool CallCalculate()
        {
            if (_alreadyCalculated || HasCurrentTile || !TileIsDown)
            {
                return false;
            }

            //Checks whether there is a church nearby.
            var allSurrTiles = Utils.GetAllSurroundingTiles(PlacedTiles.Last());
            allSurrTiles.Add(PlacedTiles.Last());
            foreach (ITile tile in allSurrTiles)
            {
                if (tile is Church)
                {
                    var church = (Church)tile;
                    List<IFigure> figures;
                    church.calculate(tile, false, out figures);
                    foreach (var figure in figures)
                    {
                        Utils.GiveBackFigureToOwner(figure, ref Players);
                    }
                }
            }

            //Searching for castle sides, paying attention to be called only once
            foreach (var i in PlacedTiles.Last().Directions)
            {
                if (i.Landscape is Castle)
                {
                    List<IFigure> figures;
                    i.Landscape.calculate(PlacedTiles.Last(), false, out figures);
                    foreach (var figure in figures)
                    {
                        Utils.GiveBackFigureToOwner(figure, ref Players);
                    }
                    break;
                }
            }

            //Searching for road sides, paying attention to be called only once
            foreach (var i in PlacedTiles.Last().Directions)
            {
                if (i.Landscape is Road)
                {
                    List<IFigure> figures;
                    i.Landscape.calculate(PlacedTiles.Last(), false, out figures);
                    foreach (var figure in figures)
                    {
                        Utils.GiveBackFigureToOwner(figure, ref Players);
                    }
                    break;
                }
            }

            _alreadyCalculated = true;
            return true;
        }
        private bool CanPlaceFigure(ITile tile, int directionIndex)
        {
            if (_figureDown)
            {
                return directionIndex == _currentSideForFigure;
            }
            
            if (CurrentPlayer.Figures.Count == 0)
            {
                return false;
            }
            if (CanPlaceFigureProperty)
            {
                if (directionIndex == 4)
                {
                    if (tile is Church backendTile)
                    {
                        return backendTile.CenterFigure == null;
                    }

                    return false;
                }

                if (directionIndex != 4)
                {
                    return tile.Directions[directionIndex].Figure == null && tile.Directions[directionIndex].Landscape.CanPlaceFigure(tile, (CardinalDirection)directionIndex, true);
                }
                return tile.Directions[directionIndex].Figure == null;
            }
            return false;
        }
        private bool CanRotate()
        {
            return HasCurrentTile;
        }
        private void CalculateGameOver()
        {
            foreach (ITile tile in PlacedTiles)
            {
                if (tile is Church && tile.CenterFigure != null)
                {
                    var churchTile = (Church)tile;
                    List<IFigure> figures;
                    churchTile.calculate(tile, true, out figures);
                    foreach (var figure in figures)
                    {
                        Utils.GiveBackFigureToOwner(figure, ref Players);
                    }
                }
                for (int i = 0; i < 4; i++)
                {
                    if (tile.Directions[i].Figure != null && !(tile.Directions[i].Landscape is Field))
                    {
                        List<IFigure> figures;
                        tile.Directions[i].Landscape.calculate(tile, true, out figures);
                        foreach (var figure in figures)
                        {
                            Utils.GiveBackFigureToOwner(figure, ref Players);
                        }
                    }
                }
            }
        }
        #endregion

        #region Private properties

        [JsonProperty]
        private List<ITile> TileStack;
        private List<ITile> PlacedTiles = new List<ITile>();
        
        private int _currentRound = 1;
        private ITile _currentTile;
        private Player _currentPlayer;
        private bool _tileIsDown;
        private bool _hasCurrentTile;
        private bool _alreadyCalculated;
        private int _currentSideForFigure = -1;
        private bool _figureDown = false;
        private Random _randomNumberGenerator;
        private bool _canPlaceFigureProperty;

        private bool TileIsDown
        {
            get => _tileIsDown;
            set
            {
                if (_tileIsDown != value)
                {
                    _tileIsDown = value;
                }
            }
        }
        
        private bool HasCurrentTile
        {
            get => _hasCurrentTile;
            set
            {
                if (_hasCurrentTile != value)
                {
                    _hasCurrentTile = value;
                }
            }
        }
        
        private bool CanPlaceFigureProperty
        {
            get => _canPlaceFigureProperty;
            set => _canPlaceFigureProperty = value;
        }
        #endregion
    }
}