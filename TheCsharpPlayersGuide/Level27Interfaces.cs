namespace ConsoleApp1;

// uhh, it turns out it just wants me to change the one I made in Level 26
// I decided to just copy out everything onto this one and change their names

// 2025 February 1-2
// Includes Level 27 - Interfaces
public class Robot2
{
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsPowered { get; set; }
    public IRobotCommand?[] Commands { get; } = new IRobotCommand?[3];
    public void Run()
    {
        foreach (IRobotCommand? command in Commands)
        {
            command?.Run(this);
            Console.WriteLine($"[{X} {Y} {IsPowered}]");
        }
    }
}

public interface IRobotCommand
{
    void Run(Robot2 robot);
}

public class OnCommand2 : IRobotCommand
{
    public void Run(Robot2 robot)
    {
        robot.IsPowered = true;
    }
}

public class OffCommand2 : IRobotCommand
{
    public void Run(Robot2 robot)
    {
        robot.IsPowered = false;
    }
}

public class NorthCommand2 : IRobotCommand
{
    public void Run(Robot2 robot)
    {
        if(robot.IsPowered) robot.Y++;
    }
}

public class SouthCommand2 : IRobotCommand
{
    public void Run(Robot2 robot)
    {
        if(robot.IsPowered) robot.Y--;
    }
}

public class EastCommand2 : IRobotCommand
{
    public void Run(Robot2 robot)
    {
        if(robot.IsPowered) robot.X--;
    }
}

public class WestCommand2 : IRobotCommand
{
    public void Run(Robot2 robot)
    {
        if(robot.IsPowered) robot.X++;
    }
}

// Authors question:
// Answer this question: Do you feel this is an improvement over using an abstract base class? Why or why not?
//
// My answer: i think so