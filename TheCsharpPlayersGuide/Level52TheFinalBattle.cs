namespace ConsoleApp1;

public class GameRPG
{
    List<ICharacter> Heroes { get; set; } = new List<ICharacter>();
    List<ICharacter> Monsters { get; set; } = new List<ICharacter>();


    public void Start()
    {
        
    }
    public void Round() // TODO
    {
        foreach (ICharacter hero in Heroes)
        {
            Console.WriteLine($"It is {hero.Name}'s turn...");
            Thread.Sleep(ICharacter.Rand.Next(400, 1700));
            //DisplayInfo(hero.PickAction(Monsters));
            Thread.Sleep(500);
            Console.WriteLine();
        }

        foreach (ICharacter monster in Monsters)
        {
            Console.WriteLine($"It is {monster.Name}'s turn...");
            Thread.Sleep(ICharacter.Rand.Next(400, 1700));
            //DisplayInfo(monster.PickAction(Heroes));
            Thread.Sleep(500);
            Console.WriteLine();
        }
    }
    
    
    public void PartyInfo(ICharacter player)
    {
        if (Heroes.Contains(player)) 
            foreach (ICharacter monster in Monsters) Console.WriteLine($"{Monsters.BinarySearch(monster)}: {monster.Name}" +
                                                                       $"({monster.CurrentHP} HP)");

        if (Monsters.Contains(player)) 
            foreach(ICharacter hero in  Heroes) Console.WriteLine($"{Monsters.BinarySearch(hero)}: {hero.Name}" +
                                                                  $"({hero.CurrentHP} HP)");
    }
}
public interface ICharacter
{
    public static Random Rand { get; } = new Random();
    public int MaxHP { get; init; }
    int CurrentHP { get; }
    string Name { get; init; }
    Player Controller { get; init; }


    public ActionResult PickAction(List<ICharacter> characters);
    public void TakeDamage(int damage);
}
public enum ActionTypes { Nothing, Attack}

public class TrueProgrammer : ICharacter
{
    public int MaxHP { get; init; } = 25;
    public int CurrentHP { get; private set; } = 25;
    public string Name { get; init; }
    public Player Controller { get; init; }
    
    public TrueProgrammer(string name) => Name = name;

    public ActionResult PickAction(List<ICharacter> characters)
    {
        ActionResult result = new() { Actor = this };
        int x = Controller.ReturnInput(2, "0: Do nothing.\n1:Do a simple attack.");
        switch (x)
        {
            case 0:
                break;
            case 1:
                SimpleAttack(characters[ICharacter.Rand.Next(0, characters.Count)], result);
                break;
        }
        return result;
    }
    public void TakeDamage(int damage) => CurrentHP -= damage;

    public void SimpleAttack(ICharacter character, ActionResult result)
    {
        result.ActionType = ActionTypes.Attack;
        result.ActionName = "PUNCH";
        result.Actor = character;
        result.DamageDealt = 1;
        character.TakeDamage(result.DamageDealt);
    }
}
public class Skeleton : ICharacter
{
    public int MaxHP { get; init; } = 5;
    public int CurrentHP { get; private set; } = 5;
    public string Name { get; init; } = "SKELETON";
    public Player Controller { get; init; }

    public Skeleton(Player controller) => Controller = controller;
    
    public ActionResult PickAction(List<ICharacter> characters)
    {
        ActionResult result = new() { Actor = this };
        int x = Controller.ReturnInput(2, "0: Do nothing.\n1:Do a simple attack.");
        switch (x)
        {
            case 0:
                break;
            case 1:
                SimpleAttack(characters[ICharacter.Rand.Next(0, characters.Count)], result);
                break;
        }
        return result;
    }

    public void SimpleAttack(ICharacter character, ActionResult result)
    {
        result.ActionType = ActionTypes.Attack;
        result.ActionName = "BONECRUNCH";
        result.Actor = character;
        result.DamageDealt = ICharacter.Rand.Next(0, 1);
        character.TakeDamage(result.DamageDealt);
    } 
    public void TakeDamage(int damage) => CurrentHP -= damage;
}

public class Player
{
    public bool BotControlled { get; private set; }
    
    public Player (bool botControlled) => BotControlled = botControlled;
    public int ReturnInput(uint amountOfChoices, string choiceInfo = null)
    {
        if (BotControlled) return ICharacter.Rand.Next(1, (int)amountOfChoices);
        Console.WriteLine(choiceInfo);
        int choiceInput = Convert.ToInt32(Console.ReadLine());
        while (!(choiceInput <= amountOfChoices && choiceInput > 0))
        {
            Console.WriteLine("Not a valid input!");
            choiceInput = Convert.ToInt32(Console.ReadLine());
        }
        return choiceInput;
    }
}

public class ActionResult
{
    public ICharacter? Actor { get; set; }
    public ICharacter? Target { get; set; }
    public ActionTypes ActionType { get; set; } = ActionTypes.Nothing;
    public string ActionName { get; set; } = "NOTHING";
    public int DamageDealt { get; set; }

    public void DisplayInfo()
    {
        if (Actor == null || ActionType == null) throw new NotImplementedException();
        if (Target == null) Console.WriteLine($"{Actor.Name} used {ActionType}.");
        else  Console.WriteLine($"{Actor.Name} used {ActionType} on {Target.Name}! ({DamageDealt} damage dealth --> " +
                                $"{Target.CurrentHP}/{Target.MaxHP})");
    }
}