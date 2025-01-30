
namespace ConsoleApp1;

public class ThePoint
{
    public int X { get; set; }
    public int Y { get; set; }
    

    public ThePoint(int x, int y)
    {
        X = x;
        Y = y;
    }
    //Mine are mutable because they are bound to change. It's like a new data type, other classes and such can use it.
    //Those classes can set it as private or use this class like however they want. (I think?)
}

public class TheColor
{
    
    private int ValidateAndSetColor(int value, int field)
    {
        if (value >= 0 && value <= 255) return value;
        Console.WriteLine("Color is out of range.");
        return field;
    }
    //Claude suggested this for a shorter code, and it's nice! It'd be a bit longer otherwise.
    private int _r, _g, _b;
    public int R { get => _r; set => _r = ValidateAndSetColor(value, _r); }
    public int G { get => _g; set => _g = ValidateAndSetColor(value, _g); }
    public int B { get => _b; set => _b = ValidateAndSetColor(value, _b); }
    
    public (int r, int g, int b) GetColor => (R, G, B);
    public TheColor(int r, int g, int b)
    {
        R = r;
        G = g;
        B = b;
    }
    
    
    //Author has this approach in his solution:
    // public static Color White  { get; } = new Color(255,  255,  255);
    //which is, I think, better for memory and such. It creates it only once, whereas this one creates a new one every time it is called.
    // https://csharpplayersguide.com/solutions/5th-edition/the-color (he also has the variables immutable)
    public static TheColor White => new TheColor(255, 255, 255);
    public static TheColor Black => new TheColor(0, 0, 0);
    public static TheColor Red => new TheColor(255, 0, 0);
    public static TheColor Green => new TheColor(0, 255, 0);
    public static TheColor Blue => new TheColor(0, 0, 255);
    public static TheColor Yellow => new TheColor(255, 255, 0);
    public static TheColor Orange => new TheColor(255, 165, 0);
    public static TheColor Purple => new TheColor(128, 0, 128);
}

public class TheCard
{
    Colour CardColour { get; init; }
    Rank CardRank { get; init; }

    public string CardType
    {
        get
        {
            if ((int)CardRank < 10) return "number";
            return "symbol";
        }
    }

    public TheCard(Colour cardColour, Rank cardRank)
    {
        CardColour = cardColour;
        CardRank = cardRank;
    }

    //my code is better than the one in solution :3 
    //https://csharpplayersguide.com/solutions/5th-edition/the-card
    public static TheCard[] CreateDeck
    {
        get
        {
            TheCard[] deck = new TheCard[56];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 14; j++)
                {
                    deck[i * 14 + j] = new TheCard((Colour)i, (Rank)j);
                    Console.WriteLine($"{i * 14 + j}: The {(Colour)i} {(Rank)j}");
                }
            }
            return deck;
        }
    }
    //Answer to the question: Because they're different kind of colours..? RGB colours wouldn't be practical for this one.
}
public enum Colour { Red, Green, Blue, Yellow }
public enum Rank { One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Dollar, Percent, Caret, Ampersand }

public class TheLockerDoor
{
    public DoorState _state { get; private set; } = DoorState.Closed;
    private int _password { get; set; }

    public TheLockerDoor(int password)
    {
        _password = password;
        Console.WriteLine("The door is currently closed");
    }

    public void ChangePassword(int password, int newPassword)
    {
        if (password == _password)
        {
            _password = newPassword;
            Console.WriteLine("Password changed.");
        }
        else Console.WriteLine("Password doesn't match.");
    }

    public void Open()
    {
        if (_state == DoorState.Closed)
        {
            _state = DoorState.Open;
            Console.WriteLine("The door is now open");
        }
        else Console.WriteLine("The door is locked. You need to unlock it.");
    }

    public void Close()
    {
        if (_state == DoorState.Open)
        {
            _state = DoorState.Closed;
            Console.WriteLine("The door is now closed");
        }
        else Console.WriteLine("The door is already closed. Duh.");
    }

    public void Unlock(int password)
    {
        if (_state == DoorState.Locked)
        {
            if (password == _password)
            {
                _state = DoorState.Closed;
                Console.WriteLine("The door is now unlocked");
            }
            else Console.WriteLine("The passwords doesn't match. The door is still locked.");
        }
        else Console.WriteLine("The door is not locked.");
    }

    public void Lock()
    {
        if (_state == DoorState.Closed)
        {
            _state = DoorState.Locked;
            Console.WriteLine("The door is now locked");
        }
        else Console.WriteLine("The door is not closed and unlocked.");
    }
}
public enum DoorState { Open, Closed, Locked }

