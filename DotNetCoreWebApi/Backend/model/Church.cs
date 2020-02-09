using System.Collections.Generic;
using System.Linq;

namespace DotNetCoreWebApi.Backend.Model
{
    public class Church : ITile
    {
        public List<IDirection> Directions { get; set; }
        public List<Speciality> Speciality { get; set; }
        public Coordinates Position { get; set; }
        public int Rotation { get; set; } = 0;
        private IFigure _centerFigure;
        public IFigure CenterFigure
        {
            get => _centerFigure;
            set
            {
                if (_centerFigure != value)
                {
                    _centerFigure = value;
                }
            }
        }

        public string PropertiesAsString { get; set; }

        public Church(List<IDirection> directions, List<Speciality> speciality)
        {
            Directions = directions;
            Speciality = speciality;
        }

        public void Rotate(int direction)
        {
            IDirection temp;
            switch (direction)
            {
                case -90:
                    temp = Directions.First();
                    for (int i = 0; i < Directions.Count - 1; i++)
                    {
                        Directions[i] = Directions[i + 1];
                    }

                    Directions[Directions.Count - 1] = temp;
                    this.Rotation += -90;
                    break;
                case 90:
                    temp = Directions.Last();
                    for (int i = Directions.Count - 1; i > 0; i--)
                    {
                        Directions[i] = Directions[i - 1];
                    }

                    Directions[0] = temp;
                    this.Rotation += 90;
                    break;
            }
        }

        public IDirection GetTileSideByCardinalDirection(CardinalDirection side)
        {
            switch (side)
            {
                case CardinalDirection.North: return Directions[0];
                case CardinalDirection.East: return Directions[1];
                case CardinalDirection.South: return Directions[2];
                case CardinalDirection.West: return Directions[3];
                default: return Directions[0];
            }
        }

        public CardinalDirection GetCardinalDirectionByIndex(int index)
        {
            switch (index)
            {
                case 0: return CardinalDirection.North;
                case 1: return CardinalDirection.East;
                case 2: return CardinalDirection.South;
                case 3: return CardinalDirection.West;
                default: return CardinalDirection.North;
            }
        }

        public override string ToString()
        {
            return "directions1: " + Directions[0].Landscape + "    directions2" +
                   Directions[1].Landscape +
                   "    directions2" + Directions[2].Landscape + "    directions4" + Directions[3].Landscape +
                   "    specialty: " + Speciality +
                   "    stringRepresentation: " + PropertiesAsString;
        }
    }
}
