using System;

namespace EvoCarcassonne.Backend
{
    public interface IOwner
    {
        int Id { get; set; }
        String Name { get; set; }
    }
}