public class ThePasswordValidator
{
    public static bool IsValid(string password)
    {
        if (password.Length < 6 || password.Length > 13)
        {
            Console.WriteLine("The password is not longer than 6 characters or shorter than 13 characters.");
            return false;
        }

        bool containsUppercase = false;
        bool containsLowercase = false;
        bool containsNumber = false;
        bool containsTorAmpersand = false;
        foreach (char c in password)
        {
            if (char.IsUpper(c)) containsUppercase = true;
            if (char.IsLower(c)) containsLowercase = true;
            if (char.IsNumber(c)) containsNumber = true;
            if (c == 'T' || c == '&') containsTorAmpersand = true;
        }

        if (containsUppercase && containsLowercase && containsNumber && !containsTorAmpersand) return true;
        else return false;
    }
}

public class TicTacToe
{
    // Author's solution kind of looks more complicated?
    // https://csharpplayersguide.com/solutions/5th-edition/tic-tac-toe
    // He has multiple classes too. Dunno, I don't like his solution.
    private SquareState[] Board { get; set; } = new SquareState[9];
    private bool _firstPlayer = true;

    private int Player1Score { get; set; } 
    private int Player2Score { get; set; } 

    public void InitiateRound()
    {
        Console.Clear();
        Console.WriteLine(_firstPlayer ? "Player 1's turn" : "Player 2's turn");
        Console.WriteLine($"{Board[0]}|{Board[1]}|{Board[2]}");
        Console.WriteLine($"{Board[3]}|{Board[4]}|{Board[5]}");
        Console.WriteLine($"{Board[6]}|{Board[7]}|{Board[8]}");
        GetInputAndChange();
        _firstPlayer = !_firstPlayer;
        if (CheckForWinnerOrDraw() == GameStatus.Ongoing) InitiateRound();
        else DisplayWinner(CheckForWinnerOrDraw());
    }

    private void DisplayWinner(GameStatus statusInput)
    {
        if (statusInput == GameStatus.Draw) Console.WriteLine("The game is draw.");
        if (statusInput == GameStatus.Win1) Console.WriteLine("Player 1 wins");
        if (statusInput == GameStatus.Win2) Console.WriteLine("Player 2 wins");
        Console.ReadKey();
    }
    
    private void GetInputAndChange()
    {
        int input;
        do
        {
            input = int.Parse(Console.ReadLine());
            input--;
            if (Board[input] != SquareState._)
                Console.WriteLine("That square is already occupied.");
        } while (!(input >= 0 || input < Board.Length) || Board[input] != SquareState._);//My brain kind of fried while
        if (_firstPlayer) Board[input] = SquareState.O;                                  //writing this
        if (!_firstPlayer) Board[input] = SquareState.X;
    }
    
    private GameStatus CheckForWinnerOrDraw()
    {
        GameStatus x = GameStatus.Ongoing;
        bool isDraw = true;
        for (int i = 0; i < Board.Length; i++)
        {
            
            if ((i == 0 || i == 3 || i == 6) && Board[i] == Board[i + 1] && Board[i] == Board[i + 2] && Board[i] != SquareState._)
            {
                x = Board[i] == SquareState.O ? GameStatus.Win1 : GameStatus.Win2;
            }
            
            if ((i == 0 || i == 1 || i == 2) && Board[i] == Board[i + 3] && Board[i] == Board[i + 6] && Board[i] != SquareState._)
            {
                x = Board[i] == SquareState.O ? GameStatus.Win1 : GameStatus.Win2;
            }
            
            if (i == 0 && Board[i] == Board[i + 4] && Board[i] == Board[i + 8] && Board[i] != SquareState._)
            {
                x = Board[i] == SquareState.O ? GameStatus.Win1 : GameStatus.Win2;
            }
            
            
            if (i == 1 && Board[i] == Board[i + 2] && Board[i] == Board[i + 4] && Board[i] != SquareState._)
            {
                x = Board[i] == SquareState.O ? GameStatus.Win1 : GameStatus.Win2;
            }
        }
        
        foreach (SquareState s in Board)
        {
            if (s == SquareState._) isDraw = false;
        }
        if (x == GameStatus.Win1) Player1Score++;
        if (x == GameStatus.Win2) Player2Score++;
        if (isDraw) x = GameStatus.Draw;
        return x;
    }
}
internal enum SquareState { _, X, O}
internal enum GameStatus { Win1, Win2, Draw, Ongoing }