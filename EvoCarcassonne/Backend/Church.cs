using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EvoCarcassonne.Model;

namespace EvoCarcassonne.Backend
{
    public class Church : ObservableObject, ITile, ILandscape
    {
        public List<IDirection> Directions { get; set; }
        public List<Speciality> Speciality { get; set; }

        private IFigure _centerFigure;
        public IFigure CenterFigure
        {
            get => _centerFigure;
            set
            {
                if (_centerFigure != value)
                {
                    _centerFigure = value;
                    OnPropertyChanged();
                }
            }
        }

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
                default:
                    Debug.WriteLine(@"[ERROR] You have given a wrong rotation value!");
                    break;
            }
        }

        public IDirection getTileSideByCardinalDirection(CardinalDirection side)
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

        public void calculate(BoardTile currentTile, bool gameover)
        {
            List<BoardTile> surroundingTiles = Utils.GetAllSurroundingTiles(currentTile);
            var churchTile = (Church)currentTile.BackendTile;
            if (gameover)
            {
                churchTile.CenterFigure.Owner.Points += surroundingTiles.Count;
                churchTile.CenterFigure = null;
            }
            else
            {
                if (surroundingTiles.Count == 8)
                {
                    if (churchTile.CenterFigure != null)
                    {
                        churchTile.CenterFigure.Owner.Points += 9;
                        churchTile.CenterFigure = null;
                    }
                }
            }
        }
    }
}
