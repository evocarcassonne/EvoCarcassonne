using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Media.Animation;
using EvoCarcassonne.Controller;
using EvoCarcassonne.Model;

namespace EvoCarcassonne.Backend
{
    public static class Utils
    {
        public static int DistanceBetweenTiles = 1;
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
                    .getTileSideByCardinalDirection(getOppositeDirection(neighborTile.Key)).Landscape;
                
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
                if (isOnTheGivenSide(currentTile, neighborTile, DistanceBetweenTiles,0))
                {
                    result.Add(CardinalDirection.East, neighborTile);
                }
                if (isOnTheGivenSide(currentTile, neighborTile, 0,DistanceBetweenTiles))
                {
                    result.Add(CardinalDirection.South, neighborTile);
                }
                if (isOnTheGivenSide(currentTile, neighborTile, -DistanceBetweenTiles,0))
                {
                    result.Add(CardinalDirection.West, neighborTile);
                }
                if (isOnTheGivenSide(currentTile, neighborTile, 0,-DistanceBetweenTiles))
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
                if (isOnTheGivenSide(currentTile, neighborTile, DistanceBetweenTiles,DistanceBetweenTiles) ||
                    isOnTheGivenSide(currentTile, neighborTile, -DistanceBetweenTiles,DistanceBetweenTiles) ||
                    isOnTheGivenSide(currentTile, neighborTile, -DistanceBetweenTiles,-DistanceBetweenTiles) ||
                    isOnTheGivenSide(currentTile, neighborTile, DistanceBetweenTiles,-DistanceBetweenTiles))
                {
                    result.Add(neighborTile);
                }
            }
            return result;
        }

        public static CardinalDirection getOppositeDirection(CardinalDirection direction)
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

        public static BoardTile getNeighborTile(Dictionary<CardinalDirection, BoardTile> tilesNextToTheGivenTile, CardinalDirection whereToGo)
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

        public static bool isOnTheGivenSide(BoardTile currentTile, BoardTile neighborTile, int diffX, int diffY)
        {
            return currentTile.Coordinates.X + diffX == neighborTile.Coordinates.X &&
                currentTile.Coordinates.Y + diffY == neighborTile.Coordinates.Y;
        }

        /// <summary>
        /// Checks whether a road is finished ot not with the given tile.
        /// </summary>
        /// <param name="currentTile"></param>
        /// <returns></returns>
        public static Dictionary<CardinalDirection, bool> IsFinishedRoad(BoardTile currentTile)
        {
            Dictionary<CardinalDirection, bool> result = new Dictionary<CardinalDirection, bool>();
            
            for (int i = 0; i < currentTile.BackendTile.Directions.Count; i++)
            {
                if (currentTile.BackendTile.Directions[i].Landscape is Road)
                {
                    result.Add((CardinalDirection)i, IsRoadFinishedGivenDirection(currentTile, (CardinalDirection)i));
                }
                /*else
                {
                    result.Add((CardinalDirection)i, false);
                }*/
            }

            return result;
        }

        public static BoardTile SearchEndOfRoadTileInGivenDirection(BoardTile currentTile, CardinalDirection whereToGo)
        {
            Dictionary<CardinalDirection, BoardTile> tilesNextToTheGivenTile = GetSurroundingTiles(currentTile);
            BoardTile neighborTile = getNeighborTile(tilesNextToTheGivenTile, whereToGo);
            if (neighborTile == null || IsEndOfRoad(neighborTile))
            {
                return neighborTile;
            }
            for (var i = 0; i < 4; i++)
            {
                if (neighborTile.BackendTile.Directions[i].Landscape is Road && i != (int)getOppositeDirection(whereToGo))
                {
                    return SearchEndOfRoadTileInGivenDirection(neighborTile, neighborTile.BackendTile.GetCardinalDirectionByIndex(i));
                }
            }
            return null;
        }

        public static bool IsRoadFinishedGivenDirection(BoardTile currentTile, CardinalDirection whereToGo)
        {
            #region Init required values
            
            Dictionary<CardinalDirection, BoardTile> tilesNextToTheGivenTile = GetSurroundingTiles(currentTile);
            BoardTile neighborTile = getNeighborTile(tilesNextToTheGivenTile, whereToGo);
            #endregion
            
            #region Calculation
            if (neighborTile == null)
            {
                return false;
            }
            if (IsEndOfRoad(neighborTile))
            {
                return true;
            }
            return searchInTilesSides(neighborTile, (int)getOppositeDirection(whereToGo));
            #endregion
            
        }

        private static bool IsEndOfRoad(BoardTile neighborTile)
        {
            return neighborTile.BackendTile.Speciality.Contains(Speciality.EndOfRoad);
        }
        
        private static bool searchInTilesSides(BoardTile neighborTile, int sideNumber)
        {
            for (int i = 0; i < 4; i++)
            {
                if (neighborTile.BackendTile.Directions[i].Landscape is Road && i != sideNumber)
                {
                    return IsRoadFinishedGivenDirection(neighborTile, neighborTile.BackendTile.GetCardinalDirectionByIndex(i));
                }
            }
            return false;
        }
        
       
    }
}
