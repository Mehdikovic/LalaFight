namespace LalaFight
{
    [System.Serializable]
    public struct Coord
    {
        public int x;
        public int y;

        public Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static bool operator ==(Coord leftCoord, Coord rightCoord)
        {
            return leftCoord.x == rightCoord.x && leftCoord.y == rightCoord.y;
        }

        public static bool operator !=(Coord leftCoord, Coord rightCoord)
        {
            return !(leftCoord == rightCoord);
        }

        public override bool Equals(object obj)
        {
            return this == (Coord)obj;
        }

        public override int GetHashCode()
        {
            return this.x * 1000 + this.y;
        }

        public override string ToString()
        {
            return "x: " + this.x + "   y:" + this.y;
        }
    }
}