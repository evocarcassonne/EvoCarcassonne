using System.Collections.Generic;

namespace Backend.Model
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