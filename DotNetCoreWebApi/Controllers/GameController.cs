using DotNetCoreWebApi.Backend.services;
using DotNetCoreWebApi.Backend.services.impl;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;

namespace DotNetCoreWebApi.Controllers
{
    [Route("api/Game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private IGameService GameService;
        private readonly ILogger<GameController> _logger;

        public GameController(ILogger<GameController> logger)
        {
            IPlayerService playerService = new PlayerService();
            GameService = new GameService(playerService);
            _logger = logger;
        }

        [EnableCors]
        [HttpPost("Create")]
        public Guid CreateSession([FromHeader] string playerName)
        {
            return GameService.CreateGameSession(playerName);
        }

        [EnableCors]
        [HttpDelete("Delete")]
        public bool DeleteSession([FromHeader] string gameId, [FromHeader] Guid playerId)
        {
            return GameService.DeleteGameSession(Guid.Parse(gameId), playerId);
        }

        [EnableCors]
        [HttpGet("/")]
        public string Home()
        {
            return "Welcome you here!";
        }
    }
}