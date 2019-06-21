﻿using System;
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
        private IOwner Player1 = new Owner(0, "Krisztian");
        private IOwner Player2 = new Owner(1, "Pista");
        private IOwner Player3 = new Owner(2, "Karcsi");
        
        [TestInitialize]
        public void Init()
        {
            var figure = new Figure(0, Player1);
            
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
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(0, new Castle(), null));
            boardTile1.BackendTile = new Tile(1, directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(0, new Road(), null));
            directions.Add(new Direction(0, new Road(), null));
            boardTile2.BackendTile = new Tile(2, directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Road(), null));
            directions.Add(new Direction(0, new Road(), null));
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(0, new Castle(), null));
            boardTile3.BackendTile = new Tile(3, directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Road(), null));
            directions.Add(new Direction(0, new Road(), null));
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(0, new Castle(), null));
            boardTile4.BackendTile = new Tile(4, directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(0, new Road(), null));
            directions.Add(new Direction(0, new Road(), null));
            directions.Add(new Direction(0, new Road(), figure));
            boardTile5.BackendTile = new Tile(5, directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(0, new Road(), null));
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(0, new Road(), null));
            boardTile6.BackendTile = new Tile(6, directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(0, new Road(), null));
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(0, new Road(), null));
            boardTile7.BackendTile = new Tile(7, directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(0, new Road(), null));
            boardTile8.BackendTile = new Tile(8, directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Road(), null));
            directions.Add(new Direction(0, new Road(), null));
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(0, new Castle(), null));
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
        public void CalculateRoad_RoadIsFinished_ReturnsNumberOfPoints_TestCase1()
        {
            Console.WriteLine(@"Route to the other end of road:");
            boardTile1.BackendTile.Directions[1].Landscape.calculate(boardTile1, false);
            
            Console.WriteLine(@"Ennyi pontom van:    " + Player1.Points);
            Assert.IsTrue(Player1.Points == 6);
        }

        [TestMethod]
        public void CalculateRoad_RoadIsFinished_ReturnsNumberOfPoints_TestCase2()
        {
            #region InitializeValues

            var figure = new Figure(0, Player1);

            var specialities = new List<Speciality>();
            var directions = new List<IDirection>();      

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Road(), null));
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(0, new Road(), null));
            directions.Add(new Direction(0, new Castle(), null));
            boardTile3.BackendTile = new Tile(3, directions, specialities);
            #endregion

            Console.WriteLine(@"Route to the other end of road:");
            boardTile1.BackendTile.Directions[1].Landscape.calculate(boardTile1, false);
            Console.WriteLine(@"Ennyi pontom van:    " + Player1.Points);
            Assert.IsTrue(Player1.Points == 5);
        }

        [TestMethod]
        public void CalculateRoad_RoadIsFinished_ReturnsNumberOfPoints_TestCase3()
        {
            #region InitializeValues

            var figure = new Figure(0, Player1);

            var directions = new List<IDirection>();
            var specialities = new List<Speciality>();

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Road(), null));
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(0, new Road(), null));
            boardTile3.BackendTile = new Tile(3, directions, specialities);
            #endregion

            Console.WriteLine(@"Route to the other end of road:");
            boardTile1.BackendTile.Directions[1].Landscape.calculate(boardTile1, false);
            
            Console.WriteLine(@"Ennyi pontom van:    " + Player1.Points);
            Assert.IsTrue(Player1.Points == 4);
        }
        
         [TestMethod]
        public void CalculateRoad_RoadIsFinished_ReturnsNumberOfPoints_TestCase4()
        {
            #region InitializeValues

            var figure = new Figure(0, Player1);

            var directions = new List<IDirection>();
            var specialities = new List<Speciality>();

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);

            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Road(), null));
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(0, new Road(), null));
            boardTile3.BackendTile = new Tile(3, directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Road(), null));
            directions.Add(new Direction(0, new Road(), null));
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(0, new Castle(), null));
            boardTile9.BackendTile = new Tile(9, directions, specialities);

            #endregion

            Console.WriteLine(@"Route to the other end of road:");
            boardTile1.BackendTile.Directions[1].Landscape.calculate(boardTile1, false);
            
            Console.WriteLine(@"Ennyi pontom van:    " + Player1.Points);
            Assert.IsTrue(Player1.Points == 4);
        }
         [TestMethod]
        public void CalculateRoad_RoadsAreNotFinishedAndGameIsOverCall_ReturnsNumberOfPoints_TestCase1()
        {
            #region InitializeValues

            var figure = new Figure(0, Player1);

            var directions = new List<IDirection>();
            var specialities = new List<Speciality>();

            specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Road(), null));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(0, new Road(), null));
            boardTile3.BackendTile = new Tile(3, directions, specialities);

            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Road(), null));
            directions.Add(new Direction(0, new Road(), null));
            directions.Add(new Direction(0, new Road(), null));
            directions.Add(new Direction(0, new Castle(), null));
            boardTile5.BackendTile = new Tile(5, directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            directions.Add(new Direction(0, new Road(), figure));
            boardTile6.BackendTile = new Tile(6, directions, specialities);
            
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Road(), null));
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(0, new Road(), null));
            boardTile8.BackendTile = new Tile(8, directions, specialities);
            #endregion

            Console.WriteLine(@"Route to the other end of road:");
           boardTile1.BackendTile.Directions[1].Landscape.calculate(boardTile6,  true);
           Console.WriteLine(@"Ennyi pontom van:    " + Player1.Points);
           Assert.IsTrue(Player1.Points == 5);
        }
        
        [TestMethod]
        public void CalculateRoad_RoadsAreNotFinishedAndGameIsOverCall_ReturnsNumberOfPoints_TestCase2()
        {
            #region InitializeValues

            var figure = new Figure(0, Player1);

            var directions = new List<IDirection>();
            var specialities = new List<Speciality>();

            specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Road(), null));
            directions.Add(new Direction(0, new Road(), null));
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(0, new Road(), null));
            boardTile3.BackendTile = new Tile(3, directions, specialities);
            
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(0, new Road(), null));
            directions.Add(new Direction(0, new Road(), null));
            directions.Add(new Direction(0, new Road(), null));
            boardTile6.BackendTile = new Tile(6, directions, specialities);
            
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Road(), null));
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(0, new Road(), null));
            boardTile8.BackendTile = new Tile(8, directions, specialities);
            
            specialities = new List<Speciality>();
            specialities.Add(Speciality.EndOfRoad);
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Road(), null));
            directions.Add(new Direction(0, new Road(), null));
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(0, new Road(), null));
            boardTile9.BackendTile = new Tile(9, directions, specialities);
            
            #endregion

            Console.WriteLine(@"Route to the other end of road:");
            boardTile1.BackendTile.Directions[1].Landscape.calculate(boardTile3, true);
            Console.WriteLine(@"Ennyi pontom van:    " + Player1.Points);
            Assert.IsTrue(Player1.Points == 7);
        }
        
        [TestMethod]
        public void CalculateRoad_RoadIsNotFinishedNotGameover_ShouldReturn0()
        {
            Console.WriteLine(@"Route to the other end of road:");
            MainController.PlacedBoardTiles.RemoveAndGet(7);
            boardTile1.BackendTile.Directions[1].Landscape.calculate(boardTile1,  false);
            Console.WriteLine(@"Ennyi pontom van:    " + Player1.Points);
            Assert.IsTrue(Player1.Points == 0);
        }
        
        [TestMethod]
        public void CalculateRoad_RoadIsFinishedNotGameover_CalledWithNotEndofroad_ShouldReturnNumberOfTiles_TestCase1()
        {
            Console.WriteLine(@"Route to the other end of road:");
            boardTile7.BackendTile.Directions[1].Landscape.calculate(boardTile7,  false);
            Console.WriteLine(@"Ennyi pontom van:    " + Player1.Points);
            Assert.IsTrue(Player1.Points == 6);
        }
        
        [TestMethod]
        public void CalculateRoad_RoadIsFinishedNotGameover_CalledWithNotEndofroad_ShouldReturnNumberOfTiles_TestCase2()
        {
            #region Initialize values

            BoardTile boardTile10 = new BoardTile();
            boardTile10.Coordinates = new Coordinates(2,3);
            List<IDirection> directions = new List<IDirection>();
            directions.Add(new Direction(0, new Road(), new Figure(0, Player1)));
            directions.Add(new Direction(1, new Field(), null));
            directions.Add(new Direction(2, new Field(), null));
            directions.Add(new Direction(3, new Field(), null));
            List<Speciality> speciality = new List<Speciality>();
            speciality.Add(Speciality.EndOfRoad);
            speciality.Add(Speciality.Colostor);
            boardTile10.BackendTile = new Church(0, directions, speciality);
            #endregion
            Console.WriteLine(@"Route to the other end of road:");
            boardTile7.BackendTile.Directions[1].Landscape.calculate(boardTile7,  false);
            Console.WriteLine(@"Route to the other end of road:");
            boardTile10.BackendTile.Directions[0].Landscape.calculate(boardTile10, false);
            Console.WriteLine(@"Ennyi pontom van:    " + Player1.Points);
            Assert.IsTrue(Player1.Points == 8);
        }
        
        [TestMethod]
        public void CalculateRoad_RoadIsFinishedGameover_CalledWithNotEndofroad_ShouldReturnNumberOfTiles_TestCase3()
        {
            Console.WriteLine(@"Route to the other end of road:");
            boardTile7.BackendTile.Directions[1].Landscape.calculate(boardTile7,  true);
            Console.WriteLine(@"Ennyi pontom van:    " + Player1.Points);
            Assert.IsTrue(Player1.Points == 6);
        }
        
         [TestMethod]
        public void CalculateRoad_RoadIsFinishedGameover_CalledWithNotEndofroad_ShouldReturnNumberOfTiles_TestCase4()
        {
            #region Initialize values
            List<IDirection> directions = new List<IDirection>();
            List<Speciality> specialities = new List<Speciality>();
            specialities.Add(Speciality.None);
            directions.Add(new Direction(0, new Road(), new Figure(0, Player1)));
            directions.Add(new Direction(1, new Castle(), null));
            directions.Add(new Direction(2, new Castle(), null));
            directions.Add(new Direction(3, new Road(), null));
            boardTile3.BackendTile.Directions = directions;
            boardTile3.BackendTile.Speciality = specialities;
            
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(1, new Road(), null));
            directions.Add(new Direction(2, new Road(), null));
            directions.Add(new Direction(3, new Castle(), null));
            boardTile1.BackendTile.Directions = directions;
            boardTile1.BackendTile.Speciality = specialities;
            
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Castle(), null));
            directions.Add(new Direction(1, new Castle(), null));
            directions.Add(new Direction(2, new Road(), null));
            directions.Add(new Direction(3, new Road(), null));
            boardTile2.BackendTile.Directions = directions;
            boardTile2.BackendTile.Speciality = specialities;
            #endregion
            Console.WriteLine(@"Route to the other end of road:");
            boardTile1.BackendTile.Directions[1].Landscape.calculate(boardTile1,  false);
            Console.WriteLine(@"Ennyi pontom van:    " + Player1.Points);
            Assert.IsTrue(Player1.Points == 4);
        }
        
        [TestMethod]
        public void CalculateRoad_RoadIsFinishedNotGameover_CalledWithNotEndofroad_ShouldReturnNumberOfTiles_ShouldDistributePoints_TestCase1()
        {
            boardTile2.BackendTile.Directions[2].Figure = new Figure(0, Player2);
            boardTile6.BackendTile.Directions[3].Figure = new Figure(1, Player3);
            Console.WriteLine(@"Route to the other end of road:");
            boardTile7.BackendTile.Directions[1].Landscape.calculate(boardTile7,  false);
            Console.WriteLine(@"Ennyi pontom van:    " + Player1.Points);
            Console.WriteLine(@"Ennyi pontom van:    " + Player2.Points);
            Console.WriteLine(@"Ennyi pontom van:    " + Player3.Points);
            Assert.IsTrue(Player1.Points == 2);
            Assert.IsTrue(Player2.Points == 2);
            Assert.IsTrue(Player3.Points == 2);
        }
        
        [TestMethod]
        public void CalculateRoad_RoadIsFinishedNotGameover_CalledWithNotEndofroad_ShouldReturnNumberOfTiles_ShouldDistributePoints_TestCase2()
        {
            boardTile2.BackendTile.Directions[2].Figure = new Figure(0, Player2);
            Console.WriteLine(@"Route to the other end of road:");
            boardTile7.BackendTile.Directions[1].Landscape.calculate(boardTile7,  false);
            Console.WriteLine(@"Ennyi pontom van:    " + Player1.Points);
            Console.WriteLine(@"Ennyi pontom van:    " + Player2.Points);
            Assert.IsTrue(Player1.Points == 3);
            Assert.IsTrue(Player2.Points == 3);
        }

        
        [TestMethod]
        public void CalculateRoad_RoadIsFinishedNotGameover_CalledWithNotEndofroad_ShouldReturnNumberOfTiles_ShouldDistributePoints_TestCase3()
        {
            boardTile2.BackendTile.Directions[2].Figure = new Figure(0, Player2);
            boardTile6.BackendTile.Directions[3].Figure = new Figure(1, Player1);
            boardTile3.BackendTile.Directions[1].Figure = new Figure(2, Player1);
            Console.WriteLine(@"Route to the other end of road:");
            boardTile7.BackendTile.Directions[1].Landscape.calculate(boardTile7,  false);
            Console.WriteLine(@"Ennyi pontom van:    " + Player1.Points);
            Console.WriteLine(@"Ennyi pontom van:    " + Player2.Points);
            Assert.IsTrue(Player1.Points == 6);
            Assert.IsTrue(Player2.Points == 0);
        }
    }      
}
