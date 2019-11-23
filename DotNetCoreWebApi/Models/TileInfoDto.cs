using System;
using DotNetCoreWebApi.Backend.Model;

namespace DotNetCoreWebApi.Models
{
    public class TileInfoDto
    {
        public string tile { get; set; }
        public Coordinates position { get; set; }
        public FigurePlacementDto figure { get; set; }
        public TileInfoDto(string tile, Coordinates position, FigurePlacementDto figurePlacement)
        {
            this.tile = tile;
            this.position = position;
            this.figure = figurePlacement;
        }
        public TileInfoDto(string tile, Coordinates position, Guid playerId, int side)
        {
            this.tile = tile;
            this.position = position;
            this.figure = new FigurePlacementDto(playerId, side);
        }

        public TileInfoDto()
        {
        }
    }
}