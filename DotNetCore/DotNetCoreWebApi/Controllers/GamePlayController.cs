using System;
using System.Net.Mime;
using DotNetCoreWebApi.Backend;
using DotNetCoreWebApi.Backend.dao;
using DotNetCoreWebApi.Backend.Model;
using DotNetCoreWebApi.Backend.services;
using DotNetCoreWebApi.Backend.services.impl;
using Microsoft.AspNetCore.Mvc;
using DotNetCoreWebApi.Models;

namespace DotNetCoreWebApi.Controllers
{
    
    [ApiController]
    [Route("api/GamePlay")]
    public class GamePlayController : ControllerBase
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
        
        
        [HttpGet("{gameId}/CurrentPlayer")]
        public PlayerDto GetCurrentPlayer(Guid gameId)
        {
            var player =  gamePlayService.GetCurrentPlayer(gameId);
            return new PlayerDto(player.playerId, player.Figures.Count, player.Owner.Name, player.Owner.Points);
        }

        [HttpGet("{gameId}/CurrentRound")]
        public int GetCurrentRound(Guid gameId)
        {
            return gamePlayService.GetCurrentRound(gameId);
        }

        [HttpPut("PlaceTile")]
        public bool PlaceTileAndFigure([FromBody] PlaceTileDto tileDto)
        {
            return gamePlayService.PlaceTileAndFigure(tileDto.gameId, TileParser.Parse(tileDto.tileProps), 
                new Coordinates(tileDto.coordinateX, tileDto.coordinateY), tileDto.side);
        }

        [HttpGet("{gameId}/GetNewTile")]
        public string GetNewTile(Guid gameId)
        {
            return gamePlayService.GetNewTile(gameId).PropertiesAsString;
        }

        [HttpPost("{gameId}/EndTurn")]
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