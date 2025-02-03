namespace ConsoleApp1;
// pretty much the same as the solution 
// https://csharpplayersguide.com/solutions/5th-edition/colored-items

public class Sword3 { } // this is the third time I'm making a sword class (I think)
public class Bow3 { }
public class Axe3 { }

public class ColoredItem<T>
{
    public T Item { get; }
    public ConsoleColor Color { get; }

    public ColoredItem(T item, ConsoleColor color)
    {
        Item = item;
        Color = color;
    }

    public void Display()
    {
        ConsoleColor x = Console.ForegroundColor;
        Console.ForegroundColor = Color;
        Console.WriteLine(Item.ToString());
        Console.ForegroundColor = x;
    }
}