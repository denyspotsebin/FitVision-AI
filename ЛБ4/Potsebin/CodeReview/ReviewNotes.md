## Code Review Notes
**Перевіряючий:** [Поцебін Денис]
**Файл:** `FitVisionSystemManager.cs`

### Проблема 1
* **(а) Рядок коду:** 65-70 (`if (Weight < 20.0f || Weight > 350.0f)` та `if (BodyFat < 2.0f || BodyFat > 70.0f)`)
* **(б) Категорія code smell:** Magic Numbers (Магічні числа).
* **(в) Рекомендація:** Винести жорстко закодовані межі ваги та жиру у константи класу (наприклад, `private const float MinWeight = 20.0f;`). Це спростить подальшу зміну вимог до бізнес-логіки.

### Проблема 2
* **(а) Рядок коду:** 104-108 (`catch (Exception ex) { Console.WriteLine($"Помилка...: {ex.Message}"); }`)
* **(б) Категорія code smell:** Swallowed Exception (Проковтнутий виняток).
* **(в) Рекомендація:** Використання `Console.WriteLine` у бізнес-логіці приховує реальні збої від системи. Необхідно впровадити інтерфейс логера (напр. `ILogger`) для запису помилок або прокидати виняток вище.

### Проблема 3
* **(а) Рядок коду:** 120 (`NotificationId = new Random().Next(1, 10000),`)
* **(б) Категорія code smell:** Hardcoded Dependency / Magic Number.
* **(в) Рекомендація:** Використання `Random` для генерації ID є ненадійним (можливі колізії). Краще використовувати `Guid.NewGuid().ToString()` або передавати генерацію ID на рівень бази даних. Число `10000` слід винести в константу.

### Проблема 4
* **(а) Рядок коду:** 28, 34, 35 (порожні методи `UpdateProfile() { }`, `ManageUser(int userId) { }`, `ModerateContent(int contentId) { }`)
* **(б) Категорія code smell:** Dead Code / Incomplete Implementation.
* **(в) Рекомендація:** Порожні методи створюють ілюзію робочого функціоналу. Їх слід або видалити згідно з принципом YAGNI, або додати тіло `throw new NotImplementedException();`.

### Проблема 5
* **(а) Рядок коду:** 135-144 (`if (!pushSuccess) { ... } else { notification.IsSent = true; }`)
* **(б) Категорія code smell:** Unnecessary Else (Зайвий блок Else) / Ускладнена логіка.
* **(в) Рекомендація:** Спростити логіку маршрутизації за допомогою Guard Clauses або прямого присвоєння. Наприклад: 
  `notification.IsSent = _notificationService.SendPush(...);`
  `if (!notification.IsSent) { notification.IsSent = _notificationService.SendEmail(...); }`
