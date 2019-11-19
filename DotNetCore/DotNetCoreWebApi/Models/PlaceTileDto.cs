using System;

namespace DotNetCoreWebApi.Models
{
    public class PlaceTileDto
    {
        public Guid gameId { get; set; }
        public string tileProps { get; set; }
        public int coordinateX { get; set; }
        public int coordinateY { get; set; }
        public int side { get; set; }
    }
}