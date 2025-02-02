namespace ConsoleApp1;

// 2025 February 1-2
public class Robot
{
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsPowered { get; set; }
    public RobotCommand?[] Commands { get; } = new RobotCommand?[3];
    public void Run()
    {
        foreach (RobotCommand? command in Commands)
        {
            command?.Run(this);
            Console.WriteLine($"[{X} {Y} {IsPowered}]");
        }
    }
}

public abstract class RobotCommand
{
    public abstract void Run(Robot robot);
}

public class OnCommand : RobotCommand
{
    public override void Run(Robot robot)
    {
        robot.IsPowered = true;
    }
}

public class OffCommand : RobotCommand
{
    public override void Run(Robot robot)
    {
        robot.IsPowered = false;
    }
}

public class NorthCommand : RobotCommand
{
    public override void Run(Robot robot)
    {
        if(robot.IsPowered) robot.Y++;
    }
}

public class SouthCommand : RobotCommand
{
    public override void Run(Robot robot)
    {
        if(robot.IsPowered) robot.Y--;
    }
}

public class EastCommand : RobotCommand
{
    public override void Run(Robot robot)
    {
        if(robot.IsPowered) robot.X--;
    }
}

public class WestCommand : RobotCommand
{
    public override void Run(Robot robot)
    {
        if(robot.IsPowered) robot.X++;
    }
}

/*
Robot robot1 = new Robot();
Console.WriteLine("Enter 3 commands for the robot to run.\nOn\nOff\nGo south/north/east/west");
string[] inputs = new string[3] { Console.ReadLine().ToLower(), Console.ReadLine().ToLower(), Console.ReadLine().ToLower() };
int current = 0;
foreach (string input in inputs)
{
    switch (input)
    {
        case "on":
            robot1.Commands[current] = new OnCommand();
            current++;
            break;
        case "off":
            robot1.Commands[current] = new OffCommand();
            current++;
            break;
        case "go south":
            robot1.Commands[current] = new SouthCommand();
            current++;
            break;
        case "go north":
            robot1.Commands[current] = new NorthCommand();
            current++;
            break;
        case "go east":
            robot1.Commands[current] = new EastCommand();
            current++;
            break;
        case "go west":
            robot1.Commands[current] = new WestCommand();
            current++;
            break;
        default:
            Console.WriteLine("Not a recognized command");
            break;
    }
}
robot1.Run();
*/