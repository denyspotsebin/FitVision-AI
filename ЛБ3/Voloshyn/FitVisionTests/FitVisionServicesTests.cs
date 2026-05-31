using System;
using Xunit;
using FitVisionAI.Services;

namespace FitVisionTests
{
    public class FitVisionServicesTests
    {
        // ТЕСТ 1: Позитивний сценарій валідації даних
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
        [Fact]
        public void CheckAvailableLimits_UnderLimit_ReturnsTrue()
        {
            // Arrange
            var service = new AIGeneratorService(); // Прибрали { UsedRequests = 0 }, бо тепер private set (за замовчуванням 0)

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
        public void ValidateData_WeightBelowLimit_ReturnsFalse()
        {
            // BVA (Негативний): Вага за нижньою межею (29.9)
            // Arrange
            var parameters = new TargetParameters { DesiredWeight = 29.9f, BodyFatPercentage = 15.0f };

            // Act
            bool result = parameters.ValidateData();

            // Assert
            Assert.False(result); // Тепер метод повертає false замість винятку
        }

        [Fact]
        public void ValidateData_WeightAtLowerLimit_ReturnsTrue()
        {
            // BVA (Позитивний): Вага рівно на нижній межі (30.0 - тепер це валідно)
            // Arrange
            var parameters = new TargetParameters { DesiredWeight = 30.0f, BodyFatPercentage = 15.0f };

            // Act
            bool result = parameters.ValidateData();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateData_WeightAboveLimit_ReturnsFalse()
        {
            // BVA (Негативний): Вага за верхньою межею (250.1)
            // Arrange
            var parameters = new TargetParameters { DesiredWeight = 250.1f, BodyFatPercentage = 15.0f };

            // Act
            bool result = parameters.ValidateData();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateData_FatBelowLimit_ReturnsFalse()
        {
            // BVA (Негативний): Відсоток жиру нижче межі (2.9)
            // Arrange
            var parameters = new TargetParameters { DesiredWeight = 70.0f, BodyFatPercentage = 2.9f };

            // Act
            bool result = parameters.ValidateData();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateData_FatAboveLimit_ReturnsFalse()
        {
            // BVA (Негативний): Відсоток жиру вище межі (50.1)
            // Arrange
            var parameters = new TargetParameters { DesiredWeight = 70.0f, BodyFatPercentage = 50.1f };

            // Act
            bool result = parameters.ValidateData();

            // Assert
            Assert.False(result);
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
            // BVA (Негативний): Ліміт вичерпано. Оскільки UsedRequests тепер private, генеруємо 5 разів вручну.
            // Arrange
            var service = new AIGeneratorService();
            var photo = new BasePhoto { IsQualityGood = true };
            var goals = new TargetParameters();

            // Використовуємо всі 5 лімітів
            for (int i = 0; i < 5; i++)
            {
                service.GenerateTransformation(photo, goals);
            }

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
            BasePhoto? nullPhoto = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => service.GenerateTransformation(nullPhoto!, new TargetParameters()));
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
            var service = new AIGeneratorService(); // Прибрали UsedRequests = 0
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