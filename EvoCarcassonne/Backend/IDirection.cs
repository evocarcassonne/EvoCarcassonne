namespace EvoCarcassonne.Backend
{
    public interface IDirection
    {
        ILandscape Landscape { get; set; }

        IFigure Figure { get; set; }
    }
}
