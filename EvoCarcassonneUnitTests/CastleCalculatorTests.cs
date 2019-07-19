using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class CastleCalculatorTests
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
        private Player Player1 = new Player("Krisztian", Brushes.Lime);
        private Player Player2 = new Player("Pista", Brushes.Gold);
        private Player Player3 = new Player("Karcsi", Brushes.Red);
        private MainController _mainController = new MainController();
        private Utils _utils;

        [TestInitialize]
        public void Init()
        {
            _utils = new Utils(_mainController);
            IEnumerable<Player> players = new List<Player>();
            players.Append(Player1);
            players.Append(Player2);
            players.Append(Player3);

            foreach (var player in players)
            {
                _mainController.Players.Add(player);
            }
            _mainController.BoardTiles = new ObservableCollection<BoardTile>();
            for(int i = 0; i < 10; i++)
            {
                for(int j = 0; j < 10;j++)
                {
                    var emptyBoardTile = new BoardTile(0, new Coordinates(j, i), $"{j};{i}", null, new Tile(null, null));
                    _mainController.BoardTiles.Add(emptyBoardTile);
                }
            }

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
            specialities.Add(Speciality.EndOfCastle);

            var directions = new List<IDirection>();
            directions.Add(new Direction(new Field(), null));
            directions.Add(new Direction(new Castle(), Player1.Figures[0]));
            directions.Add(new Direction(new Field(), null));
            directions.Add(new Direction(new Field(), null));
            boardTile1.BackendTile = new Tile(directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            specialities.Add(Speciality.Shield);
            directions = new List<IDirection>();
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Castle(), null));
            boardTile2.BackendTile = new Tile(directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfCastle);
            directions = new List<IDirection>();
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Field(), null));
            directions.Add(new Direction(new Field(), null));
            directions.Add(new Direction(new Field(), null));
            boardTile3.BackendTile = new Tile(directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfCastle);
            directions = new List<IDirection>();
            directions.Add(new Direction(new Field(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Road(), null));
            boardTile4.BackendTile = new Tile(directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            directions = new List<IDirection>();
            directions.Add(new Direction(new Castle(), Player2.Figures[0]));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Castle(), null));
            boardTile5.BackendTile = new Tile(directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(new Field(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Field(), null));
            boardTile6.BackendTile = new Tile(directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(new Field(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Field(), null));
            directions.Add(new Direction(new Castle(), null));
            boardTile7.BackendTile = new Tile(directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfCastle);
            directions = new List<IDirection>();
            directions.Add(new Direction(new Field(), null));
            directions.Add(new Direction(new Field(), null));
            directions.Add(new Direction(new Field(), null));
            directions.Add(new Direction(new Castle(), Player3.Figures[0]));
            boardTile8.BackendTile = new Tile(directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            directions = new List<IDirection>();
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Field(), null));
            boardTile9.BackendTile = new Church(directions, specialities);

            //_mainController.BoardTiles = new ObservableCollection<BoardTile>();
            _mainController.BoardTiles[boardTile1.Coordinates.X + boardTile1.Coordinates.Y*10] = boardTile1;
            _mainController.BoardTiles[boardTile2.Coordinates.X + boardTile2.Coordinates.Y*10] = boardTile2;
            _mainController.BoardTiles[boardTile3.Coordinates.X + boardTile3.Coordinates.Y*10] = boardTile3;
            _mainController.BoardTiles[boardTile4.Coordinates.X + boardTile4.Coordinates.Y*10] = boardTile4;
            _mainController.BoardTiles[boardTile5.Coordinates.X + boardTile5.Coordinates.Y*10] = boardTile5;
            _mainController.BoardTiles[boardTile6.Coordinates.X + boardTile6.Coordinates.Y*10] = boardTile6;
            _mainController.BoardTiles[boardTile7.Coordinates.X + boardTile7.Coordinates.Y*10] = boardTile7;
            _mainController.BoardTiles[boardTile8.Coordinates.X + boardTile8.Coordinates.Y*10] = boardTile8;
            _mainController.BoardTiles[boardTile9.Coordinates.X + boardTile9.Coordinates.Y*10] = boardTile9;
        }

        [TestMethod]
        public void CalculateCastle_CastleIsFinished_ReturnsNumberOfPoints_TestCase1()
        {
            boardTile3.BackendTile.Directions[0].Landscape.calculate(boardTile3, false, _utils);
            Assert.AreEqual(8, Player1.BackendOwner.Points);
        }

        [TestMethod]
        public void CalculateCastle_CastleIsFinished_ReturnsNumberOfPoints_TestCase2()
        {
            boardTile5.BackendTile.Directions[3].Landscape.calculate(boardTile5, false, _utils);
            Assert.AreEqual(10, Player2.BackendOwner.Points);
            Assert.AreEqual(10, Player3.BackendOwner.Points);
        }

        [TestMethod]
        public void CalculateCastle_CastleIsFinished_ReturnsNumberOfPoints_TestCase3()
        {
            boardTile1.BackendTile.Directions[1].Landscape.calculate(boardTile1, false, _utils);
            Assert.AreEqual(8, Player1.BackendOwner.Points);
        }

        [TestMethod]
        public void CalculateCastle_CastleIsFinished_ReturnsNumberOfPoints_TestCase4()
        {
            _mainController.BoardTiles[boardTile1.Coordinates.X + boardTile1.Coordinates.Y * 10].BackendTile = new Tile(null, null);
            boardTile2.BackendTile.Directions[2].Figure = Player1.Figures[0];
            boardTile3.BackendTile.Directions[0].Landscape.calculate(boardTile3, true, _utils);
            Assert.AreEqual(6, Player1.BackendOwner.Points);
        }

        [TestMethod]
        public void CalculateCastle_CastleIsFinished_ReturnsNumberOfPoints_TestCase5()
        {
            _mainController.BoardTiles[boardTile1.Coordinates.X + boardTile1.Coordinates.Y * 10].BackendTile = new Tile(null, null);
            boardTile2.BackendTile.Directions[2].Figure = Player1.Figures[0];
            boardTile2.BackendTile.Directions[2].Landscape.calculate(boardTile2, true, _utils);
            Assert.AreEqual(6, Player1.BackendOwner.Points);
        }

        [TestMethod]
        public void CalculateCastle_CastleIsFinished_ReturnsNumberOfPoints_TestCase6()
        {
            _mainController.BoardTiles[boardTile1.Coordinates.X + boardTile1.Coordinates.Y * 10].BackendTile = new Tile(null, null);
            boardTile2.BackendTile.Directions[2].Figure = Player1.Figures[0];
            boardTile2.BackendTile.Directions[2].Landscape.calculate(boardTile2, false, _utils);
            Assert.AreEqual(0, Player1.BackendOwner.Points);
        }

        [TestMethod]
        public void CalculateCastle_CastleIsFinished_ReturnsNumberOfPoints_TestCase7()
        {
            boardTile1.BackendTile.Directions[2].Landscape = new Castle();
            boardTile3.BackendTile.Directions[0].Landscape = new Castle();
            boardTile3.BackendTile.Directions[1].Landscape = new Field();
            boardTile3.BackendTile.Directions[2].Landscape = new Field();
            boardTile3.BackendTile.Directions[3].Landscape = new Castle();
            boardTile1.BackendTile.Speciality = new List<Speciality>();
            boardTile3.BackendTile.Speciality = new List<Speciality>();
            boardTile9.BackendTile.Speciality = new List<Speciality>();
            boardTile2.BackendTile.Directions[2].Landscape.calculate(boardTile2, false, _utils);
            Assert.AreEqual(10, Player1.BackendOwner.Points);
        }

        [TestMethod]
        public void CalculateCastle_CastleIsFinished_ReturnsNumberOfPoints_TestCase8()
        {
            boardTile3.BackendTile.Directions[0].Landscape = new Castle();
            boardTile3.BackendTile.Directions[1].Landscape = new Field();
            boardTile3.BackendTile.Directions[2].Landscape = new Field();
            boardTile3.BackendTile.Directions[3].Landscape = new Castle();
            boardTile3.BackendTile.Speciality = new List<Speciality>();
            boardTile9.BackendTile.Speciality = new List<Speciality>();
            boardTile2.BackendTile.Directions[2].Figure = Player1.Figures[0];
            _mainController.BoardTiles[boardTile1.Coordinates.X + boardTile1.Coordinates.Y * 10].BackendTile = new Tile(null, null);
            boardTile2.BackendTile.Directions[2].Landscape.calculate(boardTile2, true, _utils);
            Assert.AreEqual(8, Player1.BackendOwner.Points);
        }

        [TestMethod]
        public void CanPlaceFigure_CastleIsFinished_ReturnsNumberOfPoints_TestCase9()
        {

            Assert.IsTrue(!boardTile2.BackendTile.Directions[2].Landscape.CanPlaceFigure(boardTile2, CardinalDirection.South, _utils, true));
        }
    }
}
