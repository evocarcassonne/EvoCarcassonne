using DotNetCoreWebApi.Backend;
using DotNetCoreWebApi.Backend.Model;
using DotNetCoreWebApi.Backend.services;
using DotNetCoreWebApi.Backend.services.impl;
using DotNetCoreWebApi.Models;
using Microsoft.AspNetCore.Cors;
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
            IFigureService figureService = new FigureService();
            gamePlayService = new GamePlayService(calculateService, figureService);
        }


        [HttpPost]
        [Route("Start")]
        public bool StartGame([FromHeader] string gameId, [FromHeader] string playerId)
        {
            return gamePlayService.StartGame(Guid.Parse(gameId), Guid.Parse(playerId));
        }


        [HttpGet("CurrentPlayer")]
        public PlayerDto GetCurrentPlayer([FromHeader] string gameId)
        {
            var player = gamePlayService.GetCurrentPlayer(Guid.Parse(gameId));
            return new PlayerDto(player.playerId, player.Figures.Count, player.Owner.Name, player.Owner.Points);
        }


        [HttpGet("CurrentRound")]
        public int GetCurrentRound([FromHeader] string gameId)
        {
            return gamePlayService.GetCurrentRound(Guid.Parse(gameId));
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
            return gamePlayService.PlaceTileAndFigure(Guid.Parse(tileDto.gameId), Guid.Parse(tileDto.playerId), tile,
                new Coordinates(tileDto.coordinateX, tileDto.coordinateY), tileDto.placeFigure, tileDto.side);
        }

        [HttpGet("GetNewTile")]
        public string GetNewTile([FromHeader] string gameId)

        {
            return gamePlayService.GetNewTile(Guid.Parse(gameId)).PropertiesAsString;
        }

        [HttpPost("EndTurn")]
        public GameInfoDto EndTurn([FromHeader] string gameId, [FromHeader] string playerId)
        {
            var gamePlay = gamePlayService.EndTurn(Guid.Parse(gameId), Guid.Parse(playerId));
            var gameInfoDto = new GameInfoDto(gamePlay.CurrentRound, GetCurrentPlayer(gameId));
            foreach (var tile in gamePlay.PlacedTiles)
            {
                FigurePlacementDto figureOnTile = null;
                foreach (var i in gamePlayService.GetFiguresOnTiles(Guid.Parse(gameId), tile))
                {
                    figureOnTile = new FigurePlacementDto(i.Value.Owner.Name, i.Key);
                    break;
                }
                var tileInfo = new TileInfoDto(tile.PropertiesAsString, tile.Position, figureOnTile, tile.Rotation);

                gameInfoDto.AddTileInfoOneByOne(tileInfo);
            }
            gamePlay.Players.ForEach(player => gameInfoDto.AddPlayerOneByOne(new PlayerDto(player.playerId, player.Figures.Count, player.Owner.Name, player.Owner.Points)));
            return gameInfoDto;
        }


        [HttpGet("State")]
        public GameInfoDto GetState([FromHeader] string gameId)
        {
            var gamePlay = gamePlayService.GetState(Guid.Parse(gameId));
            var gameInfoDto = new GameInfoDto(gamePlay.CurrentRound, GetCurrentPlayer(gameId));
            foreach (var tile in gamePlay.PlacedTiles)
            {
                FigurePlacementDto figureOnTile = null;
                foreach (var i in gamePlayService.GetFiguresOnTiles(Guid.Parse(gameId), tile))
                {
                    figureOnTile = new FigurePlacementDto(i.Value.Owner.Name, i.Key);
                    break;
                }
                var tileInfo = new TileInfoDto(tile.PropertiesAsString, tile.Position, figureOnTile, tile.Rotation);

                gameInfoDto.AddTileInfoOneByOne(tileInfo);
            }
            gamePlay.Players.ForEach(player => gameInfoDto.AddPlayerOneByOne(new PlayerDto(player.playerId, player.Figures.Count, player.Owner.Name, player.Owner.Points)));
            return gameInfoDto;
        }
    }
}