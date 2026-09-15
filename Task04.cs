using System;

namespace Lab01
{
    public class Task04
    {
        public static void Run()
        {
            int systolic = int.Parse(Console.ReadLine()!);
            int diastolic = int.Parse(Console.ReadLine()!);

            string status = "";

            if (systolic < 120 && diastolic < 80)
            {
                status = "норма";
            }
            else if (systolic < 130 && diastolic < 80)
            {
                status = "підвищений";
            }
            else if (systolic < 140 || diastolic < 90)
            {
                status = "гіпертонія 1 ступеня";
            }
            else
            {
                status = "гіпертонія 2 ступеня";
            }

            Console.WriteLine($"Тиск: {systolic}/{diastolic} — {status}");
        }
    }
}