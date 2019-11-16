using Backend.Model;

namespace WebApi.Models
{
    public class TileInfoDto
    {
        public string tile { get; set; }
        public Coordinates position { get; set; }

        public TileInfoDto(string tile, Coordinates position)
        {
            this.tile = tile;
            this.position = position;
        }

        public TileInfoDto()
        {
        }
    }
}