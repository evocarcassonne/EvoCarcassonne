using System;
using System.Collections.Generic;
using Backend.dao;
using Backend.Model;

namespace Backend.Services
{
    public class PlayerService
    {
        internal GameController Controller = GameController.Instance;
        
        public void AddPlayer(Guid gameID, Player player)
        {
            if (player != null)
            {
                Controller.GetGamePlayById(gameID)?.Players.Add(player);    
            }
        }

        public void RemovePlayer(Guid gameId, Player player)
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

        public bool PlaceFigureOnTile(Guid gameId, int side)
        {
            if (Controller.GetGamePlayById(gameId) != null)
            {
                return Controller.GetGamePlayById(gameId).PlaceFigure(side);
            }

            return false;
        }
        
    }
}