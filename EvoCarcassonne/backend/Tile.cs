using System.Collections.Generic;

namespace EvoCarcassonne.backend
{
    public class Tile : ITile
    {
        private List<IDirection> directions { get; set; }
        private SpecialProperty specialProperty { get; set; }


        public void rotate(IDirection direction)
        {
            return;
        }

        public IDirection getPropertyByIndex(int index)
        {
            if (index > 3)
            {
                return null;
            }
            else
            {
                return this.directions[index];
            }
        }
    }
}