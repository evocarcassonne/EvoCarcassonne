using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetCoreWebApi.Models
{
    public class PlayerDto
    {
        public Guid PlayerId { get; set; }
        public int NumberOfFigures { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }

        public PlayerDto(Guid playerId, int numberOfFigures, string name, int points)
        {
            PlayerId = playerId;
            NumberOfFigures = numberOfFigures;
            Name = name;
            Points = points;
        }

        public PlayerDto(int numberOfFigures, string name, int points)
        {
            PlayerId = Guid.NewGuid();
            NumberOfFigures = numberOfFigures;
            Name = name;
            Points = points;
        }
        
        public PlayerDto()
        {
        }


    }
}