using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using EvoCarcassonne.Backend;
using EvoCarcassonne.Controller;
using EvoCarcassonne.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EvoCarcassonneUnitTests
{
    [TestClass]
    public class RoadFinishedTests
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

        [TestInitialize]
        public void Init()
        {
            var figure = new Figure(0, new Owner(0, "Krisztian"));
            
            boardTile1.Coordinates = new Coordinates(0, 0);
            boardTile2.Coordinates = new Coordinates(1, 0);
            boardTile3.Coordinates = new Coordinates(1, 1);
            boardTile4.Coordinates = new Coordinates(1, 2);
            boardTile5.Coordinates = new Coordinates(2, 2);
            boardTile6.Coordinates = new Coordinates(2, 1);
            boardTile7.Coordinates = new Coordinates(3, 1);
            boardTile8.Coordinates = new Coordinates(4, 1);
            boardTile9.Coordinates = new Coordinates(0, 1);


            var specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);

            var directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            boardTile1.BackendTile = new Tile(1, directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Road(), figure));
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

        }

        [TestMethod]
        public void Test()
        {
            MainController.PlacedBoardTiles.RemoveAndGet(5);
            Assert.IsTrue(Utils.IsRoadFinishedGivenDirection(boardTile6, CardinalDirection.East));
        }
        
        [TestMethod]
        public void Test2()
        {
            MainController.PlacedBoardTiles.RemoveAndGet(5);
            MainController.PlacedBoardTiles.RemoveAndGet(6);
            Dictionary<CardinalDirection, bool> result = Utils.IsFinishedRoad(boardTile6);
            bool condition;
            result.TryGetValue(CardinalDirection.West, out condition);
            Assert.IsTrue(condition);
            result.TryGetValue(CardinalDirection.East, out condition);
            Assert.IsFalse(condition);
        }
        
        [TestMethod]
        public void Test3()
        {
            MainController.PlacedBoardTiles.RemoveAndGet(5);
            BoardTile resultTile = Utils.SearchEndOfRoadTileInGivenDirection(boardTile6, CardinalDirection.West);
            Assert.AreEqual(resultTile, boardTile1);
        }
        
    }
}