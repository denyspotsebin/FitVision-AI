using System;

namespace FitVisionAI.Services
{
    public class AIGeneratorService
    {
        // Усунуто Code Smell: Magic Strings
        private const string InvalidUserIdError = "Некоректний ID користувача.";
        private const string LimitExceededError = "Помилка: Ліміт вичерпано.";
        private const string PhotoEmptyError = "Фото не може бути порожнім.";
        private const string BadLightingError = "Погана якість фото або освітлення. Генерація неможлива.";

        public int DailyLimit { get; set; } = 5;
        
        // Виправлено Broken Encapsulation (сеттер змінено на private)
        public int UsedRequests { get; private set; } = 0;

        // Метод 2 для тестування: Перевірка лімітів (взято з Sequence Diagram)
        public bool CheckAvailableLimits(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException(InvalidUserIdError);

            if (UsedRequests >= DailyLimit)
                throw new InvalidOperationException(LimitExceededError);
            
            return true;
        }

        // Метод 3 для тестування: Генерація результату
        public string GenerateTransformation(BasePhoto photo, TargetParameters goals)
        {
            if (photo == null)
                throw new ArgumentNullException(nameof(photo), PhotoEmptyError);

            if (!photo.AnalyzeLighting())
                throw new ArgumentException(BadLightingError);

            // Збільшуємо лічильник використаних запитів
            UsedRequests++;
            
            // Імітація успішної генерації та створення GeneratedResult
            return $"Transformation_Success_UserPhoto_{Guid.NewGuid()}.jpg"; 
        }
    }
}