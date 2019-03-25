namespace EvoCarcassonne.Backend
{
    public interface IDirection
    {
        int Id { get; set; }

        Landscape Landscape { get; set; }

        Figure Figure { get; set; }

        IDirection Neighbor { get; set; }
    }
}
