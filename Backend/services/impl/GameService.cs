using System;
using Backend.dao;

namespace Backend.services.impl
{
    public class GameService : IGameService
    {
        internal GameController Controller = GameController.Instance;

        public Guid CreateGameSession()
        {
            var newGamePlay = new GamePlay();
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