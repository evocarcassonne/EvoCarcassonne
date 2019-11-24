using System;
using System.Collections.Generic;
using System.Linq;
using DotNetCoreWebApi.Backend.dao;
using DotNetCoreWebApi.Backend.Model;

namespace DotNetCoreWebApi.Backend.services.impl
{
    public class PlayerService : IPlayerService
    {
        internal GameController Controller = GameController.Instance;

        public Guid Subscribe(Guid gameId, string playerName)
        {
            var gamePlay = Controller.GetGamePlayById(gameId);
            var exists = gamePlay?.Players.Any(e => e.Owner.Name == playerName);
            if (gamePlay == null || (bool)exists || gamePlay.GameState != GameState.WaitingForPlayers)
            {
                return Guid.Empty;
            }

            Player player = new Player(new Owner(playerName));
            player.playerId = Guid.NewGuid();
            gamePlay.Players.Add(player);
            gamePlay.CurrentPlayer = gamePlay.Players[0];
            return player.playerId;
        }

        public bool Unsubscribe(Guid gameId, Guid playerId)
        {
            var players = GetPlayers(gameId);
            var gamePlay = Controller.GetGamePlayById(gameId);
            Player playerToBeRemoved = null;
            foreach (var player in players.Where(player => player.playerId.Equals(playerId)))
            {
                playerToBeRemoved = player;
            }

            if (gamePlay != null && gamePlay.GameState != GameState.Started && playerToBeRemoved != null)
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