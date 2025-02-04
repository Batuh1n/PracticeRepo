namespace ConsoleApp1;

public class TheFountainOfObjects
{
    private Room[,] Grid { get; init; }

    public TheFountainOfObjects(int GridWidth, int GridHeight)
    {
        Grid = new Room[GridWidth, GridHeight];
    }
}

public abstract class Room
{
    public virtual string SeeSense { get; init; } = "You don't see much.";
    public virtual string SmellSense { get; init; } = "You don't smell anything.";
    public virtual string HearSense { get; init; } = "You don't hear anything.";
}

public class Entrance : Room
{
    public override string SeeSense { get; init; } =
        "You see light in this room coming from outside the cavern. This is the entrance.";
}

public class FountainRoom : Room
{
    public bool FountainOn { get; private set; }
    public override string HearSense
    {
        get => FountainOn
            ? "“You hear water dripping in this room. The Fountain of Objectn is here!"
            : "You hear the rushing waters from the Fountain of Objects. It has been reactivated!";
        init {}
    }

    public void TurnFountain()
    {
        FountainOn = !FountainOn;
    }
}

public class GridRooms // I'll probably delete this one
{
    public Room[,] Grid { get; init; }

    GridRooms(int width, int height)
    {
        Grid = new Room[width, height];
    }
}

public record struct Point(int X, int Y); // also this one
