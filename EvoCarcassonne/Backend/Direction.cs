namespace EvoCarcassonne.Backend
{
    public class Direction : IDirection
    {
        public int Id { get; set; }

        public Landscape Landscape { get; set; }

        public IFigure Figure { get; set; }

        public IDirection Neighbor { get; set; }

        public Direction(int id, Landscape landscape, IFigure figure, IDirection neighbor)
        {
            Id = id;
            Landscape = landscape;
            Figure = figure;
            Neighbor = neighbor;
        }

        public Direction(int id, Landscape landscape, Figure figure)
        {
            Id = id;
            Landscape = landscape;
            Figure = figure;
        }

        public Direction()
        {
        }
    }
}
