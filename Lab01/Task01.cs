using System;

namespace Lab01
{
    public class Task01
    {
        public static void Run()
        {
            double weight = double.Parse(Console.ReadLine()!);
            double height = double.Parse(Console.ReadLine()!);

            double bmi = weight / (height * height);

            Console.WriteLine("BMI: " + bmi);
        }
    }
}

