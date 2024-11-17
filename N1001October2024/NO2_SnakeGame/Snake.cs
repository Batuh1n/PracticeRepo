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
    private bool _gameover;
    private LinkedList<Coord> _snakeCoords;
    private Coord _foodposition;
    public int Score;
    private float _gamespeed;

    private readonly Dictionary<ConsoleKey, Coord> _movementKeys = new()
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
        _snakeCoords.AddFirst(new Coord(_width/2, _height/2));
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

        foreach (Coord pos in _snakeCoords)
        {
            Drawing.DrawSn(pos, ConsoleColor.Green);
        }
    }

    private Thread InputHandler() =>
        new(() =>
        {
            while (!_gameover)
            {
                while (!_stop)
                {
                    _ckey = Console.ReadKey(true).Key;
                    if (_ckey == ConsoleKey.Q) //Stops The Game
                    {
                        _stop = true;
                        break;
                    }

                    if (_movementKeys.TryGetValue(_ckey, out Coord keydirection))
                    {
                        if (!(keydirection + _cdirection == new Coord(0, 0)))
                        {
                            _cdirection = _movementKeys[_ckey];
                        }
                    }
                }

                ConsoleKey inp2;
                while (_stop && !_gameover)
                {
                    ConsoleColor foreground = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(_width*2+1, 1);
                    Console.Write("You have paused the game.");
                    Console.SetCursorPosition(_width*2+1, 2);
                    Console.Write("If you wish to continue, press Q again or R to restart.");
                    Console.ForegroundColor = foreground;
                    do
                    {
                        inp2 = Console.ReadKey(true).Key;
                        if (inp2 == ConsoleKey.Q)
                        {
                            CleanConsole.ClearConsoleLine(_width*2+1, 1);
                            CleanConsole.ClearConsoleLine(_width * 2 + 1, 2);
                            _stop = false;
                            break;
                        }
                        if (inp2 == ConsoleKey.R)
                        {
                            NewGame();
                        }
                    } while (inp2 is not (ConsoleKey.Q or ConsoleKey.R));
                }
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
        Coord foodposition;
        do
        {
            foodposition = new Coord(food.Next(1, _width - 1), food.Next(1, _height - 1));
        } while (_snakeCoords.Contains(foodposition));
        _foodposition = foodposition;
        Drawing.DrawSn(_foodposition, ConsoleColor.DarkMagenta);
    }

    private void RefreshScore()
    {
        ConsoleColor foreground = Console.ForegroundColor;
        Console.SetCursorPosition(_width*2, 0);
        CleanConsole.ClearConsoleLine(_width*2);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"Score:{Score}");
        Console.ForegroundColor = foreground;
    }

    private void NextPosition()
    {
        if (_cdirection != new Coord(0, 0))
        {
            if (_snakeCoords.Contains(_snakeCoords.First.Value + _cdirection))
            {
                Drawing.DrawSn(_snakeCoords.First.Value, ConsoleColor.DarkRed);
                GameOver();
                return;
            }   
        }
        _snakeCoords.AddFirst(_snakeCoords.First.Value + _cdirection);
        if (_snakeCoords.First.Value.X <= 0 || _snakeCoords.First.Value.X >= _width-1||
            _snakeCoords.First.Value.Y <= 0 || _snakeCoords.First.Value.Y >= _height)
        {
            Drawing.DrawSn(_snakeCoords.First.Value, ConsoleColor.DarkRed);
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
        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.Black;
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
        _gameover = true;
        Console.SetCursorPosition(_width*2+1, 1);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Game is over! If you wish to restart, press R");
    }

    private void Update()
    {
        while (!_gameover)
        {
            if (!_stop)
            {
                NextPosition();
                Thread.Sleep((int)(100/_gamespeed));
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