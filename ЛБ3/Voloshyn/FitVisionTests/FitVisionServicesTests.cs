using System;
using Xunit;
using FitVisionAI.Services;

namespace FitVisionTests
{
    public class FitVisionServicesTests
    {
        // ТЕСТ 1: Позитивний сценарій валідації даних
        // Покриває лише успішне повернення 'true', ігноруючи всі винятки.
        [Fact]
        public void ValidateData_ValidParameters_ReturnsTrue()
        {
            // Arrange
            var parameters = new TargetParameters { DesiredWeight = 75.0f, BodyFatPercentage = 15.0f };

            // Act
            bool result = parameters.ValidateData();

            // Assert
            Assert.True(result);
        }

        // ТЕСТ 2: Позитивний сценарій перевірки лімітів
        // Пропускає гілки з некоректним ID та вичерпаним лімітом.
        [Fact]
        public void CheckAvailableLimits_UnderLimit_ReturnsTrue()
        {
            // Arrange
            var service = new AIGeneratorService { UsedRequests = 0 };

            // Act
            bool result = service.CheckAvailableLimits(101);

            // Assert
            Assert.True(result);
        }

        // ТЕСТ 3: Базова перевірка аналізу освітлення
        [Fact]
        public void AnalyzeLighting_QualityIsGood_ReturnsTrue()
        {
            // Arrange
            var photo = new BasePhoto { IsQualityGood = true };

            // Act
            bool result = photo.AnalyzeLighting();

            // Assert
            Assert.True(result);
        }
    }
}