namespace EvoCarcassonne.Backend
{
    public class Owner : IOwner
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public Owner(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
