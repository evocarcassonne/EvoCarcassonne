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

        [HttpPost("{gameID}/Subscribe/{playerName}")]
        public Guid Subscribe(string gameID, string playerName)
        {
            return playerService.Subscribe(Guid.Parse(gameID), playerName);
        }

        [HttpDelete("{gameId}/Unsubscribe/{playerId}")]
        public bool Unsubscribe(string gameId, string playerId)
        {
            return playerService.Unsubscribe(Guid.Parse(gameId), Guid.Parse(playerId));
        }

        [HttpGet("{gameId}/Players")]
        public List<PlayerDto> Players(string gameId)
        {
            var players = playerService.GetPlayers(Guid.Parse(gameId));
            var results = new List<PlayerDto>();
            players.ForEach(player => results.Add(new PlayerDto(player.playerId, player.Figures.Count, player.Owner.Name, player.Owner.Points)));
            return results;
        }
    }
}
