using System.Collections.Generic;

namespace DotNetCoreWebApi.Models
{
    public class GameInfoDto
    {
        public List<PlayerDto> PlayerInfo { get; set; } = new List<PlayerDto>();
        public int CurrentRound { get; set; }
        public PlayerDto CurrentPlayer { get; set; }
        public List<TileInfoDto> TableInfo { get; set; } = new List<TileInfoDto>();

        public GameInfoDto(List<PlayerDto> playerInfo, int currentRound, PlayerDto currentPlayer, List<TileInfoDto> tableInfo)
        {
            PlayerInfo = playerInfo;
            CurrentRound = currentRound;
            CurrentPlayer = currentPlayer;
            TableInfo = tableInfo;
        }
        
        public GameInfoDto(int currentRound, PlayerDto currentPlayer)
        {
            CurrentRound = currentRound;
            CurrentPlayer = currentPlayer;
        }

        public GameInfoDto()
        {
        }

        public void AddPlayerOneByOne(PlayerDto playerDto)
        {
            if (playerDto != null)
            {
                PlayerInfo.Add(playerDto);    
            }
        }
        public void AddTileInfoOneByOne(TileInfoDto tileInfoDto)
        {
            if (tileInfoDto != null)
            {
                TableInfo.Add(tileInfoDto);
            }
        }
        
    }
}