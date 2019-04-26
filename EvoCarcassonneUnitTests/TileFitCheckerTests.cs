using System.Collections.Generic;
using System.Collections.ObjectModel;
using EvoCarcassonne.Backend;
using EvoCarcassonne.Controller;
using EvoCarcassonne.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EvoCarcassonneUnitTests
{
    [TestClass]
    public class TileFitCheckerTests
    {
        #region Creating private variables

        private BoardTile boardTile1 = new BoardTile();
        private BoardTile boardTile2 = new BoardTile();
        private BoardTile boardTile3 = new BoardTile();
        private BoardTile boardTile4 = new BoardTile();
        private BoardTile boardTile5 = new BoardTile();
        private BoardTile boardTile6 = new BoardTile();
        private BoardTile boardTile7 = new BoardTile();
        private BoardTile boardTile8 = new BoardTile();
        private BoardTile boardTile9 = new BoardTile();

        #endregion


        [TestMethod]
        public void CheckFitOfTile_TileCanFit_ReturnsTrue()
        {
            #region InitializeValues

            var figure = new Figure(0, new Owner(0, "Krisztian"));


            boardTile1.Coordinates = new Coordinates(0, 0);
            boardTile2.Coordinates = new Coordinates(10, 0);
            boardTile3.Coordinates = new Coordinates(10, 10);
            boardTile4.Coordinates = new Coordinates(10, 20);
            boardTile5.Coordinates = new Coordinates(20, 20);
            boardTile6.Coordinates = new Coordinates(20, 10);
            boardTile7.Coordinates = new Coordinates(30, 10);
            boardTile8.Coordinates = new Coordinates(40, 10);

            var directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            boardTile1.BackendTile = new Tile(1, directions, Speciality.EndOfRoad);

            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            boardTile2.BackendTile = new Tile(2, directions, Speciality.None);

            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            boardTile3.BackendTile = new Tile(3, directions, Speciality.None);

            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            boardTile4.BackendTile = new Tile(4, directions, Speciality.None);

            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Castle(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            boardTile5.BackendTile = new Tile(5, directions, Speciality.EndOfRoad);

            MainController.PlacedBoardTiles = new ObservableCollection<BoardTile>();
            MainController.PlacedBoardTiles.Add(boardTile1);
            MainController.PlacedBoardTiles.Add(boardTile2);
            //MainController.PlacedBoardTiles.Add(boardTile3);
            MainController.PlacedBoardTiles.Add(boardTile4);
            MainController.PlacedBoardTiles.Add(boardTile5);
            MainController.PlacedBoardTiles.Add(boardTile7);
            MainController.PlacedBoardTiles.Add(boardTile8);

            #endregion


            Assert.IsTrue(Utils.CheckFitOfTile(boardTile3));
        }
    }
}