using System;
using FitVisionAI.Utils;
namespace FitVisionAI
{
    class PostureAnalyzer
    {
        public string Analyze(string exerciseName)
        {
            // Імітація роботи ШІ для аналізу кутів нахилу тіла
            if (exerciseName.ToLower() == "присідання")
            {
                return "Спина рівна, кут згину колін оптимальний. Техніка правильна!";
            }
            return "Вправа розпізнається.. Тримайте поставу.";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Вітаємо у FitVision-AI ===");
            
            RegistrationForm registrationForm = new RegistrationForm();
            registrationForm.RunRegistration();
            
            Console.WriteLine("=== FitVision-AI: Тренування розпочато ===");
            
            PostureAnalyzer analyzer = new PostureAnalyzer();
            string currentExercise = "Присідання";
            
            Console.WriteLine($"Поточна вправа: {currentExercise}");
            
            string feedback = analyzer.Analyze(currentExercise);
            Console.WriteLine($"[AI Аналіз]: {feedback}");


    CalorieCalculator calc = new CalorieCalculator();
    
Console.WriteLine("\n=== Розрахунок індивідуальної норми води ===");
            
            Console.Write("Введіть вашу вагу (кг): ");
            string? weightInput = Console.ReadLine();
            double userWeight = double.Parse(weightInput ?? "0");

            Console.Write("Скільки годин тривало тренування? ");
            string? workoutInput = Console.ReadLine();
            double userWorkout = double.Parse(workoutInput ?? "0");

            double finalWater = WaterIntakeCalculator.CalculateDailyWaterIntake(userWeight, userWorkout); 
            
            Console.WriteLine($"\n[Результат]: Ваша денна норма води — {finalWater} літрів.");

        } 
    } 
} 