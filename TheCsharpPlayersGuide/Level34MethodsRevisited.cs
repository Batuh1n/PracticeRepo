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

public static class BetterRandom
{
    public static double NextDouble(this Random random, double max = 1)
    => random.NextDouble() * max;

    public static string RandomString(this Random random, params string[] inputs)
    => inputs[random.Next(0, inputs.Length - 1)];

    public static bool CoinToss(this Random random, uint trueChance, uint falseChance)
    {
        double randomNumber = random.Next(0, 100);
        if (randomNumber * (trueChance + falseChance) / 2 <= 50 * trueChance) return true;
        return false;
    }
    // seems like author made a simple
    //   public static bool CoinFlip(this Random random, double probabilityOfHeads = 0.5) => random.NextDouble() < probabilityOfHeads;
    // but, what if they enter more than 1? Mine handles all of that.
}