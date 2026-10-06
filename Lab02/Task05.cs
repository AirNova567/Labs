using System;

namespace Lab02
{
    public class Task05
    {
        public static void Run()
        {
            int n = int.Parse(Console.ReadLine()!);
            int[,] matrix = new int[n, n];

            for (int i = 0; i < n; i++)
            {
                string[] parts = Console.ReadLine()!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = int.Parse(parts[j]);
                }
            }

            int[] mainDiagonal = new int[n];
            int[] secondaryDiagonal = new int[n];
            int mainSum = 0;
            int secondarySum = 0;

            for (int i = 0; i < n; i++)
            {
                mainDiagonal[i] = matrix[i, i];
                mainSum += mainDiagonal[i];

                secondaryDiagonal[i] = matrix[i, n - 1 - i];
                secondarySum += secondaryDiagonal[i];
            }

            Console.WriteLine($"Головна діагональ: {string.Join(", ", mainDiagonal)} (сума = {mainSum})");
            Console.WriteLine($"Побічна діагональ: {string.Join(", ", secondaryDiagonal)} (сума = {secondarySum})");
        }
    }
}