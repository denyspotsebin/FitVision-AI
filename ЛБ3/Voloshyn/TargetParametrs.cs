using System;

namespace FitVisionAI.Services
{
    public class TargetParameters
    {
        // Усунуто Code Smell: Magic Numbers
        private const float MIN_WEIGHT = 30f;
        private const float MAX_WEIGHT = 250f;
        private const float MIN_FAT = 3f;
        private const float MAX_FAT = 50f;

        public float DesiredWeight { get; set; }
        public float BodyFatPercentage { get; set; }

        // Метод 1 для тестування: Перевірка валідності цільових параметрів
        public bool ValidateData()
        {
            // Виправлено Boundary Logic Error (< замість <=) 
            // та усунуто Exceptions for Control Flow (повертаємо false замість throw)
            if (DesiredWeight < MIN_WEIGHT || DesiredWeight > MAX_WEIGHT)
                return false;

            if (BodyFatPercentage < MIN_FAT || BodyFatPercentage > MAX_FAT)
                return false;

            return true;
        }
    }
}