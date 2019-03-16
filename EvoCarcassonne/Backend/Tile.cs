using System.Collections.Generic;

namespace EvoCarcassonne.Backend
{
    public class Tile : ITile
    {
        private List<IDirection> Directions { get; set; }
        private SpecialProperty SpecialProperty { get; set; }


        public void Rotate(IDirection direction)
        {
            return;
        }

        public IDirection GetPropertyByIndex(int index)
        {
            if (index > 3)
            {
                return null;
            }
            else
            {
                return this.Directions[index];
            }
        }
    }
}
