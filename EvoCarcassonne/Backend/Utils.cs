using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using EvoCarcassonne.Controller;
using EvoCarcassonne.Model;

namespace EvoCarcassonne.Backend
{
    public class Utils
    {
        public static List<string> GetResourceNames(string condition)
        {
            var asm = Assembly.GetEntryAssembly();
            var resName = asm.GetName().Name + ".g.resources";
            using (var stream = asm.GetManifestResourceStream(resName))
            using (var reader = new System.Resources.ResourceReader(stream ?? throw new InvalidOperationException()))
            {
                return reader.Cast<DictionaryEntry>().Select(entry => (string)entry.Key).Where(x => x.Contains(condition)).ToList();
            }
        }
        
        
         public static bool CheckFitOfTile(BoardTile boardTile)
        {
            
            Tile backendTile = boardTile.BackendTile;
            
            foreach (var frontEndTile in MainController.PlacedBoardTiles)
            {
                /**
                 * Check the right side of the tile.
                 */
                if (boardTile.Coordinates.X == frontEndTile.Coordinates.X + 10 && boardTile.Coordinates.Y == frontEndTile.Coordinates.Y)
                {
                    if (!backendTile.getTileSideByCardinalDirection(CardinalDirection.East).Landscape
                        .Equals(frontEndTile.BackendTile.getTileSideByCardinalDirection(CardinalDirection.West).Landscape))
                    {
                        return false;
                    }    
                }
                /**
                 * Check the down side of the tile.
                 */
                if (boardTile.Coordinates.Y == frontEndTile.Coordinates.Y + 10 && boardTile.Coordinates.X == frontEndTile.Coordinates.X)
                {
                    if (!backendTile.getTileSideByCardinalDirection(CardinalDirection.South).Landscape
                        .Equals(frontEndTile.BackendTile.getTileSideByCardinalDirection(CardinalDirection.North).Landscape))
                    {
                        return false;
                    }
                }
                /**
                 * Check the left side of the tile.
                 */
                if (boardTile.Coordinates.X == frontEndTile.Coordinates.X - 10 && boardTile.Coordinates.Y == frontEndTile.Coordinates.Y)
                {
                    if (!backendTile.getTileSideByCardinalDirection(CardinalDirection.West).Landscape
                        .Equals(frontEndTile.BackendTile.getTileSideByCardinalDirection(CardinalDirection.East).Landscape))
                    {
                        return false;
                    } 
                }
                /**
                 * Check the upper side of the tile.
                 */
                if (boardTile.Coordinates.Y == frontEndTile.Coordinates.Y - 10 && boardTile.Coordinates.X == frontEndTile.Coordinates.X)
                {
                    if (!backendTile.getTileSideByCardinalDirection(CardinalDirection.North).Landscape
                        .Equals(frontEndTile.BackendTile.getTileSideByCardinalDirection(CardinalDirection.South).Landscape))
                    {
                        return false;
                    }
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

                if (currentTile.Coordinates.Y + 10 == neighborTile.Coordinates.Y && currentTile.Coordinates.X == neighborTile.Coordinates.X)
                {
                    result.Add(CardinalDirection.South, neighborTile);
                }


                if (currentTile.Coordinates.X - 10 == neighborTile.Coordinates.X &&
                    currentTile.Coordinates.Y == neighborTile.Coordinates.Y)
                {
                    result.Add(CardinalDirection.West, neighborTile);
                }


                if (currentTile.Coordinates.Y - 10 == neighborTile.Coordinates.Y && currentTile.Coordinates.X == neighborTile.Coordinates.X)
                {
                    result.Add(CardinalDirection.North, neighborTile);
                }
            }

             return result;
         }
    }
}
