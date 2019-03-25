﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace EvoCarcassonne.Backend
{
    public class Tile : ITile
    {
        public int TileID { get; set; }
        public List<IDirection> Directions { get; set; }
        public Speciality Speciality { get; set; }


        public Tile(int tileId, List<IDirection> directions, Speciality speciality)
        {
            TileID = tileId;
            Directions = directions;
            Speciality = speciality;
        }

        /**
         * Rotate the tile depending on the given parameter.
         */
        public void Rotate(int direction)
        {
            IDirection temp;
            switch (direction)
            {
                case -90:
                    temp = Directions.First();
                    for (int i = 0; i < Directions.Count - 1; i++)
                    {
                        Directions[i] = Directions[i+1];
                    }

                    Directions[Directions.Count] = temp;
                    break;
                case 90:
                    temp = Directions.Last();
                    for (int i = 0; i < Directions.Count - 1; i++)
                    {
                        Directions[i+1] = Directions[i];
                    }

                    Directions[0] = temp;
                    break;
                default:
                    Console.WriteLine(@"[ERROR] You have given a wrong rotation value!");
                    break;
            }
        }

        /**
         * Returns the queried side of the tile. If wrong side is given, it returns with the North side of the tile.
         */
        public IDirection getTileSideByCardinalDirection(CardinalDirection side)
        {
            switch (side)
            {
                case CardinalDirection.NORTH: return Directions[0];
                case CardinalDirection.EAST: return Directions[1];
                case CardinalDirection.SOUTH: return Directions[2];
                case CardinalDirection.WEST: return Directions[3];
                default: return Directions[0];
            }
        }
    }
}
