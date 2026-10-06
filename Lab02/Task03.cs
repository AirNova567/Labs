using System;

namespace Lab02
{
    public class Task03
    {
        public static void Run()
        {
            string[] days = { "Понеділок", "Вівторок", "Середа", "Четвер", "П'ятниця", "Субота", "Неділя" };
            int[] counts = new int[7];

            int total = 0;

            for (int i = 0; i < 7; i++)
            {
                counts[i] = int.Parse(Console.ReadLine()!);
                total += counts[i];
            }

            int maxIdx = 0;
            int minIdx = 0;

            for (int i = 1; i < 7; i++)
            {
                if (counts[i] > counts[maxIdx])
                {
                    maxIdx = i;
                }
                if (counts[i] < counts[minIdx])
                {
                    minIdx = i;
                }
            }

            for (int i = 0; i < 7; i++)
            {
                Console.WriteLine($"{days[i],-12}: {counts[i]} пацієнтів");
            }

            Console.WriteLine($"Разом:        {total}");
            Console.WriteLine($"Найбільше:    {days[maxIdx]} ({counts[maxIdx]})");
            Console.WriteLine($"Найменше:     {days[minIdx]} ({counts[minIdx]})");
        }
    }
}