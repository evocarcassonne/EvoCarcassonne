using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Backend.Model;
using Backend.services;
using Backend.services.impl;
using Backend.Services;
using Backend.Services.impl;

namespace WebApi.Controllers
{
    [RoutePrefix("api/Game")]
    public class GameController : ApiController
    {
        private IGameService GameService;

        public GameController()
        {
            GameService = new GameService();
        }
        
        [HttpPost]
        [Route("Create")]
        public Guid CreateSession()
        {
            return GameService.CreateGameSession();
        }

        [HttpDelete]
        [Route("{id}/Delete")]
        public void DeleteSession([FromUri] string id)
        {
            GameService.DeleteGameSession(Guid.Parse(id));
        } 
    }
}