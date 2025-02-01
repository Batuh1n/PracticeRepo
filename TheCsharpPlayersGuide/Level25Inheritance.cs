namespace ConsoleApp1;

// small part of Level 26 - Polymorphism
public class InventoryItem
{
    public int Weight { get; protected set; }
    public int Volume { get; protected set; }

    public InventoryItem()
    {
        Weight = 1;
        Volume = 1;
    }
    public InventoryItem(int weight, int volume)
    {
        Weight = weight;
        Volume = volume;
    }
}

public class Rope : InventoryItem
{

    public Rope()
    {
        Weight = 3;
        Volume = 20;
    }

    public Rope(int weight, int volume) : base(weight, volume)
    {
        
    }

    public override string ToString() => "Rope";
}

public class Sword : InventoryItem
{

    public Sword()
    {
        Weight = 10;
        Volume = 15;
    }
    public Sword(int weight, int volume) : base(weight, volume)
    {

    }
    
    public override string ToString() => "Sword";
}

public class Pack
{
    // works well
    // the author has a few more classes for items but meh, feeling lazy
    // https://csharpplayersguide.com/solutions/5th-edition/packing-inventory
    // mine is a bit worse
    public InventoryItem?[] Package { get; private set; }
    public int MaxWeight { get; }
    public int MaxVolume { get; }
    public int MaxItems { get; }
    
    public int CurrentVolume { get; private set; }
    public int CurrentWeight { get; private set; }
    public int CurrentItemCount { get; private set; }

    public Pack(int maxWeight, int maxVolume, int maxItems)
    {
        MaxWeight = maxWeight;
        MaxVolume = maxVolume;
        Package = new InventoryItem[maxItems];
        MaxItems = maxItems;
    }

    public override string ToString()
    {
        string content = "Pack containing";

        foreach (InventoryItem item in Package)
        {
            content += " " + item.ToString();
        }
        
        return content;
    }

    public bool Add(InventoryItem item)
    {
        if (CurrentItemCount == MaxVolume)
        {
            Console.WriteLine("Pack is full.");
            return false;
        }
        
        if (item.Weight + CurrentWeight > MaxWeight)
        {
            Console.WriteLine("Too heavy");
            return false;
        }

        if (item.Volume + CurrentVolume > MaxVolume)
        {
            Console.WriteLine("Too big");
            return false;
        }

        for (int i = 0; i < MaxItems; i++) // I think I could've just used "CurrentItemCount" for inserting stuff
        {                                  // into the pack, and not a for loop
            if (Package[i] == null)
            {
                Package[i] = item;
                CurrentItemCount++;
                CurrentVolume += item.Volume;
                CurrentWeight += item.Weight;
                return true;
            }
        }

        return false;
    }
}