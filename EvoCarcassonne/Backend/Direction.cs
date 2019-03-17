namespace EvoCarcassonne.Backend
{
    class Direction : IDirection
    {
        public int Id { get; set; }

        public Landscape Landscape { get; set; }

        public Figure Figure { get; set; }

        public IDirection Neighbor { get; set; }
    }
}
