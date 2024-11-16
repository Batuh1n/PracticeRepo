using System.Globalization;

namespace NO2_SnakeGame;

using System;

record struct Coord(int X, int Y) //Structure for coordinations
{
    public static Coord operator +(Coord a, Coord b) => new Coord(a.X + b.X, a.Y + b.Y);
}

sealed class Drawing
{
    public static void DrawSn(Coord coord, ConsoleColor color)
    {
        ConsoleColor old = Console.ForegroundColor;
        Console.BackgroundColor = color;
        Console.SetCursorPosition(coord.X*2, coord.Y);
        Console.Write("  ");
        Console.BackgroundColor = old;
    }
}
public class Game
{
    readonly int _width;
    readonly int _height;
    private ConsoleKey _ckey;
    private Coord _cdirection;
    private bool _stop;
    private bool _gameover = false;
    private LinkedList<Coord> _snakeCoords;
    private Coord _foodposition;
    public int Score;
    private float _gamespeed;

    private readonly Dictionary<ConsoleKey, Coord> _movementKeys = new Dictionary<ConsoleKey, Coord>()
    {
        { ConsoleKey.A, new(-1, 0) },
        { ConsoleKey.D, new(1, 0) },
        { ConsoleKey.W, new(0, -1) },
        { ConsoleKey.S, new(0, 1) }
    };

    public Game(int width, int height, float gamespeed)
    {
        _width = width;
        _height = height;
        _gamespeed = gamespeed;
        _snakeCoords = new LinkedList<Coord>();
        _snakeCoords.AddFirst(new Coord(_width, _height/2));
        SetGrid();
        InputHandler().Start();
        GenerateFood();
        Console.CursorVisible = false;
        Update();
    }
    
    public void SetGrid()
    {
        Console.Clear();
        for (int ii = 0; ii < _height; ii++)
        {
            for (int j = 0; j < 2*_width; j++)
            {
                Console.SetCursorPosition(j, ii);
                if (j == 0 || j == 1 || ii == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\u2588");
                }
                else if (ii == _height - 1 || j == 2*_width - 1 || j == 2*_width - 2)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\u2588");
                }
                else{Console.Write("\u2588");}
                Console.ForegroundColor = ConsoleColor.Black;
            }
        }

        foreach (var pos in _snakeCoords)
        {
            Drawing.DrawSn(pos, ConsoleColor.Green);
        }
    }

    private Thread InputHandler() =>
        new(() =>
        {
            while (!_stop)
            {
                _ckey = Console.ReadKey(true).Key;
                if (_ckey == ConsoleKey.Q) //Stops The Game
                {
                    _stop = true;
                    break;
                }

                if (!(_movementKeys[_ckey] + _cdirection == new Coord(0, 0)))
                {
                    _cdirection = _movementKeys[_ckey];
                }
            }

            ConsoleKey inp2;
            while (_stop && !_gameover)
            {
                Console.SetCursorPosition(_width*2+1, 1);
                Console.Write("You have paused the game.\nIf you wish to continue, press Q again or R to restart.");
                do
                {
                    inp2 = Console.ReadKey(true).Key;
                    if (inp2 == ConsoleKey.Q)
                    {
                        CleanConsole.ClearConsoleLine();
                        _stop = false;
                        break;
                    }
                    if (inp2 == ConsoleKey.R)
                    {
                        NewGame();
                    }
                } while (inp2 is not (ConsoleKey.Q or ConsoleKey.R));
            }

            while (_gameover)
            {
                _ckey = Console.ReadKey(true).Key;
                if (_ckey == ConsoleKey.R)
                {
                    NewGame();
                }
            }
        });

    private void GenerateFood()
    {
        Random food = new Random();
        _foodposition = new Coord(food.Next(1, _width-1), food.Next(1, _height-1));
        Drawing.DrawSn(_foodposition, ConsoleColor.DarkMagenta);
    }

    private void RefreshScore()
    {
        Console.SetCursorPosition(_width*2+6, 0);
        CleanConsole.ClearConsoleLine(_width*2+6);
        Console.Write(Score);
    }

    private void NextPosition()
    {
        _snakeCoords.AddFirst(_snakeCoords.First.Value + _cdirection);
        if (_snakeCoords.First.Value.X == (0 | 1 | _width*2 - 1 | _width*2) || _snakeCoords.First.Value.Y == (0 | _height))
        {
            GameOver();
        }
        else
        {
            Drawing.DrawSn(_snakeCoords.First.Value, ConsoleColor.Green);
        }

        if (_snakeCoords.First.Value != _foodposition)
        {
            Drawing.DrawSn(_snakeCoords.Last.Value, ConsoleColor.Black);
            _snakeCoords.RemoveLast();
        }
        else
        {
            Score++;
            RefreshScore();
            GenerateFood();
        }
    }

    public static void NewGame()
    {
        Console.Clear();
        int width;
        int height;
        float gamespeed;
        Console.Write(
            "Choose width and height and gamespeed for a new game. Higher the game speed, the faster the game becomes. Width and height cannot be lower than 5 or greater than 100.");
        Console.Write("\nDefault=\n");
        Console.Write("Width:");
        while (!int.TryParse(Console.ReadLine(), out width) || width < 5 || width > 100)
        {
            Console.SetCursorPosition(0, 4);
            Console.WriteLine(
                "That's invalid. It is either not greater than 5, lower than 100 or just not a number."); //line4 
            Console.SetCursorPosition(6, 3); //line 3 (width)
            CleanConsole.ClearConsoleLine(6);
        }

        Console.SetCursorPosition(0, 4);
        CleanConsole.ClearConsoleLine();
        Console.Write("Height:"); //line 4
        while (!int.TryParse(Console.ReadLine(), out height) || height < 5 || height > 100)
        {
            Console.SetCursorPosition(0, 5);
            Console.WriteLine("That's invalid. It is either not greater 5 or just not a number."); //line 5
            Console.SetCursorPosition(7, 4); //line 4 (height)
            CleanConsole.ClearConsoleLine(7);
        }

        Console.SetCursorPosition(0, 5);
        CleanConsole.ClearConsoleLine();
        Console.Write("Gamespeed:"); //line 5
        while (!float.TryParse(Console.ReadLine(), CultureInfo.InvariantCulture, out gamespeed) || gamespeed < 0.1 ||
               gamespeed > 20)
        {
            Console.SetCursorPosition(0, 6);
            Console.WriteLine("That's invalid. It is either lower than 0.1, greater than 20 or just not a number.");
            Console.SetCursorPosition(10, 5);
            CleanConsole.ClearConsoleLine(10);
        }
        Console.Clear();
        Game game = new Game(width, height, gamespeed);
    }
    
    public void GameOver()
    {
        _stop = true;
        Console.SetCursorPosition(_width+1, 1);
        Console.Write("Game is over! If you wish to restart, press R");
    }

    private void Update()
    {
        while (!_gameover)
        {
            while (!_stop)
            {
                NextPosition();
                Thread.Sleep(100);
            }
        }
    }
}
class Snake
{
    static void Main()
    {
        Game.NewGame();
    }
}