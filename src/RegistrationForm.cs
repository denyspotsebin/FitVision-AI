namespace FitVisionAI
{
    class RegistrationForm
    {
        //Реалізувати реєстраційну форму яка містить поля Email, пароль 8+ символів, чи є вам 18 років?
        // Якщо дані некоректні — вивести помилку червоним кольором і запитати знову.
        // Після успішної валідації вивести повідомлення зеленим кольором.
public string Email { get; set; } = string.Empty;
public string Password { get; set; } = string.Empty;
        public bool IsAdult { get; set; }
        // Метод для запуску процесу реєстрації
        public void RunRegistration()
        {
            Console.WriteLine("=== Реєстрація в FitVision-AI ===");
            Console.Write("Введіть ваш Email: ");
            Email = Console.ReadLine() ?? string.Empty;

            Console.Write("Введіть ваш пароль (не менше 8 символів): ");
            Password = Console.ReadLine() ?? string.Empty;

            Console.Write("Ви старше 18 років? (так/ні): ");
            string? ageResponse = Console.ReadLine();
            IsAdult = ageResponse?.ToLower() == "так";

            Validate();
        }
        public bool Validate()
        {
            if (string.IsNullOrEmpty(Email) || !Email.Contains("@"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Некоректний Email. Спробуйте ще раз.");
                Console.ResetColor();
                return false;
            }

            if (string.IsNullOrEmpty(Password) || Password.Length < 8)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Пароль повинен містити не менше 8 символів. Спробуйте ще раз.");
                Console.ResetColor();
                return false;
            }

            if (!IsAdult)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ви повинні бути старше 18 років для реєстрації. Спробуйте ще раз.");
                Console.ResetColor();
                return false;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Реєстрація успішна!");
            Console.ResetColor();
            return true;
        }
    }
}