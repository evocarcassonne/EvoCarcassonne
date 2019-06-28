using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EvoCarcassonne.Models;
using EvoCarcassonne.ViewModels;

namespace EvoCarcassonne.Backend
{
    public  class Utils
    {
        private  int DistanceBetweenTiles = 1;
        private readonly MainController _mainController;

        public Utils(MainController mainController)
        {
            _mainController = mainController;
        }

        public List<string> GetResourceNames(string condition)
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
        
        public  bool CheckFitOfTile(BoardTile boardTile)
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

        public  Dictionary<CardinalDirection, BoardTile> GetSurroundingTiles(BoardTile currentTile)
        {
            Dictionary<CardinalDirection, BoardTile> result =
                new Dictionary<CardinalDirection, BoardTile>();

            foreach (var neighborTile in _mainController.PlacedBoardTiles)
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
        public  List<BoardTile> GetAllSurroundingTiles(BoardTile currentTile)
        {

            List<BoardTile> result = new List<BoardTile>();
            foreach (var list in GetSurroundingTiles(currentTile))
            {
                result.Add(list.Value);
            }
            foreach (var neighborTile in _mainController.PlacedBoardTiles)
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

        public  CardinalDirection GetOppositeDirection(CardinalDirection direction)
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

        public  BoardTile GetNeighborTile(Dictionary<CardinalDirection, BoardTile> tilesNextToTheGivenTile, CardinalDirection whereToGo)
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

        private  bool IsOnTheGivenSide(BoardTile currentTile, BoardTile neighborTile, int diffX, int diffY)
        {
            return currentTile.Coordinates.X + diffX == neighborTile.Coordinates.X &&
                currentTile.Coordinates.Y + diffY == neighborTile.Coordinates.Y;
        }
    }
}
