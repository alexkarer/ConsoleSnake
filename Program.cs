using System;
using System.Threading;

namespace ConsoleSnake
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please set Font to Rasterschrift 8x8, press  any key to continue");
            Console.ReadKey();

            ConsoleUtility.SetUpConsole();
            ConsoleUtility.PrintBorder();

            Snake snake = new Snake(SnakeDirection.right, new Coordinate(20, 11), 4, ConsoleColor.DarkGreen, '☼');
            SnakeDirection snakeDirection = SnakeDirection.right;

            while(true)
            {
                snakeDirection = ConsoleUtility.GetInput(snakeDirection);
                if (!snake.MoveSnake(snakeDirection))
                    break;

                Thread.Sleep(100);
            }

            Console.SetCursorPosition(0, 65);
        }
    }

    class ConsoleUtility
    {
        public static void SetUpConsole()
        {
            Console.CursorVisible = false;
            Console.Title = "ConsoleSnake";
            Console.SetWindowSize(70, 70);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
        }
        public static void PrintBorder()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(10, 10);
            for(int i = 0; i < 50; i++)
            {
                Console.Write('█');
            }

            for (int i = 11; i < 59; i++)
            {
                Console.SetCursorPosition(10, i);
                Console.Write('█');
                Console.SetCursorPosition(59, i);
                Console.Write('█');
            }

            Console.SetCursorPosition(10, 59);
            for (int i = 0; i < 50; i++)
            {
                Console.Write('█');
            }
        }

        public static SnakeDirection GetInput(SnakeDirection snakeDirection)
        {
            if(Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                
                if(key.Key == ConsoleKey.RightArrow)
                {
                    return SnakeDirection.right;
                }
                else if(key.Key == ConsoleKey.LeftArrow)
                {
                    return SnakeDirection.left;
                }
                else if (key.Key == ConsoleKey.UpArrow)
                {
                    return SnakeDirection.up;
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    return SnakeDirection.down;
                }
                else
                {
                    return snakeDirection;
                }
            }
            return snakeDirection;
        }
    }
}
