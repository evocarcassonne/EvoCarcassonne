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
    }
}
