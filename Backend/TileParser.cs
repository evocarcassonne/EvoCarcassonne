﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using ConsoleApplication1;
using Newtonsoft.Json;

namespace Backend
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
        public ObservableCollection<ITile> TileStack { get; }

        public TileParser()
        {
            TileStack = GetTileStack();
        }

        public TileParser(List<string> resourcesNames)
        {
            TileStack = GetTileStack(resourcesNames);
        }
        
        #region Private methods

        private ObservableCollection<ITile> GetTileStack()
        {
            var tileStack = new ObservableCollection<ITile>();
            JsonReaderObject items;
            using (StreamReader r = new StreamReader(GetResourceNames("tiles") + "\file.json"))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<JsonReaderObject>(json);
            }

            var tilesFromJson = items.carcassonne.Where(type => type.gametype == "default").Select(e => e.defaultTiles).FirstOrDefault();

            AddTile(tileStack,
                tilesFromJson?.Find(name => name.StartsWith("s")));

            if (tilesFromJson != null)
            {
                foreach (var tileName in tilesFromJson)
                {
                    for (var i = 0; i < ParseTileCount(tileName); i++)
                    {
                        AddTile(tileStack, tileName);
                    }
                }
            }
            return tileStack;
        }

        private ObservableCollection<ITile> GetTileStack(List<string> resourcesNames)
        {
            var tileStack = new ObservableCollection<ITile>();

            /*var assemblyPath = Assembly.GetExecutingAssembly().Location;

            // 4 jegyzékkel fentebb megy, dirty hack
            for (int i = 0; i < 4; i++)
            {
                assemblyPath = Path.GetDirectoryName(assemblyPath);
            }*/
            
            JsonReaderObject items;
            using (StreamReader r = new StreamReader(GetResourceNames("tiles") + "\file.json"))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<JsonReaderObject>(json);
            }

            var tilesFromJson = items.carcassonne.Where(type => type.gametype == "default").Select(e => e.defaultTiles).FirstOrDefault();


            if (tilesFromJson != null)
            {
                foreach (var tileName in tilesFromJson)
                {
                    for (var i = 0; i < ParseTileCount(tileName); i++)
                    {
                        AddTile(tileStack, tileName);
                    }
                }
            }

            return tileStack;
        }
        
        private void AddTile(ICollection<ITile> tileStack, string tileName)
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
            tileStack.Add(backendTile);
        }

        private int ParseTileCount(string tileName)
        {
            return Convert.ToInt32(tileName[1].ToString());
        }

        private List<IDirection> ParseTileDirections(string tileName)
        {
            var directions = new List<IDirection>();

            foreach (var c in tileName.Substring(2, 4))
            {
                var direction = new Direction(ParseLandscape(c), null);

                directions.Add(direction);
            }

            return directions;
        }

        private List<Speciality> ParseTileSpecialities(string tileName)
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

        private ILandscape ParseLandscape(char landscapeCharacter)
        {
            ILandscape landscape;

            switch (landscapeCharacter)
            {
                default:
                    landscape = new Field();
                    break;
                case '1':
                    landscape = new Road();
                    break;
                case '2':
                    landscape = new Castle();
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