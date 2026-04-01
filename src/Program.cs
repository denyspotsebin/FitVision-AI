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
            double burned = calc.CalculateCaloriesBurned(75.5, 30);
            Console.WriteLine($"[AI Калькулятор]: Ви спалили {burned} калорій.");

            Console.WriteLine("\n=== Розрахунок норми води ===");
            
            double waterNorm = WaterIntakeCalculator.CalculateDailyWaterIntake(75.5, 1.5); 
            
            Console.WriteLine($"[AI Калькулятор]: Ваша денна норма води становить {waterNorm} літрів.");
            
            Console.WriteLine($"[AI Калькулятор]: Ваша денна норма води становить {waterNorm} літрів.");
            
            Console.WriteLine($"[AI Калькулятор]: Ваша денна норма води становить {waterNorm} літрів.");
        } 
    }
} 