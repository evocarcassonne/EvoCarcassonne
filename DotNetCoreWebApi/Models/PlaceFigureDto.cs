using System;

namespace DotNetCoreWebApi.Models
{
    public class PlaceFigureDto
    {
        public string gameId { get; set; }
        public string playerId { get; set; }
        public int side { get; set; }
    }
}