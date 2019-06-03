using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using EvoCarcassonne.Controller;
using EvoCarcassonne.Model;

namespace EvoCarcassonne.Backend
{
    public static class Utils
    {
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


        public static bool IsFinishedRoad(BoardTile currentTile)
        {
            return false;
        }

        public static Dictionary<CardinalDirection, BoardTile> GetSurroundingTiles(BoardTile currentTile)
        {
            Dictionary<CardinalDirection, BoardTile> result =
                new Dictionary<CardinalDirection, BoardTile>();

            foreach (var neighborTile in MainController.PlacedBoardTiles)
            {
                if (isOnTheGivenSide(currentTile, neighborTile, 10,0))
                {
                    result.Add(CardinalDirection.East, neighborTile);
                }
                if (isOnTheGivenSide(currentTile, neighborTile, 0,10))
                {
                    result.Add(CardinalDirection.South, neighborTile);
                }
                if (isOnTheGivenSide(currentTile, neighborTile, -10,0))
                {
                    result.Add(CardinalDirection.West, neighborTile);
                }
                if (isOnTheGivenSide(currentTile, neighborTile, 0,-10))
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
                if (isOnTheGivenSide(currentTile, neighborTile, 10,10) ||
                    isOnTheGivenSide(currentTile, neighborTile, -10,10) ||
                    isOnTheGivenSide(currentTile, neighborTile, -10,-10) ||
                    isOnTheGivenSide(currentTile, neighborTile, 10,-10))
                {
                    result.Add(neighborTile);
                }
            }
            return result;
        }

        private static CardinalDirection getOppositeDirection(CardinalDirection direction)
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
       
    }
}
