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
            var churchCalculatorService = new ChurchCalculatorService();
            var castleCalculatorService = new CastleCalculatorService();
            var roadCalculatorService = new RoadCalculatorService();
            IFigureService figureService = new FigureService();
            ICalculateService calculateService = new CalculatorService(churchCalculatorService,
                                                                       castleCalculatorService,
                                                                       roadCalculatorService,
                                                                       figureService);
            gamePlayService = new GamePlayService(calculateService, figureService);
        }

        [EnableCors]
        [HttpPost]
        [Route("Start")]
        public bool StartGame([FromHeader] string gameId, [FromHeader] string playerId)
        {
            Guid startedGame = Guid.Empty;
            if (Guid.TryParse(gameId, out startedGame))
                return gamePlayService.StartGame(startedGame, Guid.Parse(playerId));

            return false;
        }

        [EnableCors]
        [HttpGet("CurrentPlayer")]
        public PlayerDto GetCurrentPlayer([FromHeader] string gameId)
        {
            Guid GameId = Guid.Empty;
            if (Guid.TryParse(gameId, out GameId))
            {
                var player = gamePlayService.GetCurrentPlayer(Guid.Parse(gameId));
                return new PlayerDto(player.playerId, player.Figures.Count, player.Owner.Name, player.Owner.Points);
            }
            return new PlayerDto();
        }

        [EnableCors]
        [HttpGet("CurrentRound")]
        public int GetCurrentRound([FromHeader] string gameId)
        {
            Guid GameId = Guid.Empty;
            if (Guid.TryParse(gameId, out GameId))
            {
                return gamePlayService.GetCurrentRound(Guid.Parse(gameId));
            }
            return -1;
        }

        [EnableCors]
        [HttpPost("PlaceTile")]
        public bool PlaceTile([FromBody] PlaceTileDto tileDto)
        {
            Guid GameId = Guid.Empty;
            Guid PlayerId = Guid.Empty;
            if (Guid.TryParse(tileDto.gameId, out GameId) && Guid.TryParse(tileDto.playerId, out PlayerId))
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
                return gamePlayService.PlaceTile(GameId, PlayerId, tile,
                    new Coordinates(tileDto.coordinateX, tileDto.coordinateY));
            }
            return false;
        }

        [EnableCors]
        [HttpPost("PlaceFigure")]
        public bool PlaceFigure([FromBody] PlaceFigureDto figureDto)
        {
            Guid GameId = Guid.Empty;
            Guid PlayerId = Guid.Empty;
            if (Guid.TryParse(figureDto.gameId, out GameId) && Guid.TryParse(figureDto.playerId, out PlayerId))
            {
                return gamePlayService.PlaceFigure(GameId, PlayerId, figureDto.side);
            }
            return false;
        }

        [EnableCors]
        [HttpGet("GetNewTile")]
        public string GetNewTile([FromHeader] string gameId, [FromHeader] string playerId)
        {
            Guid GameId = Guid.Empty;
            Guid PlayerId = Guid.Empty;
            if (Guid.TryParse(gameId, out GameId) && Guid.TryParse(playerId, out PlayerId))
            {
                return gamePlayService.GetNewTile(GameId, PlayerId).PropertiesAsString;
            }
            return "";
        }

        [EnableCors]
        [HttpPost("EndTurn")]
        public GameInfoDto EndTurn([FromHeader] string gameId, [FromHeader] string playerId)
        {
            Guid GameId = Guid.Empty;
            Guid PlayerId = Guid.Empty;
            if (Guid.TryParse(gameId, out GameId) && Guid.TryParse(playerId, out PlayerId))
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
                gameInfoDto.GameState = gamePlay.GameState.ToString();
                gamePlay.Players.ForEach(player => gameInfoDto.AddPlayerOneByOne(new PlayerDto(player.playerId, player.Figures.Count, player.Owner.Name, player.Owner.Points)));
                return gameInfoDto;
            }
            return new GameInfoDto();
        }

        [EnableCors]
        [HttpGet("State")]
        public GameInfoDto GetState([FromHeader] string gameId)
        {
            Guid GameId = Guid.Empty;
            if (Guid.TryParse(gameId, out GameId))
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
                gameInfoDto.GameState = gamePlay.GameState.ToString();
                gamePlay.Players.ForEach(player => gameInfoDto.AddPlayerOneByOne(new PlayerDto(player.playerId, player.Figures.Count, player.Owner.Name, player.Owner.Points)));
                return gameInfoDto;
            }
            return new GameInfoDto();
        }

    }
}