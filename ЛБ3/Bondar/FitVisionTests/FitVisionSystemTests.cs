using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using FitVisionAI.Core;

namespace FitVisionAI.Tests
{
    public class FitVisionSystemTests
    {
        // ==========================================
        // ТЕСТИ ДЛЯ МЕТОДУ: ValidateData
        // ==========================================

        [Fact]
        public void ValidateData_MinimumValidWeight_ReturnsTrue()
        {
            // Техніка: BVA (Позитивний) - Нижня границя ваги (20.0)
            
            // Arrange
            var data = new FitnessData 
            { 
                Weight = 20.0f, 
                BodyFat = 15.0f, 
                Date = DateTime.Now 
            };

            // Act
            bool result = data.ValidateData();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateData_WeightBelowMinimum_ThrowsArgumentOutOfRangeException()
        {
            // Техніка: BVA (Негативний) - Вихід за нижню границю ваги (19.9)
            
            // Arrange
            var data = new FitnessData 
            { 
                Weight = 19.9f, 
                BodyFat = 15.0f, 
                Date = DateTime.Now 
            };

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => data.ValidateData());
        }

        [Fact]
        public void ValidateData_MaximumValidBodyFat_ReturnsTrue()
        {
            // Техніка: BVA (Позитивний) - Верхня границя відсотка жиру (70.0)
            
            // Arrange
            var data = new FitnessData 
            { 
                Weight = 80.0f, 
                BodyFat = 70.0f, 
                Date = DateTime.Now 
            };

            // Act
            bool result = data.ValidateData();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateData_BodyFatAboveMaximum_ThrowsArgumentOutOfRangeException()
        {
            // Техніка: BVA (Негативний) - Вихід за верхню границю жиру (70.1)
            
            // Arrange
            var data = new FitnessData 
            { 
                Weight = 80.0f, 
                BodyFat = 70.1f, 
                Date = DateTime.Now 
            };

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => data.ValidateData());
        }

        [Fact]
        public void ValidateData_FutureDate_ThrowsArgumentException()
        {
            // Техніка: EP (Негативний) - Дата у майбутньому
            
            // Arrange
            var data = new FitnessData 
            { 
                Weight = 80.0f, 
                BodyFat = 15.0f, 
                Date = DateTime.Now.AddDays(1) 
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => data.ValidateData());
        }

        // ==========================================
        // ТЕСТИ ДЛЯ МЕТОДУ: ProcessFitnessDataBatch
        // ==========================================

        [Fact]
        public void ProcessFitnessDataBatch_NullUser_ThrowsArgumentNullException()
        {
            // Техніка: EP (Негативний) - Перевірка на null для користувача
            
            // Arrange
            var mockService = new Mock<INotificationService>();
            var manager = new FitVisionSystemManager(mockService.Object);
            var records = new List<FitnessData> { new FitnessData { Weight = 80.0f, BodyFat = 15.0f } };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => manager.ProcessFitnessDataBatch(null, records));
        }

