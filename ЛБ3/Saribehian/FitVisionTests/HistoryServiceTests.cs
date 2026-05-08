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
        public void SaveTransformation_ValidData_ReturnsTrue()
        {
            // Arrange (Техника: EP, Позитивный)
            string userId = "user1";
            string photo = "http://photo.com/1.jpg";
            string result = "Success";

            // Act
            bool isSaved = _service.SaveTransformation(userId, photo, result);

            // Assert
            Assert.True(isSaved);
        }

        [Fact]
        public void SaveTransformation_EmptyUserId_ThrowsArgumentException()
        {
            // Arrange (Техника: EP, Негативный)
            string userId = "";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _service.SaveTransformation(userId, "url", "res"));
        }

        [Fact]
        public void GetUserHistory_ExistingRecords_ReturnsList()
        {
            // Arrange (Техника: EP, Позитивный)
            _service.SaveTransformation("user1", "url", "res");

            // Act
            var history = _service.GetUserHistory("user1", 7);

            // Assert
            Assert.NotEmpty(history);
            Assert.Single(history);
        }

        [Fact]
        public void GetUserHistory_NoRecordsInPeriod_ThrowsInvalidOperationException()
        {
            // Arrange (Техника: EP, Негативный)
            // БД пуста

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _service.GetUserHistory("user1", 1));
        }

        [Fact]
        public void GetUserHistory_DaysLimitZero_ThrowsArgumentOutOfRangeException()
        {
            // Arrange (Техника: BVA, Негативный, межа 0)
            int days = 0;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _service.GetUserHistory("user1", days));
        }

        [Fact]
        public void GetUserHistory_DaysLimitNegative_ThrowsArgumentOutOfRangeException()
        {
            // Arrange (Техника: BVA, Негативный, за межою -1)
            int days = -1;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _service.GetUserHistory("user1", days));
        }

        [Fact]
        public void GetUserHistory_MinimumValidDaysLimit_ReturnsResultOrThrows()
        {
            // Arrange (Техника: BVA, Позитивный, межа 1)
            int days = 1;

            // Act & Assert (Если истории нет, бросит InvalidOperationException - это валидное поведение)
            Assert.Throws<InvalidOperationException>(() => _service.GetUserHistory("user1", days));
        }

        [Fact]
        public void ClearUserHistory_WithExistingRecords_ReturnsRemovedCount()
        {
            // Arrange (Техника: EP, Позитивный)
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
            // Arrange (Техника: EP, Позитивный)
            // Для юзера user2 ничего не сохраняли

            // Act
            int removed = _service.ClearUserHistory("user2");

            // Assert
            Assert.Equal(0, removed);
        }

        [Fact]
        public void ClearUserHistory_NullUserId_ThrowsArgumentException()
        {
            // Arrange (Техника: EP, Негативный)
            string? userId = null;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _service.ClearUserHistory(userId!));
        }
    }
}