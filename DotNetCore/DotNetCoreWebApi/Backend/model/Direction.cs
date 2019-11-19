using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DotNetCoreWebApi.Backend.Model
{
    public class Direction : IDirection
    {
        private IFigure _figure;

        public ILandscape Landscape { get; set; }
        public ITile Neighbor { get; set; }
        public IFigure Figure
        {
            get => _figure;
            set
            {
                if (_figure != value)
                {
                    _figure = value;
                    //OnPropertyChanged();
                }
            }
        }
        
        public Direction(){}

        public Direction(ILandscape landscape, IFigure figure)
        {
            Landscape = landscape;
            Figure = figure;
        }
        
        public Direction(ILandscape landscape, IFigure figure, ITile neighbor)
        {
            Landscape = landscape;
            Figure = figure;
            Neighbor = neighbor;
        }
    }
}
