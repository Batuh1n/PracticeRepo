using System.Globalization;

namespace ConsoleApp1;

/* I basically got bored with this one. It was kind of bad anyways...
 * I've looked up the solutions on the author's site, they're a lot better.
 * https://csharpplayersguide.com/solutions/5th-edition/
 * ^Level 52
 */
public class GameRPG
{
    List<ICharacter> Heroes { get; set; } = new List<ICharacter>();
    List<ICharacter> Monsters { get; set; } = new List<ICharacter>();
    Player HeroPlayer { get; set; }
    Player MonstersPlayer { get; set; }
    public uint RoundNum { get; private set; }

    public void Start()
    {
        Console.SetWindowSize(70, 40);
        Console.SetBufferSize(100, 40);
        Console.WriteLine("Should the player of Heroes be AI controlled? \n0: Player\n1: An AI");
        if (Console.ReadKey(true).KeyChar == '0') HeroPlayer = new(false);
        else HeroPlayer = new(true);
        Console.WriteLine("Should the player of Monsters be AI controlled? \n0: Player\n1: An AI");
        if (Console.ReadKey(true).KeyChar == '0') MonstersPlayer = new(false);
        else MonstersPlayer = new(true);

        if (HeroPlayer.BotControlled == false)
        {
            Console.WriteLine("Player of Heroes, choose a name for your character.");
            Heroes.Add(new TrueProgrammer(HeroPlayer, Console.ReadLine()));
        }
        else Heroes.Add(new TrueProgrammer(HeroPlayer, "True Programmer"));
        
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
                Console.WriteLine("\nNext turn is starting...");
                Thread.Sleep(1500);
                Console.Clear();
                
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
        PartyInfo();
        foreach (ICharacter hero in Heroes)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"It is {hero.Name}'s turn...");
            if (hero.Controller.BotControlled) Thread.Sleep(ICharacter.Rand.Next(400, 1700));
            hero.PickAction(Monsters).DisplayInfo();

            var MonsterRemoval = Monsters.Where(m => m.CurrentHP <= 0).ToList();
            foreach (var monster in MonsterRemoval) Monsters.Remove(monster);

