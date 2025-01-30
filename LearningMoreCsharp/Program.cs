namespace ConsoleApp1;
//2025 January

//The C# Player's Guide's first boss battle
public class HuntingTheManticore
{
    private int _manticore;
    private int _city;
    private int _round;
    private int _distance;
    private int _cannonDamage;
    private int _cannonDistance;

    public HuntingTheManticore()
    {
        Start();
    }
    
    public void Start()
    {
        Console.WriteLine("First player, please choose Manticore's distance from the city!");
        _distance = int.Parse(Console.ReadLine());
        _manticore = 10;
        _city = 15;
        _round = 0;
        Console.Clear();
        Console.WriteLine("Player 2, it is your turn!\n-----------------------------------------------------------");
        NextRound();
    }

    void NextRound()
    {
        _round++;
        if (_round % 3 == 0 && _round % 5 == 0)
            _cannonDamage = 10;
        else if (_round % 3 == 0 || _round % 5 == 0)
            _cannonDamage = 3;
        else
            _cannonDamage = 1;
        Console.WriteLine($"STATUS: Round: {_round} City: {_city}/15 Manticore: {_manticore}/10");
        Console.WriteLine($"The cannon is expected to deal {_cannonDamage} damage this round.\nEnter desired cannon range:");
        _cannonDistance = int.Parse(Console.ReadLine());
        if (_cannonDistance > _distance) 
            Console.WriteLine("That round OVERSHOT the target.");
        else if (_cannonDistance < _distance)
            Console.WriteLine("That round FELL SHORT of the target.");
        else
        {
            Console.WriteLine("That round was a DIRECT HIT!");
            _manticore -= _cannonDamage;
        }

        if (_manticore >= 0)
        {
            _city--;
        }
        
        if (_manticore <= 0)
            End(false);
        else if(_city <= 0)
            End(true);
        else
            NextRound();
    }

    void End(bool mantiwin)
    {
        if (mantiwin)
            Console.WriteLine("Manticore wins.");
        else
            Console.WriteLine("City wins");
    }

}

public class Chest
{
    // creates a chest you can lock/unlock/open/close
    private ChestState _cheststate;
    
    //I just decided to copypaste this after trying to write a 30 lines long complex code that didn't work at all
    public void ChangeState()
    {
        while (true)
        {
            Console.Write($"The chest is {_cheststate}. What do you want to do? ");

            string input = Console.ReadLine();

            if (_cheststate == ChestState.Locked && input == "unlock") _cheststate = ChestState.Closed;
            if (_cheststate == ChestState.Closed && input == "open") _cheststate = ChestState.Open;
            if (_cheststate == ChestState.Open && input == "close") _cheststate = ChestState.Closed;
            if (_cheststate == ChestState.Closed && input == "lock") _cheststate = ChestState.Locked;
        }
    }
}

public class Foods
{
    (FoodType Type, MainIng Ingridient, Seasoning Seasoning ) Food;

    //makes something
    public void MakeFood()
    {
        Console.WriteLine("What kind of food would you like? Soup, stew, or gumbo?");
        string typeInput = Console.ReadLine().ToLower();
        if (typeInput == "soup") Food.Type = FoodType.Soup;
        if (typeInput == "stew") Food.Type = FoodType.Stew;
        if (typeInput == "gumbo") Food.Type = FoodType.Gumbo;
        Console.WriteLine("What ingridient would you like? Mushrooms, potatoes or carrots?");
        string ingridientInput = Console.ReadLine().ToLower();
        if (ingridientInput == "mushrooms") Food.Ingridient = MainIng.Mushrooms;
        if (ingridientInput == "potatoes") Food.Ingridient = MainIng.Potatoes;
        if (ingridientInput == "carrots") Food.Ingridient = MainIng.Carrots;
        Console.WriteLine("What kind of seasoning would you like? Sweet, salty or spicy?");
        string seasoningInput = Console.ReadLine().ToLower();
        if (seasoningInput == "sweet") Food.Seasoning = Seasoning.Sweet;
        if (seasoningInput == "salty") Food.Seasoning = Seasoning.Salty;
        if (seasoningInput == "spicy") Food.Seasoning = Seasoning.Spicy;
        Console.WriteLine($"A {Food.Seasoning} {Food.Ingridient} {Food.Type} has been made!");

    }
}

public class Arrow
{
    // solution in https://csharpplayersguide.com/solutions/5th-edition/arrow-factories is definitely a lot better
    ArrowHead HeadType { get; set; }
    ArrowFletching FletchingType { get; set; }
    float Length { get; set; } = 0;
    
    
    public Arrow(ArrowHead headtype, ArrowFletching fletchingtype, float length)
    {
        HeadType = headtype;
        FletchingType = fletchingtype;
        Length = length;
    }

    public float GetCost()
    {
        float price = 0;
        if (HeadType == ArrowHead.Obsidian) price += 5;
        if (HeadType == ArrowHead.Steel) price += 10;
        if (HeadType == ArrowHead.Wood) price += 3;
        
        if (FletchingType == ArrowFletching.Plastic) price += 10;
        if (FletchingType == ArrowFletching.GooseF) price += 3;
        if (FletchingType == ArrowFletching.TurkeyF) price += 5;
        price += (float)(Length * 0.05);
        return price;
    }
    
    public static Arrow CreateEliteArrow() => new Arrow(ArrowHead.Steel, ArrowFletching.Plastic, 95);
    public static Arrow CreateBeginnerArrow() => new Arrow(ArrowHead.Wood, ArrowFletching.GooseF, 75);
    public static Arrow CreateMarksmanArrow() => new Arrow(ArrowHead.Steel, ArrowFletching.GooseF, 65);
}

class Program
{
    static void Main()
    {

        TicTacToe newGame = new TicTacToe();
        newGame.InitiateRound();
        
        //'Start' being the starting number, it counts downwards till it's reached 1.
        void CountDownwards(int start)
        {
            if (start == 1) return;
            Console.WriteLine(start);
            start--;
            CountDownwards(start);
        }
        
    }
    
}


enum ChestState { Open, Closed, Locked }

//page 143 (163)
enum FoodType { Soup, Stew, Gumbo }
enum MainIng { Mushrooms, Carrots, Potatoes }
enum Seasoning { Spicy, Salty, Sweet }

//page 154 (174), arrow
public enum ArrowHead { Steel, Wood, Obsidian }
public enum ArrowFletching { Plastic, TurkeyF, GooseF }