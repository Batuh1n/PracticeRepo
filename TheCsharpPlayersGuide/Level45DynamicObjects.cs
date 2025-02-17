using System.Dynamic;
using System.Runtime.CompilerServices;

namespace ConsoleApp1;

public static class Adds
{
    public static dynamic Add(dynamic first, dynamic second)
    {
        return first + second;
    }
}

public static class MakeRobots
{
    public static void MakeLoop() // Very similar to authors solution: https://csharpplayersguide.com/solutions/5th-edition/the-robot-factory
    {
        int newID = 1;
        while (true)
        {
            dynamic variable = new ExpandoObject();
            variable.ID = newID++;
            Console.WriteLine("Do you want to name this robot?");
            if (Console.ReadLine()?.ToLower() == "yes") {Console.WriteLine("Name?"); variable.Name = Console.ReadLine();}
            
            Console.WriteLine("Does this robot have a specific size?");
            if (Console.ReadLine()?.ToLower() == "yes")
            {
                Console.WriteLine("Width?");
                variable.Width = Console.ReadLine();
                Console.WriteLine("Height?");
                variable.Height = Console.ReadLine();
            }
            
            Console.WriteLine("Does this robot need a specific color?");
            if (Console.ReadLine()?.ToLower() == "yes") {Console.WriteLine("Color?"); variable.Color = Console.ReadLine();}
            
            foreach (KeyValuePair<string, object> property in (IDictionary<string, object>)variable)
                Console.WriteLine($"{property.Key}: {property.Value}");
        }
    }
}
