using System;
using DotNetCoreWebApi.Backend.Model;

namespace DotNetCoreWebApi.Models
{
    public class FigurePlacementDto
    {
        public string Player { get; set; }
        public int Side { get; set; }

        public FigurePlacementDto() { }
        public FigurePlacementDto(string player, int side)
        {
            Player = player;
            Side = side;
        }
    }
}