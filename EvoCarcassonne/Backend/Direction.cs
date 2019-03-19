namespace EvoCarcassonne.Backend
{
    public class Direction : IDirection
    {
        private int Id { get; set; }

        private Landscape Landscape { get; set; }

        private Figure Figure { get; set; }

        private IDirection Neighbor { get; set; }
    }
}
