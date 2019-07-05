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
        
        public bool CheckFitOfTile(BoardTile boardTile)
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
        public List<BoardTile> GetAllSurroundingTiles(BoardTile currentTile)
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

        public CardinalDirection GetOppositeDirection(CardinalDirection direction)
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

        public BoardTile GetNeighborTile(Dictionary<CardinalDirection, BoardTile> tilesNextToTheGivenTile, CardinalDirection whereToGo)
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

        public void GiveBackFigureToOwner(IFigure figure)
        {
            if (figure != null)
            {
                foreach (var player in _mainController.Players)
                {
                    if (player.BackendOwner.Equals(figure.Owner))
                    {
                        player.Figures.Add(new Figure(figure.Owner));
                    }
                }
            }
        }
        
        public void DistributePoints(int result, List<IFigure> figuresOnTiles)
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
        private bool IsOnTheGivenSide(BoardTile currentTile, BoardTile neighborTile, int diffX, int diffY)
        {
            return currentTile.Coordinates.X + diffX == neighborTile.Coordinates.X &&
                   currentTile.Coordinates.Y + diffY == neighborTile.Coordinates.Y;
        }
    }
}
