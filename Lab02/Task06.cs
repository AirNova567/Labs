using System;

namespace Lab02
{
    public class Task06
    {
        public static void Run()
        {
            int n = int.Parse(Console.ReadLine()!);

            int[][] doctors = new int[n][];

            int[] sums = new int[n];
            double[] averages = new double[n];

            int maxIncome = int.MinValue;
            int bestDoctorIndex = 0;

            for (int i = 0; i < n; i++)
            {
                int k = int.Parse(Console.ReadLine()!);
                doctors[i] = new int[k];

                int currentSum = 0;
                for (int j = 0; j < k; j++)
                {
                    doctors[i][j] = int.Parse(Console.ReadLine()!);
                    currentSum += doctors[i][j];
                }

                sums[i] = currentSum;
                averages[i] = (double)currentSum / k;

                if (currentSum > maxIncome)
                {
                    maxIncome = currentSum;
                    bestDoctorIndex = i;
                }
            }

            for (int i = 0; i < n; i++)
            {
                int k = doctors[i].Length;
                Console.WriteLine($"Лікар {i + 1}: {k} прийоми, сума={sums[i]} грн, середня={averages[i]:F2} грн");
            }

            Console.WriteLine($"Найбільший дохід: Лікар {bestDoctorIndex + 1} ({maxIncome} грн)");
        }
    }
}