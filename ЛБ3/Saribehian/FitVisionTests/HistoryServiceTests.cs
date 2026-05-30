using Xunit;
using FitVision.Services;
using System;
using System.Collections.Generic;

namespace FitVisionTests
{
    public class HistoryServiceTests
    {
        private readonly HistoryService _service;

        public HistoryServiceTests()
        {
            _service = new HistoryService();
        }

        [Fact]
        public void SaveTransformation_ValidData_SavesRecord()
        {
            // Arrange
            string userId = "user1";
            string photo = "http://photo.com/1.jpg";
            string result = "Success";

            // Act
            _service.SaveTransformation(userId, photo, result);
            var history = _service.GetUserHistory(userId, 7);

            // Assert (Перевіряємо, що запис зберігся і його можна дістати)
            Assert.NotEmpty(history);
            Assert.Single(history);
        }

        [Fact]
        public void SaveTransformation_EmptyUserId_ThrowsArgumentException()
        {
            // Arrange
            string userId = "";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _service.SaveTransformation(userId, "url", "res"));
        }

        [Fact]
        public void GetUserHistory_ExistingRecords_ReturnsList()
        {
            // Arrange
            _service.SaveTransformation("user1", "url", "res");

            // Act
            var history = _service.GetUserHistory("user1", 7);

            // Assert
            Assert.NotEmpty(history);
            Assert.Single(history);
        }

        [Fact]
        public void GetUserHistory_NoRecordsInPeriod_ReturnsEmptyList()
        {
            // Arrange
            // БД порожня

            // Act 
            var history = _service.GetUserHistory("user1", 1);

            // Assert (Очікуємо порожній список, а не Exception)
            Assert.Empty(history);
        }

        [Fact]
        public void GetUserHistory_DaysLimitZero_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            int days = 0;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _service.GetUserHistory("user1", days));
        }

        [Fact]
        public void GetUserHistory_DaysLimitNegative_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            int days = -1;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _service.GetUserHistory("user1", days));
        }

        [Fact]
        public void GetUserHistory_MinimumValidDaysLimit_ReturnsEmptyListIfNoData()
        {
            // Arrange
            int days = 1;

            // Act 
            var history = _service.GetUserHistory("user1", days);

            // Assert 
            Assert.Empty(history);
        }

        [Fact]
        public void ClearUserHistory_WithExistingRecords_ReturnsRemovedCount()
        {
            // Arrange
            _service.SaveTransformation("user1", "url1", "res1");
            _service.SaveTransformation("user1", "url2", "res2");

            // Act
            int removed = _service.ClearUserHistory("user1");

            // Assert
            Assert.Equal(2, removed);
        }

        [Fact]
        public void ClearUserHistory_NoRecords_ReturnsZero()
        {
            // Arrange
            // Для user2 нічого не зберігали

            // Act
            int removed = _service.ClearUserHistory("user2");

            // Assert
            Assert.Equal(0, removed);
        }

        [Fact]
        public void ClearUserHistory_NullUserId_ThrowsArgumentException()
        {
            // Arrange
            string? userId = null;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _service.ClearUserHistory(userId!));
        }
    }
}