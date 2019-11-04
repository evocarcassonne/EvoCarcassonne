using System.Collections.Generic;
using Backend.Model;

namespace Backend
{
    public class Utils
    {
        private int DistanceBetweenTiles = 1;
        
        public static bool CheckFitOfTile(ITile tile)
        {
            Dictionary<CardinalDirection, ITile> surroundingTiles = GetSurroundingTiles(tile);
            if (surroundingTiles.Count == 0)
            {
                return false;
            }
            foreach (var neighborTile in surroundingTiles)
            {
                ILandscape currentTileLandscape = tile.getTileSideByCardinalDirection(neighborTile.Key).Landscape;
                ILandscape neighborTileLandscape = neighborTile.Value.getTileSideByCardinalDirection(GetOppositeDirection(neighborTile.Key)).Landscape;
                
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
            
            /*foreach (var neighborTile in _mainController.PlacedITiles)
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
            }*/
            return result;
        }
        public static List<ITile> GetAllSurroundingTiles(ITile currentTile)
        {

            Dictionary<CardinalDirection, ITile> immediateNeighbors = GetSurroundingTiles(currentTile);
            List<ITile> result = new List<ITile>();
            result.AddRange(immediateNeighbors.Values);
            foreach (var neighborTile in immediateNeighbors)
            {
                /*if (IsOnTheGivenSide(currentTile, neighborTile, DistanceBetweenTiles,DistanceBetweenTiles) ||
                    IsOnTheGivenSide(currentTile, neighborTile, -DistanceBetweenTiles,DistanceBetweenTiles) ||
                    IsOnTheGivenSide(currentTile, neighborTile, -DistanceBetweenTiles,-DistanceBetweenTiles) ||
                    IsOnTheGivenSide(currentTile, neighborTile, DistanceBetweenTiles,-DistanceBetweenTiles))
                {
                    result.Add(neighborTile);
                }*/
                ITile neighbor = null;
                if (neighborTile.Key == CardinalDirection.East)
                {
                    neighbor = neighborTile.Value.getTileSideByCardinalDirection(CardinalDirection.North).Neighbor;
                }else if (neighborTile.Key == CardinalDirection.South)
                {
                    neighbor = neighborTile.Value.getTileSideByCardinalDirection(CardinalDirection.East).Neighbor;
                }else if (neighborTile.Key == CardinalDirection.West)
                {
                    neighbor = neighborTile.Value.getTileSideByCardinalDirection(CardinalDirection.South).Neighbor;
                }else if (neighborTile.Key == CardinalDirection.North)
                {
                    neighbor = neighborTile.Value.getTileSideByCardinalDirection(CardinalDirection.West).Neighbor;
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

        public static void GiveBackFigureToOwner(IFigure figure, ref List<Player> players)
        {
            if (figure != null)
            {
                foreach (var player in players)
                {
                    if (player.Owner.Equals(figure.Owner))
                    {
                        player.Figures.Add(new Figure(figure.Owner));
                    }
                }
            }
        }
        
        public static void DistributePoints(int result, List<IFigure> figuresOnTiles)
        {
            var points = new List<int>();
            var players = new List<IOwner>();
            var playersToGetPoints = new List<IOwner>();
            foreach (var i in figuresOnTiles)
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
                for (int j = 0; j < figuresOnTiles.Count; j++)
                {
                    if (players[i].Equals(figuresOnTiles[j].Owner))
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

            if (players.Count!=0)
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
    }
}
