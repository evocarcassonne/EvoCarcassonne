using System;

namespace EvoCarcassonne.Backend
{
    public interface IOwner
    {
        string Name { get; set; }
        int Points { get; set; }
    }
}
