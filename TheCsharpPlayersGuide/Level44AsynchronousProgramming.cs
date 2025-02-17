
namespace ConsoleApp1;


public static class WeirdMethods
{
    public static Random random = new();
    
    public static int RandomlyCreate(string word)
    {
        int wordLength = word.Length;
        int attempts = 0;
        string wordCreation = string.Empty;
        while (wordCreation != word)
        {
            attempts++;
            wordCreation = string.Empty;
            for (int i = 0; i < wordLength; i++)
            {
                wordCreation += (char)('a' + random.Next(26));
            }
        }

        return attempts;
    }

    public static async Task<int> RandomlyCreateAsync(string word)
    {
        return await Task.Run(() => RandomlyCreate(word));
    }

    public static async void Output(Task<int> task)
    {
        DateTime start = DateTime.Now;
        await task;
        int attempts = task.Result;
        Console.WriteLine($"attempts: {attempts}, time: {(DateTime.Now - start).TotalSeconds} seconds");
    }
}