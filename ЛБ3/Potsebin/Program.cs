using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;


namespace FitVisionAI_LB3
{
    public class Account
    {
        protected string ipAddress;
        public void Connect()
        {
            ipAddress = "192.168.1.100";
            Console.WriteLine($"[Account] З'єднання встановлено. IP: {ipAddress}");
        }
    }

    public class AuthService
    {
        private string tokenSecret = "secret_key_123";

        public bool Authenticate(string email, string pass)
        {
            Console.WriteLine("[AuthService] Спроба авторизації...");
            
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pass))
            {
                throw new ArgumentException("Помилка: Email та пароль не можуть бути порожніми.");
            }

            if (email == "user@fitvision.com" && pass == "qwerty")
            {
                Console.WriteLine("[AuthService] Успіх: Авторизація пройдена.");
                return true;
            }
            
            Console.WriteLine("[AuthService] Відмова: Невірні облікові дані.");
            return false;
        }

        public bool ValidateToken(string token)
        {
            return token == tokenSecret;
        }
    }

    public class Profile
    {
        private int profileId;
        private DateTime createdAt;
        public List<Photo> Photos { get; set; } = new List<Photo>();

        public Profile(int id)
        {
            profileId = id;
            createdAt = DateTime.Now;
        }

        public void UpdateProfile()
        {
            Console.WriteLine($"[Profile] Профіль #{profileId} успішно оновлено.");
        }
    }

    public class Photo
    {
        private string photoId;
        private string url;
        private float fileSize;

        public Photo(string id, string url, float size)
        {
            this.photoId = id;
            this.url = url;
            this.fileSize = size;
        }

        public bool Upload()
        {
            try
            {
                if (ValidateQuality())
                {
                    Console.WriteLine($"[Photo] Фото за посиланням {url} успішно завантажено в систему.");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Photo] Системна помилка під час завантаження: {ex.Message}");
                return false;
            }
        }

        public bool ValidateQuality()
        {
            Console.WriteLine("[Photo] Аналіз якості зображення...");
            
            if (fileSize <= 0)
            {
                throw new InvalidOperationException("Розмір файлу некоректний (0 або менше).");
            }

            if (fileSize > 15.0f)
            {
                throw new Exception("Файл занадто великий. Обмеження: 15 МБ.");
            }

            if (!url.EndsWith(".jpg") && !url.EndsWith(".png"))
            {
                Console.WriteLine("[Photo] Попередження: Неоптимальний формат. AI найкраще працює з .jpg або .png.");
            }

            Console.WriteLine("[Photo] Якість прийнятна.");
            return true;
        }
    }

    public class User : Account
    {
        private int userId;
        private string email;
        private string passwordHash;
        public Profile UserProfile { get; set; }
        private AuthService authService;

        public User(int id, string email, AuthService auth)
        {
            this.userId = id;
            this.email = email;
            this.authService = auth;
        }

        public void Register()
        {
            Console.WriteLine("\n--- Старт реєстрації користувача ---");
            
            bool hasAtSymbol = false;
            bool hasDot = false;
            
            foreach (char c in email)
            {
                if (c == '@') hasAtSymbol = true;
                if (c == '.') hasDot = true;
            }

            if (!hasAtSymbol || !hasDot)
            {
                throw new FormatException("Введено некоректний email. Реєстрацію скасовано.");
            }

            passwordHash = "hashed_12345";
            UserProfile = new Profile(userId);
            Console.WriteLine($"[User] Користувач {email} зареєстрований успішно. Профіль створено.");
        }

        public bool Login(string pass)
        {
            return authService.Authenticate(email, pass);
        }
    }
    
    
    [ExcludeFromCodeCoverage]
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Ініціалізація FitVision-AI ===\n");
            
            AuthService auth = new AuthService();
            User user1 = new User(1, "user@fitvision.com", auth);
            
            try 
            {
                user1.Connect();
                user1.Register();
                
                if (user1.Login("qwerty"))
                {
                    Console.WriteLine("\n--- Завантаження першого фото ---");
                    Photo myPhoto = new Photo("img_01", "body_front.jpg", 4.2f);
                    user1.UserProfile.Photos.Add(myPhoto);
                    myPhoto.Upload();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[КРИТИЧНА ПОМИЛКА СИСТЕМИ]: {ex.Message}");
            }
            
            Console.WriteLine("\nРоботу програми завершено.");
        }
    }
}