using System;

namespace Lab01
{
    public class Task05
    {
        public static void Run()
        {
            int day = int.Parse(Console.ReadLine()!);

            string result = day switch
            {
                1 => "День: Понеділок, 08:00–18:00",
                2 => "День: Вівторок, 08:00–18:00",
                3 => "День: Середа, 09:00–17:00",
                4 => "День: Четвер, 08:00–18:00",
                5 => "День: П'ятниця, 08:00–16:00",
                6 => "День: Субота, 09:00–14:00",
                7 => "День: Неділя — вихідний",
                _ => "День: невідомий день"
            };

            Console.WriteLine(result);
        }
    }
}