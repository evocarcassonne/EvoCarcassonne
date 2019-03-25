namespace EvoCarcassonne.Backend
{
    public class Direction : IDirection
    {
        public int Id { get; set; }

        public Landscape Landscape { get; set; }

        public Figure Figure { get; set; }

        public IDirection Neighbor { get; set; }

        public Direction(int id, Landscape landscape, Figure figure, IDirection neighbor)
        {
            Id = id;
            Landscape = landscape;
            Figure = figure;
            Neighbor = neighbor;
        }
    }
}