        [Fact]
        public void ProcessFitnessDataBatch_EmptyRecordsList_ThrowsArgumentException()
        {
            // Техніка: BVA (Негативний) - Нижня границя кількості записів (0)
            
            // Arrange
            var mockService = new Mock<INotificationService>();
            var manager = new FitVisionSystemManager(mockService.Object);
            var user = new User { UserId = 1 };
            var emptyRecords = new List<FitnessData>();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => manager.ProcessFitnessDataBatch(user, emptyRecords));
        }

        [Fact]
        public void ProcessFitnessDataBatch_MixedValidAndInvalidRecords_ReturnsOnlyValidCount()
        {
            // Техніка: EP (Позитивний) - Змішаний пакет даних (1 валідний, 1 помилковий)
            
            // Arrange
            var mockService = new Mock<INotificationService>();
            var manager = new FitVisionSystemManager(mockService.Object);
            var user = new User { UserId = 1 };
            var mixedRecords = new List<FitnessData>
            {
                new FitnessData { DataId = 1, Weight = 80.0f, BodyFat = 15.0f, Date = DateTime.Now }, // Валідний
                new FitnessData { DataId = 2, Weight = 10.0f, BodyFat = 15.0f, Date = DateTime.Now }  // Невалідний (вага < 20)
            };

            // Act
            int savedCount = manager.ProcessFitnessDataBatch(user, mixedRecords);

            // Assert
            Assert.Equal(1, savedCount);
            Assert.Single(user.FitnessRecords); // Перевіряємо, що додано лише 1 запис
        }

        // ==========================================
        // ТЕСТИ ДЛЯ МЕТОДУ: ProcessAnalysisAndNotify
        // ==========================================

        [Fact]
        public void ProcessAnalysisAndNotify_NotificationsDisabled_DoesNotSend()
        {
            // Техніка: EP (Позитивний) - Сповіщення вимкнені в налаштуваннях
            
            // Arrange
            var mockService = new Mock<INotificationService>();
            var manager = new FitVisionSystemManager(mockService.Object);
            var user = new User { UserId = 1, NotificationsEnabled = false };

            // Act
            var result = manager.ProcessAnalysisAndNotify(user, "Test Analysis");

            // Assert
            Assert.False(result.IsSent);
            mockService.Verify(s => s.SendPush(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void ProcessAnalysisAndNotify_PushSuccess_ReturnsIsSentTrue()
        {
            // Техніка: EP (Позитивний) - Успішна відправка Push-сповіщення
            
            // Arrange
            var mockService = new Mock<INotificationService>();
            mockService.Setup(s => s.SendPush(It.IsAny<int>(), It.IsAny<string>())).Returns(true);
            var manager = new FitVisionSystemManager(mockService.Object);
            var user = new User { UserId = 1, NotificationsEnabled = true };

            // Act
            var result = manager.ProcessAnalysisAndNotify(user, "Test Analysis");

            // Assert
            Assert.True(result.IsSent);
            mockService.Verify(s => s.SendPush(user.UserId, It.IsAny<string>()), Times.Once);
            mockService.Verify(s => s.SendEmail(It.IsAny<int>(), It.IsAny<string>()), Times.Never); // Email не викликається
        }

        [Fact]
        public void ProcessAnalysisAndNotify_PushFailsEmailSuccess_ReturnsIsSentTrue()
        {
            // Техніка: EP (Позитивний) - Фолбек на Email при помилці Push
            
            // Arrange
            var mockService = new Mock<INotificationService>();
            mockService.Setup(s => s.SendPush(It.IsAny<int>(), It.IsAny<string>())).Returns(false); // Push падає
            mockService.Setup(s => s.SendEmail(It.IsAny<int>(), It.IsAny<string>())).Returns(true); // Email працює
            
            var manager = new FitVisionSystemManager(mockService.Object);
            var user = new User { UserId = 1, NotificationsEnabled = true };

            // Act
            var result = manager.ProcessAnalysisAndNotify(user, "Test Analysis");

            // Assert
            Assert.True(result.IsSent);
            mockService.Verify(s => s.SendPush(user.UserId, It.IsAny<string>()), Times.Once);
            mockService.Verify(s => s.SendEmail(user.UserId, It.IsAny<string>()), Times.Once); // Перевіряємо фолбек
        }

        [Fact]
        public void ProcessAnalysisAndNotify_ServiceThrowsException_HandlesGracefullyAndReturnsIsSentFalse()
        {
            // Техніка: EP (Негативний) - Збій на стороні NotificationService
            
            // Arrange
            var mockService = new Mock<INotificationService>();
            mockService.Setup(s => s.SendPush(It.IsAny<int>(), It.IsAny<string>())).Throws(new Exception("Network Error"));
            
            var manager = new FitVisionSystemManager(mockService.Object);
            var user = new User { UserId = 1, NotificationsEnabled = true };

            // Act
            var result = manager.ProcessAnalysisAndNotify(user, "Test Analysis");

            // Assert
            Assert.False(result.IsSent); // Виняток перехоплено, програма не впала
        }
    }
}