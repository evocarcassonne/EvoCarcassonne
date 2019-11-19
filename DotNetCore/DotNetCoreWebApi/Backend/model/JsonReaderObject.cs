using System.Collections.Generic;

namespace DotNetCoreWebApi.Backend.Model
{
    public class JsonReaderObject
    {
        public List<GameType> carcassonne { get; set; } = new List<GameType>();
    }

    public class GameType
    {
        public string gametype { get; set; }
        public List<string> defaultTiles { get; set; }
    }
}