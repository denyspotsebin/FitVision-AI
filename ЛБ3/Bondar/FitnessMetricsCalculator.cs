using System;
using System.Collections.Generic;
using System.Linq;

namespace FitVisionAI.Core
{
    // ==========================================
    // 1. МОДЕЛІ ДАНИХ (Згідно з Class Diagram)
    // ==========================================

    public abstract class Account
    {
        public string IpAddress { get; set; }
        public DateTime LastLogin { get; set; }
        public virtual void Connect() { Console.WriteLine("Connected"); }
    }

    public class User : Account
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        
        // Властивість для діаграми послідовності (перевірка статус сповіщень)
        public bool NotificationsEnabled { get; set; } = true;

        // Композиція: користувач володіє даними
        public List<FitnessData> FitnessRecords { get; set; } = new List<FitnessData>();
        
        // Агрегація: сповіщення пов'язані з користувачем
        public List<Notification> Notifications { get; set; } = new List<Notification>();

        public void UpdateProfile() { /* Оновлення профілю */ }
    }

    public class Admin : User
    {
        public int AdminLevel { get; set; }
        public void ManageUser(int userId) { /* Логіка адміна */ }
        public void ModerateContent(int contentId) { /* Модерація */ }
    }

    public class Notification
    {
        public int NotificationId { get; set; }
        public string Message { get; set; }
        public bool IsSent { get; set; }

        public string CreateMessage() => $"[FitVision-AI]: {Message}";
    }

    // Інтерфейс для Dependency Injection (щоб легше було тестувати)
    public interface INotificationService
    {
        bool SendPush(int userId, string msg);
        bool SendEmail(int userId, string msg);
    }

    // ==========================================
    // 2. БІЗНЕС-ЛОГІКА (3 нетривіальні методи)
    // ==========================================

    public class FitnessData
    {
        public int DataId { get; set; }
        public float Weight { get; set; }
        public float BodyFat { get; set; }
        public DateTime Date { get; set; }

        // МЕТОД 1: Валідація даних (Умови, Викидання винятків)
        public bool ValidateData()
        {
            if (Weight <= 20.0f || Weight > 350.0f)
                throw new ArgumentOutOfRangeException(nameof(Weight), "Вага має бути в межах від 20 до 350 кг.");
            
            if (BodyFat < 2.0f || BodyFat > 70.0f)
                throw new ArgumentOutOfRangeException(nameof(BodyFat), "Відсоток жиру має бути від 2 до 70%.");
            
            if (Date > DateTime.Now)
                throw new ArgumentException("Дата запису не може бути в майбутньому.", nameof(Date));

            return true;
        }
    }

    public class FitVisionSystemManager
    {
        private readonly INotificationService _notificationService;

        public FitVisionSystemManager(INotificationService notificationService)
        {
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }

        // МЕТОД 2: Обробка пакету фітнес-даних (Цикли, Умови, Перехоплення винятків)
        public int ProcessFitnessDataBatch(User user, List<FitnessData> newRecords)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (newRecords == null || !newRecords.Any()) throw new ArgumentException("Пакет даних не може бути порожнім.");

            int validRecordsCount = 0;

            foreach (var record in newRecords)
            {
                try
                {
                    // Якщо дані валідні, додаємо їх (Композиція)
                    if (record.ValidateData())
                    {
                        user.FitnessRecords.Add(record);
                        validRecordsCount++;
                    }
                }
                catch (Exception ex)
                {
                    // Логуємо помилку для конкретного запису, але продовжуємо обробку циклу
                    Console.WriteLine($"Помилка валідації запису {record.DataId}: {ex.Message}");
                }
            }

            return validRecordsCount;
        }

        // МЕТОД 3: Повна реалізація Sequence Diagram FR-07 (Сповіщення)
        public Notification ProcessAnalysisAndNotify(User user, string analysisResultMsg)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var notification = new Notification
            {
                NotificationId = new Random().Next(1, 10000),
                Message = analysisResultMsg,
                IsSent = false
            };
            
            user.Notifications.Add(notification); // Агрегація

            // Згідно з Sequence Diagram: перевірка чи увімкнені сповіщення
            if (!user.NotificationsEnabled)
            {
                Console.WriteLine($"[Лог] Користувач {user.UserId} вимкнув сповіщення. Подію залоговано.");
                return notification; // IsSent залишається false
            }

            // Згідно з Sequence Diagram: надсилання через Notification Service
            string formattedMsg = notification.CreateMessage();
            try
            {
                // Намагаємось відправити Push, якщо ні - фолбек на Email
                bool pushSuccess = _notificationService.SendPush(user.UserId, formattedMsg);
                if (!pushSuccess)
                {
                    bool emailSuccess = _notificationService.SendEmail(user.UserId, formattedMsg);
                    notification.IsSent = emailSuccess;
                }
                else
                {
                    notification.IsSent = true;
                }
            }
            catch (Exception)
            {
                // Помилка сервісу сповіщень не повинна ламати систему (наприклад, відсутній API ключ)
                notification.IsSent = false;
            }

            return notification;
        }
    }
}
