using System.Globalization;

namespace ConsoleApp1;

public class GameRPG
{
    List<ICharacter> Heroes { get; set; } = new List<ICharacter>();
    List<ICharacter> Monsters { get; set; } = new List<ICharacter>();
    Player HeroPlayer { get; set; }
    Player MonstersPlayer { get; set; }
    public uint RoundNum { get; private set; }
    
    public void Start()
    {
        Console.WriteLine("Should the player of Heroes be AI controlled? \n0: Player\n 1: An AI");
        if (Console.ReadKey().KeyChar == '0') HeroPlayer = new(false);
        else HeroPlayer = new(true);
        Console.WriteLine("Should the player of Monsters be AI controlled? \n0: Player\n 1: An AI");
        if (Console.ReadKey().KeyChar == '0') MonstersPlayer = new(false);
        else MonstersPlayer = new(true);
        
        StartGameLoop();
    }

    public void StartGameLoop()
    {
        while (true)
        {
            RoundNum++;
            Monsters = RoundProgressEnemy(RoundNum);
            while (true)
            {
                Round();
                if (Heroes.Count == 0 || Monsters.Count == 0) break;
            }

            if (Heroes.Count == 0)
            {
                Console.WriteLine("The heroes have lost!");
                return;
            }

            if (Monsters.Count == 0)
            {
                Console.WriteLine("The monsters have been defeated, and a new battle shall bagin!");
            }
        }
    }
    public void Round()
    {
        foreach (ICharacter hero in Heroes)
        {
            Console.WriteLine($"It is {hero.Name}'s turn...");
            if (hero.Controller.BotControlled)Thread.Sleep(ICharacter.Rand.Next(400, 1700));
            hero.PickAction(Monsters).DisplayInfo();
            
            var MonsterRemoval = Monsters.Where(m => m.CurrentHP <= 0).ToList();
            foreach  (var monster in MonsterRemoval) Monsters.Remove(monster);
            
            Thread.Sleep(500);
            Console.WriteLine();
        }

        foreach (ICharacter monster in Monsters)
        {
            Console.WriteLine($"It is {monster.Name}'s turn...");
            Thread.Sleep(ICharacter.Rand.Next(400, 1700));
            monster.PickAction(Heroes).DisplayInfo();
            
            var MonsterRemoval = Heroes.Where(m => m.CurrentHP <= 0).ToList();
            foreach  (var hero in MonsterRemoval) Heroes.Remove(hero);
            
            Thread.Sleep(500);
            Console.WriteLine();
        }
    }
    
    
    private List<ICharacter> RoundProgressEnemy(uint round)
        => round switch
        {
            1 => new List<ICharacter>() { new Skeleton(MonstersPlayer)},
            2 => new List<ICharacter>() { new Skeleton(MonstersPlayer), new Skeleton(MonstersPlayer)},
        };
    
    public void PartyInfo()
    {
        var Current = Console.GetCursorPosition();
        
        Console.WriteLine("HEROES");
        int longest = 0, height = 1;
        foreach (var hero in Heroes)
        {
            height++;
            string info = $"{Heroes.BinarySearch(hero)}: {hero.Name} ({hero.CurrentHP}/{hero.MaxHP})";
            if (info.Length > longest) longest = info.Length;
            Console.WriteLine(info);
        }
        // TODO
        foreach (var monster in Monsters)
        {
            
        }
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

    public TrueProgrammer(Player player, string name)
    {
        Name = name;
        Controller = player;
    }

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
        result.ActionName = "PUNCH";
        result.Actor = character;
        result.DamageDealt = 1;
        character.TakeDamage(result.DamageDealt);
    }
    
    public void TakeDamage(int damage)
    {
        if (CurrentHP - damage < 0) CurrentHP = 0;
        CurrentHP -= damage;
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

    public void TakeDamage(int damage)
    {
        if (CurrentHP - damage < 0) CurrentHP = 0;
        CurrentHP -= damage;
    }
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