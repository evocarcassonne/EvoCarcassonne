namespace EvoCarcassonne.Models
{
    public class Coordinates
    {
        public int X { get; }
        public int Y { get; }

        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({ X },{ Y })";
        }

        public override bool Equals(object obj)
        {
            Coordinates other = (Coordinates)obj;
            return other != null && (X == other.X && Y == other.Y);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
