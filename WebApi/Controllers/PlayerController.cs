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
    public class PlayerController : ApiController
    {

        private IPlayerService playerService;
        private List<Player> players; 

        public PlayerController()
        {
            playerService = new PlayerService();
            players = new List<Player>();
            players.Add(new Player(new Owner("Alma")));
            players.Add(new Player(new Owner("Korte")));
            players.Add(new Player(new Owner("Szilva")));
            players.Add(new Player(new Owner("Barack")));
        }

        public IHttpActionResult GetAllProducts()
        {
            return Ok(players);
        }
    }
}
