using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EvoCarcassonne.Backend
{
    public class Figure : IFigure
    {
        public int Id { get; set; }

        public IOwner Owner { get; set; }

        public Figure(int id, IOwner owner)
        {
            Id = id;
            Owner = owner;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
