using System;
using System.Collections.Generic;
using DotNetCoreWebApi.Backend.Model;

namespace DotNetCoreWebApi.Models
{
    public class TileInfoDto
    {
        public string tile { get; set; }
        public Coordinates position { get; set; }
        public List<FigurePlacementDto> figure { get; set; }
        public TileInfoDto(string tile, Coordinates position, List<FigurePlacementDto> figurePlacement)
        {
            this.tile = tile;
            this.position = position;
            this.figure = figurePlacement;
        }

        public TileInfoDto()
        {
        }
    }
}