            Thread.Sleep(500);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }

        foreach (ICharacter monster in Monsters)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"It is {monster.Name}'s turn...");
            Thread.Sleep(ICharacter.Rand.Next(400, 1700));
            monster.PickAction(Heroes).DisplayInfo();

            var MonsterRemoval = Heroes.Where(m => m.CurrentHP <= 0).ToList();
            foreach (var hero in MonsterRemoval) Heroes.Remove(hero);

            Thread.Sleep(500);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }
    }


    private List<ICharacter> RoundProgressEnemy(uint round)
        => round switch
        {
            1 => new List<ICharacter>() { new Skeleton(MonstersPlayer) },
            2 => new List<ICharacter>() { new Skeleton(MonstersPlayer), new Skeleton(MonstersPlayer) },
        };

    public void PartyInfo()
    {
        var Current = Console.GetCursorPosition();

        Console.SetCursorPosition(Current.Left, Current.Top);
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.Write("HEROES");
        int heroLongest = 6, heroTallest = 1;
        foreach (ICharacter hero in Heroes)
        {
            Console.SetCursorPosition(0, Current.Top + heroTallest);
            string infoText = $"{Heroes.IndexOf(hero)}: {hero.Name} ({hero.CurrentHP}/{hero.MaxHP})";
            if (infoText.Length > heroLongest) heroLongest = infoText.Length;
            Console.Write(infoText);
            heroTallest++;
        }
        
        Console.SetCursorPosition(heroLongest + 2, Current.Top);
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.Write("MONSTERS");
        int monsterTallest = 1;
        foreach (var monster in Monsters)
        {
            Console.SetCursorPosition(heroLongest + 2, Current.Top + monsterTallest);
            string infoText = $"{Monsters.IndexOf(monster)}: {monster.Name} ({monster.CurrentHP}/{monster.MaxHP})";
            Console.Write(infoText);
            monsterTallest++;
        }

        Console.ForegroundColor = ConsoleColor.DarkGray;
        
        int tallest;
        if (heroTallest >= monsterTallest) tallest = heroTallest;
        else tallest = monsterTallest;
        for (int i = 0; i <= tallest; i++)
        {
            Console.SetCursorPosition(Current.Left + heroLongest + 1, Current.Top + i );
            Console.Write("|");
        }
        Console.SetCursorPosition(0, Current.Top + tallest);
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine();
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

        int minAction = Controller.BotControlled ? 1 : 0;
        int action = Controller.ReturnInput(minAction, 2, "0: Do nothing.\n1:Do a simple attack.");
        
        int choice = 0;
        if (action != 0)
            choice = Controller.ReturnInput(0, characters.Count, "Choose a character from the enemy to attack.");
        switch (action)
        {
            case 0:
                break;
            case 1:
                SimpleAttack(characters[choice], result);
                break;
        }
        return result;
    }

    public void SimpleAttack(ICharacter character, ActionResult result)
    {
        result.ActionType = ActionTypes.Attack;
        result.ActionName = "PUNCH";
        result.Target = character;
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

        int minAction = Controller.BotControlled ? 1 : 0;
        int action = Controller.ReturnInput(minAction, 2, "0: Do nothing.\n1:Do a simple attack.");
        
        int choice = 0;
        if (action != 0)
            choice = Controller.ReturnInput(0, characters.Count, "Choose a character from the enemy to attack.");
        
        switch (action)
        {
            case 0:
                break;
            case 1:
                SimpleAttack(characters[choice], result);
                break;
        }
        return result;
    }

    public void SimpleAttack(ICharacter character, ActionResult result)
    {
        result.ActionType = ActionTypes.Attack;
        result.ActionName = "BONECRUNCH";
        result.Target = character;
        result.DamageDealt = ICharacter.Rand.Next(0, 2);
        character.TakeDamage(result.DamageDealt);
    }

    public void TakeDamage(int damage)
    {
        if (CurrentHP - damage < 0) CurrentHP = 0;
        CurrentHP -= damage;
    }
}

public class UncodedOne : ICharacter
{
    public int MaxHP { get; init; } = 5;
    public int CurrentHP { get; private set; } = 15;
    public string Name { get; init; } = "THE UNCODED ONE";
    public Player Controller { get; init; }

    public UncodedOne(Player controller) => Controller = controller;
    
    public ActionResult PickAction(List<ICharacter> characters)
    {
        ActionResult result = new() { Actor = this };

        int minAction = Controller.BotControlled ? 1 : 0;
        int action = Controller.ReturnInput(minAction, 2, "0: Do nothing.\n1:Do a simple attack.");
        
        int choice = 0;
        if (action != 0)
            choice = Controller.ReturnInput(0, characters.Count, "Choose a character from the enemy to attack.");
        
        switch (action)
        {
            case 0:
                break;
            case 1:
                SimpleAttack(characters[choice], result);
                break;
        }
        return result;
    }

    public void SimpleAttack(ICharacter character, ActionResult result)
    {
        result.ActionType = ActionTypes.Attack;
        result.ActionName = "UNRAVELING ATTACK";
        result.Target = character;
        result.DamageDealt = ICharacter.Rand.Next(0, 3);
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
    public int ReturnInput(int lowerLimit, int upperLimit, string choiceInfo = null)
    {
        if (BotControlled) return ICharacter.Rand.Next(lowerLimit, upperLimit);
        Console.WriteLine(choiceInfo);
        int.TryParse(Console.ReadLine(), out int choiceInput);
        while (!(choiceInput < upperLimit && choiceInput >= lowerLimit || choiceInput.GetType() == typeof(int)))
        {
            Console.WriteLine("Not a valid input!");
            int.TryParse(Console.ReadLine(), out choiceInput);
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
        else  Console.WriteLine($"{Actor.Name} used {ActionType} on {Target.Name}! ({DamageDealt} damage was dealt --> " +
                                $"{Target.CurrentHP}/{Target.MaxHP})");
    }
}