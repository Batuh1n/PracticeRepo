namespace ConsoleApp1;


public class CharberryTree
{
    public event Action? Ripened;
    private Random _random = new Random();
    public bool Ripe { get; set; }
    public void MaybeGrow()
    {
        // Only a tiny chance of ripening each time, but we try a lot!
        if (_random.NextDouble() < 0.00000001 && !Ripe)
        {
            Ripe = true;
            Ripened?.Invoke();
        }
    }
}

public class Notifier
{
    public Notifier(CharberryTree tree) => tree.Ripened += OnRipened;

    public void OnRipened() => Console.WriteLine("The charberry tree has ripened!");
}

public class Harvester
{
    private CharberryTree _tree;

    public Harvester(CharberryTree tree)
    {
        tree.Ripened += OnRipened;
        _tree = tree;
    }
    

    public void OnRipened()
    {
        _tree.Ripe = false;
        Console.WriteLine("Tree has been harvested!");
    }
}

/* For Main:
        CharberryTree tree = new CharberryTree();
        Notifier notifier = new(tree);
        Harvester harvester = new(tree);
        while (true)
        {
            tree.MaybeGrow();
        }
*/