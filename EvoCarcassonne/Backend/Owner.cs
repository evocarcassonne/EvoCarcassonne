using System;

namespace EvoCarcassonne.Backend
{
    public class Owner : IOwner
    {
        public int Id { get; set; }
        public String Name { get; set; }


        public Owner(int id, String name)
        {
            Id = id;
            Name = name;
        }
    }
}
