// Function to calculate BMI based on weight and height
using System;

class Program
{
    static double CalculateBMI(double weight, double height)
    {
        return weight / (height * height);
    }

    static void Main(string[] args)
    {
        // Example usage
        double weight = 70; // kg
        double height = 1.75; // m
        double bmi = CalculateBMI(weight, height);
        Console.WriteLine($"BMI: {bmi}");
    }
}