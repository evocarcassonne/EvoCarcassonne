using System.ComponentModel;

namespace EvoCarcassonne.Backend
{
    public interface IDirection : INotifyPropertyChanged
    {
        int Id { get; set; }

        ILandscape Landscape { get; set; }

        IFigure Figure { get; set; }

        IDirection Neighbor { get; set; }
    }
}
