using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Media.Imaging;
using EvoCarcassonne.Models;

namespace EvoCarcassonne.Backend
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
        public ObservableCollection<BoardTile> TileStack { get; }

        private Utils Utils { get; set; }

        public TileParser(Utils utils)
        {
            Utils = utils;
            TileStack = GetTileStack();
        }

        private ObservableCollection<BoardTile> GetTileStack()
        {
            var tileDictionary = GetTileDictionary();
            var tileStack = new ObservableCollection<BoardTile>();

            AddTile(tileStack, tileDictionary.FirstOrDefault(entry => entry.Key.StartsWith("S")));

            foreach (var entry in tileDictionary)
            {
                var tileName = entry.Key;

                for (var i = 0; i < ParseTileCount(tileName); i++)
                {
                    AddTile(tileStack, entry);
                }
            }

            if (tileStack.Count != 72)
            {
                throw new InvalidOperationException();
            }

            return tileStack;
        }

        private void AddTile(ICollection<BoardTile> tileStack, KeyValuePair<string, BitmapImage> entry)
        {
            var tileName = entry.Key;
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

            var tile = new BoardTile(0, null, null, entry.Value, backendTile);

            tileStack.Add(tile);
        }

        private int ParseTileCount(string tileName)
        {
            return Convert.ToInt32(tileName[1].ToString());
        }

        private List<IDirection> ParseTileDirections(string tileName)
        {
            var directions = new List<IDirection>();

            foreach (var c in tileName.Substring(2,4))
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

        private Dictionary<string, BitmapImage> GetTileDictionary()
        {
            var output = new Dictionary<string, BitmapImage>();

            using (var resourceSet = Resources.Tiles.ResourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true))
            {
                foreach (DictionaryEntry entry in resourceSet)
                {
                    output.Add((string) entry.Key, ((Bitmap)entry.Value).ToBitmapImage());
                }
            }

            if (output.Count != 24)
            {
                throw new InvalidOperationException();
            }

            return output;
        }
    }
}
