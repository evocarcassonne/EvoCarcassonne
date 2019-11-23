using System;

namespace DotNetCoreWebApi.Models
{
    public class PlaceTileDto
    {
        public Guid gameId { get; set; }
        public Guid playerId { get; set; }
        public string tileProps { get; set; }
        public int coordinateX { get; set; }
        public int coordinateY { get; set; }
        public bool placeFigure { get; set; }
        public int side { get; set; }
    }
}