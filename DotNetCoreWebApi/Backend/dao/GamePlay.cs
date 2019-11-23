using DotNetCoreWebApi.Backend.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DotNetCoreWebApi.Backend.dao
{
    public class GamePlay
    {
        public Guid Id { get; set; }
        [JsonProperty]
        public List<ITile> TileStack { get; set; }
        public List<ITile> PlacedTiles { get; set; } = new List<ITile>();
        public List<Player> Players = new List<Player>();
        public int CurrentRound { get; set; } = 1;
        public Player CurrentPlayer { get; set; }
        public bool AlreadyCalculated { get; set; }
        public Random RandomNumberGenerator { get; set; }
        public bool TileIsDown { get; set; } = false;
        public bool HasCurrentTile { get; set; } = false;
        public bool CanPlaceFigureProperty { get; set; } = false;
        public int CurrentSideForFigure { get; set; } = -1;
        public bool FigureDown { get; set; } = false;

        public GamePlay(List<ITile> tileStack)
        {
            Id = Guid.NewGuid();

            if (TileStack == null || TileStack.Count == 0)
            {
                TileStack = tileStack;

                var starterTile = TileStack.RemoveAndGet(0);
                starterTile.Position = new Coordinates(5, 5);
                CanPlaceFigureProperty = false;
                PlacedTiles.Add(starterTile);
            }

            RandomNumberGenerator = new Random();
        }
    }
}