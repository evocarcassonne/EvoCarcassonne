using System.Collections.Generic;
using System.Linq;
using EvoCarcassonne.Controller;
using EvoCarcassonne.Model;

namespace EvoCarcassonne.Backend
{
    public class Castle : ILandscape
    {

        #region Private Members

        private BoardTile FirstTile { get; set; }
        private bool Good { get; set; } = true;
        private int Points { get; set; }
        private BoardTile CurrentBoardTile { get; set; }
        private CardinalDirection FromDirection { get; set; }
        private List<BoardTile> BoardTileList { get; set; } = new List<BoardTile>();
        private int Count { get; set; } = 0;

        #endregion

        public int calculate()
        {
            throw new System.NotImplementedException();
        }

        public Castle()
        {
           
        }

        public int calculate(BoardTile currentTile, CardinalDirection whereToGo, bool firstCall, bool gameover)
        {
            // Check if the castle is not finished         
            if (!Good)
            {
                Points = 0;
                return 0;
            }

            // Get the CurrentTile's coordinate
            var x = currentTile.Tag.ToString().Split(';').Select(int.Parse).ToArray();
            var index = x[1] + (x[0] * 10);

            

            // If it is the first tile, reset the properties...
            if (firstCall)
            {
                FirstTile = currentTile;
                CurrentBoardTile = currentTile;
                Points = 0;
                whereToGo = CardinalDirection.North;
                Good = true;
                BoardTileList.Clear();
                Count = 0;
                
            } else
            {
                // ... if not, get the from direction
                if(!CheckEndOfCastle(CurrentBoardTile)) 
                    FromDirection = getFromDirection(whereToGo);
            }

            // If the CurrentTile is already checked, return
            if (!firstCall && IsChecked(CurrentBoardTile, BoardTileList))
                return 0;

            // If it is an empty tile...
            if (CurrentBoardTile.BackendTile.Directions == null)
            {
                // ... then check if it has double EndOfCastle sides
                if (CheckEndOfCastle(FirstTile) && CountEndOfCastleSides(FirstTile) == 2 && Count == 0)
                {
                    Count++;
                    Good = true;
                    return 0;
                } else
                {
                    Good = false;
                    return 0;
                }               
            }

            // If it is an EndOfCastle tile and it isn't the first tile...
            if (CheckEndOfCastle(CurrentBoardTile) && !firstCall)
            {
                Points++;
                return Points;
            }

            // North
            if (CurrentBoardTile.BackendTile.Directions[0].Landscape is Castle)
            {
                // If it isn't the first call, we only have to check 3 sides
                if (!firstCall && FromDirection != CardinalDirection.North)
                {
                    BoardTileList.Add(CurrentBoardTile);
                    CurrentBoardTile = MainController.BoardTiles[index - 10];
                    calculate(CurrentBoardTile, CardinalDirection.North, false, false); 
                    CurrentBoardTile = currentTile;
                }

                // If it is the first call, the FromDirection doesn't matter
                if (firstCall)
                {
                    BoardTileList.Add(CurrentBoardTile);
                    CurrentBoardTile = MainController.BoardTiles[index - 10];
                    calculate(CurrentBoardTile, CardinalDirection.North, false, false);
                    CurrentBoardTile = currentTile;
                }
            }

            // East
            if (CurrentBoardTile.BackendTile.Directions[1].Landscape is Castle)
            {
                if (!firstCall && FromDirection != CardinalDirection.East)
                {
                    BoardTileList.Add(CurrentBoardTile);
                    CurrentBoardTile = MainController.BoardTiles[index + 1];
                    calculate(CurrentBoardTile, CardinalDirection.East, false, false);
                    
                    CurrentBoardTile = currentTile;
                }

                if (firstCall)
                {
                    BoardTileList.Add(CurrentBoardTile);
                    CurrentBoardTile = MainController.BoardTiles[index + 1];
                    calculate(CurrentBoardTile, CardinalDirection.East, false, false);
                    CurrentBoardTile = currentTile;
                }
            }

            // South
            if (CurrentBoardTile.BackendTile.Directions[2].Landscape is Castle)
            {
                if (!firstCall && FromDirection != CardinalDirection.South)
                {
                    BoardTileList.Add(CurrentBoardTile);
                    CurrentBoardTile = MainController.BoardTiles[index + 10];
                    calculate(CurrentBoardTile, CardinalDirection.South, false, false);
                    
                    CurrentBoardTile = currentTile;
                }

                if (firstCall)
                {
                    BoardTileList.Add(CurrentBoardTile);
                    CurrentBoardTile = MainController.BoardTiles[index + 10];
                    calculate(CurrentBoardTile, CardinalDirection.South, false, false);
                    CurrentBoardTile = currentTile;
                }
            }

            // West
            if (CurrentBoardTile.BackendTile.Directions[3].Landscape is Castle)
            {
                if (!firstCall && FromDirection != CardinalDirection.West)
                {
                    BoardTileList.Add(CurrentBoardTile);
                    CurrentBoardTile = MainController.BoardTiles[index - 1];
                    calculate(CurrentBoardTile, CardinalDirection.West, false, false);
                    
                    CurrentBoardTile = currentTile;
                }

                if (firstCall)
                {
                    BoardTileList.Add(CurrentBoardTile);
                    CurrentBoardTile = MainController.BoardTiles[index - 1];
                    calculate(CurrentBoardTile, CardinalDirection.West, false, false);
                    CurrentBoardTile = currentTile;
                }
            }
           

            if (Good)
                Points++;
            else
                Points = 0;


            if (firstCall && Points == 1)
                Points = 0;

            return Points;
        }



        #region Private Helpers 

        private CardinalDirection getFromDirection(CardinalDirection c)
        {
            switch (c)
            {
                case CardinalDirection.North:
                    return CardinalDirection.South;
                case CardinalDirection.East:
                    return CardinalDirection.West;
                case CardinalDirection.South:
                    return CardinalDirection.North;
                case CardinalDirection.West:
                    return CardinalDirection.East;
                default:
                    return 0;
            }
        }

        private bool CheckEndOfCastle(BoardTile bt)
        {
            foreach (var item in bt.BackendTile.Speciality)
            {
                if (item == Speciality.EndOfCastle)
                    return true;
            }

            return false;
        }

        private bool IsChecked(BoardTile currentTile, List<BoardTile> btList)
        {
            foreach (var item in btList)
            {
                if (item == currentTile)
                    return true;
            }

            return false;
        }


        private int CountEndOfCastleSides(BoardTile currentTile)
        {
            if (currentTile.BackendTile.Directions == null)
                return 0;

            var a = 0;
            for (int i = 0; i < 4; i++)
            {
                if (currentTile.BackendTile.Directions[i].Landscape is Castle)
                    a++;
            }
            return a;
        }
        #endregion




        public override bool Equals(object obj)
        {
            return obj is Castle;
        }
    }
}
