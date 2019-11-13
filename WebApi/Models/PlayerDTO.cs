using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class PlayerDTO
    {
        public int NumberOfFigures { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }

        public PlayerDTO(int numberOfFigures, string name, int points)
        {
            NumberOfFigures = numberOfFigures;
            Name = name;
            Points = points;
        }

        public PlayerDTO()
        {
        }


    }
}