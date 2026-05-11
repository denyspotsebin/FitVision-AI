# Таблиця проєктування тестів (Test Design) - Волошин Роман

**Модулі, що тестуються:** `TargetParameters`, `AIGeneratorService` (у файлі `FitVisionServices.cs`)

| ID | Тест-кейс | Вхідні дані | Очікуваний результат | Техніка (EP/BVA) | Статус |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **1** | Вага нижче нижньої межі | `DesiredWeight = 30.0f` | Виняток `ArgumentOutOfRangeException` | BVA (Негативний) | pass |
| **2** | Вага на нижній межі (валідна) | `DesiredWeight = 30.1f` | Повертає `true` | BVA (Позитивний) | pass |
| **3** | Вага на верхній межі (валідна) | `DesiredWeight = 250.0f` | Повертає `true` | BVA (Позитивний) | pass |
| **4** | Вага вище верхньої межі | `DesiredWeight = 250.1f` | Виняток `ArgumentOutOfRangeException` | BVA (Негативний) | pass |
| **5** | Жир нижче нижньої межі | `BodyFatPercentage = 2.9f` | Виняток `ArgumentOutOfRangeException` | BVA (Негативний) | pass |
| **6** | Жир вище верхньої межі | `BodyFatPercentage = 50.1f` | Виняток `ArgumentOutOfRangeException` | BVA (Негативний) | pass |
| **7** | Валідні середні значення | `Weight = 70.0f`, `Fat = 15.0f` | Повертає `true` | EP (Позитивний) | pass |
| **8** | Некоректний ID користувача | `userId = 0` | Викидає `ArgumentException` | BVA (Негативний) | pass |
| **9** | Ліміт запитів не вичерпано | `UsedRequests = 4` | Повертає `true` | EP (Позитивний) | pass |
| **10** | Ліміт запитів вичерпано | `UsedRequests = 5` | Викидає `InvalidOperationException` | BVA (Негативний) | pass |
| **11** | Фото відсутнє (null) | `photo = null` | Викидає `ArgumentNullException` | EP (Негативний) | pass |
| **12** | Погана якість фото | `IsQualityGood = false` | Викидає `ArgumentException` | EP (Негативний) | pass |
| **13** | Успішна генерація | `IsQualityGood = true` | Повертає ім'я файлу (рядок) | EP (Позитивний) | pass |
