using System;

namespace Lab01
{
    public class Task03
    {
        public static void Run()
        {
            int year = int.Parse(Console.ReadLine()!);
            int age = 2026 - year;

            Console.WriteLine($"Вік: {age} р.");

            if (age < 18)
            {
                Console.WriteLine("Категорія: дитина");
            }
            else if (age >= 18 && age < 60) 
            {
                Console.WriteLine("Категорія: дорослий");
            }
            else
            {
                Console.WriteLine("Категорія: пенсіонер");
            }
        }
    }
}