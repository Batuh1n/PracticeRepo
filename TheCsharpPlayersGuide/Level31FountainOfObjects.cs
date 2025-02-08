using System.Drawing;
using Microsoft.VisualBasic;

namespace ConsoleApp1;

// February 4-8th, 2025
// Expansions: "Small, Medium or Large",
public class TheFountainOfObjects
{
    Random random = new();
    private Room[,] Grid { get; init; }
    private Point GridSize { get; }
    public Room CurrentRoom
    {
        get => Grid[CurrentPosition.X, CurrentPosition.Y];
    }
    public Point CurrentPosition
    {
        get => field;
        set
        {
            if (ValidMove(value)) field = value; // somehow you can't access field if you use "=>" on setter
            else Console.WriteLine("Can't move there.");
        }
    }
    private FountainRoom Fountain { get; init; }
    private EntranceRoom Entrance { get; init; }

    public TheFountainOfObjects(Point XYSize)
    {
        Grid = new Room[XYSize.X, XYSize.Y];
        GridSize = XYSize;
        CurrentPosition = new Point(0, 0);
        Grid[random.Next(1, XYSize.X), random.Next(1, XYSize.Y)] = Fountain;
        Grid[0, 0] = Entrance;
        for (int i = 0; i < GridSize.X; i++)
            for (int i2 = 0; i < GridSize.Y; i++)
                Grid[i, i2] = new Room();
        
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
        if (input == "interact") CurrentRoom.Interact();
        if (input == "interact" && Fountain.FountainOn && CurrentRoom is EntranceRoom) Win();
    }
    private void MoveNorth() => CurrentPosition = new Point(CurrentPosition.X, CurrentPosition.Y + 1);
    private void MoveSouth() => CurrentPosition = new Point(CurrentPosition.X, CurrentPosition.Y - 1);
    private void MoveEast() => CurrentPosition = new Point(CurrentPosition.X - 1, CurrentPosition.Y);
    private void MoveWest() => CurrentPosition = new Point(CurrentPosition.X + 1, CurrentPosition.Y);

    
    private void OneRound()
    {
        Console.WriteLine("----------------------------------------------------------------------------------");
        Console.WriteLine($"You are in the room at {CurrentPosition.X}th Row, {CurrentPosition.Y}th Column");
        Console.WriteLine("What do you wish to do?"); // You don't usually smell and hear manually in IRL but here, you do.
        string choice = Console.ReadLine().ToLower();
        CheckAndCall(choice);
    }

    public void StartAndLoop()
    {
        Console.WriteLine("You are in a cavern full of rooms. You have to find the fountain and turn it on to leave.");
        Console.WriteLine("Valid inputs: see, hear, smell, move west/south/north/east");
        Console.ReadKey();
        bool gameLoop = true;
        while (gameLoop)
        {
            OneRound();
            
        }
    }

    public void Win()
    {
        // win blah blah
    }
}

public class Room
{
    public virtual string SeeSense { get; init; } = "You don't see much.";
    public virtual string SmellSense { get; init; } = "You don't smell anything.";
    public virtual string HearSense { get; init; } = "You don't hear anything.";

    public virtual void Interact()
    {
        
    }
}

public class EntranceRoom : Room
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
            ? "You hear water dripping in this room. The Fountain of Object is here!"
            : "You hear the rushing waters from the Fountain of Objects. It has been reactivated!";
        init {}
    }

    public override void Interact()
    {
        FountainOn = !FountainOn;
    }
}

