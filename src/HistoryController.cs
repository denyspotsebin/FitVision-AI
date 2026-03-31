using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitVisionAI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize] // Розкоментувати, коли до проєкту буде прикручено JWT-токени
    public class HistoryController : ControllerBase
    {
        // Модель даних для відображення однієї генерації
        public class GenerationRecord
        {
            public string Id { get; set; }
            public string ResultImageUrl { get; set; }
            public string TargetPhysique { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        /// <summary>
        /// Повертає список усіх збережених генерацій для поточного авторизованого користувача.
        /// </summary>
        [HttpGet("/api/history")]
        public IActionResult GetUserHistory()
        {
            // TODO: Отримати реальний ID користувача з токена авторизації (наприклад, з User.Claims)
            string currentUserId = "user_12345"; 

            // Тимчасова заглушка (Mock-дані) замість звернення до реальної бази даних
            var mockHistory = new List<GenerationRecord>
            {
                new GenerationRecord 
                { 
                    Id = "gen_001", 
                    ResultImageUrl = "https://fitvision-ai.com/images/res_001.jpg", 
                    TargetPhysique = "Рельєф", 
                    CreatedAt = DateTime.UtcNow.AddDays(-2) 
                },
                new GenerationRecord 
                { 
                    Id = "gen_002", 
                    ResultImageUrl = "https://fitvision-ai.com/images/res_002.jpg", 
                    TargetPhysique = "Набір маси", 
                    CreatedAt = DateTime.UtcNow.AddHours(-5) 
                }
            };

            // Головна логіка: фільтруємо по юзеру (в майбутньому) і СОРТУЄМО ЗА ДАТОЮ (від новіших до старіших)
            var sortedHistory = mockHistory
                .OrderByDescending(record => record.CreatedAt)
                .ToList();

            // Повертаємо HTTP 200 OK та JSON із даними
            return Ok(sortedHistory);
        }
    }
}