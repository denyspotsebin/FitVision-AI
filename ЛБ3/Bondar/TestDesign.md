# Таблиця проєктування тестів (Test Design) - Бондар Артем

**Модуль, що тестується:** `FitVisionSystemManager.cs`

| ID | Тест-кейс | Вхідні дані | Очікуваний результат | Техніка (EP/BVA) | Статус |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **1** | `ValidateData`: мінімально допустима вага | `Weight=20.0, BodyFat=15.0, Date=Today` | Повертає `true`, валідація успішна | BVA (Позитивний, межа) | pass |
| **2** | `ValidateData`: вага нижче допустимої | `Weight=19.9, BodyFat=15.0, Date=Today` | Викидає `ArgumentOutOfRangeException` | BVA (Негативний, за межею) | pass |
| **3** | `ValidateData`: максимально допустимий жир | `Weight=80.0, BodyFat=70.0, Date=Today` | Повертає `true`, валідація успішна | BVA (Позитивний, межа) | pass |
| **4** | `ValidateData`: жир вище допустимого | `Weight=80.0, BodyFat=70.1, Date=Today` | Викидає `ArgumentOutOfRangeException` | BVA (Негативний, за межею) | pass |
| **5** | `ValidateData`: дата запису з майбутнього | `Weight=80.0, BodyFat=15.0, Date=Tomorrow` | Викидає `ArgumentException` | EP (Негативний) | pass |
| **6** | `ProcessFitnessDataBatch`: null замість user | `user=null, newRecords=[1 valid]` | Викидає `ArgumentNullException` | EP (Негативний) | pass |
| **7** | `ProcessFitnessDataBatch`: порожній пакет | `user=User, newRecords=[]` | Викидає `ArgumentException` | BVA (Негативний, межа) | pass |
| **8** | `ProcessFitnessDataBatch`: мікс записів | `user=User, newRecords=[1 valid, 1 invalid]`| Повертає `1` (кількість збережених) | EP (Позитивний) | pass |
| **9** | `ProcessAnalysisAndNotify`: вимкнені сповіщення | `user.NotificationsEnabled=false` | Повертає об'єкт `Notification` (`IsSent=false`) | EP (Позитивний) | pass |
| **10** | `ProcessAnalysisAndNotify`: успішний Push | `NotificationsEnabled=true, SendPush=true`| Повертає об'єкт `Notification` (`IsSent=true`) | EP (Позитивний) | pass |
| **11** | `ProcessAnalysisAndNotify`: збій Push, успіх Email | `SendPush=false, SendEmail=true` | Повертає об'єкт `Notification` (`IsSent=true`) | EP (Позитивний) | pass |
| **12** | `ProcessAnalysisAndNotify`: помилка сервісу | `user.Enabled=true, Service=Exception` | Повертає об'єкт `Notification` (`IsSent=false`) | EP (Негативний) | pass |
