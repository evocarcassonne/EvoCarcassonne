using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.dao;
using Backend.Model;

namespace Backend.services.impl
{
    public class GamePlayService : IGamePlayService
    {
        internal GameController Controller = GameController.Instance;

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

        public bool PlaceTileAndFigure(Guid gameId, ITile tileToPlace, Coordinates coordinates, int side)
        {
            var currentGamePlay = Controller.GetGamePlayById(gameId);
            if (currentGamePlay == null) return false;
            if (currentGamePlay.PlaceTile(tileToPlace, coordinates))
            {
                return currentGamePlay.PlaceFigure(tileToPlace, side);
            }
            
            return false;
        }

        public ITile GetNewTile(Guid gameId)
        {
            return Controller.GetGamePlayById(gameId)?.GetNewTile();
        }

        public GamePlay EndTurn(Guid gameId)
        {
            var gamePlay = Controller.GetGamePlayById(gameId);
            gamePlay?.EndTurn();
            return gamePlay;
        }
    }
}
