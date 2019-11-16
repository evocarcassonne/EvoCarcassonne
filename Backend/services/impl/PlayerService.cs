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
        
        public Guid Subscribe(Guid gameId, string playerName)
        {
            var gamePlay = Controller.GetGamePlayById(gameId);
            var exists = gamePlay?.Players.Any(e => e.Owner.Name == playerName);
            if ( gamePlay == null || (bool) exists)
            {
                return Guid.Empty;
            }

            Player player = new Player(new Owner(playerName));
            player.playerId = Guid.NewGuid();
            if (gamePlay.Players.Count == 0)
            {
                gamePlay.Owner = player.playerId;
            }
            gamePlay.Players.Add(player);
            gamePlay.CurrentPlayer = gamePlay.Players[0];
            return player.playerId;
        }

        public bool Unsubscribe(Guid gameId, Guid playerId)
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
                return true;
            }

            return false;
        }

        public List<Player> GetPlayers(Guid gameId)
        {
            return Controller.GetGamePlayById(gameId)?.Players;
        }
    }
}