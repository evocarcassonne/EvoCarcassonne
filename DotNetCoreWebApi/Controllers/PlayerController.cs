using System;
using System.Collections.Generic;
using System.Net.Mime;
using DotNetCoreWebApi.Backend.Model;
using DotNetCoreWebApi.Backend.services;
using DotNetCoreWebApi.Backend.services.impl;
using Microsoft.AspNetCore.Mvc;
using DotNetCoreWebApi.Models;

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

        [HttpPost("Subscribe")]
        public Guid Subscribe([FromHeader] string gameId, [FromHeader] string playerName)
        {
            return playerService.Subscribe(Guid.Parse(gameId), playerName);
        }

        [HttpDelete("Unsubscribe")]
        public bool Unsubscribe([FromHeader] string gameId, [FromHeader] string playerId)
        {
            return playerService.Unsubscribe(Guid.Parse(gameId), Guid.Parse(playerId));
        }

        [HttpGet("Players")]
        public List<PlayerDto> Players([FromHeader] string gameId)
        {
            var players = playerService.GetPlayers(Guid.Parse(gameId));
            var results = new List<PlayerDto>();
            players.ForEach(player => results.Add(new PlayerDto(player.playerId, player.Figures.Count, player.Owner.Name, player.Owner.Points)));
            return results;
        }
    }
}
