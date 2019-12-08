using DotNetCoreWebApi.Backend.services;
using DotNetCoreWebApi.Backend.services.impl;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Logging;

namespace DotNetCoreWebApi.Controllers
{
    [Route("api/Game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private IGameService GameService;
        private readonly ILogger<GameController> _logger;

        public GameController(ILogger<GameController> logger, IGameService gameService)
        {
            GameService = gameService;
            _logger = logger;
        }

        [HttpPost("Create")]
        public Guid CreateSession([FromHeader] string playerName)
        {
            return GameService.CreateGameSession(playerName);
        }

        [HttpDelete("Delete")]
        public bool DeleteSession([FromHeader] string gameId, [FromHeader] Guid playerId)
        {
            return GameService.DeleteGameSession(Guid.Parse(gameId), playerId);
        }

        [HttpGet("/")]
        public string Home()
        {
            return "Welcome you here!";
        }
    }
}