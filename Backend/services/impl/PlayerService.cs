using System;
using System.Collections.Generic;
using System.Linq;
using Backend.dao;
using Backend.Model;
using Backend.services;

namespace Backend.Services.impl
{
    public class PlayerService : IPlayerService
    {
        internal GameController Controller = GameController.Instance;
        
        public Guid Subscribe(Guid gameID, Player player)
        {
            if (player != null)
            {
                player.playerId = Guid.NewGuid();
                Controller.GetGamePlayById(gameID)?.Players.Add(player);    
            }
            return player.playerId;
        }

        public void Unsubscribe(Guid gameId, Guid playerId)
        {
            var players = GetPlayers(gameId);
            Player playerToBeRemoved = null;
            foreach (var player in players.Where(player => player.playerId.Equals(playerId)))
            {
                playerToBeRemoved = player;
            }

            if (playerToBeRemoved != null)
            {
                players.Remove(playerToBeRemoved);
            }
        }

        public List<Player> GetPlayers(Guid gameId)
        {
            return Controller.GetGamePlayById(gameId)?.Players;
        }

        public Player GetCurrentPlayer(Guid gameId)
        {
            return Controller.GetGamePlayById(gameId)?.CurrentPlayer;
        }

        public Guid CreateGameSession()
        {
            var newGamePlay = new GamePlay();
            Controller.GamePlays.Add(newGamePlay);
            return newGamePlay.Id;
        }

        public void DeleteGameSession(Guid gameId)
        {
            foreach (GamePlay play in Controller.GamePlays)
            {
                if (play.Id == gameId)
                {
                    Controller.GamePlays.Remove(play);
                    break;
                }
            }
            
        }
    }
}