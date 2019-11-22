using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using DotNetCoreWebApi.Backend.Model;
using Newtonsoft.Json;

namespace DotNetCoreWebApi.Backend
{
    /*
     * file name format: tileLevel; count; landscape[North,East,South,West]; speciality... .png
     *
     * tileLevel: T - normal tile, S - starter tile
     * count: 1 - 9
     * landscape: 0 - field, 1 - road, 2 - castle, 3 - church
     * speciality: 0 - none, 1 - shield, 2 - colostor, 3 - endOfRoad, 4 - endOfCastle
     */

    public class TileParser
    {
        public List<ITile> TileStack { get; set; }


        public TileParser()
        {
            TileStack = GetTileStack();
        }

        public TileParser(List<string> resourcesNames)
        {
            TileStack = GetTileStack(resourcesNames);
        }

        public static ITile Parse(string tileToParse)
        {
            ITile result;
            if (ParseTileSpecialities(tileToParse).Contains(Speciality.Colostor))
            {
                result = new Church(ParseTileDirections(tileToParse), ParseTileSpecialities(tileToParse));
            }
            else
            {
                result = new Tile(ParseTileDirections(tileToParse), ParseTileSpecialities(tileToParse));
            }

            result.PropertiesAsString = tileToParse;
            return result;
        }

        #region Private methods

        private static List<ITile> GetTileStack()
        {
            var tileStack = new List<ITile>();
            var defaultTiles = new List<string>();
            JsonReaderObject items;
            using (StreamReader r = new StreamReader("TileDefinitions.json"))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<JsonReaderObject>(json);
            }

            defaultTiles = items.carcassonne.Where(type => type.gametype == "default").Select(e => e.defaultTiles).FirstOrDefault();

            if (defaultTiles != null)
            {
                string firstTile = defaultTiles.Find(e => e.StartsWith("S"));
                AddTile(tileStack, firstTile);
                foreach (var tileName in defaultTiles)
                {
                    for (var i = 1; i < ParseTileCount(tileName); i++)
                    {
                        AddTile(tileStack, tileName);
                    }
                }
            }

            return tileStack;
        }

        private static List<ITile> GetTileStack(List<string> resourcesNames)
        {
            var tileStack = new List<ITile>();

            if (resourcesNames != null)
            {
                foreach (var tileName in resourcesNames)
                {
                    for (var i = 0; i < ParseTileCount(tileName); i++)
                    {
                        AddTile(tileStack, tileName);
                    }
                }
            }

            return tileStack;
        }

        private static void AddTile(ICollection<ITile> tileStack, string tileName)
        {
            var tileSpecialities = ParseTileSpecialities(tileName);

            ITile backendTile;
            if (tileSpecialities.Contains(Speciality.Colostor))
            {
                backendTile = new Church(ParseTileDirections(tileName), tileSpecialities);
            }
            else
            {
                backendTile = new Tile(ParseTileDirections(tileName), tileSpecialities);
            }

            backendTile.PropertiesAsString = tileName;
            tileStack.Add(backendTile);
        }

        private static int ParseTileCount(string tileName)
        {
            return Convert.ToInt32(tileName[1].ToString());
        }

        private static List<IDirection> ParseTileDirections(string tileName)
        {
            var directions = new List<IDirection>();

            foreach (var c in tileName.Substring(2, 4))
            {
                var direction = new Direction(ParseLandscape(c), null);

                directions.Add(direction);
            }

            return directions;
        }

        private static List<Speciality> ParseTileSpecialities(string tileName)
        {
            var specialities = new List<Speciality>();
            var specialitySubstring = tileName.Substring(6);

            foreach (var c in specialitySubstring)
            {
                Speciality speciality;

                switch (c)
                {
                    default:
                        speciality = Speciality.None;
                        break;
                    case '1':
                        speciality = Speciality.Shield;
                        break;
                    case '2':
                        speciality = Speciality.Colostor;
                        break;
                    case '3':
                        speciality = Speciality.EndOfRoad;
                        break;
                    case '4':
                        speciality = Speciality.EndOfCastle;
                        break;
                }

                specialities.Add(speciality);
            }

            return specialities;
        }

        private static Landscape ParseLandscape(char landscapeCharacter)
        {
            Landscape landscape;

            switch (landscapeCharacter)
            {
                default:
                    landscape = Landscape.Field;
                    break;
                case '1':
                    landscape = Landscape.Road;
                    break;
                case '2':
                    landscape = Landscape.Castle;
                    break;
            }

            return landscape;
        }

        private List<string> GetResourceNames(string condition)
        {
            var asm = Assembly.GetEntryAssembly();
            var resName = asm.GetName().Name + ".g.resources";
            using (var stream = asm.GetManifestResourceStream(resName))
            using (var reader = new System.Resources.ResourceReader(stream ?? throw new InvalidOperationException()))
            {
                return reader.Cast<DictionaryEntry>().Select(entry => "/" + (string)entry.Key)
                    .Where(x => x.Contains(condition)).ToList();
            }
        }
        #endregion
    }
}
