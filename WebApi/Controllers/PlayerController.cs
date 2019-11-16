using System;
using System.Collections.Generic;
using System.Web.Http;
using Backend.Model;
using Backend.services;
using Backend.Services.impl;
using WebApi.Models;

namespace WebApi.Controllers
{
    [RoutePrefix("api/Player")]
    public class PlayerController : ApiController
    {

        private readonly IPlayerService playerService;

        public PlayerController()
        {
            playerService = new PlayerService();
        }

        [HttpPost]
        [Route("{gameID}/Subscribe/{playerName}")]
        public Guid Subscribe([FromUri] string gameID, [FromUri] string playerName)
        {
            return playerService.Subscribe(Guid.Parse(gameID), playerName);
        }

        [HttpDelete]
        [Route("{gameId}/Unsubscribe/{playerId}")]
        public bool Unsubscribe([FromUri] string gameId, [FromUri] string playerId)
        {
            return playerService.Unsubscribe(Guid.Parse(gameId), Guid.Parse(playerId));
        }
        
        [HttpGet]
        [Route("{gameId}/Players")]
        public List<PlayerDto> Players([FromUri] string gameId)
        {
            var players =  playerService.GetPlayers(Guid.Parse(gameId));
            var results = new List<PlayerDto>();
            players.ForEach(player => results.Add(new PlayerDto(player.playerId, player.Figures.Count, player.Owner.Name, player.Owner.Points)));
            return results;
        }
    }
}
