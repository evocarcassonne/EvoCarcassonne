using System.Collections.Generic;
using System.Linq;

namespace DotNetCoreWebApi.Backend.Model
{
    public class Church : ITile, ILandscape
    {
        public List<IDirection> Directions { get; set; }
        public List<Speciality> Speciality { get; set; }
        public Coordinates Position { get; set; }
        private IFigure _centerFigure;
        public IFigure CenterFigure
        {
            get => _centerFigure;
            set
            {
                if (_centerFigure != value)
                {
                    _centerFigure = value;
                    //OnPropertyChanged();
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
                    break;
                case 90:
                    temp = Directions.Last();
                    for (int i = Directions.Count - 1; i > 0; i--)
                    {
                        Directions[i] = Directions[i - 1];
                    }

                    Directions[0] = temp;
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

        public void calculate(ITile currentTile, bool gameover, out List<IFigure> figuresToGiveBack)
        {
            List<ITile> surroundingTiles = new List<ITile>();
            figuresToGiveBack = new List<IFigure>();
            currentTile.Directions.ForEach(e => surroundingTiles.Add(e.Neighbor));
            
            var churchTile = (Church)currentTile;
            if (gameover)
            {
                churchTile.CenterFigure.Owner.Points += surroundingTiles.Count;
                churchTile.CenterFigure = null;
            }
            else
            {
                if (surroundingTiles.Count == 8 && surroundingTiles.All(e => e != null))
                {
                    if (churchTile.CenterFigure != null)
                    {
                        churchTile.CenterFigure.Owner.Points += 9;
                        figuresToGiveBack.Add(churchTile.CenterFigure);
                        churchTile.CenterFigure = null;
                    }
                }
            }
        }

        public bool CanPlaceFigure(ITile currentTile, CardinalDirection whereToGo, bool firstCall)
        {
            return true;
        }
    }
}
