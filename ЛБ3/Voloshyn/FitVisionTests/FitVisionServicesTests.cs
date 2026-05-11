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
        // --- Тести для TargetParameters.ValidateData (Межі та Винятки) ---

        [Fact]
        public void ValidateData_WeightBelowLimit_ThrowsException()
        {
            // BVA (Негативний): Вага на нижній межі (30)
            // Arrange
            var parameters = new TargetParameters { DesiredWeight = 30.0f, BodyFatPercentage = 15.0f };

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => parameters.ValidateData());
        }

        [Fact]
        public void ValidateData_WeightAtLowerLimit_ReturnsTrue()
        {
            // BVA (Позитивний): Вага трохи вище нижньої межі (30.1)
            // Arrange
            var parameters = new TargetParameters { DesiredWeight = 30.1f, BodyFatPercentage = 15.0f };

            // Act
            bool result = parameters.ValidateData();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateData_WeightAboveLimit_ThrowsException()
        {
            // BVA (Негативний): Вага за верхньою межею (250.1)
            // Arrange
            var parameters = new TargetParameters { DesiredWeight = 250.1f, BodyFatPercentage = 15.0f };

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => parameters.ValidateData());
        }

        [Fact]
        public void ValidateData_FatBelowLimit_ThrowsException()
        {
            // BVA (Негативний): Відсоток жиру нижче межі (2.9)
            // Arrange
            var parameters = new TargetParameters { DesiredWeight = 70.0f, BodyFatPercentage = 2.9f };

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => parameters.ValidateData());
        }

        [Fact]
        public void ValidateData_FatAboveLimit_ThrowsException()
        {
            // BVA (Негативний): Відсоток жиру вище межі (50.1)
            // Arrange
            var parameters = new TargetParameters { DesiredWeight = 70.0f, BodyFatPercentage = 50.1f };

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => parameters.ValidateData());
        }

        // --- Тести для AIGeneratorService (Межі та Винятки) ---

        [Fact]
        public void CheckAvailableLimits_InvalidUserId_ThrowsException()
        {
            // BVA (Негативний): Некоректний ID (0)
            // Arrange
            var service = new AIGeneratorService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => service.CheckAvailableLimits(0));
        }

        [Fact]
        public void CheckAvailableLimits_LimitReached_ThrowsException()
        {
            // BVA (Негативний): Ліміт вичерпано (UsedRequests = 5)
            // Arrange
            var service = new AIGeneratorService { UsedRequests = 5 };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => service.CheckAvailableLimits(1));
        }

        // --- Тести для GenerateTransformation ---

        [Fact]
        public void GenerateTransformation_NullPhoto_ThrowsException()
        {
            // EP (Негативний): Передача null замість об'єкта фото
            // Arrange
            var service = new AIGeneratorService();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => service.GenerateTransformation(null, new TargetParameters()));
        }

        [Fact]
        public void GenerateTransformation_BadQualityPhoto_ThrowsException()
        {
            // EP (Негативний): Спроба генерації з поганим освітленням
            // Arrange
            var service = new AIGeneratorService();
            var photo = new BasePhoto { IsQualityGood = false };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => service.GenerateTransformation(photo, new TargetParameters()));
        }

        [Fact]
        public void GenerateTransformation_ValidData_ReturnsResultAndIncrementsLimit()
        {
            // EP (Позитивний): Успішний сценарій генерації
            // Arrange
            var service = new AIGeneratorService { UsedRequests = 0 };
            var photo = new BasePhoto { IsQualityGood = true };

            // Act
            string result = service.GenerateTransformation(photo, new TargetParameters());

            // Assert
            Assert.NotNull(result);
            Assert.Contains("Transformation_Success", result);
            Assert.Equal(1, service.UsedRequests); // Перевірка, що лічильник зріс
        }
    }
}