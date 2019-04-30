using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using EvoCarcassonne.Model;

namespace EvoCarcassonne.Backend
{
    //
    // abcdef.png
    // a=landscapeNorth b=landscapeEast c=landscapeSouth d=landscapeWest e=speciality f=count
    // landscape: 0-road, 1-field, 2-castle
    // speciality: 0-none, 1-shield, 2-colostor
    // count: int, 1 <= x
    //

    public class TileParser
    {
        public static ObservableCollection<BoardTile> GetTileStack()
        {
            var cardStack = new ObservableCollection<BoardTile>();
            var tileNameList = Utils.GetResourceNames(@"tiles");

            AddToCardStack(cardStack, tileNameList.Find(n => n.Contains("201003")));

            foreach (var name in tileNameList)
            {
                for (var i = 0; i < int.Parse(Path.GetFileNameWithoutExtension(name)[5].ToString()); i++)
                {
                    AddToCardStack(cardStack, name);
                }
            }

            /*
            var random = new Random();
            while (cardStack.Count < 100)
            {
                AddToCardStack(cardStack, tileNameList[random.Next(tileNameList.Count)]);
            }
            */

            return cardStack;
        }

        private static void AddToCardStack(ICollection<BoardTile> cardStack, string name)
        {
            var nameWithoutExtension = Path.GetFileNameWithoutExtension(name);
            cardStack.Add(new BoardTile
            {
                Tag = null,
                Coordinates = null,
                Image = name,
                Angle = 0,
                BackendTile = new Tile(-1, ParseDirection(nameWithoutExtension.Substring(0, 4)),
                    ParseSpeciality(nameWithoutExtension[4]))
            });
        }

        private static List<IDirection> ParseDirection(string name)
        {
            var direction = new List<IDirection>();

            foreach (var n in name)
            {
                direction.Add(new Direction()
                {
                    Landscape = ParseLandscape(n),
                    Id = -1,
                    Figure = null,
                    Neighbor = null
                });
            }

            return direction;
        }

        private static Speciality ParseSpeciality(char speciality)
        {
            switch (speciality)
            {
                default:
                    return Speciality.None;
                case '1':
                    return Speciality.Shield;
                case '2':
                    return Speciality.Colostor;
            }
        }

        private static Landscape ParseLandscape(int landscape)
        {
            switch (landscape)
            {
                default:
                    return Landscape.Road;
                case '1':
                    return Landscape.Field;
                case '2':
                    return Landscape.Castle;
            }
        }
    }
}
