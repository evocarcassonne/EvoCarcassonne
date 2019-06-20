using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EvoCarcassonne.Controller;
using EvoCarcassonne.Model;

namespace EvoCarcassonne.Backend
{
    public class Church : ITile, ILandscape
    {
        public int TileID { get; set; }
        public List<IDirection> Directions { get; set; }
        public List<Speciality> Speciality { get; set; }
        private bool IsColostor = false;

        public Church(int tileId, List<IDirection> directions, List<Speciality> speciality)
        {
            TileID = tileId;
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


        public void calculate(BoardTile currentTile, bool gameover)
        {
            throw new System.NotImplementedException();
        }

        public int calculate(BoardTile currentTile, CardinalDirection whereToGo, bool firstCall, bool gameover)
        {
            List<BoardTile> surroundingTiles = Utils.GetAllSurroundingTiles(currentTile);
            if (gameover)
            {
                return surroundingTiles.Count;
            }
            else
            {
                if (surroundingTiles.Count==8)
                {
                    return 9;
                }
                else
                {
                    return 0;
                }
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
    }
}
