using DotNetCoreWebApi.Backend.Model;
using System.Collections.Generic;

namespace DotNetCoreWebApi.Backend.Utils
{
    public class TileUtils
    {
        public static bool CheckFitOfTile(ITile tile, Dictionary<CardinalDirection, ITile> surroundingTiles)
        {
            if (surroundingTiles.Count == 0)
            {
                return false;
            }
            foreach (var neighborTile in surroundingTiles)
            {
                Landscape currentTileLandscape = tile.GetTileSideByCardinalDirection(neighborTile.Key).Landscape;
                Landscape neighborTileLandscape = neighborTile.Value.GetTileSideByCardinalDirection(GetOppositeDirection(neighborTile.Key)).Landscape;

                if (!currentTileLandscape.Equals(neighborTileLandscape))
                {
                    return false;
                }
            }
            return true;
        }

        public static Dictionary<CardinalDirection, ITile> GetSurroundingTiles(ITile currentTile)
        {
            Dictionary<CardinalDirection, ITile> result =
                new Dictionary<CardinalDirection, ITile>();

            for (int i = 0; i < 4; i++)
            {
                var tile = currentTile.Directions[i].Neighbor;
                if (tile != null)
                {
                    result.Add((CardinalDirection)i, tile);
                }
            }

            return result;
        }
        public static List<ITile> GetAllSurroundingTiles(ITile currentTile)
        {

            Dictionary<CardinalDirection, ITile> immediateNeighbors = GetSurroundingTiles(currentTile);
            List<ITile> result = new List<ITile>();
            result.AddRange(immediateNeighbors.Values);
            foreach (var neighborTile in immediateNeighbors)
            {
                ITile neighbor = null;
                if (neighborTile.Key == CardinalDirection.East)
                {
                    neighbor = neighborTile.Value.GetTileSideByCardinalDirection(CardinalDirection.North).Neighbor;
                }
                else if (neighborTile.Key == CardinalDirection.South)
                {
                    neighbor = neighborTile.Value.GetTileSideByCardinalDirection(CardinalDirection.East).Neighbor;
                }
                else if (neighborTile.Key == CardinalDirection.West)
                {
                    neighbor = neighborTile.Value.GetTileSideByCardinalDirection(CardinalDirection.South).Neighbor;
                }
                else if (neighborTile.Key == CardinalDirection.North)
                {
                    neighbor = neighborTile.Value.GetTileSideByCardinalDirection(CardinalDirection.West).Neighbor;
                }
                if (neighbor != null)
                {
                    result.Add(neighbor);
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

        public static ITile GetNeighborTile(Dictionary<CardinalDirection, ITile> tilesNextToTheGivenTile, CardinalDirection whereToGo)
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
