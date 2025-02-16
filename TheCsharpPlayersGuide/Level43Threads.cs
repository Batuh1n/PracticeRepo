namespace ConsoleApp1;

// Mine is definitely more complicated than author's solution: https://csharpplayersguide.com/solutions/5th-edition/the-repeating-stream
public class RecentNumbers
{
    private object obj = new();
    public int Recent1
    {
        get {lock(obj) return field;}
        set
        {
            lock (obj)
            {
                Recent2 = field;
                field = value;
            }
        }
    }
    public int Recent2
    {
        get {lock(obj) return field;}
        set
        {
            lock (obj)
            {
                Recent3 = field;
                field = value;
            }
        }
    }

    public int Recent3
    {
        get {lock(obj) return field;}
        set {lock(obj) field = value;}
    }
}

public static class LoopForeverClass
{
    static Random random = new();
    public static void LoopForever(ref object? obj)
    {
        RecentNumbers recent;
        if (obj is RecentNumbers) recent = (RecentNumbers)obj;
        else return;

        while (true)
        {
            int i =  random.Next(0, 9);
            Console.WriteLine(i);
            recent.Recent1 = i;
            Thread.Sleep(1000);
        }
    }
}
/*
    For main:
            RecentNumbers recentNumbers = new RecentNumbers();
        Thread thread = new Thread((object? obj) => LoopForeverClass.LoopForever(ref obj));
        thread.Start(recentNumbers);

        while (true)
        {
            Console.ReadKey(false);
            if (recentNumbers.Recent1 == recentNumbers.Recent2 || recentNumbers.Recent2 == recentNumbers.Recent3
                || recentNumbers.Recent2 == recentNumbers.Recent3) Console.WriteLine("You found a duplicate!");
            else Console.WriteLine("That is not a duplciate");
                
*/