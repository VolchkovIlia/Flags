using System;
using System.Collections.Generic;

class Program
{
    [Flags]
    enum Direction
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 4,
        Right = 8
    }

    static void Main()
    {
        int[,] matrix = {
            {1,     1,      1,      1,      1},
            {100,   100,    1,      100,    1},
            {1,     1,      1,      1,      1},
            {1,     100,    100,    100,    1},
            {1,     1,      1,      1,      1}
        };

        Console.Write("Введите значение X: ");
        int x = int.Parse(Console.ReadLine()!);
        Console.Write("Введите значение Y: ");
        int y = int.Parse(Console.ReadLine()!);

        var (result, sequence) = FindMinimumCost(matrix, x, y);
        Console.WriteLine($"Минимальная стоимость маршрута в клетку ({x}, {y}): {result}");

        Console.Write("Последовательность шагов: ");
        foreach (var step in sequence)
        {
            Console.Write(step + " ");
        }
        Console.WriteLine();
    }

    static (int, List<Direction>) FindMinimumCost(int[,] matrix, int x, int y)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        int[,] dp = new int[rows, cols];
        Direction[,] path = new Direction[rows, cols];

        dp[0, 0] = matrix[0, 0];

        for (int j = 1; j <= y; j++)
        {
            dp[0, j] = dp[0, j - 1] + matrix[0, j];
            path[0, j] = Direction.Left;
        }

        for (int i = 1; i <= x; i++)
        {
            dp[i, 0] = dp[i - 1, 0] + matrix[i, 0];
            path[i, 0] = Direction.Up;
        }

        for (int i = 1; i <= x; i++)
        {
            for (int j = 1; j <= y; j++)
            {
                int minCost = Math.Min(dp[i - 1, j], Math.Min(dp[i - 1, j - 1], dp[i, j - 1]));
                dp[i, j] = minCost + matrix[i, j];

                if (minCost == dp[i - 1, j])
                    path[i, j] |= Direction.Up;
                if (minCost == dp[i - 1, j - 1])
                    path[i, j] |= Direction.Left;
                if (minCost == dp[i, j - 1])
                    path[i, j] |= Direction.Right;
            }
        }

        List<Direction> sequence = new List<Direction>();
        int currentX = x;
        int currentY = y;
        while (currentX > 0 || currentY > 0)
        {
            Direction currentDirection = path[currentX, currentY];
            sequence.Add(currentDirection);
            if ((currentDirection & Direction.Up) != 0)
                currentX--;
            if ((currentDirection & Direction.Left) != 0)
                currentY--;
        }
        sequence.Reverse();

        return (dp[x, y], sequence);
    }
}
 