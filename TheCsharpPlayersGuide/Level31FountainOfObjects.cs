using System.Drawing;

namespace ConsoleApp1;

// February 4-6th, 2025
public class TheFountainOfObjects
{
    private Room[,] Grid { get; init; }
    private Point GridSize { get; }
    public Room CurrentRoom
    {
        get => Grid[CurrentPosition.X, CurrentPosition.Y];
        init => field = value;
    }

    public Point CurrentPosition
    {
        get => field;
        set
        {
            if (ValidMove(value)) field = value; // somehow you can't access field if you use "=>"
            else Console.WriteLine("Can't move there.");
        }
    }

    public TheFountainOfObjects(Point XYSize)
    {
        Grid = new Room[XYSize.X, XYSize.Y];
        GridSize = XYSize;
        CurrentRoom = Grid[0, 0];
        CurrentPosition = new Point(0, 0);
    }

    private bool ValidMove(Point p)
    {
        bool isValid = false;
        if (Math.Abs(CurrentPosition.X - p.X) == 1 && p.Y - CurrentPosition.Y == 0) isValid = true;
        if (Math.Abs(CurrentPosition.Y - p.Y) == 1 && p.Y - CurrentPosition.Y == 0) isValid = true;
        if (p.X > GridSize.X || p.Y > GridSize.Y) isValid = false;
        return isValid;
    }

    private void CheckAndCall(string input)
    {
        if (input == "see") Console.WriteLine(CurrentRoom.SeeSense);
        if (input == "hear") Console.WriteLine(CurrentRoom.HearSense);
        if (input == "smell") Console.WriteLine(CurrentRoom.SmellSense);
        if (input == "move west") MoveWest();
        if (input == "move north") MoveNorth();
        if (input == "move east") MoveEast();
        if (input == "move south") MoveSouth();
    }
    private void MoveNorth() => CurrentPosition = new Point(CurrentPosition.X, CurrentPosition.Y + 1);
    private void MoveSouth() => CurrentPosition = new Point(CurrentPosition.X, CurrentPosition.Y - 1);
    private void MoveEast() => CurrentPosition = new Point(CurrentPosition.X - 1, CurrentPosition.Y);
    private void MoveWest() => CurrentPosition = new Point(CurrentPosition.X + 1, CurrentPosition.Y);

    public void OneRound()
    {
        Console.WriteLine($"You are in the room at {CurrentPosition.X}th Row, {CurrentPosition.Y}th Column");
        Console.WriteLine("What do you wish to do?");
        string choice = Console.ReadLine().ToLower();
        if (choice == "see") Console.WriteLine(CurrentRoom.SeeSense);
        if (choice == "hear") Console.WriteLine(CurrentRoom.HearSense);
        if (choice == "smell") Console.WriteLine(CurrentRoom.SmellSense);
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

