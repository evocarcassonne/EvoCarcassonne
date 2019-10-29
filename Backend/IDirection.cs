namespace Backend
{
    public interface IDirection
    {
        ILandscape Landscape { get; set; }

        IFigure Figure { get; set; }
        
        ITile Neighbor { get; set; }
        
    }
}
