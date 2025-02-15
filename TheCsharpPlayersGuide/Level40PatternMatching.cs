namespace ConsoleApp1;

public class Potion
{
    public PotionType potionType { get; private set; }

    public void Add(PotionIngridient ingridient)
    {
        potionType = (potionType, ingridient) switch
        {
            (PotionType.Water, PotionIngridient.Stardust) => PotionType.Elixir,
            (PotionType.Elixir, PotionIngridient.SnakeVenom) => PotionType.Poison,
            (PotionType.Elixir, PotionIngridient.DragonBreath) => PotionType.Flying,
            (PotionType.Elixir, PotionIngridient.ShadowGlass) => PotionType.Invisibility,
            (PotionType.Elixir, PotionIngridient.EyeshineGem) => PotionType.NightSight,
            (PotionType.NightSight, PotionIngridient.ShadowGlass) => PotionType.Cloudy,
            (PotionType.Invisibility, PotionIngridient.EyeshineGem) => PotionType.Cloudy,
            (PotionType.Cloudy, PotionIngridient.Stardust) => PotionType.Wraith,
            _ => PotionType.Ruined
        };
        Console.WriteLine($"Your potion is now: {potionType.ToString()}");
    }

    public static PotionIngridient AskIngridient()
    {
        ListIngridients();
        string ingridientInput = Console.ReadLine().ToLower().Replace(" ", string.Empty);
        return ingridientInput switch
        {
            "water" => PotionIngridient.Water,
            "stardust" => PotionIngridient.Stardust,
            "snakevenom" => PotionIngridient.SnakeVenom,
            "dragonbreath" => PotionIngridient.DragonBreath,
            "shadowglass" => PotionIngridient.ShadowGlass,
            "eyeshinegem" => PotionIngridient.EyeshineGem,
            _ => AskIngridient()
        };
    }

    public static void ListIngridients() => Console.WriteLine("All ingridients: Water, Stardust, SnakeVenom," +
                                                              " DragonBreath, ShadowGlass, EyeshineGem");
}

public enum PotionType { Water, Elixir, Poison, Flying, Invisibility, NightSight, Cloudy, Wraith, Ruined}
public enum PotionIngridient { Water, Stardust, SnakeVenom, DragonBreath, ShadowGlass, EyeshineGem,}