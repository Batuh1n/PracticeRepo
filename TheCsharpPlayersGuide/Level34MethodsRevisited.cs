namespace ConsoleApp1;

/*
 (Author's solution, it's just better)
 int number;
do
{
    Console.Write("Enter an integer: ");
} while (!int.TryParse(Console.ReadLine(), out number));
Console.WriteLine(number);

 */

public static class SaferNumberCrunching
{
    static int AskValidInt()
    {
        Console.WriteLine("Enter an number");
        string? input = Console.ReadLine();
        int result;
        while (int.TryParse(input, out result))
        {
            Console.WriteLine("Not valid.");
            input = Console.ReadLine();
        }

        return result;
    }
    
    static double AskValidDouble()
    {
        Console.WriteLine("Enter an number");
        string? input = Console.ReadLine();
        double result;
        while (!double.TryParse(input, out result))
        {
            Console.WriteLine("Not valid.");
            input = Console.ReadLine();
        }

        return result;
    }
    
    
}