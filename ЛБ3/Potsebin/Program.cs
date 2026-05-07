using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;


namespace FitVisionAI_LB3
{
    public class Account
    {
        protected string ipAddress;

        /// <summary>
        /// Імітує підключення акаунта до сервера та встановлення IP-адреси.
        /// </summary>
        public void Connect()
        {
            ipAddress = "192.168.1.100";
            Console.WriteLine($"[Account] З'єднання встановлено. IP: {ipAddress}");
        }
    }
    /// <summary>
    /// Сервіс для управління авторизацією та безпекою.
    /// Перевіряє облікові дані та валідує токени доступу.
    /// </summary>
    public class AuthService
    {
        private string tokenSecret = "secret_key_123";

        /// <summary>
        /// Перевіряє введені користувачем email та пароль.
        /// Містить нетривіальну логіку генерації винятків для некоректних вхідних даних.
        /// </summary>
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


        /// <summary>
        /// Перевіряє чи відповідає наданий токен секретному ключу системи.
        /// </summary>
        public bool ValidateToken(string token)
        {
            return token == tokenSecret;
        }
    }


    /// <summary>
    /// Клас профілю користувача.
    /// Зберігає додаткову інформацію та агрегує фотографії.
    /// </summary>
    public class Profile
    {
        private int profileId;
        private DateTime createdAt;
        public List<Photo> Photos { get; set; } = new List<Photo>();


        /// <summary>
        /// Конструктор для створення нового профілю з прив'язкою до ID користувача.
        /// </summary>
        public Profile(int id)
        {
            profileId = id;
            createdAt = DateTime.Now;
        }
        /// <summary>
        /// Імітує процес оновлення даних профілю в базі даних.
        /// </summary>
        public void UpdateProfile()
        {
            Console.WriteLine($"[Profile] Профіль #{profileId} успішно оновлено.");
        }
    }
    
    /// <summary>
    /// Клас для роботи з фотографіями користувача.
    /// Відповідає за валідацію метаданих (розмір, формат) та імітацію завантаження.
    /// </summary>
    public class Photo
    {
        private string photoId;
        private string url;
        private float fileSize;

        /// <summary>
        /// Ініціалізує новий об'єкт фотографії із заданими параметрами.
        /// </summary>
        public Photo(string id, string url, float size)
        {
            this.photoId = id;
            this.url = url;
            this.fileSize = size;
        }

        /// <summary>
        /// Запускає процес завантаження фото. 
        /// Використовує блок try-catch для безпечної обробки помилок якості файлу.
        /// </summary>
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

        /// <summary>
        /// Аналізує розмір та розширення файлу.
        /// Генерує винятки, якщо файл занадто великий або пошкоджений (розмір <= 0).
        /// </summary>
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

    /// <summary>
    /// Клас Користувача, що наслідує загальний Акаунт.
    /// Реалізує логіку реєстрації та доступу до сервісу авторизації.
    /// </summary>
    public class User : Account
    {
        private int userId;
        private string email;
        private string passwordHash;
        public Profile UserProfile { get; set; }
        private AuthService authService;

        /// <summary>
        /// Створює користувача з посиланням на зовнішній сервіс авторизації.
        /// </summary>
        public User(int id, string email, AuthService auth)
        {
            this.userId = id;
            this.email = email;
            this.authService = auth;
        }

        /// <summary>
        /// Проводить валідацію формату email (наявність @ та крапки) через цикл.
        /// Створює композитний об'єкт Profile у разі успіху.
        /// </summary>
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

        /// <summary>
        /// Делегує перевірку пароля сервісу авторизації.
        /// </summary>
        public bool Login(string pass)
        {
            return authService.Authenticate(email, pass);
        }
    }
    
    /// <summary>
    /// Точка входу в програму. 
    /// Виключена зі звіту про покриття коду, оскільки відповідає лише за демонстрацію UI.
    /// </summary>
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