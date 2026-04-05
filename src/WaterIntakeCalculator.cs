using System;

namespace FitVisionAI.Utils
{
    public class WaterIntakeCalculator
    {
        // Напиши функцію, яка розраховує денну норму води (в літрах) для людини.
        // Формула: Вага (кг) * 0.03 + (Час тренування в годинах * 0.4).
        // Функція повинна приймати вагу (double weight) та час (double workoutHours).
        public static double CalculateDailyWaterIntake(double weight, double workoutHours)
        {
            if (weight <= 0)
            {
                throw new ArgumentException("Вага повинна бути додатною.");
            }
            if (workoutHours < 0)
            {
                throw new ArgumentException("Час тренування не може бути від'ємним.");
            }

            double waterIntake = weight * 0.03 + workoutHours * 0.4;
            return Math.Round(waterIntake, 2); // Округлюємо до 2 знаків після коми
        }
    }
}