using System;
using System.Web.Http;
using Backend;
using Backend.dao;
using Backend.Model;
using Backend.services;
using Backend.services.impl;
using WebApi.Models;

namespace WebApi.Controllers
{
    [RoutePrefix("api/GamePlay")]
    public class GamePlayController : ApiController
    {
        private readonly IGamePlayService gamePlayService;

        public GamePlayController()
        {
            gamePlayService = new GamePlayService();
        }

        [HttpPost]
        [Route("{gameId}/Start")]
        public void StartGame([FromUri] Guid gameId)
        {
            gamePlayService.StartGame(gameId);
        }
        
        
        [HttpGet]
        [Route("{gameId}/CurrentPlayer")]
        public Player GetCurrentPlayer(Guid gameId)
        {
            return gamePlayService.GetCurrentPlayer(gameId);
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
            return gamePlayService.PlaceTileAndFigure(tileDto.gameId, TileParser.Parse(tileDto.tileProps), new Coordinates(tileDto.coordinateX, tileDto.coordinateY), tileDto.side);
        }

        [HttpGet]
        [Route("{gameId}/GetNewTile")]
        public string GetNewTile(Guid gameId)
        {
            return gamePlayService.GetNewTile(gameId).PropertiesAsString;
        }

        [HttpPost]
        [Route("{gameId}/EndTurn")]
        public GamePlay EndTurn(Guid gameId)
        {
            return gamePlayService.EndTurn(gameId);
        }
        
    }
}