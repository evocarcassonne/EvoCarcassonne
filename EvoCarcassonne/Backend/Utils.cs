using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            foreach (var neighborTile in surroundingTiles)
            {
                if (!boardTile.BackendTile.getTileSideByCardinalDirection(neighborTile.Key).Landscape
                    .Equals(neighborTile.Value.BackendTile
                        .getTileSideByCardinalDirection(getOppositeDirection(neighborTile.Key)).Landscape))
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
                if (currentTile.Coordinates.X + 10 == neighborTile.Coordinates.X &&
                    currentTile.Coordinates.Y == neighborTile.Coordinates.Y)
                {
                    result.Add(CardinalDirection.East, neighborTile);
                }

                if (currentTile.Coordinates.Y + 10 == neighborTile.Coordinates.Y &&
                    currentTile.Coordinates.X == neighborTile.Coordinates.X)
                {
                    result.Add(CardinalDirection.South, neighborTile);
                }


                if (currentTile.Coordinates.X - 10 == neighborTile.Coordinates.X &&
                    currentTile.Coordinates.Y == neighborTile.Coordinates.Y)
                {
                    result.Add(CardinalDirection.West, neighborTile);
                }


                if (currentTile.Coordinates.Y - 10 == neighborTile.Coordinates.Y &&
                    currentTile.Coordinates.X == neighborTile.Coordinates.X)
                {
                    result.Add(CardinalDirection.North, neighborTile);
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

        
    }
}
