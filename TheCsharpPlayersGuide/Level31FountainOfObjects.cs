using System.Drawing;

namespace ConsoleApp1;

// February 4-9th, 2025
// Expansions: "Small, Medium or Large", "Pit"
// Solution: https://csharpplayersguide.com/solutions/5th-edition/fountain-of-objects-combined
//
// Author's solution is definitely better. I probably should've used interfaces more.
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
            else Console.WriteLine("Can't move there. Likely out of bounds.");
        }
    }

    private bool _ongoing = true;
    private FountainRoom Fountain { get; } = new();
    private EntranceRoom Entrance { get; } = new();
    private PitRoom Pit { get; } = new();

    public TheFountainOfObjects(Point XYSize)
    {
        Grid = new Room[XYSize.X + 1, XYSize.Y + 1];
        GridSize = XYSize;
        for (int x = 0; x <= GridSize.X; x++)
        {
            for (int y = 0; y <= GridSize.Y; y++) Grid[x, y] = new Room();
        }
        CurrentPosition = new Point(0, 0);
        Grid[random.Next(1, XYSize.X), random.Next(1, XYSize.Y)] = Fountain;
        bool isOccupied;
        for (int x = (int)Math.Round((double)(XYSize.X * XYSize.Y / 16)); x <= GridSize.X; x++)
        {
            do
            {
                isOccupied = false;
                Point randomLocation = new Point(random.Next(1, XYSize.X), random.Next(1, XYSize.Y));
                if (Grid[randomLocation.X, randomLocation.Y].GetType() == typeof(Room)) Grid[randomLocation.X, randomLocation.Y] = Pit;
                else isOccupied = true;
            } while (isOccupied);
        }
        Grid[0, 0] = Entrance;
        
    }

    private bool ValidMove(Point p)
    {
        bool isValid = true;
        if (p.X > GridSize.X || p.Y > GridSize.Y) isValid = false;
        if (p.X < 0 || p.Y < 0) isValid = false;
        return isValid;
    }

    private bool CheckForPits()
    {
        bool thereIsPit = false;
        for (int x = CurrentPosition.X - 1; x <= CurrentPosition.X + 1; x++)
        {
            for (int y = CurrentPosition.Y - 1; y <= CurrentPosition.Y + 1; y++)
            {
                Point coord = new Point(x, y);
                if (ValidMove(coord) && Grid[coord.X, coord.Y].GetType() == typeof(PitRoom)) thereIsPit = true;
            }
        }
        return thereIsPit;
    }
    
    private bool CheckAndCall(string input)
    {
        if (input == "see") Console.WriteLine(CurrentRoom.SeeSense);
        else if (input == "hear") Console.WriteLine(CurrentRoom.HearSense);
        else if (input == "smell") Console.WriteLine(CurrentRoom.SmellSense);
        else if (input == "move west" || input == "west") MoveWest();
        else if (input == "move north" || input == "north") MoveNorth();
        else if (input == "move east" || input == "east") MoveEast();
        else if (input == "move south" || input == "south") MoveSouth();
        else if (input == "interact") CurrentRoom.Interact();
        else if (input == "interact" && Fountain.FountainOn && CurrentRoom is EntranceRoom) Win();
        else Console.WriteLine("Invalid command");
        if (input == "move west" || input == "move north" || input == "move east" || input == "move south"
            || input == "west" || input == "north" || input == "east" || input == "south") return true;
        return false;
    }
    private void MoveNorth() => CurrentPosition = new Point(CurrentPosition.X, CurrentPosition.Y + 1);
    private void MoveSouth() => CurrentPosition = new Point(CurrentPosition.X, CurrentPosition.Y - 1);
    private void MoveEast() => CurrentPosition = new Point(CurrentPosition.X - 1, CurrentPosition.Y);
    private void MoveWest() => CurrentPosition = new Point(CurrentPosition.X + 1, CurrentPosition.Y);

    
    private void OneRound()
    {
        Console.WriteLine("----------------------------------------------------------------------------------");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"You are in the room at {CurrentPosition.X}th Row, {CurrentPosition.Y}th Column");
        Console.ForegroundColor = ConsoleColor.Green; 
        if (CurrentRoom.HearSense != "You don't hear anything.") Console.WriteLine(CurrentRoom.HearSense);
        else if (CurrentRoom.SmellSense != "You don't smell anything.") Console.WriteLine(CurrentRoom.SmellSense);
        else if (CurrentRoom.SeeSense != "You don't see much.") Console.WriteLine(CurrentRoom.SeeSense); 
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You don't notice anything interesting in this room");
        }

        Console.ForegroundColor = ConsoleColor.DarkRed;
        if (CheckForPits()) Console.WriteLine("You feel a draft. There is a pit in a nearby room.");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("What do you wish to do?");
        Console.ForegroundColor = ConsoleColor.White;
        string choice;
        do
        {
            choice = Console.ReadLine().ToLower();
        } while (!CheckAndCall(choice));
    }

    public void StartAndLoop()
    {
        Console.WriteLine("You are in a cavern full of rooms. You have to find the fountain and turn it on to leave.");
        Console.WriteLine("Valid inputs: see, hear, smell, move west/south/north/east, interact (interact with-");
        Console.WriteLine("whatever is in the room.");
        Console.ReadKey();
        while (_ongoing)
        {
            OneRound();
            if (CurrentRoom is PitRoom) Lose();
        }
    }

    private void Win()
    {
        _ongoing = false;
        Console.WriteLine("You win!!!");
        Console.ReadKey();
    }

    private void Lose()
    {
        _ongoing = false;
        Console.WriteLine("You lose.");
        Console.ReadKey();
    }
}

public class Room
{
    public virtual string SeeSense { get; init; } = "You don't see much.";
    public virtual string SmellSense { get; init; } = "You don't smell anything.";
    public virtual string HearSense { get; init; } = "You don't hear anything.";

    public virtual void Interact()
    {
        Console.WriteLine("There is nothing to interact with in this room.");
    }
}

public class EntranceRoom : Room
{
    public override string SeeSense { get; init; } =
        "You see light in this room coming from outside the cavern. This is the entrance.";

    public override void Interact()
    {
        
    }
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

public class PitRoom : Room
{
    
}


