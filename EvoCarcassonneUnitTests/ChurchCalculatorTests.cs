using System.Collections.Generic;
using System.Collections.ObjectModel;
using EvoCarcassonne.Backend;
using EvoCarcassonne.Controller;
using EvoCarcassonne.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EvoCarcassonneUnitTests
{
    [TestClass]
    public class ChurchCalculatorTests
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
        
        [TestInitialize]
        public void Init()
        {
            var figure = new Figure(0, Player1);
            
            boardTile1.Coordinates = new Coordinates(0, 0);
            boardTile2.Coordinates = new Coordinates(1, 0);
            boardTile3.Coordinates = new Coordinates(2, 0);
            boardTile4.Coordinates = new Coordinates(0, 1);
            boardTile5.Coordinates = new Coordinates(1, 1);
            boardTile6.Coordinates = new Coordinates(2, 1);
            boardTile7.Coordinates = new Coordinates(0, 2);
            boardTile8.Coordinates = new Coordinates(1, 2);
            boardTile9.Coordinates = new Coordinates(2, 2);
            
            var specialities = new List<Speciality>();
            specialities.Add(Speciality.None);

            var directions = new List<IDirection>();
            directions.Add(new Direction(0, new Field(), null));
            directions.Add(new Direction(0, new Field(), figure));
            directions.Add(new Direction(0, new Field(), null));
            directions.Add(new Direction(0, new Field(), null));
            boardTile1.BackendTile = new Tile(1, directions, specialities);
            boardTile2.BackendTile = new Tile(2, directions, specialities);
            boardTile3.BackendTile = new Tile(3, directions, specialities);
            boardTile4.BackendTile = new Tile(4, directions, specialities);
            boardTile6.BackendTile = new Tile(6, directions, specialities);
            boardTile7.BackendTile = new Tile(7, directions, specialities);
            boardTile8.BackendTile = new Tile(8, directions, specialities);
            boardTile9.BackendTile = new Tile(9, directions, specialities);

            specialities = new List<Speciality>();
            specialities.Add(Speciality.Colostor);
            directions = new List<IDirection>();
            directions.Add(new Direction(0, new Field(), null));
            directions.Add(new Direction(0, new Field(), null));
            directions.Add(new Direction(0, new Field(), null));
            directions.Add(new Direction(0, new Field(), null));
            var chuchTile = new Church(5, directions, specialities);
            chuchTile.CenterFigure = figure;
            boardTile5.BackendTile = chuchTile;
            
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
        public void ChurchCalculate_FinishedChurchNotGameover_ShouldReturn_9()
        {
            var church = (Church)boardTile5.BackendTile;
            church.calculate(boardTile5, false);
            Assert.AreEqual(9, Player1.Points);
        }
        
        [TestMethod]
        public void ChurchCalculate_NotFinishedChurchGameover_ShouldReturn_3()
        {
            var church = (Church)boardTile5.BackendTile;
            MainController.PlacedBoardTiles.RemoveAndGet(8);
            MainController.PlacedBoardTiles.RemoveAndGet(7);
            MainController.PlacedBoardTiles.RemoveAndGet(6);
            MainController.PlacedBoardTiles.RemoveAndGet(5);
            MainController.PlacedBoardTiles.RemoveAndGet(3);
            church.calculate(boardTile5, true);
            Assert.AreEqual(3, Player1.Points);
        }
        
        [TestMethod]
        public void ChurchCalculate_NotFinishedChurchNotGameover_ShouldReturn_0()
        {
            var church = (Church)boardTile5.BackendTile;
            MainController.PlacedBoardTiles.RemoveAndGet(8);
            church.calculate(boardTile5, false);
            Assert.AreEqual(0, Player1.Points);
        }
    }
}