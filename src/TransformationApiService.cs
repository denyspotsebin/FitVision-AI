using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FitVisionAI.Services
{
    /// <summary>
    /// Сервіс для взаємодії із зовнішнім AI API для обробки фотографій.
    /// </summary>
    public class TransformationApiService
    {
        private readonly HttpClient _httpClient;

        public TransformationApiService()
        {
            _httpClient = new HttpClient();
            // Тут в майбутньому буде базова адреса вашого AI API (наприклад, OpenAI або Midjourney API)
            // _httpClient.BaseAddress = new Uri("https://api.external-ai.com/v1/");
        }

        /// <summary>
        /// Відправляє фотографію та цільові параметри тіла до AI API.
        /// </summary>
        public async Task<string> ProcessImageAsync(string imagePath, string targetPhysique)
        {
            Console.WriteLine($"[Лог] Відправка фото: {imagePath}");
            Console.WriteLine($"[Лог] Цільові параметри: {targetPhysique}");

            // TODO: Реалізувати формування multipart/form-data запиту та обробку JSON-відповіді
            
            // Тимчасова імітація затримки мережі
            await Task.Delay(1000);

            // Повертаємо фейковий URL згенерованого зображення для тестування
            return "https://fitvision-ai.com/results/fake_transformed_image.jpg";
        }
    }
}