namespace ConsoleApp1;

public class GameRPG
{
    List<ICharacter> Heroes { get; set; } = new List<ICharacter>();
    List<ICharacter> Monsters { get; set; } = new List<ICharacter>();

    
    public void Round()
    {
        
    }
}
public interface ICharacter
{
    int HP { get; }
    string Name { get; init; }


    public void PickAction(List<ICharacter> characters); 
    public static void DoNothing(){}
    public void TakeDamage(int damage);
}

public class Skeleton : ICharacter
{
    public int HP { get; private set; }
    public string Name { get; init; } = "SKELETON";

    public void PickAction(List<ICharacter> characters)
    {
        
    }
    public int SimpleAttack() => 1;
    public void TakeDamage(int damage) => HP -= damage;
}

public class ActionResult
{
    public ICharacter Actor { get; set; }
    public ICharacter Target { get; set; }
    public string ActionName { get; set; }
}