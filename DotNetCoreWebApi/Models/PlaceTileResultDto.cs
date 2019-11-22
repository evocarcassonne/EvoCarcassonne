using System;

namespace DotNetCoreWebApi.Models
{
    public class PlaceTileResultDto
    {
        public bool DidPlaceTile { get; set; }
        public bool DidPlaceFigure { get; set; }
        
        public PlaceTileResultDto(){}
        public PlaceTileResultDto(bool didPlaceTile, bool didPlaceFigure){
            DidPlaceTile = didPlaceTile;
            DidPlaceFigure = didPlaceFigure;
        }
    }
}