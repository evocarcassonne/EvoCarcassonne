using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EvoCarcassonne.Backend;
using EvoCarcassonne.Controller;
using EvoCarcassonne.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EvoCarcassonneUnitTests
{
    [TestClass]
    public class RoadCalculatorTest
    {
        private BoardTile boardTile1 = new BoardTile();
        private BoardTile boardTile2 = new BoardTile();
        private BoardTile boardTile3 = new BoardTile();
        private BoardTile boardTile4 = new BoardTile();
        private BoardTile boardTile5 = new BoardTile();
        private BoardTile boardTile6 = new BoardTile();
        private BoardTile boardTile7 = new BoardTile();
        private BoardTile boardTile8 = new BoardTile();
        private BoardTile boardTile9 = new BoardTile();


        [TestMethod]
        public void CalculateRoad_RoadIsFinished_ReturnsNumberOfPoints_TestCase1()
        {
            #region InitializeValues

            var figure = new Figure(0, new Owner(0, "Krisztian"));

            var specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);

            var directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));

            boardTile1.Coordinates = new Coordinates(0, 0);
            boardTile2.Coordinates = new Coordinates(10, 0);
            boardTile3.Coordinates = new Coordinates(10, 10);
            boardTile4.Coordinates = new Coordinates(10, 20);
            boardTile5.Coordinates = new Coordinates(20, 20);
            boardTile6.Coordinates = new Coordinates(20, 10);
            boardTile7.Coordinates = new Coordinates(30, 10);
            boardTile8.Coordinates = new Coordinates(40, 10);

            boardTile1.BackendTile = new Tile(1, directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            boardTile2.BackendTile = new Tile(2, directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            boardTile3.BackendTile = new Tile(3, directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            boardTile4.BackendTile = new Tile(4, directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            boardTile5.BackendTile = new Tile(5, directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            boardTile6.BackendTile = new Tile(6, directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            boardTile7.BackendTile = new Tile(7, directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            boardTile8.BackendTile = new Tile(8, directions, specialities);

            MainController.PlacedBoardTiles = new ObservableCollection<BoardTile>();
            MainController.PlacedBoardTiles.Add(boardTile1);
            MainController.PlacedBoardTiles.Add(boardTile2);
            MainController.PlacedBoardTiles.Add(boardTile3);
            MainController.PlacedBoardTiles.Add(boardTile4);
            MainController.PlacedBoardTiles.Add(boardTile5);
            MainController.PlacedBoardTiles.Add(boardTile6);
            MainController.PlacedBoardTiles.Add(boardTile7);
            MainController.PlacedBoardTiles.Add(boardTile8);

            #endregion

            Console.WriteLine(@"Route to the other end of road:");
            var result = boardTile1.BackendTile.Directions[1].Landscape
                .calculate(boardTile1, CardinalDirection.East, true);
            Console.WriteLine(@"Ez az eredmeny    " + result);

            Assert.IsTrue(result == 6);
        }

        [TestMethod]
        public void CalculateRoad_RoadIsFinished_ReturnsNumberOfPoints_TestCase2()
        {
            #region InitializeValues

            var figure = new Figure(0, new Owner(0, "Krisztian"));

            var specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);

            var directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));


            boardTile1.Coordinates = new Coordinates(0, 0);
            boardTile2.Coordinates = new Coordinates(10, 0);
            boardTile3.Coordinates = new Coordinates(10, 10);
            boardTile4.Coordinates = new Coordinates(10, 20);
            boardTile5.Coordinates = new Coordinates(20, 20);
            boardTile6.Coordinates = new Coordinates(20, 10);
            boardTile7.Coordinates = new Coordinates(30, 10);
            boardTile8.Coordinates = new Coordinates(40, 10);

            boardTile1.BackendTile = new Tile(1, directions, specialities);


            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            boardTile2.BackendTile = new Tile(2, directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            boardTile3.BackendTile = new Tile(3, directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            boardTile4.BackendTile = new Tile(4, directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            boardTile5.BackendTile = new Tile(5, directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            boardTile6.BackendTile = new Tile(6, directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            boardTile7.BackendTile = new Tile(7, directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            boardTile8.BackendTile = new Tile(8, directions, specialities);

            MainController.PlacedBoardTiles = new ObservableCollection<BoardTile>();
            MainController.PlacedBoardTiles.Add(boardTile1);
            MainController.PlacedBoardTiles.Add(boardTile2);
            MainController.PlacedBoardTiles.Add(boardTile3);
            MainController.PlacedBoardTiles.Add(boardTile4);
            MainController.PlacedBoardTiles.Add(boardTile5);
            MainController.PlacedBoardTiles.Add(boardTile6);
            MainController.PlacedBoardTiles.Add(boardTile7);
            MainController.PlacedBoardTiles.Add(boardTile8);

            #endregion

            Console.WriteLine(@"Route to the other end of road:");
            var result = boardTile1.BackendTile.Directions[1].Landscape
                .calculate(boardTile1, CardinalDirection.East, true);
            Console.WriteLine(@"Ez az eredmeny    " + result);

            Assert.IsTrue(result == 5);
        }

        [TestMethod]
        public void CalculateRoad_RoadIsFinished_ReturnsNumberOfPoints_TestCase3()
        {
            #region InitializeValues

            var figure = new Figure(0, new Owner(0, "Krisztian"));

            var directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));


            boardTile1.Coordinates = new Coordinates(0, 0);
            boardTile2.Coordinates = new Coordinates(10, 0);
            boardTile3.Coordinates = new Coordinates(10, 10);
            boardTile4.Coordinates = new Coordinates(10, 20);
            boardTile5.Coordinates = new Coordinates(20, 20);
            boardTile6.Coordinates = new Coordinates(20, 10);
            boardTile7.Coordinates = new Coordinates(30, 10);
            boardTile8.Coordinates = new Coordinates(40, 10);

            var specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);

            boardTile1.BackendTile = new Tile(1, directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            boardTile2.BackendTile = new Tile(2, directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            boardTile3.BackendTile = new Tile(3, directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            boardTile4.BackendTile = new Tile(4, directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            boardTile5.BackendTile = new Tile(5, directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            boardTile6.BackendTile = new Tile(6, directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            boardTile7.BackendTile = new Tile(7, directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            boardTile8.BackendTile = new Tile(8, directions, specialities);

            MainController.PlacedBoardTiles = new ObservableCollection<BoardTile>();
            MainController.PlacedBoardTiles.Add(boardTile1);
            MainController.PlacedBoardTiles.Add(boardTile2);
            MainController.PlacedBoardTiles.Add(boardTile3);
            MainController.PlacedBoardTiles.Add(boardTile4);
            MainController.PlacedBoardTiles.Add(boardTile5);
            MainController.PlacedBoardTiles.Add(boardTile6);
            MainController.PlacedBoardTiles.Add(boardTile7);
            MainController.PlacedBoardTiles.Add(boardTile8);

            #endregion

            Console.WriteLine(@"Route to the other end of road:");
            var result = boardTile1.BackendTile.Directions[1].Landscape
                .calculate(boardTile2, CardinalDirection.East, true);
            Console.WriteLine(@"Ez az eredmeny    " + result);

            Assert.IsTrue(result == 0);
        }

        [TestMethod]
        public void CalculateRoad_RoadIsFinished_ReturnsNumberOfPoints_TestCase4()
        {
            #region InitializeValues

            var figure = new Figure(0, new Owner(0, "Krisztian"));

            var directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));


            boardTile1.Coordinates = new Coordinates(0, 0);
            boardTile2.Coordinates = new Coordinates(10, 0);
            boardTile3.Coordinates = new Coordinates(10, 10);
            boardTile4.Coordinates = new Coordinates(10, 20);
            boardTile5.Coordinates = new Coordinates(20, 20);
            boardTile6.Coordinates = new Coordinates(20, 10);
            boardTile7.Coordinates = new Coordinates(30, 10);
            boardTile8.Coordinates = new Coordinates(40, 10);
            boardTile9.Coordinates = new Coordinates(0, 10);

            var specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);
            boardTile1.BackendTile = new Tile(1, directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            boardTile2.BackendTile = new Tile(2, directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            boardTile3.BackendTile = new Tile(3, directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            boardTile4.BackendTile = new Tile(4, directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            boardTile5.BackendTile = new Tile(5, directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            boardTile6.BackendTile = new Tile(6, directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            boardTile7.BackendTile = new Tile(7, directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            boardTile8.BackendTile = new Tile(8, directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            boardTile9.BackendTile = new Tile(9, directions, specialities);

            MainController.PlacedBoardTiles = new ObservableCollection<BoardTile>();
            MainController.PlacedBoardTiles.Add(boardTile1);
            MainController.PlacedBoardTiles.Add(boardTile2);
            MainController.PlacedBoardTiles.Add(boardTile3);
            MainController.PlacedBoardTiles.Add(boardTile4);
            MainController.PlacedBoardTiles.Add(boardTile5);
            MainController.PlacedBoardTiles.Add(boardTile6);
            MainController.PlacedBoardTiles.Add(boardTile7);
            MainController.PlacedBoardTiles.Add(boardTile8);
            MainController.PlacedBoardTiles.Add(boardTile9);

            #endregion

            Console.WriteLine(@"Route to the other end of road:");
            var result = boardTile1.BackendTile.Directions[1].Landscape
                .calculate(boardTile1, CardinalDirection.East, true);
            Console.WriteLine(@"Ez az eredmeny    " + result);

            Assert.IsTrue(result == 4);
        }
    }
}