namespace EvoCarcassonne.Backend
{
    public interface IFigure
    {
        Owner Owner { get; set; }
        int Id { get; set; }
    }
}
