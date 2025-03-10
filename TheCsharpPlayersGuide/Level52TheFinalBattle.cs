namespace ConsoleApp1;

public class GameRPG
{
    List<ICharacter> Heroes { get; set; } = new List<ICharacter>();
    List<ICharacter> Monsters { get; set; } = new List<ICharacter>();


    public void Start()
    {
        
    }
    public void Round()
    {
        foreach (ICharacter hero in Heroes)
        {
            Console.WriteLine($"It is {hero.Name}'s turn...");
            Thread.Sleep(ICharacter.Rand.Next(400, 1700));
            DisplayInfo(hero.PickAction(Monsters));
            Thread.Sleep(500);
            Console.WriteLine();
        }

        foreach (ICharacter monster in Monsters)
        {
            Console.WriteLine($"It is {monster.Name}'s turn...");
            Thread.Sleep(ICharacter.Rand.Next(400, 1700));
            DisplayInfo(monster.PickAction(Heroes));
            Thread.Sleep(500);
            Console.WriteLine();
        }
    }

    public void DisplayInfo(ActionResult result)
    {
        if (result.Actor == null || result.ActionName == null) throw new NotImplementedException();
        string resultString = result switch
        {
            var z when z.Target == null => $"{z.Actor.Name} did {z.ActionName}.",
            var z when z.ActionName == ActionTypes.SimpleAttack => $"{z.Actor.Name} attacked {z.Target}!",
        };
        Console.WriteLine(resultString);
    }
}
public interface ICharacter
{
    public static Random Rand { get; } = new Random();
    int HP { get; }
    string Name { get; init; }
    Player Controller { get; init; }


    public ActionResult PickAction(List<ICharacter> characters);
    public void TakeDamage(int damage);
}
public enum ActionTypes { Nothing, SimpleAttack}

public class TrueProgrammer : ICharacter
{
    public int HP { get; private set; } = 50;
    public string Name { get; init; }
    public Player Controller { get; init; }

    public ActionResult PickAction(List<ICharacter> characters)
    {
        return null;
    }
    public void TakeDamage(int damage) => HP -= damage;
}
public class Skeleton : ICharacter
{
    public int HP { get; private set; } = 20;
    public string Name { get; init; } = "SKELETON";
    public Player Controller { get; init; }

    public Skeleton(Player controller) => Controller = controller;
    public ActionResult PickAction(List<ICharacter> characters)
    {
        ActionResult result = new() { Actor = this };
        int x = ICharacter.Rand.Next(0, 1);
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
        result.ActionName = ActionTypes.SimpleAttack;
        result.Actor = character;
        character.TakeDamage(2);
    } 
    public void TakeDamage(int damage) => HP -= damage;
}

public class Player
{
    public bool BotControlled { get; private set; }

    public int ReturnInput(uint amountOfChoices, string choiceInfo = null)
    {
        if (BotControlled) return ICharacter.Rand.Next(0, (int)amountOfChoices);
        Console.WriteLine(choiceInfo);
        int choiceIpnut = Convert.ToInt32(Console.ReadLine());
        while (!(choiceIpnut <= amountOfChoices && choiceIpnut > 0))
        {
            Console.WriteLine("Not a valid input!");
            choiceIpnut = Convert.ToInt32(Console.ReadLine());
        }
        return choiceIpnut;
    }
}

public class ActionResult
{
    public ICharacter? Actor { get; set; }
    public ICharacter? Target { get; set; }
    public ActionTypes ActionName { get; set; } = ActionTypes.Nothing;
}