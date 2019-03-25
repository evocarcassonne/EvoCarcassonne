namespace EvoCarcassonne.Backend
{
    public class Figure : IFigure
    {
        public int Id { get; set; }
        public Owner Owner { get; set; }

        public Figure(int id, Owner owner)
        {
            Id = id;
            Owner = owner;
        }
    }
}
