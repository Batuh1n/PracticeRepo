namespace NO2_SnakeGame;

public static class CleanConsole
{
    public static void ClearConsoleLine()
    {
        int currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth)); 
        Console.SetCursorPosition(0, currentLineCursor);
    }

    public static void ClearConsoleLine(int selectx)
    {
        int currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(selectx, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth-selectx)); 
        Console.SetCursorPosition(selectx, currentLineCursor);
    }

    public static void ClearConsoleLine(int selectx, int selecty)
    {
        int currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(selectx, selecty);
        Console.Write(new string(' ', Console.WindowWidth-selectx)); 
        Console.SetCursorPosition(selectx, currentLineCursor);
    }
}