using System;

namespace FitVisionAI
{
    class CalorieCalculator
    {
        // Функція, яка розраховує спалені калорії під час тренування
        // Приймає: вагу (в кг) та тривалість вправи (в хвилинах)
        // Повертає: кількість спалених калорій (тип double)
        public double CalculateCaloriesBurned(double weightKg, double durationMinutes)
        {
            // Середній коефіцієнт спалювання калорій для помірної активності
            const double caloriesPerMinutePerKg = 0.1; // Це приблизне значення, може змінюватися в залежності від типу вправи

            // Розрахунок спалених калорій
            double caloriesBurned = weightKg * caloriesPerMinutePerKg * durationMinutes;

            return caloriesBurned;
        }
    }
}