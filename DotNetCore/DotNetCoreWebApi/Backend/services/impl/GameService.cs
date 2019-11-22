using DotNetCoreWebApi.Backend.dao;
using System;

namespace DotNetCoreWebApi.Backend.services.impl
{
    public class GameService : IGameService
    {
        internal GameController Controller = GameController.Instance;

        public Guid CreateGameSession()
        {
            var tileStack = new TileParser().TileStack;
            var newGamePlay = new GamePlay(tileStack);
            Controller.GamePlays.Add(newGamePlay);
            return newGamePlay.Id;
        }

        public void DeleteGameSession(Guid gameId)
        {
            foreach (var play in Controller.GamePlays)
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