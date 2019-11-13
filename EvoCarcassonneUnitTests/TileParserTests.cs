using System;
using System.Collections.Generic;
using System.Linq;
using Backend;
using Backend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EvoCarcassonneUnitTests
{
    [TestClass]
    public class TileParserTests
    {
        private TileParser tileParser;
        private List<string> resourcesList;

        [TestInitialize]
        public void Init()
        {
            resourcesList = new List<string>();
            resourcesList.Add("S321014");
            resourcesList.Add("T102020");
            resourcesList.Add("T111113");
            resourcesList.Add("T122021");
            resourcesList.Add("T122123");
            resourcesList.Add("T122221");
            resourcesList.Add("T2001023");
            resourcesList.Add("T202021");
            resourcesList.Add("T220021");
            resourcesList.Add("T221121");
            resourcesList.Add("T222004");
            resourcesList.Add("T2221213");
            resourcesList.Add("T302024");
            resourcesList.Add("T320020");
            resourcesList.Add("T320114");
            resourcesList.Add("T321104");
            resourcesList.Add("T3211134");
            resourcesList.Add("T321120");
            resourcesList.Add("T322020");
            resourcesList.Add("T400002");
            resourcesList.Add("T401113");
            resourcesList.Add("T520004");
            resourcesList.Add("T810100");
            resourcesList.Add("T900110");
        }

        [TestMethod]
        public void InitTileStack()
        {
            tileParser = new TileParser(resourcesList);
            Assert.AreEqual(71, tileParser.TileStack.Count);
        }

        [TestMethod]
        public void TestEquals()
        {
            var field1 = new Field();
            var field2 = field1;
            Assert.AreEqual(field1, field2);
        }

        [TestMethod]
        public void TestGetResourcesWithParam()
        {
            TileParser parser2 = new TileParser(resourcesList);
            Console.WriteLine("parser2: " + parser2);
            foreach (var tile in parser2.TileStack)
            {
                Console.WriteLine("tile: " + tile);
            }
            Console.WriteLine("size: " + parser2.TileStack.Count);
            TileParser parser1 = new TileParser();
            Console.WriteLine("parser1: " + parser1);
            //Assert.IsTrue(parser1.TileStack.Intersect(parser2.TileStack).Any());
        }

    }
}
