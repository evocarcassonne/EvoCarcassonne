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
        private MainController _mainController = new MainController();
        private Utils _utils;
        #endregion

        [TestInitialize]
        public void Initialize()
        {
            #region InitializeValues

            _utils = new Utils(_mainController);
            IEnumerable<Player> players = new List<Player>();
            Player Player1 = new Player("Krisz", Brushes.Red);
            players.Append(Player1);
            _mainController.Players.Add(Player1);
            var figure = new Figure(Player1.BackendOwner);

            boardTile1.Coordinates = new Coordinates(0, 0);
            boardTile2.Coordinates = new Coordinates(1, 0);
            boardTile3.Coordinates = new Coordinates(1, 1);
            boardTile4.Coordinates = new Coordinates(1, 2);
            boardTile5.Coordinates = new Coordinates(2, 2);
            boardTile6.Coordinates = new Coordinates(2, 1);
            boardTile7.Coordinates = new Coordinates(3, 1);
            boardTile8.Coordinates = new Coordinates(4, 1);


            var specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);
            var directions = new List<IDirection>();
            directions.Add(new Direction(new Castle(), figure));
            directions.Add(new Direction(new Road(), figure));
            directions.Add(new Direction(new Castle(), figure));
            directions.Add(new Direction(new Castle(), figure));
            boardTile1.BackendTile = new Tile(directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            directions = new List<IDirection>();
            directions.Add(new Direction(new Castle(), figure));
            directions.Add(new Direction(new Road(), figure));
            directions.Add(new Direction(new Road(), figure));
            directions.Add(new Direction(new Castle(), figure));
            boardTile2.BackendTile = new Tile(directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(new Road(), figure));
            directions.Add(new Direction(new Castle(), figure));
            directions.Add(new Direction(new Road(), figure));
            directions.Add(new Direction(new Castle(), figure));
            boardTile3.BackendTile = new Tile(directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(new Road(), figure));
            directions.Add(new Direction(new Road(), figure));
            directions.Add(new Direction(new Castle(), figure));
            directions.Add(new Direction(new Castle(), figure));
            boardTile4.BackendTile = new Tile(directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);
            directions = new List<IDirection>();
            directions.Add(new Direction(new Castle(), figure));
            directions.Add(new Direction(new Castle(), figure));
            directions.Add(new Direction(new Castle(), figure));
            directions.Add(new Direction(new Road(), figure));
            boardTile5.BackendTile = new Tile(directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            directions = new List<IDirection>();
            directions.Add(new Direction(new Castle(), figure));
            directions.Add(new Direction(new Road(), figure));
            directions.Add(new Direction(new Castle(), figure));
            directions.Add(new Direction(new Road(), figure));
            boardTile6.BackendTile = new Tile(directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(new Castle(), figure));
            directions.Add(new Direction(new Road(), figure));
            directions.Add(new Direction(new Castle(), figure));
            directions.Add(new Direction(new Road(), figure));
            boardTile7.BackendTile = new Tile(directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);
            directions = new List<IDirection>();
            directions.Add(new Direction(new Castle(), figure));
            directions.Add(new Direction(new Castle(), figure));
            directions.Add(new Direction(new Castle(), figure));
            directions.Add(new Direction(new Road(), figure));
            boardTile8.BackendTile = new Tile(directions, specialities);

            #endregion
        }

        [TestMethod]
        public void CheckFitOfTile_TileCanFit_ReturnsTrue_TestCase1()
        {
            
            _mainController.PlacedBoardTiles = new ObservableCollection<BoardTile>();
            _mainController.PlacedBoardTiles.Add(boardTile1);
            _mainController.PlacedBoardTiles.Add(boardTile2);
            _mainController.PlacedBoardTiles.Add(boardTile3);
            _mainController.PlacedBoardTiles.Add(boardTile4);

            Assert.IsTrue(_utils.CheckFitOfTile(boardTile3));
        }

        [TestMethod]
        public void CheckFitOfTile_TileCanFit_ReturnsTrue_TestCase2()
        {
            var figure = new Figure(new Owner("Krisztian"));

            var specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            List<IDirection> directions = new List<IDirection>();
            directions.Add(new Direction(new Road(), figure));
            directions.Add(new Direction(new Road(), figure));
            directions.Add(new Direction(new Castle(), figure));
            directions.Add(new Direction(new Castle(), figure));
            boardTile3.BackendTile = new Tile(directions, specialities);

            _mainController.PlacedBoardTiles = new ObservableCollection<BoardTile>();
            _mainController.PlacedBoardTiles.Add(boardTile1);
            _mainController.PlacedBoardTiles.Add(boardTile2);
            _mainController.PlacedBoardTiles.Add(boardTile3);
            _mainController.PlacedBoardTiles.Add(boardTile5);
            _mainController.PlacedBoardTiles.Add(boardTile6);
            _mainController.PlacedBoardTiles.Add(boardTile7);

            Assert.IsTrue(_utils.CheckFitOfTile(boardTile6));
        }

        [TestMethod]
        public void CheckFitOfTile_TileCanFit_ReturnsFalse_TestCase1()
        {
            var figure = new Figure(new Owner("Krisztian"));

            var specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            List<IDirection> directions = new List<IDirection>();
            directions.Add(new Direction(new Road(), figure));
            directions.Add(new Direction(new Castle(), figure));
            directions.Add(new Direction(new Road(), figure));
            directions.Add(new Direction(new Castle(), figure));
            boardTile3.BackendTile = new Tile(directions, specialities);

            _mainController.PlacedBoardTiles = new ObservableCollection<BoardTile>();
            _mainController.PlacedBoardTiles.Add(boardTile1);
            _mainController.PlacedBoardTiles.Add(boardTile2);
            _mainController.PlacedBoardTiles.Add(boardTile3);
            _mainController.PlacedBoardTiles.Add(boardTile5);
            _mainController.PlacedBoardTiles.Add(boardTile6);
            _mainController.PlacedBoardTiles.Add(boardTile7);

            Assert.IsFalse(_utils.CheckFitOfTile(boardTile6));
        }

        [TestMethod]
        public void CheckFitOfTile_TileCanFit_ReturnsFalse_TestCase2()
        {
            
            _mainController.PlacedBoardTiles = new ObservableCollection<BoardTile>();
            _mainController.PlacedBoardTiles.Add(boardTile1);
            _mainController.PlacedBoardTiles.Add(boardTile2);
            _mainController.PlacedBoardTiles.Add(boardTile3);
            _mainController.PlacedBoardTiles.Add(boardTile5);
            _mainController.PlacedBoardTiles.Add(boardTile6);
            _mainController.PlacedBoardTiles.Add(boardTile7);
            _mainController.PlacedBoardTiles.Add(boardTile8);

            Assert.IsTrue(_utils.CheckFitOfTile(boardTile8));
        }
 [TestMethod]
        public void CheckFitOfTile_ChurchCanFit_ReturnsFalse_TestCase1()
        {
            _mainController.PlacedBoardTiles = new ObservableCollection<BoardTile>();
            _mainController.PlacedBoardTiles.Add(boardTile1);
            _mainController.PlacedBoardTiles.Add(boardTile2);
            _mainController.PlacedBoardTiles.Add(boardTile3);
            _mainController.PlacedBoardTiles.Add(boardTile5);
            _mainController.PlacedBoardTiles.Add(boardTile6);
            _mainController.PlacedBoardTiles.Add(boardTile7);
            
            var specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);
            var directions = new List<IDirection>();
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), null));
            
            boardTile8.BackendTile = new Church(directions, specialities);
            _mainController.PlacedBoardTiles.Add(boardTile8);

            Assert.IsTrue(_utils.CheckFitOfTile(boardTile8));
        }

    }
}