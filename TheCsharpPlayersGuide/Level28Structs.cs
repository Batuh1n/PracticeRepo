namespace ConsoleApp1;

public struct Coordinate
{
    public int X { get; } // properties and fields should be immutable
    public int Y { get; }
    
    public Coordinate () {}

    public Coordinate(int x, int y)
    {
        X = x;
        Y = y;
    }

    // https://csharpplayersguide.com/solutions/5th-edition/room-coordinates
    public bool CheckAdjacent(Coordinate otherCoord) // Author has a static one, it's slightly different 
    {
        if (Math.Abs(X - otherCoord.X) == 1) return true;
        if (Math.Abs(Y - otherCoord.X) == 1) return true;
        return false;
    }
}