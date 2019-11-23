using System;
using DotNetCoreWebApi.Backend.Model;

namespace DotNetCoreWebApi.Models
{
    public class FigurePlacementDto
    {
        public Guid Player { get; set; }
        public int Side { get; set; }

        public FigurePlacementDto() { }
        public FigurePlacementDto(Guid player, int side)
        {
            Player = player;
            Side = side;
        }
    }
}