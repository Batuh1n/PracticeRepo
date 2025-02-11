namespace ConsoleApp1;

public class ExeptisGame
{
    private List<int> _numbers = new List<int>();
    private Random _random = new Random();
    private int _number;
    public ExeptisGame()
    {
        _number = _random.Next(0, 9);
    }

    public void Play()
    {
        try
        {
            while (true)
            {
                int input = int.Parse(Console.ReadLine());
                if (input == _number) throw new NotImplementedException("i forgot to implent winning");
                else if (input >= 0 && input <= 9) _numbers.Add(input); // Rider detects that exception as TODO item.
                if (_numbers.Contains(input))                           // Interesting. 
                    Console.WriteLine("Already chosen");
            }
        }
        catch (NotImplementedException)
        {
            Console.WriteLine("You win? You win!!! :3"); // Appearently you were supposed to lose, not win.
        }
    }
}
// Answer this question: Did you make a custom exception type or use an existing one, and why did
// you choose what you did?
// My answer: I used an existing one because it's technically not an implented feature, so I used that.

// You could write this program without exceptions, but the requirements
// demanded an exception for learning purposes. If you didn’t have that requirement, would you have
// used an exception? Why or why not?
// My answer: I wouldn't because.. it makes no sense to use exception here. 

// Solution: https://csharpplayersguide.com/solutions/5th-edition/exceptis-game