using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 洒下一片深情
{
    class SnakeGame
    {
        private const int Width = 20;
        private const int Height = 20;
        private const char SnakeChar = '*';
        private const char FoodChar = '%';

        private static readonly Random Random = new Random();
        private static readonly List<(int x, int y)> Snake = new List<(int x, int y)>();
        private static (int x, int y) Food;

        public static void Start()
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(Width + 1, Height + 1);
            Console.SetBufferSize(Width + 1, Height + 1);

            InitializeSnake();
            SpawnFood();

            var direction = (1, 0);
            var running = true;

            while (running)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.LeftArrow:
                            if (direction != (1, 0))
                            {
                                direction = (-1, 0);
                            }
                            break;
                        case ConsoleKey.RightArrow:
                            if (direction != (-1, 0))
                            {
                                direction = (1, 0);
                            }
                            break;
                        case ConsoleKey.UpArrow:
                            if (direction != (0, 1))
                            {
                                direction = (0, -1);
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            if (direction != (0, -1))
                            {
                                direction = (0, 1);
                            }
                            break;
                        case ConsoleKey.Escape:
                            running = false;
                            break;
                    }
                }

                var (dx, dy) = direction;
                var (headX, headY) = Snake[0];
                var newHead = (headX + dx, headY + dy);

                if (newHead == Food)
                {
                    Snake.Insert(0, newHead);
                    SpawnFood();
                }
                else if (Snake.Contains(newHead) || newHead.Item1 < 0 || newHead.Item1 >= Width || newHead.Item2 < 0 || newHead.Item2 >= Height)
                {
                    Console.Clear();
                    Console.WriteLine("Game Over!");
                    running = false;
                }
                else
                {
                    Snake.Insert(0, newHead);
                    Snake.RemoveAt(Snake.Count - 1);
                }

                Draw();
                System.Threading.Thread.Sleep(100);
            }
        }

        private static void InitializeSnake()
        {
            Snake.Add((Width / 2, Height / 2));
            Snake.Add((Width / 2 + 1, Height / 2));
            Snake.Add((Width / 2 + 2, Height / 2));
        }

        private static void SpawnFood()
        {
            do
            {
                Food = (Random.Next(Width), Random.Next(Height));
            } while (Snake.Contains(Food));

            Console.SetCursorPosition(Food.x, Food.y);
            Console.Write(FoodChar); // 添加这行，绘制食物
        }

        private static void Draw()
        {
            Console.Clear();

            foreach (var (x, y) in Snake)
            {
                Console.SetCursorPosition(x, y);
                Console.Write(SnakeChar);
            }

            Console.SetCursorPosition(Food.x, Food.y);
            Console.Write(FoodChar); // 添加这行，绘制食物
        }
    }
}
