using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using EvoCarcassonne.Backend;
using EvoCarcassonne.Models;
using EvoCarcassonne.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EvoCarcassonneUnitTests
{
    [TestClass]
    public class FieldCalculatorTest
    {
        private BoardTile boardTile1 = new BoardTile();
        
        [TestInitialize]
        public void Init()
        {
            boardTile1.Coordinates = new Coordinates(0, 0);
            var specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);

            var directions = new List<IDirection>();
            directions.Add(new Direction(new Field(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Castle(), null));
            boardTile1.BackendTile = new Tile(directions, specialities);
        }

        [TestMethod]
        public void test1()
        {
            boardTile1.BackendTile.Directions[0].Landscape.calculate(boardTile1, false, new Utils(new MainController()));
            boardTile1.BackendTile.Directions[0].Landscape.CanPlaceFigure(boardTile1, CardinalDirection.East, new Utils(new MainController()), false);
        }
    }
}
