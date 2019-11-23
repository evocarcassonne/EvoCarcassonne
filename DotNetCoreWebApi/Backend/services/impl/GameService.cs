using DotNetCoreWebApi.Backend.dao;
using System;

namespace DotNetCoreWebApi.Backend.services.impl
{
    public class GameService : IGameService
    {
        internal GameController Controller = GameController.Instance;
        private IPlayerService PlayerService = new PlayerService();

        public Guid CreateGameSession(string playerName)
        {
            var tileStack = new TileParser().TileStack;
            var newGamePlay = new GamePlay(tileStack);
            Controller.AddGamePlay(newGamePlay);
            PlayerService.Subscribe(newGamePlay.Id, playerName);
            return newGamePlay.Id;
        }

        public bool DeleteGameSession(Guid gameId, Guid playerId)
        {
            var gameplay = Controller.GetGamePlayById(gameId);
            if (gameplay != null && gameplay.Players[0].playerId == playerId)
            {
                return Controller.DeleteGamePlay(gameId);
            }
            return false;
        }
    }
}