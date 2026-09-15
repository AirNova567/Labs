using System;

namespace Lab01
{
    public class Task08
    {
        public static void Run()
        {
            double weight = double.Parse(Console.ReadLine()!);
            double height = double.Parse(Console.ReadLine()!);
            double price = double.Parse(Console.ReadLine()!);
            int count = int.Parse(Console.ReadLine()!);
            int sale = int.Parse(Console.ReadLine()!);
            int birthYear = int.Parse(Console.ReadLine()!);
            int systolic = int.Parse(Console.ReadLine()!);
            int diastolic = int.Parse(Console.ReadLine()!);

            double bmi = CalculateBMI(weight, height);
            string bmiCategory = GetBMICategory(bmi);

            double totalCost = CalculateCost(price, count, sale);

            int age = 2026 - birthYear;
            string ageCategory = GetAgeCategory(age);

            string pressureStatus = GetPressureStatus(systolic, diastolic);

            Console.WriteLine($"ІМТ: {bmi:F2} -> {bmiCategory}");
            Console.WriteLine($"Сума: {totalCost:F2} грн");
            Console.WriteLine($"Вік: {age} р., категорія: {ageCategory}");
            Console.WriteLine($"Тиск: {systolic}/{diastolic} — {pressureStatus}");
        }

        public static double CalculateBMI(double weight, double height)
        {
            return weight / (height * height);
        }

        public static string GetBMICategory(double bmi)
        {
            if (bmi < 18.5)
                return "недостатня вага";
            if (bmi < 25.0)
                return "норма";
            if (bmi < 30.0)
                return "надмірна вага";
            return "ожиріння";
        }

        public static double CalculateCost(double price, int count, int sale)
        {
            return price * count * (1 - sale / 100.0);
        }

        public static string GetAgeCategory(int age)
        {
            if (age < 18)
                return "дитина";
            if (age < 60)
                return "дорослий";
            return "пенсіонер";
        }

        public static string GetPressureStatus(int systolic, int diastolic)
        {
            if (systolic < 120 && diastolic < 80)
                return "норма";
            if (systolic < 130 && diastolic < 80)
                return "підвищений";
            if (systolic < 140 || diastolic < 90)
                return "гіпертонія 1 ступеня";
            return "гіпертонія 2 ступеня";
        }
    }
}