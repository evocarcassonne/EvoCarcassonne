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

        public GameController(ILogger<GameController> logger)
        {
            GameService = new GameService();
            _logger = logger;
        }
        
        [HttpPost("Create")]
        public Guid PostSession()
        {
            return GameService.CreateGameSession();
        }

        [HttpDelete("{id}/Delete")]
        public void DeleteSession(string id)
        {
            GameService.DeleteGameSession(Guid.Parse(id));
        }

        [HttpGet("/")]
        public string Home()
        {
            return "Welcome you here!";
        }
    }
}