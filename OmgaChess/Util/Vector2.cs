namespace OmgaChess.Util
{
    public struct Vector2
    {
        public int x, y;

        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }
        public static Vector2 operator *(Vector2 a, int b)
        {
            return new Vector2(a.x * b, a.y * b);
        }
        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return a.x == b.x && a.y == b.y;
        }
        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return !(a == b);
        }
        public static Vector2 up => new Vector2(0, 1);
        public static Vector2 down => new Vector2(0, -1);
        public static Vector2 left => new Vector2(-1, 0);
        public static Vector2 right => new Vector2(1, 0);

        public readonly static List<Vector2> verHoz = new List<Vector2>() { up, down, left, right };
        public readonly static List<Vector2> diag = new List<Vector2>() { up + right, up + left, down + right, down + left };
        public readonly static List<Vector2> allDir = verHoz.Concat(diag).ToList();
    }
}