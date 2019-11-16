using System;
using System.Collections.Generic;
using System.Web.Http;
using Backend.Model;
using Backend.services;
using Backend.Services.impl;

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
        public List<Player> Players([FromUri] string gameId)
        {
            return playerService.GetPlayers(Guid.Parse(gameId));
        }
    }
}
