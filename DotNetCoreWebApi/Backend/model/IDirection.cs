namespace DotNetCoreWebApi.Backend.Model
{
    public interface IDirection
    {
        Landscape Landscape { get; set; }

        IFigure Figure { get; set; }

        ITile Neighbor { get; set; }

    }
}