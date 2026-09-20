using System;

namespace Lab02
{
    public class Task02
    {
        public static void Run()
        {
            int n = int.Parse(Console.ReadLine()!);
            int[] queue = new int[n];

            for (int i = 0; i < n; i++)
            {
                queue[i] = int.Parse(Console.ReadLine()!);
            }

            string before = string.Join(" ", queue);

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (queue[j] > queue[j + 1])
                    {
                        (queue[j], queue[j + 1]) = (queue[j + 1], queue[j]);
                    }
                }
            }

            string after = string.Join(" ", queue);

            Console.WriteLine($"Черга (до):    {before}");
            Console.WriteLine($"Черга (після): {after}");
            Console.WriteLine($"Найдешевший:   {queue[0]} грн");
            Console.WriteLine($"Найдорожчий:   {queue[n - 1]} грн");
        }
    }
}