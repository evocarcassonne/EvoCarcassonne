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

        [HttpPost]
        [Route("{gameId}/Unsubscribe/{playerId}")]
        void Unsubscribe(Guid gameId, Guid playerId)
        {
            playerService.Unsubscribe(gameId, playerId);
        }
        
        [HttpGet]
        [Route("{gameId}/Players")]
        public List<Player> Players([FromUri] string gameId)
        {
            return playerService.GetPlayers(Guid.Parse(gameId));
        }
        
        [HttpGet]
        [Route("{gameId}/CurrentPlayer")]
        public Player GetCurrentPlayer([FromUri] Guid gameId)
        {
            return playerService.GetCurrentPlayer(gameId);
        }
    }
}
