namespace Core.Spatial
{
    public class Coordinate3D : Coordinate
    {
        public int Z { get; set; }

        public Coordinate3D(int x, int y, int z) : base(x, y)
        {
            Z = z;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Coordinate3D;
            return other != null && X == other.X && Y == other.Y && Z == other.Z;
        }

        public override int GetHashCode()
        {
            return 47 * X + 37 * Y + Z;
        }

        public override string ToString()
        {
            return string.Format("({0},{1},{2})", X, Y, Z);
        }
    }

    public class Coordinate3D<T> : Coordinate3D
    {
        public T Data { get; set; }

        public Coordinate3D(int x, int y, int z, T data) : base(x, y, z)
        {
            Data = data;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Coordinate3D<T>;
            return other != null && X == other.X && Y == other.Y && Z == other.Z
                && ((Data == null && other.Data == null) || Data.Equals(other.Data));
        }

        public override int GetHashCode()
        {
            return 107 * X + 47 * Y + 37 * Z + Data.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("({0},{1},{2}>{3})", X, Y, Z, Data);
        }
    }
}
