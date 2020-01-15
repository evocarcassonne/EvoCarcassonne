using System;
using System.Collections.Generic;
using DotNetCoreWebApi.Backend.Model;

namespace DotNetCoreWebApi.Models
{
    public class TileInfoDto
    {
        public string tile { get; set; }
        public Coordinates position { get; set; }
        public int rotation { get; set; }
        public FigurePlacementDto figure { get; set; }
        public TileInfoDto(string tile, Coordinates position, FigurePlacementDto figurePlacement, int rotation)
        {
            this.tile = tile;
            this.position = position;
            this.rotation = rotation;
            this.figure = figurePlacement;
        }

        public TileInfoDto()
        {
        }
    }
}