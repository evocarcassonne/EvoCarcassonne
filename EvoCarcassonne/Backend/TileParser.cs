using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
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

        public TileParser()
        {
            TileStack = GetTileStack();
        }

        private ObservableCollection<BoardTile> GetTileStack()
        {
            var tileStack = new ObservableCollection<BoardTile>();
            var tileResourcePathList = GetResourceNames("tiles");


            AddTile(tileStack,
                tileResourcePathList.Find(name => Path.GetFileNameWithoutExtension(name).StartsWith("s")));

            foreach (var resourcePath in tileResourcePathList)
            {
                var tileName = Path.GetFileNameWithoutExtension(resourcePath);

                for (var i = 0; i < ParseTileCount(tileName); i++)
                {
                    AddTile(tileStack, resourcePath);
                }
            }

            if (tileStack.Count != 72)
            {
                throw new InvalidOperationException();
            }

            return tileStack;
        }

       
        #region Private methods

        private static void AddTile(ICollection<BoardTile> tileStack, string resourcePath)
        {
            var tileName = Path.GetFileNameWithoutExtension(resourcePath);
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

            var tile = new BoardTile(0, null, null, resourcePath, backendTile);

            tileStack.Add(tile);
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

        private static ILandscape ParseLandscape(char landscapeCharacter)
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

        public List<string> GetResourceNames(string condition)
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
