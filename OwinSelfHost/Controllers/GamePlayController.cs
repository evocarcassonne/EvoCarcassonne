using System;
using System.Web.Http;
using Backend;
using Backend.dao;
using Backend.Model;
using Backend.services;
using Backend.services.impl;
using OwinSelfHost.Models;

namespace OwinSelfHost.Controllers
{
    [RoutePrefix("api/GamePlay")]
    public class GamePlayController : ApiController
    {
        private readonly IGamePlayService gamePlayService;

        public GamePlayController()
        {
            gamePlayService = new GamePlayService();
        }

/*        [HttpPost]
        [Route("{gameId}/Start")]
        public void StartGame([FromUri] Guid gameId)
        {
            gamePlayService.StartGame(gameId);
        }*/
        
        
        [HttpGet]
        [Route("{gameId}/CurrentPlayer")]
        public PlayerDto GetCurrentPlayer(Guid gameId)
        {
            var player =  gamePlayService.GetCurrentPlayer(gameId);
            return new PlayerDto(player.playerId, player.Figures.Count, player.Owner.Name, player.Owner.Points);
        }

        [HttpGet]
        [Route("{gameId}/CurrentRound")]
        public int GetCurrentRound(Guid gameId)
        {
            return gamePlayService.GetCurrentRound(gameId);
        }

        [HttpPut]
        [Route("PlaceTile")]
        public bool PlaceTileAndFigure([FromBody] PlaceTileDto tileDto)
        {
            return gamePlayService.PlaceTileAndFigure(tileDto.gameId, TileParser.Parse(tileDto.tileProps), 
                new Coordinates(tileDto.coordinateX, tileDto.coordinateY), tileDto.side);
        }

        [HttpGet]
        [Route("{gameId}/GetNewTile")]
        public string GetNewTile(Guid gameId)
        {
            return gamePlayService.GetNewTile(gameId).PropertiesAsString;
        }

        [HttpPost]
        [Route("{gameId}/EndTurn")]
        public GameInfoDto EndTurn(Guid gameId)
        {
            var gamePlay =  gamePlayService.EndTurn(gameId);
            var gameInfoDto = new GameInfoDto(gamePlay.CurrentRound, GetCurrentPlayer(gameId));
            gamePlay.PlacedTiles.ForEach(tile => gameInfoDto.AddTileInfoOneByOne(new TileInfoDto(tile.PropertiesAsString, tile.Position)));
            gamePlay.Players.ForEach(player => gameInfoDto.AddPlayerOneByOne(new PlayerDto(player.playerId, player.Figures.Count, player.Owner.Name,player.Owner.Points)));
            return gameInfoDto;
        }
        
    }
}