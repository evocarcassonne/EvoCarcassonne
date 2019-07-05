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
            Figure figure = new Figure(new Owner("Pista"));

            directions.Add(new Direction(new Castle(), figure));
            directions.Add(new Direction(new Road(), figure));
            directions.Add(new Direction(new Road(), figure));
            directions.Add(new Direction(new Castle(), figure));
            this.tile = new Tile(directions, specialities);
        }

        [TestMethod]
        public void Rotate_RotateLeft_ReturnsVoid()
        {
            tile.Rotate(-90);
            Assert.IsTrue(tile.Directions[0].Landscape is Road);
            Assert.IsTrue(tile.Directions[1].Landscape is Road);
            Assert.IsTrue(tile.Directions[2].Landscape is Castle);
            Assert.IsTrue(tile.Directions[3].Landscape is Castle);
        }

        [TestMethod]
        public void Rotate_RotateRight_ReturnsVoid()
        {
            tile.Rotate(90);
            Assert.IsTrue(tile.Directions[2].Landscape is Road);
            Assert.IsTrue(tile.Directions[3].Landscape is Road);
            Assert.IsTrue(tile.Directions[0].Landscape is Castle);
            Assert.IsTrue(tile.Directions[1].Landscape is Castle);
        }

        [TestMethod]
        public void Rotate_RotateTwiceRight_ReturnsVoid()
        {
            tile.Rotate(90);
            tile.Rotate(90);
            Assert.IsTrue(tile.Directions[0].Landscape is Road);
            Assert.IsTrue(tile.Directions[1].Landscape is Castle);
            Assert.IsTrue(tile.Directions[2].Landscape is Castle);
            Assert.IsTrue(tile.Directions[3].Landscape is Road);

        }
    }
}
