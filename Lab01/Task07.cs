using System;

namespace Lab01
{
    public class Task07
    {
        public static void Run()
        {
            int n = int.Parse(Console.ReadLine()!);

            decimal[] costs = new decimal[n];

            for (int i = 0; i < n; i++)
            {
                costs[i] = decimal.Parse(Console.ReadLine()!);
            }

            decimal total = 0;
            decimal min = decimal.MaxValue;
            decimal max = decimal.MinValue;

            foreach (decimal cost in costs)
            {
                total += cost;
                if (cost < min) min = cost;
                if (cost > max) max = cost;
            }

            decimal average = total / n;

            int aboveAverageCount = 0;
            for (int i = 0; i < costs.Length; i++)
            {
                if (costs[i] > average)
                {
                    aboveAverageCount++;
                }
            }

            int expensiveIndex = -1;
            int index = 0;
            while (index < costs.Length)
            {
                if (costs[index] > 1000)
                {
                    expensiveIndex = index;
                    break;
                }
                index++;
            }

            string firstExpensiveStr = "немає";
            if (expensiveIndex != -1)
            {
                firstExpensiveStr = $"#{expensiveIndex + 1} — {costs[expensiveIndex]:F2} грн";
            }

            Console.WriteLine("=== Звіт по прийомах ===");
            Console.WriteLine($"Кількість:        {n}");
            Console.WriteLine($"Загальна сума:    {total:F2} грн");
            Console.WriteLine($"Середня:          {average:F2} грн");
            Console.WriteLine($"Мін / Макс:       {min:F2} / {max:F2} грн");
            Console.WriteLine($"Вище середнього:  {aboveAverageCount} з {n}");
            Console.WriteLine($"Перший > 1000:    {firstExpensiveStr}");
            Console.WriteLine("========================");
        }
    }
}