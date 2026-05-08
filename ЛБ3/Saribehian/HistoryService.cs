using System;
using System.Collections.Generic;
using System.Linq;

namespace FitVision.Services
{
    // Допоміжна модель для збереження даних (імітація таблиці БД)
    public class TransformationRecord
    {
        public string UserId { get; set; }
        public string PhotoUrl { get; set; }
        public string Result { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class HistoryService
    {
        // Імітація бази даних для модульного тестування (ізольованість)
        private readonly List<TransformationRecord> _database = new List<TransformationRecord>();

        // Метод 1: Збереження результату трансформації (FR-05)
        public bool SaveTransformation(string userId, string photoUrl, string result)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
            
            if (string.IsNullOrWhiteSpace(photoUrl))
                throw new ArgumentException("Photo URL cannot be empty.", nameof(photoUrl));

            if (string.IsNullOrWhiteSpace(result))
                throw new ArgumentException("Transformation result cannot be empty.", nameof(result));

            var record = new TransformationRecord
            {
                UserId = userId,
                PhotoUrl = photoUrl,
                Result = result,
                CreatedAt = DateTime.UtcNow
            };

            _database.Add(record);
            return true;
        }

        // Метод 2: Отримання історії з фільтрацією за днями (FR-06)
        public List<TransformationRecord> GetUserHistory(string userId, int daysLimit)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID is required.", nameof(userId));

            if (daysLimit <= 0)
                throw new ArgumentOutOfRangeException(nameof(daysLimit), "Days limit must be greater than zero.");

            var cutoffDate = DateTime.UtcNow.AddDays(-daysLimit);
            
            var userHistory = _database
                .Where(r => r.UserId == userId && r.CreatedAt >= cutoffDate)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();

            if (!userHistory.Any())
                throw new InvalidOperationException("No history found for this user in the specified period.");

            return userHistory;
        }

        // Метод 3: Очищення історії користувача 
        public int ClearUserHistory(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID is required.", nameof(userId));

            var recordsToRemove = _database.Where(r => r.UserId == userId).ToList();

            if (!recordsToRemove.Any())
                return 0; // Нічого видаляти, повертаємо 0

            int removedCount = 0;
            // Використовуємо цикл для демонстрації "нетривіальної логіки"
            foreach (var record in recordsToRemove)
            {
                _database.Remove(record);
                removedCount++;
            }

            return removedCount;
        }
        
        // Метод для тестів: дозволяє попередньо заповнити нашу "БД" даними
        public void SeedDatabase(TransformationRecord record)
        {
            _database.Add(record);
        }
    }
}