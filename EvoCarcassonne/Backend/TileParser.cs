﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using EvoCarcassonne.Model;

namespace EvoCarcassonne.Backend
{
    /*
     * file name format: tileLevel; count; landscape[North,East,South,West]; speciality... .png
     *
     * tileLevel: 0 - normal tile, S - starter tile
     * count: 1 - 9
     * landscape: 0 - field, 1 - road, 2 - castle, 3 - church
     * speciality: 0 - none, 1 - shield, 2 - colostor, 3 - endOfRoad, 4 - endOfCastle
     */

    public static class TileParser
    {
        public static ObservableCollection<BoardTile> GetTileStack()
        {
            var tileStack = new ObservableCollection<BoardTile>();
            var tileResourcePathList = Utils.GetResourceNames(@"tiles");

            AddTile(tileStack, tileResourcePathList.Find(t => Path.GetFileNameWithoutExtension(t).StartsWith("s")));

            foreach (var resourcePath in tileResourcePathList)
            {
                var tileName = Path.GetFileNameWithoutExtension(resourcePath);

                for (var i = 0; i < ParseTileCount(tileName); i++)
                {
                    AddTile(tileStack, resourcePath);
                }
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
                backendTile = new Church(-1, ParseTileDirections(tileName), tileSpecialities);
            }
            else
            {
                backendTile = new Tile(-1, ParseTileDirections(tileName), tileSpecialities);
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

            foreach (var c in tileName.Substring(2,4))
            {
                var direction = new Direction(-1, ParseLandscape(c), null, null);

                directions.Add(direction);
            }

            return directions;
        }

        private static List<Speciality> ParseTileSpecialities(string tileName)
        {
            var specialities = new List<Speciality>();
            var specialitySubstring = tileName.Substring(5);

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
                /* case '3':
                    landscape = new Church();
                    break; */
            }

            return landscape;
        }

        #endregion

    }
}
