using DotNetCoreWebApi.Backend.dao;
using DotNetCoreWebApi.Backend.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetCoreWebApi.Backend.services.impl
{
    public class GamePlayService : IGamePlayService
    {
        internal GameController Controller = GameController.Instance;
        private ICalculateService calculateService;

        public GamePlayService(ICalculateService calculateService)
        {
            this.calculateService = calculateService;
        }

        public Player GetCurrentPlayer(Guid gameId)
        {
            return Controller.GetGamePlayById(gameId)?.CurrentPlayer;
        }

        public int GetCurrentRound(Guid gameId)
        {
            if (Controller.GetGamePlayById(gameId) != null)
            {
                return Controller.GetGamePlayById(gameId).CurrentRound;
            }

            return -1;
        }

        public bool PlaceTileAndFigure(Guid gameId, Guid playerId, ITile tileToPlace, Coordinates coordinates, bool placeFigure, int side)
        {
            var currentGamePlay = Controller.GetGamePlayById(gameId);
            if (currentGamePlay == null || currentGamePlay.CurrentPlayer.playerId != playerId
            || tileToPlace.PropertiesAsString != currentGamePlay.CurrentTile.PropertiesAsString || currentGamePlay.GameState != GameState.Started)
            {
                return false;
            }
            if (PlaceTile(tileToPlace, coordinates, currentGamePlay))
            {
                if (placeFigure)
                {
                    bool placedFigure = PlaceFigure(tileToPlace, side, currentGamePlay);
                    return placedFigure;
                }
                return true;

            }

            return false;
        }

        public bool StartGame(Guid gameId, Guid playerId)
        {
            var gamePlay = Controller.GetGamePlayById(gameId);
            if (gamePlay != null && gamePlay.GameState != GameState.Started
                && gamePlay.Players != null && gamePlay.Players.Count != 0
                && gamePlay.Players[0].playerId == playerId)
            {
                gamePlay.GameState = GameState.Started;
                return gamePlay.GameState == GameState.Started;
            }
            return false;
        }

        public ITile GetNewTile(Guid gameId)
        {
            var gamePlay = Controller.GetGamePlayById(gameId);

            if (gamePlay == null || gamePlay.TileStack.Count == 0 || gamePlay.HasCurrentTile || gamePlay.TileIsDown || gamePlay.GameState != GameState.Started)
            {
                return new Tile(new List<IDirection>(), new List<Speciality>());
            }

            gamePlay.TileIsDown = false;
            gamePlay.HasCurrentTile = true;
            gamePlay.AlreadyCalculated = false;
            gamePlay.CurrentTile = gamePlay.TileStack.RemoveAndGet(gamePlay.RandomNumberGenerator.Next(gamePlay.TileStack.Count));
            return gamePlay.CurrentTile;
        }

        public GamePlay EndTurn(Guid gameId, Guid playerId)
        {
            var gamePlay = Controller.GetGamePlayById(gameId);

            if (!(!gamePlay.HasCurrentTile && gamePlay.TileIsDown) || gamePlay.CurrentPlayer.playerId != playerId || gamePlay.GameState != GameState.Started)
            {
                return gamePlay;
            }

            if (gamePlay.TileStack.Count == 0)
            {
                CalculateGameOver(gamePlay);
                return gamePlay;
            }

            CallCalculate(gamePlay);
            gamePlay.FigureDown = false;
            gamePlay.TileIsDown = false;
            gamePlay.CanPlaceFigureProperty = false;
            gamePlay.CurrentRound++;
            gamePlay.CurrentPlayer = gamePlay.Players[(gamePlay.Players.IndexOf(gamePlay.CurrentPlayer) + 1) % gamePlay.Players.Count];

            return gamePlay;
        }

        private bool PlaceTile(ITile tileToPlace, Coordinates coordinates, GamePlay gamePlay)
        {
            bool canPlaceTile = !(!gamePlay.HasCurrentTile || gamePlay.TileIsDown);

            gamePlay.PlacedTiles.ForEach(t =>
            {
                if (!canPlaceTile || t.Position.Equals(coordinates))
                {
                    canPlaceTile = false;
                }
            });
            if (!canPlaceTile || gamePlay.GameState != GameState.Started)
            {
                return false;
            }
            tileToPlace.Position = coordinates;
            if (!Utils.CheckFitOfTile(tileToPlace, SurroundingTilesByCoordinates(tileToPlace, gamePlay)))
            {
                tileToPlace.Position = null;
                return false;
            }

            gamePlay.PlacedTiles.Add(tileToPlace);
            UpdateTiles(tileToPlace, gamePlay);
            gamePlay.CanPlaceFigureProperty = true;
            gamePlay.TileIsDown = true;
            gamePlay.HasCurrentTile = false;
            return true;
        }

        /// <summary>
        /// Places a figure on the tile
        /// </summary>
        /// <param name="currentTile">The tile, to put figure on</param>
        /// <param name="side">Which side of tile should be the figure placed</param>
        public bool PlaceFigure(ITile tileToPlace, int side, GamePlay gamePlay)
        {
            if (!CanPlaceFigure(tileToPlace, side, gamePlay))
            {
                return false;
            }

            if (gamePlay.FigureDown)
            {
                gamePlay.FigureDown = false;
                gamePlay.CurrentSideForFigure = -1;

                IFigure figureToGetDown = null;
                if (side == 4 && tileToPlace is Church)
                {
                    figureToGetDown = tileToPlace.CenterFigure;
                    tileToPlace.CenterFigure = null;
                }
                else
                {
                    figureToGetDown = tileToPlace.Directions[side].Figure;
                    tileToPlace.Directions[side].Figure = null;
                }

                if (figureToGetDown != null)
                {
                    Utils.GiveBackFigureToOwner(figureToGetDown, ref gamePlay.Players);
                }
            }
            else
            {
                var playerFigure = gamePlay.CurrentPlayer.Figures.RemoveAndGet(0);
                if (side == 4 && tileToPlace is Church)
                {
                    tileToPlace.CenterFigure = playerFigure;
                }
                else
                {
                    tileToPlace.Directions[side].Figure = playerFigure;
                }

                gamePlay.FigureDown = true;
                gamePlay.CurrentSideForFigure = side;
            }

            return true;
        }

        private bool CanPlaceFigure(ITile tile, int directionIndex, GamePlay gamePlay)
        {
            if (gamePlay.FigureDown)
            {
                return directionIndex == gamePlay.CurrentSideForFigure;
            }

            if (gamePlay.CurrentPlayer.Figures.Count == 0)
            {
                return false;
            }
            if (gamePlay.CanPlaceFigureProperty)
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
                    return tile.Directions[directionIndex].Figure == null && calculateService.CanPlaceFigure(tile, (CardinalDirection)directionIndex, true);
                }
                return tile.Directions[directionIndex].Figure == null;
            }
            return false;
        }

        private void CalculateGameOver(GamePlay gamePlay)
        {
            foreach (ITile tile in gamePlay.PlacedTiles)
            {
                if (tile is Church && tile.CenterFigure != null)
                {
                    var churchTile = (Church)tile;
                    List<IFigure> figures;
                    calculateService.Calculate(tile, true, out figures);
                    foreach (var figure in figures)
                    {
                        Utils.GiveBackFigureToOwner(figure, ref gamePlay.Players);
                    }
                }
                for (int i = 0; i < 4; i++)
                {
                    if (tile.Directions[i].Figure != null && !(tile.Directions[i].Landscape == Landscape.Field))
                    {
                        List<IFigure> figures;
                        calculateService.Calculate(tile, true, out figures);
                        foreach (var figure in figures)
                        {
                            Utils.GiveBackFigureToOwner(figure, ref gamePlay.Players);
                        }
                    }
                }
            }
        }

        private Dictionary<CardinalDirection, ITile> SurroundingTilesByCoordinates(ITile justPlacedTile, GamePlay gamePlay)
        {
            var result = new Dictionary<CardinalDirection, ITile>();
            foreach (var placedTile in gamePlay.PlacedTiles)
            {
                if (placedTile.Position.X == justPlacedTile.Position.X + 1 && placedTile.Position.Y == justPlacedTile.Position.Y)
                {
                    result.Add(CardinalDirection.East, placedTile);
                    break;
                }
                if (placedTile.Position.X == justPlacedTile.Position.X && placedTile.Position.Y == justPlacedTile.Position.Y + 1)
                {
                    result.Add(CardinalDirection.South, placedTile);
                    break;
                }
                if (placedTile.Position.X == justPlacedTile.Position.X - 1 && placedTile.Position.Y == justPlacedTile.Position.Y)
                {
                    result.Add(CardinalDirection.West, placedTile);
                    break;
                }
                if (placedTile.Position.X == justPlacedTile.Position.X && placedTile.Position.Y == justPlacedTile.Position.Y - 1)
                {
                    result.Add(CardinalDirection.North, placedTile);
                    break;
                }
            }
            return result;
        }

        private void UpdateTiles(ITile justPlacedTile, GamePlay gamePlay)
        {
            foreach (var placedTile in SurroundingTilesByCoordinates(justPlacedTile, gamePlay))
            {
                placedTile.Value.Directions[(int)Utils.GetOppositeDirection(placedTile.Key)].Neighbor = justPlacedTile;
                justPlacedTile.Directions[(int)placedTile.Key].Neighbor = placedTile.Value;
            }
        }

        private void CallCalculate(GamePlay gamePlay)
        {
            if (gamePlay.AlreadyCalculated || gamePlay.HasCurrentTile || !gamePlay.TileIsDown)
            {
                return;
            }

            List<IFigure> figures;
            calculateService.Calculate(gamePlay.PlacedTiles.Last(), false, out figures);
            foreach (var figure in figures)
            {
                Utils.GiveBackFigureToOwner(figure, ref gamePlay.Players);
            }
            gamePlay.AlreadyCalculated = true;
        }

        public Dictionary<int, IFigure> GetFiguresOnTiles(Guid gameId, ITile tile)
        {
            Dictionary<int, IFigure> result = new Dictionary<int, IFigure>();
            for (int i = 0; i < tile.Directions.Count; i++)
            {
                var direction = tile.Directions[i];
                if (direction.Figure != null)
                {
                    result.Add(i, direction.Figure);
                }
            }
            if (tile is Church)
            {
                var churchFigure = ((Church)tile).CenterFigure;
                if (churchFigure != null)
                {
                    result.Add(4, churchFigure);
                }
            }
            return result;
        }

    }
}
