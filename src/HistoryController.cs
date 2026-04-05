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
            public string Id { get; set; } = string.Empty;
            public string ResultImageUrl { get; set; } = string.Empty;
            public string TargetPhysique { get; set; } = string.Empty;
            public DateTime CreatedAt { get; set; }
        }

        /// <summary>
        /// Повертає список усіх збережених генерацій для поточного авторизованого користувача.
        /// </summary>
        [HttpGet("/api/history")]
        public IActionResult GetUserHistory([FromQuery] string? query = null)
        {
            string currentUserId = "user_12345";
            Console.WriteLine($"[Debug] Авторизовано користувача: {currentUserId}");

            var mockHistory = new List<GenerationRecord>
            {
                new GenerationRecord { Id = "gen_001", ResultImageUrl = "res_001.jpg", TargetPhysique = "Рельєф", CreatedAt = DateTime.UtcNow.AddDays(-2) },
                new GenerationRecord { Id = "gen_002", ResultImageUrl = "res_002.jpg", TargetPhysique = "Набір маси", CreatedAt = DateTime.UtcNow.AddHours(-5) }
            };

            // Додаємо логіку пошуку за ключовим словом
            var results = mockHistory.AsQueryable();
            if (!string.IsNullOrEmpty(query))
            {
                results = results.Where(r => r.TargetPhysique.Contains(query, StringComparison.OrdinalIgnoreCase));
            }

            return Ok(results.OrderByDescending(record => record.CreatedAt).ToList());
        }
    }
}