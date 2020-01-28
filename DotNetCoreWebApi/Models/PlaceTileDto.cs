using System;

namespace DotNetCoreWebApi.Models
{
    public class PlaceTileDto
    {
        public string gameId { get; set; }
        public string playerId { get; set; }
        public string tileProps { get; set; }
        public int RotateAngle { get; set; }
        public int coordinateX { get; set; }
        public int coordinateY { get; set; }
    }
}