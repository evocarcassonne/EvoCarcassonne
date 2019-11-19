using System;
using System.Collections.Generic;

namespace DotNetCoreWebApi.Backend.dao
{
    public class GameController
    {
        public List<GamePlay> GamePlays = new List<GamePlay>();
        private static GameController _instance;

        public static GameController Instance
        {
            get => _instance ?? (_instance = new GameController());
            set => _instance = value;
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