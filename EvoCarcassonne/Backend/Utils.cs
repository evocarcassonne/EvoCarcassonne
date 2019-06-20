using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Media.Animation;
using EvoCarcassonne.Controller;
using EvoCarcassonne.Model;

namespace EvoCarcassonne.Backend
{
    public static class Utils
    {
        private static BoardTile FirstTile = new BoardTile();
        private static int DistanceBetweenTiles = 1;
        public static Dictionary<CardinalDirection, IFigure> FiguresOnBoardTile = new Dictionary<CardinalDirection, IFigure>();
        public static List<string> GetResourceNames(string condition)
        {
            var asm = Assembly.GetEntryAssembly();
            var resName = asm.GetName().Name + ".g.resources";
            using (var stream = asm.GetManifestResourceStream(resName))
            using (var reader = new System.Resources.ResourceReader(stream ?? throw new InvalidOperationException()))
            {
                return reader.Cast<DictionaryEntry>().Select(entry => (string)entry.Key)
                    .Where(x => x.Contains(condition)).ToList();
            }
        }

        public static bool CheckFitOfTile(BoardTile boardTile)
        {
            Dictionary<CardinalDirection, BoardTile> surroundingTiles = GetSurroundingTiles(boardTile);
            if (surroundingTiles.Count == 0)
            {
                return false;
            }
            foreach (var neighborTile in surroundingTiles)
            {
                ILandscape currentTileLandscape = boardTile.BackendTile.getTileSideByCardinalDirection(neighborTile.Key).Landscape;
                ILandscape neighborTileLandscape = neighborTile.Value.BackendTile
                    .getTileSideByCardinalDirection(GetOppositeDirection(neighborTile.Key)).Landscape;
                
                if (!currentTileLandscape.Equals(neighborTileLandscape))
                {
                    return false;
                }
            }

            return true;
        }

        public static Dictionary<CardinalDirection, BoardTile> GetSurroundingTiles(BoardTile currentTile)
        {
            Dictionary<CardinalDirection, BoardTile> result =
                new Dictionary<CardinalDirection, BoardTile>();

            foreach (var neighborTile in MainController.PlacedBoardTiles)
            {
                if (IsOnTheGivenSide(currentTile, neighborTile, DistanceBetweenTiles,0))
                {
                    result.Add(CardinalDirection.East, neighborTile);
                }
                if (IsOnTheGivenSide(currentTile, neighborTile, 0,DistanceBetweenTiles))
                {
                    result.Add(CardinalDirection.South, neighborTile);
                }
                if (IsOnTheGivenSide(currentTile, neighborTile, -DistanceBetweenTiles,0))
                {
                    result.Add(CardinalDirection.West, neighborTile);
                }
                if (IsOnTheGivenSide(currentTile, neighborTile, 0,-DistanceBetweenTiles))
                {
                    result.Add(CardinalDirection.North, neighborTile);
                }
            }
            return result;
        }
        public static List<BoardTile> GetAllSurroundingTiles(BoardTile currentTile)
        {

            List<BoardTile> result = new List<BoardTile>();
            foreach (var list in GetSurroundingTiles(currentTile))
            {
                result.Add(list.Value);
            }
            foreach (var neighborTile in MainController.PlacedBoardTiles)
            {
                if (IsOnTheGivenSide(currentTile, neighborTile, DistanceBetweenTiles,DistanceBetweenTiles) ||
                    IsOnTheGivenSide(currentTile, neighborTile, -DistanceBetweenTiles,DistanceBetweenTiles) ||
                    IsOnTheGivenSide(currentTile, neighborTile, -DistanceBetweenTiles,-DistanceBetweenTiles) ||
                    IsOnTheGivenSide(currentTile, neighborTile, DistanceBetweenTiles,-DistanceBetweenTiles))
                {
                    result.Add(neighborTile);
                }
            }
            return result;
        }

        public static CardinalDirection GetOppositeDirection(CardinalDirection direction)
        {
            switch (direction)
            {
                case CardinalDirection.North: return CardinalDirection.South;
                case CardinalDirection.South: return CardinalDirection.North;
                case CardinalDirection.West: return CardinalDirection.East;
                case CardinalDirection.East: return CardinalDirection.West;
                default: return CardinalDirection.South;
            }
        }

        public static BoardTile GetNeighborTile(Dictionary<CardinalDirection, BoardTile> tilesNextToTheGivenTile, CardinalDirection whereToGo)
        {
            foreach (var pair in tilesNextToTheGivenTile)
            {
                if (pair.Key == whereToGo)
                {
                    return pair.Value;
                }
            }
            return null;
        }

