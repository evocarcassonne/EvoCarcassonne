using System;
using System.Collections.Generic;
using EvoCarcassonne.Backend;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EvoCarcassonneUnitTests
{
    [TestClass]
    public class TileTests
    {
        private ITile tile;

        [TestInitialize]
        public void Initialize()
        {
            List<IDirection> directions = new List<IDirection>();
            List<Speciality> specialities = new List<Speciality>();
            Figure figure = new Figure(0, new Owner(0, "Pista"));

            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(1, new Road(), figure));
            directions.Add(new Direction(2, new Road(), figure));
            directions.Add(new Direction(3, new Castle(), figure));
            this.tile = new Tile(0, directions, specialities);
        }

        [TestMethod]
        public void Rotate_RotateLeft_ReturnsVoid()
        {
            Console.WriteLine(tile.ToString());
            tile.Rotate(-90);
            Console.WriteLine(tile.ToString());
            Assert.IsTrue(tile.Directions[0].Landscape is Road);
            Assert.IsTrue(tile.Directions[1].Landscape is Road);
            Assert.IsTrue(tile.Directions[2].Landscape is Castle);
            Assert.IsTrue(tile.Directions[3].Landscape is Castle);
        }

        [TestMethod]
        public void Rotate_RotateRight_ReturnsVoid()
        {
            Console.WriteLine(tile.ToString());
            tile.Rotate(90);
            Console.WriteLine(tile.ToString());
            Assert.IsTrue(tile.Directions[2].Landscape is Road);
            Assert.IsTrue(tile.Directions[3].Landscape is Road);
            Assert.IsTrue(tile.Directions[0].Landscape is Castle);
            Assert.IsTrue(tile.Directions[1].Landscape is Castle);
        }

        [TestMethod]
        public void Rotate_RotateTwiceRight_ReturnsVoid()
        {
            Console.WriteLine(tile.ToString());
            tile.Rotate(90);
            tile.Rotate(90);
            Console.WriteLine(tile.ToString());
            Assert.IsTrue(tile.Directions[0].Landscape is Road);
            Assert.IsTrue(tile.Directions[1].Landscape is Castle);
            Assert.IsTrue(tile.Directions[2].Landscape is Castle);
            Assert.IsTrue(tile.Directions[3].Landscape is Road);

        }
    }
}
