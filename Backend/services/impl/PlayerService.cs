using System;
using System.Collections.Generic;
using Backend.dao;
using Backend.Model;
using Backend.services;

namespace Backend.Services.impl
{
    public class PlayerService : IPlayerService
    {
        internal GameController Controller = GameController.Instance;
        
        public void Subscribe(Guid gameID, Player player)
        {
            if (player != null)
            {
                Controller.GetGamePlayById(gameID)?.Players.Add(player);    
            }
        }

        public void Unsubscribe(Guid gameId, Player player)
        {
            Controller.GetGamePlayById(gameId)?.Players.Remove(player);
        }

        public List<Player> GetPlayers(Guid gameId)
        {
            return Controller.GetGamePlayById(gameId)?.Players;
        }

        public Player GetCurrentPlayer(Guid gameId)
        {
            return Controller.GetGamePlayById(gameId)?.CurrentPlayer;
        }
    }
}