using System;
using System.Collections.Generic;
using System.Linq;

namespace FitVisionAI.Core
{
    // ==========================================
    // 1. МОДЕЛІ ДАНИХ
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
        
        public bool NotificationsEnabled { get; set; } = true;

        public List<FitnessData> FitnessRecords { get; set; } = new List<FitnessData>();
        public List<Notification> Notifications { get; set; } = new List<Notification>();

        public void UpdateProfile() { }
    }

    public class Admin : User
    {
        public int AdminLevel { get; set; }
        public void ManageUser(int userId) { }
        public void ModerateContent(int contentId) { }
    }

    public class Notification
    {
        public int NotificationId { get; set; }
        public string Message { get; set; }
        public bool IsSent { get; set; }

        public string CreateMessage() => $"[FitVision-AI]: {Message}";
    }

    public interface INotificationService
    {
        bool SendPush(int userId, string msg);
        bool SendEmail(int userId, string msg);
    }

    // ==========================================
    // 2. БІЗНЕС-ЛОГІКА
    // ==========================================

    public class FitnessData
    {
        public int DataId { get; set; }
        public float Weight { get; set; }
        public float BodyFat { get; set; }
        public DateTime Date { get; set; }

        public bool ValidateData()
        {
            if (Weight < 20.0f || Weight > 350.0f)
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

        public int ProcessFitnessDataBatch(User user, List<FitnessData> newRecords)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (newRecords == null || !newRecords.Any()) throw new ArgumentException("Пакет даних не може бути порожнім.");

            int validRecordsCount = 0;

            foreach (var record in newRecords)
            {
                try
                {
                    if (record.ValidateData())
                    {
                        user.FitnessRecords.Add(record);
                        validRecordsCount++;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка валідації запису {record.DataId}: {ex.Message}");
                }
            }

            return validRecordsCount;
        }

        public Notification ProcessAnalysisAndNotify(User user, string analysisResultMsg)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var notification = new Notification
            {
                NotificationId = new Random().Next(1, 10000),
                Message = analysisResultMsg,
                IsSent = false
            };
            
            user.Notifications.Add(notification);

            if (!user.NotificationsEnabled)
            {
                return notification; 
            }

            string formattedMsg = notification.CreateMessage();
            try
            {
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
                notification.IsSent = false;
            }

            return notification;
        }
    }
}