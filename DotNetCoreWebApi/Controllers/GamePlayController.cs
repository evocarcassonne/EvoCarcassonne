using DotNetCoreWebApi.Backend;
using DotNetCoreWebApi.Backend.Model;
using DotNetCoreWebApi.Backend.services;
using DotNetCoreWebApi.Backend.services.impl;
using DotNetCoreWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace DotNetCoreWebApi.Controllers
{

    [ApiController]
    [Route("api/GamePlay")]
    public class GamePlayController : ControllerBase
    {
        private readonly IGamePlayService gamePlayService;

        public GamePlayController()
        {
            ICalculateService calculateService = new CalculatorService();
            gamePlayService = new GamePlayService(calculateService);
        }

        [HttpPost]
        [Route("Start")]
        public bool StartGame([FromHeader] Guid gameId, [FromHeader] Guid playerId)
        {
            return gamePlayService.StartGame(gameId, playerId);
        }


        [HttpGet("CurrentPlayer")]
        public PlayerDto GetCurrentPlayer([FromHeader] Guid gameId)
        {
            var player = gamePlayService.GetCurrentPlayer(gameId);
            return new PlayerDto(player.playerId, player.Figures.Count, player.Owner.Name, player.Owner.Points);
        }

        [HttpGet("CurrentRound")]
        public int GetCurrentRound([FromHeader] Guid gameId)
        {
            return gamePlayService.GetCurrentRound(gameId);
        }

        [HttpPost("PlaceTile")]
        public bool PlaceTileAndFigure([FromBody] PlaceTileDto tileDto)
        {
            var tile = TileParser.Parse(tileDto.tileProps);
            if (Math.Abs(tileDto.RotateAngle) % 90 != 0) { return false; }
            for (int i = 0; i < Math.Abs(tileDto.RotateAngle) / 90; i++)
            {
                if (tileDto.RotateAngle < 0)
                {
                    tile.Rotate(-90);
                }
                else if (tileDto.RotateAngle > 0)
                {
                    tile.Rotate(90);
                }
            }
            return gamePlayService.PlaceTileAndFigure(tileDto.gameId, tileDto.playerId, tile,
                new Coordinates(tileDto.coordinateX, tileDto.coordinateY), tileDto.placeFigure, tileDto.side);
        }

        [HttpGet("GetNewTile")]
        public string GetNewTile([FromHeader] Guid gameId)
        {
            return gamePlayService.GetNewTile(gameId).PropertiesAsString;
        }

        [HttpPost("EndTurn")]
        public GameInfoDto EndTurn([FromHeader] Guid gameId, [FromHeader] Guid playerId)
        {
            var gamePlay = gamePlayService.EndTurn(gameId, playerId);
            var gameInfoDto = new GameInfoDto(gamePlay.CurrentRound, GetCurrentPlayer(gameId));
            foreach (var tile in gamePlay.PlacedTiles)
            {
                List<FigurePlacementDto> figuresOnTiles = new List<FigurePlacementDto>();
                foreach (var i in gamePlayService.GetFiguresOnTiles(gameId, tile))
                {
                    figuresOnTiles.Add(new FigurePlacementDto(i.Value.Owner.Name, i.Key));
                }
                var tileInfo = new TileInfoDto(tile.PropertiesAsString, tile.Position, figuresOnTiles);
                gameInfoDto.AddTileInfoOneByOne(tileInfo);
            }
            gamePlay.Players.ForEach(player => gameInfoDto.AddPlayerOneByOne(new PlayerDto(player.playerId, player.Figures.Count, player.Owner.Name, player.Owner.Points)));
            return gameInfoDto;
        }
    }
}