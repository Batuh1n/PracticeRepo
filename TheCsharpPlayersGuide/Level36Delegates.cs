namespace ConsoleApp1;

public class Sieve(Func<int, bool> method) // "Primary Constructor"
{
    public bool IsGood(int number) => method(number);
    
    public static bool EvenNumber(int number) => number % 2 == 0;
    public static bool OddNumber(int number) => number % 2 != 0;
    public static bool TenMultiple(int number) => number % 10 == 0;
}

// For Main:
/*
        Sieve newsomething = new(Sieve.TenMultiple);
        Console.WriteLine("Enter a number to test it out!");
        while (true)
            if (int.TryParse(Console.ReadLine(), out int x)) Console.WriteLine(newsomething.IsGood(x).ToString());
*/