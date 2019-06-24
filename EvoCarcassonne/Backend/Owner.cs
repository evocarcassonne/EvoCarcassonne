using System;

namespace EvoCarcassonne.Backend
{
    public class Owner : IOwner
    {
        public string Name { get; set; }

        public int Points { get; set; } = 0;

        public Owner(string name)
        {
            Name = name;
        }
        
        public override bool Equals(object obj)
        {
            var other = (Owner)obj;
            return Points == other.Points && Name == other.Name;
        }
    }
}
