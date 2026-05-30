using System;
using System.Collections.Generic;
using System.Linq;

namespace FitVision.Services
{
    public class TransformationRecord
    {
        public string UserId { get; set; }
        public string PhotoUrl { get; set; }
        public string Result { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class HistoryService
    {
        private readonly List<TransformationRecord> _database = new List<TransformationRecord>();

        public void SaveTransformation(string userId, string photoUrl, string result)
        {
            ValidateUserId(userId);

            var record = new TransformationRecord
            {
                UserId = userId,
                PhotoUrl = photoUrl,
                Result = result,
                CreatedAt = DateTime.UtcNow
            };

            _database.Add(record);
        }

        public List<TransformationRecord> GetUserHistory(string userId, int daysLimit)
        {
            ValidateUserId(userId);

            if (daysLimit <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(daysLimit), "Days limit must be greater than zero.");
            }

            var cutoffDate = DateTime.UtcNow.AddDays(-daysLimit);

            var userHistory = _database
                .Where(r => r.UserId == userId && r.CreatedAt >= cutoffDate)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();

            return userHistory;
        }

        public int ClearUserHistory(string userId)
        {
            ValidateUserId(userId);

            int removedCount = _database.RemoveAll(r => r.UserId == userId);

            return removedCount;
        }

        public void SeedDatabase(TransformationRecord record)
        {
            _database.Add(record);
        }

        private void ValidateUserId(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID is required.", nameof(userId));
            }
        }
    }
}