using System.Collections.Generic;

namespace ConsoleApplication1
{
    public class JsonReaderObject
    {
        public List<GameType> carcassonne { get; set; }
    }

    public class GameType
    {
        public string gametype { get; set; }
        public List<string> defaultTiles { get; set; }
    }
}