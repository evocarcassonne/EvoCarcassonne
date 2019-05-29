using System.Collections.Generic;
using EvoCarcassonne.Controller;
using EvoCarcassonne.Model;

namespace EvoCarcassonne.Backend
{
    public class Castle : ILandscape
    {
        private BoardTile firstTile { get; set; }
        private bool good { get; set; } = true;
        public int points { get; set; } = 0;    
        public int whereToGo { get; set; }
        public List<BoardTile> CheckedBoardtiles { get; set; }

        public Castle()
        {
        }

        public int calculate(BoardTile currentTile, int fromDirection,  bool firstCall)
        {
            var x = currentTile.Tag;
            var currentTileCoordinate = x[1] + (x[0] * 10);

            if (currentTile.Image == null)
                return 0;

            if (firstCall)
            {
                points = 0;
                good = true;
                CheckedBoardtiles.Clear();
                firstTile = currentTile;
            }
                

            if (!firstCall && !(currentTile.BackendTile.Directions[unfinishedCastle(fromDirection)] is Castle))
                good = false;


            if (!CheckEndOfCaste(currentTile))
                CheckedBoardtiles.Add(currentTile);

            while (good)
            {
                if (CheckEndOfCaste(currentTile) && !firstCall && currentTile.BackendTile.Directions[fromDirection] is Castle)
                {
                    points++;
                    return points;
                }

                if (currentTile.BackendTile.Directions[0].Landscape is Castle && fromDirection != 0 && currentTile != firstTile && !AlreadyCheckedBoardtile(CheckedBoardtiles, currentTile))
                {
                    whereToGo = 2;
                    currentTile = MainController.BoardTiles[currentTileCoordinate - 10];
                    calculate(currentTile,2, false);    
                }

                if (currentTile.BackendTile.Directions[1].Landscape is Castle && fromDirection != 1 && currentTile != firstTile && !AlreadyCheckedBoardtile(CheckedBoardtiles, currentTile))
                {
                    currentTile = MainController.BoardTiles[currentTileCoordinate + 1];
                    calculate(currentTile, 3, false);
                }

                if (currentTile.BackendTile.Directions[2].Landscape is Castle && fromDirection != 2 && currentTile != firstTile && !AlreadyCheckedBoardtile(CheckedBoardtiles, currentTile))
                {
                    currentTile = MainController.BoardTiles[currentTileCoordinate + 10];
                    calculate(currentTile, 0, false);
                }

                if (currentTile.BackendTile.Directions[3].Landscape is Castle && fromDirection != 3 && currentTile != firstTile && !AlreadyCheckedBoardtile(CheckedBoardtiles, currentTile))
                {
                    currentTile = MainController.BoardTiles[currentTileCoordinate - 1];
                    calculate(currentTile, 1, false);
                }
                points++;
            }

            if (good == false)
                points = 0;

            return points;
        }

        private bool AlreadyCheckedBoardtile(List<BoardTile> btList, BoardTile bTile)
        {
            foreach(BoardTile bt in btList)
            {
                if (bTile == bt)
                    return true;
            }

            return false;
        }

        private int unfinishedCastle(int fromDirection)
        {
            switch (fromDirection)
            {
                case 0: return 2; break;
                case 1: return 3; break;
                case 2: return 0; break;
                case 3: return 1; break;
                default: return 0; 
            }

        }

        private bool CheckEndOfCaste(BoardTile currentTile)
        {
            foreach (var tile in currentTile.BackendTile.Speciality)
            {
                if (tile == Speciality.EndOfCastle)
                {
                    return true;
                }
            }

            return false;
        }

        public int calculate(BoardTile currentTile, CardinalDirection whereToGo, bool firstCall)
        {
            throw new System.NotImplementedException();
        }
    }
}
