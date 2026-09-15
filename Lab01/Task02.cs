using System;

namespace Lab01
{
    public class Task02
    {
        public static void Run()
        {
            double price = double.Parse(Console.ReadLine()!);
            int count = int.Parse(Console.ReadLine()!);
            int sale = int.Parse(Console.ReadLine()!);

            double total = price * count * (1 - sale / 100.0);

            Console.WriteLine($"Сума: {total:F2} грн");
        }
    }
}