namespace EvoCarcassonne.Backend
{
    public class Direction : IDirection
    {
        public int Id { get; set; }

        public ILandscape Landscape { get; set; }

        public IFigure Figure { get; set; }

        public IDirection Neighbor { get; set; }

        public Direction(int id, ILandscape landscape, IFigure figure, IDirection neighbor)
        {
            Id = id;
            Landscape = landscape;
            Figure = figure;
            Neighbor = neighbor;
        }

        public Direction(int id, ILandscape landscape, Figure figure)
        {
            Id = id;
            Landscape = landscape;
            Figure = figure;
        }

    }
}
