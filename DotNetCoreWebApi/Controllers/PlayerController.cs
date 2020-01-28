using System;
using System.Collections.Generic;
using DotNetCoreWebApi.Backend.services;
using DotNetCoreWebApi.Backend.services.impl;
using Microsoft.AspNetCore.Mvc;
using DotNetCoreWebApi.Models;
using Microsoft.AspNetCore.Cors;

namespace DotNetCoreWebApi.Controllers
{

    [ApiController]
    [Route("api/Player")]
    public class PlayerController : ControllerBase
    {

        private readonly IPlayerService playerService;

        public PlayerController()
        {
            playerService = new PlayerService();
        }

        [EnableCors]
        [HttpPost("Subscribe")]
        public Guid Subscribe([FromHeader] string gameId, [FromHeader] string playerName)
        {
            Guid GameId = Guid.Empty;
            if (Guid.TryParse(gameId, out GameId))
            {
                return playerService.Subscribe(GameId, playerName);
            }
            return Guid.Empty;
        }

        [EnableCors]
        [HttpDelete("Unsubscribe")]
        public bool Unsubscribe([FromHeader] string gameId, [FromHeader] string playerId)
        {
            Guid GameId = Guid.Empty;
            Guid PlayerId = Guid.Empty;
            if (Guid.TryParse(gameId, out GameId) && Guid.TryParse(playerId, out PlayerId))
            {
                return playerService.Unsubscribe(Guid.Parse(gameId), Guid.Parse(playerId));
            }
            return false;
        }

        [EnableCors]
        [HttpGet("Players")]
        public List<PlayerDto> Players([FromHeader] string gameId)
        {
            Guid GameId = Guid.Empty;
            Guid PlayerId = Guid.Empty;
            if (Guid.TryParse(gameId, out GameId))
            {
                var players = playerService.GetPlayers(Guid.Parse(gameId));
                var results = new List<PlayerDto>();
                players.ForEach(player => results.Add(new PlayerDto(player.playerId, player.Figures.Count, player.Owner.Name, player.Owner.Points)));
                return results;
            }
            return new List<PlayerDto> { new PlayerDto() };
        }
    }
}
