namespace Core.Spatial
{
    public class Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            return obj is Coordinate coordinate && X == coordinate.X && Y == coordinate.Y;
        }

        public override int GetHashCode()
        {
            return X * 37 + Y;
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", X, Y);
        }
    }

    public class Coordinate<T> : Coordinate
    {
        public T Data { get; set; }

        public Coordinate(int x, int y, T data) : base(x, y)
        {
            Data = data;
        }

        public override bool Equals(object obj)
        {
            return obj is Coordinate<T> coordinate && X == coordinate.X && Y == coordinate.Y
                && ((Data == null && coordinate.Data == null) || Data.Equals(coordinate.Data));
        }

        public override int GetHashCode()
        {
            return 47 * X + 37 * Y + Data.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("({0},{1}>{2})", X, Y, Data);
        }
    }
}
