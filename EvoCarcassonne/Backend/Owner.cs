using System;

namespace EvoCarcassonne.Backend
{
    public class Owner : IOwner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Points { get; set; } = 0;

        public Owner(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
