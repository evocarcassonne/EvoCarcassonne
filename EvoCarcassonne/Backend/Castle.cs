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
        private bool firstCall { get; set; } = true;
        private List<IFigure> FiguresOnTiles { get; set; } = new List<IFigure>();
        private CardinalDirection StarterWhereToGo { get; set; }
        private bool DeleteFigures { get; set; } = false;
        private List<BoardTile> PlacedCastleTiles { get; set; } = new List<BoardTile>();
        private bool Gameover { get; set; }
        private bool OutOfRange { get; set; } = false;

        public Castle()
        {
        }

        public void calculate(BoardTile currentTile, bool gameover)
        {
            Gameover = gameover;
            PlacedCastleTiles.Clear();

            int result = 0;
            for (int i = 0; i < 4; i++)
            {
                DeleteFigures = false;
                if (CheckEndOfCastle(currentTile) && currentTile.BackendTile.Directions[i].Landscape is Castle)
                {
                    firstCall = true;                   
                    result += CalculateWithDirections(currentTile, (CardinalDirection)i);

                    firstCall = true;
                    DistributePoints(result);
                    FiguresOnTiles = new List<IFigure>();
                    if (result > 0)
                    {
                        DeleteFigures = true;
                        CalculateWithDirections(currentTile, (CardinalDirection)i);
                    }
                    result = 0;

                }
                else if (!CheckEndOfCastle(currentTile))
                {

                    firstCall = true;
                    result += CalculateCastle(currentTile, false);

                    firstCall = true;
                    DistributePoints(result);
                    FiguresOnTiles = new List<IFigure>();
                    if (result > 0)
                    {
                        DeleteFigures = true;
                        CalculateCastle(FirstTile, false);
                    }
                    result = 0;

                    break;
                }
            }
        }


        private int CalculateWithDirections(BoardTile currentTile, CardinalDirection whereToGo)
        {
            // If it is the first tile, reset the properties...
            if (firstCall)
            {
                FirstTile = currentTile;
                CurrentBoardTile = currentTile;
                FirstTile = currentTile;
                Points = 0;
                FinishedCastle = true;
                BoardTileList.Clear();
                FiguresOnTiles.Clear();
                StarterWhereToGo = whereToGo;
            }


            // Check if the castle is not finished         
            if (!FinishedCastle && !Gameover)
                return 0;
            
            if (OutOfRange)
            {
                OutOfRange = false;
                return 0;
            }

            // Get the CurrentTile's coordinate
            var x = currentTile.Coordinates.X;
            var y = currentTile.Coordinates.Y;
            var index = x + (y * 10);

            if (x < 0 || y < 0 || x > 9 || y > 9)
                return 0;



            if (IsChecked(CurrentBoardTile, BoardTileList))
                return 0;


            if (CurrentBoardTile.BackendTile.Directions == null)
            {
                if (!Gameover)
                    FinishedCastle = false;
                return 0;
            }

            // If it is an EndOfCastle tile and it isn't the first tile...
            if (CheckEndOfCastle(CurrentBoardTile) && FirstTile != CurrentBoardTile)
            {
                if (CurrentBoardTile.BackendTile.Directions[getFromDirection((int)whereToGo)].Figure != null)
                    FiguresOnTiles.Add(CurrentBoardTile.BackendTile.Directions[getFromDirection((int)whereToGo)].Figure);

                if (DeleteFigures && CurrentBoardTile.BackendTile.Directions[getFromDirection((int)whereToGo)].Figure != null)
                    CurrentBoardTile.BackendTile.Directions[getFromDirection((int)whereToGo)].Figure = null;

                Points += 2;
                return 0;
            }




            for (int i = 0; i < 4; i++)
            {
                if (CurrentBoardTile.BackendTile.Directions[i].Landscape is Castle)
                {
                    if (firstCall && i != (int)StarterWhereToGo)
                    {
                    }
                    else
                    {
                        if (CurrentBoardTile == FirstTile && (int)StarterWhereToGo == getFromDirection((int)whereToGo) && !firstCall)
                            return 0;

                        if (CurrentBoardTile.BackendTile.Directions[i].Figure != null)
                        {
                            FiguresOnTiles.Add(CurrentBoardTile.BackendTile.Directions[i].Figure);
                        }

                        if (DeleteFigures && CurrentBoardTile.BackendTile.Directions[i].Figure != null)
                            CurrentBoardTile.BackendTile.Directions[i].Figure = null;

                        whereToGo = (CardinalDirection)i;

                        BoardTileList.Add(CurrentBoardTile);
                        PlacedCastleTiles.Add(CurrentBoardTile);
                        firstCall = false;
                        CurrentBoardTile = MainController.BoardTiles[GetIndex(i, index)];
                        CalculateWithDirections(CurrentBoardTile, (CardinalDirection)i);
                        CurrentBoardTile = currentTile;

                        if (CurrentBoardTile == FirstTile)
                            break;
                    }


                }
            }


            if (FinishedCastle)
            {
                Points += 2;
                if (CheckShield(CurrentBoardTile))
                    Points += 2;
            }                
            else
                Points = 0;

            if (DeleteFigures)
                Points = 0;

            if (FirstTile == CurrentBoardTile && Points == 1)
                Points = 0;


            return Points;
        }

        private bool CheckShield(BoardTile bt)
        {
            foreach (var item in bt.BackendTile.Speciality)
            {
                if (item == Speciality.Shield)
                    return true;
            }

            return false;
        }

        private void DistributePoints(int result)
        {
            var points = new List<int>();
            var players = new List<IOwner>();
            var playersToGetPoints = new List<IOwner>();
            foreach (var i in FiguresOnTiles)
            {
                if (!players.Contains(i.Owner))
                {
                    players.Add(i.Owner);
                }
            }
            int maxIndex = 0;
            for (int i = 0; i < players.Count; i++)
            {
                int currentCount = 0;
                for (int j = 0; j < FiguresOnTiles.Count; j++)
                {
                    if (players[i].Equals(FiguresOnTiles[j].Owner))
                    {
                        currentCount++;
                    }
                }
                points.Add(currentCount);
                if (points[i] > points[maxIndex])
                {
                    maxIndex = i;
                }
            }

            if (players.Count != 0)
            {
                playersToGetPoints.Add(players[maxIndex]);
            }
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i] == points[maxIndex] && i != maxIndex)
                {
                    playersToGetPoints.Add(players[i]);
                }
            }
            foreach (var i in playersToGetPoints)
            {
                i.Points += result;
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



        private int GetIndex(int side, int index)
        {            
            switch (side)
            {
                case 0:
                    if (index - 10 < 0)
                    {
                        OutOfRange = true;
                        return 0;
                    }

                    return index - 10;
                case 1:
                    if (index + 1 > MainController.BoardTiles.Count)
                    {
                        OutOfRange = true;
                        return 0;
                    }


                    return index + 1;
                case 2:
                    if (index + 10 > MainController.BoardTiles.Count)
                    {
                        OutOfRange = true;
                        return 0;
                    }


                    return index + 10;
                case 3:
                    if (index - 1 < 0)
                    {
                        OutOfRange = true;
                        return 0;
                    }


                    return index - 1;
                default: return 0;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Castle;
        }

        // If the current tile doesn't have endofcastle speciality 
        private int CalculateCastle(BoardTile currentTile, bool gameover)
        {
            // Check if the castle is not finished         
            if (!FinishedCastle && !Gameover)
                return 0;

            // Get the CurrentTile's coordinate
            var x = currentTile.Coordinates.X;
            var y = currentTile.Coordinates.Y;
            var index = x + (y * 10);

            if (x < 0 || y < 0 || x > 9 || y > 9)
                return 0;


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
                FiguresOnTiles.Clear();

            }


            if (IsChecked(CurrentBoardTile, BoardTileList))
                return 0;


            if (CurrentBoardTile.BackendTile.Directions == null)
            {
                if (!Gameover)
                    FinishedCastle = false;

                return 0;
            }


            // If it is an EndOfCastle tile and it isn't the first tile...
            if (CheckEndOfCastle(CurrentBoardTile) && FirstTile != CurrentBoardTile)
            {
                if (CurrentBoardTile.BackendTile.Directions[getFromDirection(whereToGo)].Figure != null)
                    FiguresOnTiles.Add(CurrentBoardTile.BackendTile.Directions[getFromDirection(whereToGo)].Figure);

                if (DeleteFigures && CurrentBoardTile.BackendTile.Directions[getFromDirection((int)whereToGo)].Figure != null)
                    CurrentBoardTile.BackendTile.Directions[getFromDirection((int)whereToGo)].Figure = null;

                Points += 2;
                return 0;
            }




            for (int i = 0; i < 4; i++)
            {
                if (CurrentBoardTile.BackendTile.Directions[i].Landscape is Castle)
                {
                    if (CurrentBoardTile.BackendTile.Directions[i].Figure != null)
                    {
                        FiguresOnTiles.Add(CurrentBoardTile.BackendTile.Directions[i].Figure);
                    }

                    if (DeleteFigures && CurrentBoardTile.BackendTile.Directions[i].Figure != null)
                        CurrentBoardTile.BackendTile.Directions[i].Figure = null;

                    whereToGo = i;
                    BoardTileList.Add(CurrentBoardTile);
                    CurrentBoardTile = MainController.BoardTiles[GetIndex(i, index)];
                    CalculateCastle(CurrentBoardTile, false);
                    CurrentBoardTile = currentTile;
                }
            }


            if (FinishedCastle)
            {
                Points += 2;
                if (CheckShield(CurrentBoardTile))
                    Points += 2;
            }                
            else
                Points = 0;

            if (DeleteFigures)
                Points = 0;


            if (FirstTile == CurrentBoardTile && Points == 1)
                Points = 0;




            return Points;
        }
    }
}
