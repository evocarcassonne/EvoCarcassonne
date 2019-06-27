using System.Collections.Generic;
using EvoCarcassonne.Controller;
using EvoCarcassonne.Model;

namespace EvoCarcassonne.Backend
{
    public class Castle : ILandscape
    {
        private int whereToGo { get; set; }
        private BoardTile FirstTile { get; set; }
        private bool FinishedCastle { get; set; } = true;
        private int Points { get; set; }
        private BoardTile CurrentBoardTile { get; set; }
        private List<BoardTile> BoardTileList { get; set; } = new List<BoardTile>();
        private int Count { get; set; } = 0;
        private bool firstCall { get; set; } = true;
        private List<IFigure> FiguresOnTiles { get; set; } = new List<IFigure>();
        private int DeleteDirection { get; set; }
        public int calculate()
        {
            throw new System.NotImplementedException();
        }

        public Castle()
        {
        }

        public void calculate(BoardTile currentTile, bool gameover)
        {
            // Check if the castle is not finished         
            if (!FinishedCastle)
                return;

            // Get the CurrentTile's coordinate
            var x = currentTile.Coordinates.X;
            var y = currentTile.Coordinates.Y;
            var index = x + (y * 10);



            // If it is the first tile, reset the properties...
            if (firstCall)
            {
                firstCall = false;
                FirstTile = currentTile;
                CurrentBoardTile = currentTile;
                FirstTile = currentTile;
                Points = 0;
                FinishedCastle = true;
                BoardTileList.Clear();
                Count = 0;
                FiguresOnTiles.Clear();

            }           


            if (IsChecked(CurrentBoardTile, BoardTileList))
                return;


            if (CurrentBoardTile.BackendTile.Directions == null)
            {
                
                if (CheckEndOfCastle(FirstTile) && CountEndOfCastleSides(FirstTile) == 2 && BoardTileList.Count > 2 && Count == 0)
                {
                    FinishedCastle = false;
                    return;
                }
            }

            // If it is an empty tile...
            if (CurrentBoardTile.BackendTile.Directions == null)
            {
                // ... then check if it has double EndOfCastle sides
                if (CheckEndOfCastle(FirstTile) && CountEndOfCastleSides(FirstTile) == 2 && Count == 0)
                {
                    Count++;
                    FinishedCastle = true;
                    return;
                }
                else
                {
                    FinishedCastle = false;
                    return;
                }
            }

            // If it is an EndOfCastle tile and it isn't the first tile...
            if (CheckEndOfCastle(CurrentBoardTile) && FirstTile != CurrentBoardTile)
            {
                if (CurrentBoardTile.BackendTile.Directions[getFromDirection(whereToGo)].Figure != null)
                    FiguresOnTiles.Add(CurrentBoardTile.BackendTile.Directions[getFromDirection(whereToGo)].Figure);

                Points++;
                return;
            }

            


            for (int i = 0; i < 4; i++)
            {
                if (CurrentBoardTile.BackendTile.Directions[i].Landscape is Castle)
                {
                    if (CurrentBoardTile.BackendTile.Directions[i].Figure != null)
                    {
                        FiguresOnTiles.Add(CurrentBoardTile.BackendTile.Directions[i].Figure);
                    }

                    whereToGo = i;
                    BoardTileList.Add(CurrentBoardTile);
                    CurrentBoardTile = MainController.BoardTiles[GetIndex(i,index)];
                    calculate(CurrentBoardTile, false);
                    CurrentBoardTile = currentTile;
                }
            }


            if (FinishedCastle)
                Points++;
            else
                Points = 0;


            if (FirstTile == CurrentBoardTile && Points == 1)
                Points = 0;

            if (CheckEndOfCastle(FirstTile) && CountEndOfCastleSides(FirstTile) == 2 && CurrentBoardTile == FirstTile && Count == 0 && FinishedCastle)
                Points++;

            if (FirstTile == CurrentBoardTile && FinishedCastle && Points != 0)
                DistributePoints(Points);
             

           

            return;
        }      



        private void DistributePoints(int result)
        {
            var playerone = 0;
            var playertwo = 0;
            var players = new List<IOwner>();
            foreach (var i in FiguresOnTiles)
            {
                if (!players.Contains(i.Owner))
                {
                    players.Add(i.Owner);
                }
            }

            for (int j = 0; j < FiguresOnTiles.Count; j++)
            {
                if (players[0].Equals(FiguresOnTiles[j].Owner))
                {
                    playerone++;
                }
                else
                {
                    playertwo++;
                }
            }

            if (playerone > playertwo)
                players[0].Points += result;
            else if (playerone == 0 && playertwo == 0)
                return;
            else if (playertwo > playerone)
                players[1].Points += result;
            else if (playertwo == playerone)
            {
                players[0].Points += (result / 2);
                players[1].Points += (result / 2);
            }
            
        }


        private int getFromDirection(int c)
        {
            switch (c)
            {
                case 0:
                    return 2;
                case 1:
                    return 3;
                case 2:
                    return 0;
                case 3:
                    return 1;
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


        private int GetIndex(int side, int index)
        {
            switch(side)
            {
                case 0:
                    return index - 10;
                case 1:
                    return index + 1;
                case 2:
                    return index + 10;
                case 3:
                    return index - 1;
                default: return 0;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Castle;
        }
    }
}
