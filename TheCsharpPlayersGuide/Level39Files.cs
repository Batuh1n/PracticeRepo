namespace ConsoleApp1;

public class TheLongGame
{
    // Pretty much the same as author's solution.
    // https://csharpplayersguide.com/solutions/5th-edition/the-long-game
    public static void Run()
    {
        int score = 0;
        Console.WriteLine("Enter your name.");
        string name = Console.ReadLine();
        if (File.Exists($"{name}.txt"))
        {
            score = int.Parse(File.ReadAllText($"{name}.txt"));
        }
        while (true)
        {
            Console.WriteLine($"Score: {score}");
            ConsoleKey key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.Enter) break;
            score++;
        }
        File.WriteAllText($"{name}.txt", $"{score}");
    }
    
}