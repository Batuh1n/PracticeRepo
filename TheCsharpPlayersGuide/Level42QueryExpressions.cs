namespace ConsoleApp1;

public class TheThreeLenses
{
    public static IEnumerable<int> Procedural(int[] numbers)
    {
        List<int> result = new List<int>();
        foreach (int number in numbers) if(number % 2 == 0) result.Add(number);
        result.Sort();
        foreach (int number in result) yield return number*2;
    }

    public static IEnumerable<int> KeywordQuery(int[] numbers)
    {
        var result = from n in numbers
            where n % 2 == 0
            orderby n ascending
            select n*2;
        return result;
    }

    public static IEnumerable<int> MethodQuery(int[] numbers)
    {
        var result = numbers
            .Where(n => n % 2 == 0)
            .OrderBy(n => n)
            .Select(n => n * 2);
        return result;
    }
}

/*
 For Main Method:
 
        int[] something = new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        foreach (int n in TheThreeLenses.Procedural(something)) Console.Write(n + " ");
        Console.WriteLine();
        foreach (int n in TheThreeLenses.KeywordQuery(something)) Console.Write(n + " ");
        Console.WriteLine();
        foreach (int n in TheThreeLenses.MethodQuery(something)) Console.Write(n + " ");
*/