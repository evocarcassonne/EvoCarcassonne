using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DotNetCoreWebApi.Backend.Model
{
    public class Direction : IDirection
    {
        private IFigure _figure;

        public Landscape Landscape { get; set; }
        public ITile Neighbor { get; set; }
        public IFigure Figure
        {
            get => _figure;
            set
            {
                if (_figure != value)
                {
                    _figure = value;
                }
            }
        }

        public Direction() { }

        public Direction(Landscape landscape, IFigure figure)
        {
            Landscape = landscape;
            Figure = figure;
        }

        public Direction(Landscape landscape, IFigure figure, ITile neighbor)
        {
            Landscape = landscape;
            Figure = figure;
            Neighbor = neighbor;
        }
    }
}
