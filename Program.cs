using System;

class Program
{
    // Function to calculate BMI based on weight and height
    static double CalculateBMI(double weight, double height)
    {
        return weight / (height * height);
    }

    static void Main(string[] args)
    {
        double weight = 70;
        double height = 1.75;
        double bmi = CalculateBMI(weight, height);

        Console.WriteLine($"BMI: {bmi:F2}");
    }
}
