using System;
using System.Collections.Generic;

namespace DotNetCoreWebApi.Backend.dao
{
    public class GameController
    {
        private List<GamePlay> GamePlays = new List<GamePlay>();
        private static GameController _instance;

        public static GameController Instance
        {
            get => _instance ?? (_instance = new GameController());
            set => _instance = value;
        }

        internal void AddGamePlay(GamePlay newGamePlay)
        {
            if (newGamePlay != null)
            {
                GamePlays.Add(newGamePlay);
            }
        }

        internal bool DeleteGamePlay(Guid gameId)
        {
            foreach (var i in GamePlays)
            {
                if (i.Id == gameId)
                {
                    GamePlays.Remove(GetGamePlayById(gameId));
                    return true;
                }
            }
            return false;
        }

        public GamePlay GetGamePlayById(Guid gameplayID)
        {
            foreach (GamePlay gamePlay in GamePlays)
            {
                if (gamePlay.Id == gameplayID)
                {
                    return gamePlay;
                }
            }
            return null;
        }
    }
}