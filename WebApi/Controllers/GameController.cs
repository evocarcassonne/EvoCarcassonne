using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Backend.Model;
using Backend.services;
using Backend.Services;
using Backend.Services.impl;

namespace WebApi.Controllers
{
    [RoutePrefix("api/Game")]
    public class GameController : ApiController
    {
        private IPlayerService playerService;

        public GameController()
        {
            playerService = new PlayerService();
        }
        
        [HttpPost]
        [Route("Create")]
        public Guid CreateSession()
        {
            return playerService.CreateGameSession();
        }

        [HttpDelete]
        [Route("{id}/Delete")]
        public void DeleteSession([FromUri] string id)
        {
            playerService.DeleteGameSession(Guid.Parse(id));
        } 
    }
}