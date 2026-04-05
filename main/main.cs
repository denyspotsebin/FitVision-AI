using System;

namespace FitVisionAI
{
    class Program
    {
        static void Main(string[] args)
        {
            // Example usage
            double weight = 70; // kg
            double height = 1.75; // m
            double bmi = CalculateBMI(weight, height);
            Console.WriteLine($"BMI: {bmi}");
        }

        // Функція для розрахунку індексу маси тіла (BMI)
        static double CalculateBMI(double weight, double height)
        {
            return weight / (height * height);
        }
    }
}