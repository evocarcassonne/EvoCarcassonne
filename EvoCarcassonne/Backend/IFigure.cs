namespace EvoCarcassonne.Backend
{
    public interface IFigure
    {
        IOwner Owner { get; set; }
        int Id { get; set; }
    }
}
