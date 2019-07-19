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
            
            var figure = new Figure(Player1.BackendOwner);
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
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), figure));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Castle(), null));
            boardTile1.BackendTile = new Tile(directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            directions = new List<IDirection>();
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Road(), null));
            boardTile2.BackendTile = new Tile(directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Castle(), null));
            boardTile3.BackendTile = new Tile(directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Castle(), null));
            boardTile4.BackendTile = new Tile(directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);
            directions = new List<IDirection>();
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Road(), figure));
            boardTile5.BackendTile = new Tile(directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            directions = new List<IDirection>();
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), null));
            boardTile6.BackendTile = new Tile(directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), null));
            boardTile7.BackendTile = new Tile(directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);
            directions = new List<IDirection>();
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), null));
            boardTile8.BackendTile = new Tile(directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            directions = new List<IDirection>();
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Castle(), null));
            boardTile9.BackendTile = new Tile(directions, specialities);
            
            _mainController.PlacedBoardTiles = new ObservableCollection<BoardTile>();
            _mainController.PlacedBoardTiles.Add(boardTile1);
            _mainController.PlacedBoardTiles.Add(boardTile2);
            _mainController.PlacedBoardTiles.Add(boardTile3);
            _mainController.PlacedBoardTiles.Add(boardTile4);
            _mainController.PlacedBoardTiles.Add(boardTile5);
            _mainController.PlacedBoardTiles.Add(boardTile6);
            _mainController.PlacedBoardTiles.Add(boardTile7);
            _mainController.PlacedBoardTiles.Add(boardTile8);
            _mainController.PlacedBoardTiles.Add(boardTile9);
        }
        
        
        [TestMethod]
        public void CalculateRoad_RoadIsFinished_ReturnsNumberOfPoints_TestCase1()
        {
            boardTile1.BackendTile.Directions[1].Landscape.calculate(boardTile1, false, _utils);
            Assert.IsTrue(Player1.BackendOwner.Points == 6);
        }

        [TestMethod]
        public void CalculateRoad_RoadIsFinished_ReturnsNumberOfPoints_TestCase2()
        {
            #region InitializeValues

            var figure = Player1.Figures[0];

            var specialities = new List<Speciality>();
            var directions = new List<IDirection>();      

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            
            directions = new List<IDirection>();
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Castle(), null));
            boardTile3.BackendTile = new Tile(directions, specialities);
            #endregion

            boardTile1.BackendTile.Directions[1].Landscape.calculate(boardTile1, false, _utils);
            Assert.IsTrue(Player1.BackendOwner.Points == 5);
        }

        [TestMethod]
        public void CalculateRoad_RoadIsFinished_ReturnsNumberOfPoints_TestCase3()
        {
            #region InitializeValues

            var figure = Player1.Figures[0];

            var directions = new List<IDirection>();
            var specialities = new List<Speciality>();

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            directions = new List<IDirection>();
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), null));
            boardTile3.BackendTile = new Tile(directions, specialities);
            #endregion

            boardTile1.BackendTile.Directions[1].Landscape.calculate(boardTile1, false,_utils);
            Assert.IsTrue(Player1.BackendOwner.Points == 4);
        }
        
         [TestMethod]
        public void CalculateRoad_RoadIsFinished_ReturnsNumberOfPoints_TestCase4()
        {
            #region InitializeValues

            var figure = Player1.Figures[0];

            var directions = new List<IDirection>();
            var specialities = new List<Speciality>();

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);

            directions = new List<IDirection>();
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), null));
            boardTile3.BackendTile = new Tile(directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Castle(), null));
            boardTile9.BackendTile = new Tile(directions, specialities);

            #endregion

            boardTile1.BackendTile.Directions[1].Landscape.calculate(boardTile1, false,_utils);
            
            Assert.IsTrue(Player1.BackendOwner.Points == 4);
        }
         [TestMethod]
        public void CalculateRoad_RoadsAreNotFinishedAndGameIsOverCall_ReturnsNumberOfPoints_TestCase1()
        {
            #region InitializeValues

            var figure = Player1.Figures[0];

            var directions = new List<IDirection>();
            var specialities = new List<Speciality>();

            specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);
            directions = new List<IDirection>();
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Road(), figure));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), null));
            boardTile3.BackendTile = new Tile(directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Castle(), null));
            boardTile5.BackendTile = new Tile(directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            
            directions = new List<IDirection>();
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), figure));
            directions.Add(new Direction(new Road(), figure));
            directions.Add(new Direction(new Road(), figure));
            boardTile6.BackendTile = new Tile(directions, specialities);
            
            directions = new List<IDirection>();
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), null));
            boardTile8.BackendTile = new Tile(directions, specialities);
            #endregion

           boardTile1.BackendTile.Directions[1].Landscape.calculate(boardTile6,  true, _utils);
           Assert.IsTrue(Player1.BackendOwner.Points == 5);
        }
        
        [TestMethod]
        public void CalculateRoad_RoadsAreNotFinishedAndGameIsOverCall_ReturnsNumberOfPoints_TestCase2()
        {
            #region InitializeValues

            var figure = Player1.Figures[0];

            var directions = new List<IDirection>();
            var specialities = new List<Speciality>();

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            directions = new List<IDirection>();
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), null));
            boardTile3.BackendTile = new Tile(directions, specialities);
            
            directions = new List<IDirection>();
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Road(), null));
            boardTile6.BackendTile = new Tile(directions, specialities);
            
            directions = new List<IDirection>();
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), null));
            boardTile8.BackendTile = new Tile(directions, specialities);
            
            specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);
            directions = new List<IDirection>();
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), null));
            boardTile9.BackendTile = new Tile(directions, specialities);
            
            #endregion

            
            boardTile1.BackendTile.Directions[1].Landscape.calculate(boardTile3, true, _utils);
            
            Assert.IsTrue(Player1.BackendOwner.Points == 7);
        }
        
        [TestMethod]
        public void CalculateRoad_RoadIsNotFinishedNotGameover_ShouldReturn0()
        {
            
            _mainController.PlacedBoardTiles.RemoveAndGet(7);
            boardTile1.BackendTile.Directions[1].Landscape.calculate(boardTile1,  false, _utils);
            
            Assert.IsTrue(Player1.BackendOwner.Points == 0);
        }
        
        [TestMethod]
        public void CalculateRoad_RoadIsFinishedNotGameover_CalledWithNotEndofroad_ShouldReturnNumberOfTiles_TestCase1()
        {
            
            boardTile7.BackendTile.Directions[1].Landscape.calculate(boardTile7,  false, _utils);
            
            Assert.IsTrue(Player1.BackendOwner.Points == 6);
        }
        
        [TestMethod]
        public void CalculateRoad_RoadIsFinishedNotGameover_CalledWithNotEndofroad_ShouldReturnNumberOfTiles_TestCase2()
        {
            #region Initialize values

            BoardTile boardTile10 = new BoardTile();
            boardTile10.Coordinates = new Coordinates(2,3);
            List<IDirection> directions = new List<IDirection>();
            directions.Add(new Direction(new Road(), Player1.Figures[0]));
            directions.Add(new Direction(new Field(), null));
            directions.Add(new Direction(new Field(), null));
            directions.Add(new Direction(new Field(), null));
            List<Speciality> speciality = new List<Speciality>();
            speciality.Add(Speciality.EndOfRoad);
            speciality.Add(Speciality.Colostor);
            boardTile10.BackendTile = new Church(directions, speciality);
            #endregion
            
            boardTile7.BackendTile.Directions[1].Landscape.calculate(boardTile7,  false, _utils);
            
            boardTile10.BackendTile.Directions[0].Landscape.calculate(boardTile10, false, _utils);
            
            Assert.IsTrue(Player1.BackendOwner.Points == 8);
        }
        
        [TestMethod]
        public void CalculateRoad_RoadIsFinishedGameover_CalledWithNotEndofroad_ShouldReturnNumberOfTiles_TestCase3()
        {
            
            boardTile7.BackendTile.Directions[1].Landscape.calculate(boardTile7,  true,_utils);
            
            Assert.IsTrue(Player1.BackendOwner.Points == 6);
        }
        
         [TestMethod]
        public void CalculateRoad_RoadIsFinishedGameover_CalledWithNotEndofroad_ShouldReturnNumberOfTiles_TestCase4()
        {
            #region Initialize values
            List<IDirection> directions = new List<IDirection>();
            List<Speciality> specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            directions.Add(new Direction(new Road(), Player1.Figures[0]));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), null));
            boardTile3.BackendTile.Directions = directions;
            boardTile3.BackendTile.Speciality = specialities;
            
            directions = new List<IDirection>();
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Castle(), null));
            boardTile1.BackendTile.Directions = directions;
            boardTile1.BackendTile.Speciality = specialities;
            
            directions = new List<IDirection>();
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Road(), null));
            boardTile2.BackendTile.Directions = directions;
            boardTile2.BackendTile.Speciality = specialities;
            #endregion
            
            boardTile1.BackendTile.Directions[1].Landscape.calculate(boardTile1,  false, _utils);
            
            Assert.IsTrue(Player1.BackendOwner.Points == 4);
        }
        
        [TestMethod]
        public void CalculateRoad_RoadIsFinishedNotGameover_CalledWithNotEndofroad_ShouldReturnNumberOfTiles_ShouldDistributePoints_TestCase1()
        {
            boardTile2.BackendTile.Directions[2].Figure = Player2.Figures[0];
            boardTile6.BackendTile.Directions[1].Figure = Player3.Figures[0];
            
            boardTile7.BackendTile.Directions[1].Landscape.calculate(boardTile7,  false, _utils);
            
            
            
            Assert.IsTrue(Player1.BackendOwner.Points == 6);
            Assert.IsTrue(Player2.BackendOwner.Points == 6);
            Assert.IsTrue(Player3.BackendOwner.Points == 6);
        }
        
        [TestMethod]
        public void CalculateRoad_RoadIsFinishedNotGameover_CalledWithNotEndofroad_ShouldReturnNumberOfTiles_ShouldDistributePoints_TestCase2()
        {
            boardTile2.BackendTile.Directions[2].Figure = Player2.Figures[0];
            
            boardTile7.BackendTile.Directions[1].Landscape.calculate(boardTile7,  false, _utils);
            
            
            Assert.IsTrue(Player1.BackendOwner.Points == 6);
            Assert.IsTrue(Player2.BackendOwner.Points == 6);
        }

        
        [TestMethod]
        public void CalculateRoad_RoadIsFinishedNotGameover_CalledWithNotEndofroad_ShouldReturnNumberOfTiles_ShouldDistributePoints_TestCase3()
        {
            boardTile2.BackendTile.Directions[2].Figure = Player2.Figures[0];
            boardTile6.BackendTile.Directions[3].Figure = Player1.Figures[0];
            boardTile3.BackendTile.Directions[1].Figure = Player1.Figures[1];
            
            boardTile7.BackendTile.Directions[1].Landscape.calculate(boardTile7,  false, _utils);
            
            
            Assert.IsTrue(Player1.BackendOwner.Points == 6);
            Assert.IsTrue(Player2.BackendOwner.Points == 0);
        }
        
        
        [TestMethod]
        public void CalculateRoad_RoadIsFinishedNotGameover_CalledWithNotEndofroad_ShouldReturnNumberOfTiles_ShouldDistributePoints_TestCase4()
        {
            boardTile8.BackendTile.Directions[3].Figure = Player2.Figures[0];
            
            boardTile7.BackendTile.Directions[1].Landscape.calculate(boardTile7,  false, _utils);
            
            
            Assert.IsTrue(Player1.BackendOwner.Points == 6);
            Assert.IsTrue(Player2.BackendOwner.Points == 6);
        }
        
        [TestMethod]
        public void CalculateRoad_RoadIsNotFinishedGameover_CalledWithNotEndofroad_ShouldReturnNumberOfTiles_ShouldDistributePoints_TestCase1()
        {
            boardTile8.BackendTile.Directions[3].Figure = Player2.Figures[0];
            boardTile7.BackendTile.Directions[3].Figure = Player1.Figures[0];
            _mainController.PlacedBoardTiles.RemoveAndGet(1);
            _mainController.PlacedBoardTiles.RemoveAndGet(0);
            
            boardTile7.BackendTile.Directions[1].Landscape.calculate(boardTile7,  true, _utils);
            
            
            Assert.IsTrue(Player1.BackendOwner.Points == 4);
            Assert.IsTrue(Player2.BackendOwner.Points == 4);
        }
        
        [TestMethod]
        public void CalculateRoad_RoadIsFinishedNotGameover_CalledWithEndofroad_ShouldReturnNumberOfTiles_ShouldDistributePoints_TestCase2()
        {
            #region Initialize values

            var directions = new List<IDirection>();
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Road(), Player2.Figures[0]));
            directions.Add(new Direction(new Road(), Player3.Figures[0]));
            directions.Add(new Direction(new Castle(), null));
            var specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);
            boardTile3.BackendTile = new Tile(directions, specialities);
            #endregion
            boardTile4.BackendTile.Directions[0].Figure = Player3.Figures[0];
            boardTile3.BackendTile.Directions[0].Landscape.calculate(boardTile3,  false, _utils);
            Assert.IsTrue(Player1.BackendOwner.Points == 3);
            Assert.IsTrue(Player2.BackendOwner.Points == 4);
            Assert.IsTrue(Player3.BackendOwner.Points == 3);
        }
        
        [TestMethod]
        public void CalculateRoad_RoadIsFinishedNotGameover_CalledWithEndofroad_ShouldReturnNumberOfTiles_ShouldDistributePoints_TestCase3()
        {
            #region Initialize values

            var directions = new List<IDirection>();
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Road(), Player2.Figures[0]));
            directions.Add(new Direction(new Road(), Player3.Figures[0]));
            directions.Add(new Direction(new Castle(), null));
            var specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);
            boardTile3.BackendTile = new Tile(directions, specialities);
            boardTile4.BackendTile.Directions[0].Figure = Player3.Figures[1];
            _mainController.PlacedBoardTiles.RemoveAndGet(0);
            #endregion
            
            
            boardTile3.BackendTile.Directions[0].Landscape.calculate(boardTile3,  false, _utils);
            
            
            
            Assert.IsTrue(Player1.BackendOwner.Points == 0);
            Assert.IsTrue(Player2.BackendOwner.Points == 4);
            Assert.IsTrue(Player3.BackendOwner.Points == 3);
        }
        /*

        [TestMethod]
        public void CalculateRoad_RoadIsFinishedNotGameover_CalledWithEndofroadAndChurch_ShouldReturnNumberOfTiles()
        {
            #region Initialize values

            var directions = new List<IDirection>();
            directions.Add(new Direction(new Field(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Field(), null));
            directions.Add(new Direction(new Field(), null));
            var specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);
            specialities.Add(Speciality.Colostor);
            boardTile3.BackendTile = new Church(directions, specialities);
            boardTile3.BackendTile.CenterFigure = Player1.Figures[3];
            boardTile4.BackendTile = new Church(directions, specialities);
            boardTile4.BackendTile.CenterFigure = Player1.Figures[5];
            boardTile5.BackendTile.Directions[3].Figure = null;
            directions = new List<IDirection>();
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), null));
            specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);
            specialities.Add(Speciality.Shield);
            boardTile8.BackendTile = new Tile(directions, specialities);
            _mainController.PlacedBoardTiles.RemoveAndGet(0);
            #endregion
            
            
            boardTile3.BackendTile.Directions[1].Landscape.calculate(boardTile6,  false, _utils);
            
            
            boardTile4.BackendTile.Directions[1].Landscape.calculate(boardTile4, false, _utils);
            
            Assert.IsTrue(Player1.BackendOwner.Points == 6);
        }   */
        
        
        [TestMethod]
        public void CalculateRoad_RoadIsFinishedGameover_CalledWithEndofroad_ShouldReturnNumberOfTiles()
        {
            #region Initialize values

            var directions = new List<IDirection>();
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Road(), null));
            var specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);
            boardTile1.BackendTile = new Tile(directions, specialities);
            directions = new List<IDirection>();
            directions.Add(new Direction(new Field(), null));
            directions.Add(new Direction(new Field(), null));
            directions.Add(new Direction(new Field(), null));
            directions.Add(new Direction(new Road(), null));
            boardTile2.BackendTile = new Church(directions, specialities);
            specialities.Add(Speciality.Colostor);
            
            #endregion
            boardTile2.BackendTile.CenterFigure = Player1.Figures[0];
            boardTile2.BackendTile.Directions[3].Landscape.calculate(boardTile2,  true, _utils);
            Console.WriteLine("Pontok    " + Player1.BackendOwner.Points);
            Assert.IsTrue(Player1.BackendOwner.Points == 0);
        }
        
        [TestMethod]
        public void CalculateRoad_RoadIsFinishedGameover_CalledWithEndofroad_ShouldReturnNumberOfTiles1()
        {
            #region Initialize values

            var directions = new List<IDirection>();
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Road(), null));
            var specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);
            boardTile1.BackendTile = new Tile(directions, specialities);
            directions = new List<IDirection>();
            directions.Add(new Direction(new Field(), null));
            directions.Add(new Direction(new Field(), null));
            directions.Add(new Direction(new Field(), null));
            directions.Add(new Direction(new Road(), Player1.Figures[0]));
            boardTile2.BackendTile = new Church(directions, specialities);
            specialities.Add(Speciality.Colostor);
            
            #endregion
            boardTile2.BackendTile.CenterFigure = Player1.Figures[0];
            boardTile2.BackendTile.Directions[3].Landscape.calculate(boardTile2,  true, _utils);
            Console.WriteLine("Pontok    " + Player1.BackendOwner.Points);
            Assert.IsTrue(Player1.BackendOwner.Points == 2);
        }
        
        [TestMethod]
        public void CanPlaceFigure_RoadIsFinished_ReturnsNumberOfPoints_TestCase()
        {
            #region Initialize Values
            var figure = new Figure(Player1.BackendOwner);
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
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), figure));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Castle(), null));
            boardTile1.BackendTile = new Tile(directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            directions = new List<IDirection>();
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Road(), null));
            boardTile2.BackendTile = new Tile(directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Castle(), null));
            boardTile3.BackendTile = new Tile(directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(new Field(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Castle(), null));
            boardTile4.BackendTile = new Tile(directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            directions = new List<IDirection>();
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Field(), null));
            directions.Add(new Direction(new Field(), null));
            directions.Add(new Direction(new Road(), null));
            boardTile5.BackendTile = new Tile(directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), null));
            boardTile6.BackendTile = new Tile(directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), null));
            boardTile7.BackendTile = new Tile(directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);
            directions = new List<IDirection>();
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Road(), null));
            boardTile8.BackendTile = new Tile(directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            directions = new List<IDirection>();
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Road(), null));
            directions.Add(new Direction(new Castle(), null));
            directions.Add(new Direction(new Castle(), null));
            boardTile9.BackendTile = new Tile(directions, specialities);

            _mainController.PlacedBoardTiles = new ObservableCollection<BoardTile>();
            _mainController.PlacedBoardTiles.Add(boardTile1);
            _mainController.PlacedBoardTiles.Add(boardTile2);
            _mainController.PlacedBoardTiles.Add(boardTile3);
            _mainController.PlacedBoardTiles.Add(boardTile4);
            _mainController.PlacedBoardTiles.Add(boardTile5);
            _mainController.PlacedBoardTiles.Add(boardTile6);
            _mainController.PlacedBoardTiles.Add(boardTile7);
            _mainController.PlacedBoardTiles.Add(boardTile8);
            _mainController.PlacedBoardTiles.Add(boardTile9);
            #endregion

            Assert.IsTrue(!boardTile8.BackendTile.Directions[3].Landscape.CanPlaceFigure(boardTile8, CardinalDirection.West, _utils, true));

        }
    }
}
