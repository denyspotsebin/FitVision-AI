using System;

namespace FitVisionAI.Services
{
    public class TargetParameters
    {
        public float DesiredWeight { get; set; }
        public float BodyFatPercentage { get; set; }

        // Метод 1 для тестування: Перевірка валідності цільових параметрів
        public bool ValidateData()
        {
            // Граничні значення для ваги: від 30 до 250 кг
            if (DesiredWeight <= 30 || DesiredWeight > 250)
                throw new ArgumentOutOfRangeException(nameof(DesiredWeight), "Вага повинна бути від 30 до 250 кг.");

            // Граничні значення для відсотка жиру: від 3 до 50%
            if (BodyFatPercentage < 3 || BodyFatPercentage > 50)
                throw new ArgumentOutOfRangeException(nameof(BodyFatPercentage), "Відсоток жиру має бути від 3 до 50.");

            return true;
        }
    }

    public class BasePhoto
    {
        public bool IsQualityGood { get; set; }

        public bool AnalyzeLighting()
        {
            return IsQualityGood;
        }
    }

    public class AIGeneratorService
    {
        public int DailyLimit { get; set; } = 5;
        public int UsedRequests { get; set; } = 0;

        // Метод 2 для тестування: Перевірка лімітів (взято з Sequence Diagram)
        public bool CheckAvailableLimits(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("Некоректний ID користувача.");

            if (UsedRequests >= DailyLimit)
                throw new InvalidOperationException("Помилка: Ліміт вичерпано.");
            
            return true;
        }

        // Метод 3 для тестування: Генерація результату
        public string GenerateTransformation(BasePhoto photo, TargetParameters goals)
        {
            if (photo == null)
                throw new ArgumentNullException(nameof(photo), "Фото не може бути порожнім.");

            if (!photo.AnalyzeLighting())
                throw new ArgumentException("Погана якість фото або освітлення. Генерація неможлива.");

            // Збільшуємо лічильник використаних запитів
            UsedRequests++;
            
            // Імітація успішної генерації та створення GeneratedResult
            return $"Transformation_Success_UserPhoto_{Guid.NewGuid()}.jpg"; 
        }
    }
}