        private static bool IsOnTheGivenSide(BoardTile currentTile, BoardTile neighborTile, int diffX, int diffY)
        {
            return currentTile.Coordinates.X + diffX == neighborTile.Coordinates.X &&
                currentTile.Coordinates.Y + diffY == neighborTile.Coordinates.Y;
        }

        /// <summary>
        /// Checks whether a road is finished ot not with the given tile.
        /// </summary>
        /// <param name="currentTile"></param>
        /// <returns>Returns a Dictionary, which tells that whether a road is finished in the given direction. If there is no road on the given tile, it returns an empty dictionary.</returns>
        public static Dictionary<CardinalDirection, bool> IsFinishedRoad(BoardTile currentTile)
        {
            Dictionary<CardinalDirection, bool> result = new Dictionary<CardinalDirection, bool>();
            for (int i = 0; i < currentTile.BackendTile.Directions.Count; i++)
            {
                if (currentTile.BackendTile.Directions[i].Landscape is Road)
                {
                    result.Add((CardinalDirection)i, IsRoadFinishedInGivenDirection(currentTile, (CardinalDirection)i, true));
                }
            }
            return result;
        }

        /// <summary>
        /// Checks if the conditions are fulfilled. If not calls itself again until one of the conditions is fulfilled.
        /// </summary>
        /// <param name="currentTile">The tile we check</param>
        /// <param name="whereToGo">The direction we are supposed to expand the checking</param>
        /// <param name="firstCall">Is is the first call or not</param>
        /// <returns>A bool which tells whether a road is finished in the given direction</returns>
        public static bool IsRoadFinishedInGivenDirection(BoardTile currentTile, CardinalDirection whereToGo, bool firstCall)
        {
            BoardTile neighborTile = GetNeighborTile(GetSurroundingTiles(currentTile), whereToGo);
            if (firstCall)
            {
                FirstTile = currentTile;
            }
            #region Calculation
            if (neighborTile == null)
            {
                return false;
            }
            if (IsEndOfRoad(neighborTile) || FirstTile.Coordinates.X == neighborTile.Coordinates.X && FirstTile.Coordinates.Y == neighborTile.Coordinates.Y)
            {
                return true;
            }
            return SearchInTilesSides(neighborTile, (int)GetOppositeDirection(whereToGo));
            #endregion
        }
        
        /// <summary>
        /// Checks whether the given tile is end of a road or not
        /// </summary>
        /// <param name="currentTile">The examined tile</param>
        /// <returns>True if the given tile is end of road, false if not</returns>
        public static bool IsEndOfRoad(BoardTile currentTile)
        {
            return currentTile.BackendTile.Speciality.Contains(Speciality.EndOfRoad);
        }
        
        /// <summary>
        /// Checks the given tile's side to find roads there, and calls back IsRoadFinishedInGivenDirection
        /// </summary>
        /// <param name="neighborTile">The examined tile</param>
        /// <param name="sideNumber">The index of the side that the road comes from. Aims to avoid infinite loops going back and fourth</param>
        /// <returns>True or false. If the road is finished then it returns true, else it is false.</returns>
        private static bool SearchInTilesSides(BoardTile neighborTile, int sideNumber)
        {
            for (int i = 0; i < 4; i++)
            {
                if (neighborTile.BackendTile.Directions[i].Landscape is Road && i != sideNumber)
                {
                    return IsRoadFinishedInGivenDirection(neighborTile, neighborTile.BackendTile.GetCardinalDirectionByIndex(i), false);
                }
            }
            return false;
        }
        
        /// <summary>
        /// It is supposed to be called after IsFinishedRoad has at least one true value. With the parameters it is going to go, and find the end of road, which is just put down.
        /// </summary>
        /// <param name="currentTile">The tile which has just been put down</param>
        /// <param name="whereToGo">The direction where to search for end of road tile</param>
        /// <returns>The end of road tile found in the given direction. Null if there is no end of road tile</returns>
        public static BoardTile SearchEndOfRoadTileInGivenDirection(BoardTile currentTile, CardinalDirection whereToGo)
        {
            BoardTile neighborTile = GetNeighborTile(GetSurroundingTiles(currentTile), whereToGo);
            if (neighborTile == null || IsEndOfRoad(neighborTile))
            {
                return neighborTile;
            }
            for (var i = 0; i < 4; i++)
            {
                if (neighborTile.BackendTile.Directions[i].Figure != null)
                {
                    FiguresOnBoardTile.Add((CardinalDirection)i, neighborTile.BackendTile.Directions[i].Figure);
                }
                if (neighborTile.BackendTile.Directions[i].Landscape is Road && i != (int)GetOppositeDirection(whereToGo))
                {
                    return SearchEndOfRoadTileInGivenDirection(neighborTile, neighborTile.BackendTile.GetCardinalDirectionByIndex(i));
                }
            }
            return null;
        }
       
    }
}
