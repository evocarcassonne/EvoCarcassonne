using System;
using System.Collections.Generic;
using EvoCarcassonne.Backend;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EvoCarcassonneUnitTests
{
    [TestClass]
    public class TileParserTests
    {
        private TileParser tileParser;

        [TestMethod]
        public void InitTileStack()
        {
            List<string> resources = new List<string>();
            resources.Add("S321014");
            resources.Add("T102020");
            resources.Add("T111113");
            resources.Add("T122021");
            resources.Add("T122123");
            resources.Add("T122221");
            resources.Add("T2001023");
            resources.Add("T202021");
            resources.Add("T220021");
            resources.Add("T221121");
            resources.Add("T222004");
            resources.Add("T2221213");
            resources.Add("T302024");
            resources.Add("T320020");
            resources.Add("T320114");
            resources.Add("T321104");
            resources.Add("T3211134");
            resources.Add("T321120");
            resources.Add("T322020");
            resources.Add("T400002");
            resources.Add("T401113");
            resources.Add("T520004");
            resources.Add("T810100");
            resources.Add("T900110");
            tileParser = new TileParser(resources);
        }

    }
}
