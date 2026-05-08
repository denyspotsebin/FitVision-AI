# Таблиця проєктування тестів (Test Design) - Сарібегян Арсен

**Модуль, що тестується:** `HistoryService.cs`

| ID | Тест-кейс | Вхідні дані | Очікуваний результат | Техніка (EP/BVA) | Статус |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **1** | `SaveTransformation`: валідні дані | userId="user1", photo="url", result="ok" | Повертає `true`, запис додано | EP (Позитивний) | pass |
| **2** | `SaveTransformation`: порожній userId | userId="", photo="url", result="ok" | Викидає `ArgumentException` | EP (Негативний) | pass |
| **3** | `GetUserHistory`: існуючий юзер, є історія | userId="user1", daysLimit=7 | Повертає список із 1 запису | EP (Позитивний) | pass |
| **4** | `GetUserHistory`: немає записів за період | userId="user1", daysLimit=1 | Викидає `InvalidOperationException` | EP (Негативний) | pass |
| **5** | `GetUserHistory`: межа daysLimit = 0 | userId="user1", daysLimit=0 | Викидає `ArgumentOutOfRangeException` | BVA (Негативний, межа) | pass |
| **6** | `GetUserHistory`: за межею daysLimit = -1 | userId="user1", daysLimit=-1 | Викидає `ArgumentOutOfRangeException` | BVA (Негативний, за межею) | pass |
| **7** | `GetUserHistory`: мінімально допустимий daysLimit | userId="user1", daysLimit=1 | Повертає історію або виняток (залежно від БД) | BVA (Позитивний, межа+1) | pass |
| **8** | `ClearUserHistory`: є записи для видалення | userId="user1" (наявні 2 записи) | Повертає 2 (кількість видалених) | EP (Позитивний) | pass |
| **9** | `ClearUserHistory`: немає записів для юзера | userId="user2" (порожньо) | Повертає 0 | EP (Позитивний) | pass |
| **10** | `ClearUserHistory`: null замість userId | userId=null | Викидає `ArgumentException` | EP (Негативний) | pass |