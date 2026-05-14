# Таблиця проєктування тестів (Test Design)

**Модуль:** `FitVisionAI.Core`
**Класи, що тестуються:** `FitnessData`, `FitVisionSystemManager`
**Фреймворк:** xUnit, Moq
**Загальна кількість тест-кейсів:** 12

## Використані техніки тестування:
*   **EP (Equivalence Partitioning):** Розбиття на класи еквівалентності.
*   **BVA (Boundary Value Analysis):** Аналіз граничних значень.

## Тест-кейси

| Тест-кейс (Що тестуємо) | Вхідні дані | Очікуваний результат | Техніка | Статус |
| :--- | :--- | :--- | :--- | :--- |
| **TC-01: Мінімально допустима вага** | `Weight=20.0, BodyFat=15.0, Date=Today` | True, валідація успішна | BVA | Pass |
| **TC-02: Вага нижче допустимої** | `Weight=19.9, BodyFat=15.0, Date=Today` | Виняток `ArgumentOutOfRangeException` | BVA | Pass |
| **TC-03: Максимально допустимий жир** | `Weight=80.0, BodyFat=70.0, Date=Today` | True, валідація успішна | BVA | Pass |
| **TC-04: Відсоток жиру вище допустимого**| `Weight=80.0, BodyFat=70.1, Date=Today` | Виняток `ArgumentOutOfRangeException` | BVA | Pass |
| **TC-05: Дата запису з майбутнього** | `Weight=80.0, BodyFat=15.0, Date=Tomorrow`| Виняток `ArgumentException` | EP | Pass |
| **TC-06: Null замість об'єкта user** | `user=null, newRecords=[1 valid]` | Виняток `ArgumentNullException` | EP | Pass |
| **TC-07: Порожній пакет даних** | `user=User, newRecords=[]` | Виняток `ArgumentException` | BVA | Pass |
| **TC-08: Частково валідний пакет даних** | `user=User, newRecords=[1 valid, 1 invalid]`| Повертає `1` (кількість збережених), помилковий ігнорується | EP | Pass |
| **TC-09: Сповіщення вимкнені юзером** | `user.NotificationsEnabled=false, msg="ok"`| Об'єкт `Notification` (`IsSent=false`) | EP | Pass |
| **TC-10: Успішне Push-сповіщення** | `user.NotificationsEnabled=true, SendPush=true` | Об'єкт `Notification` (`IsSent=true`) | EP | Pass |
| **TC-11: Фолбек на Email при збої Push** | `user.NotificationsEnabled=true, SendPush=false, SendEmail=true`| Об'єкт `Notification` (`IsSent=true`) | EP | Pass |
| **TC-12: Помилка сервісу сповіщень** | `user.NotificationsEnabled=true, Service=Exception` | Об'єкт `Notification` (`IsSent=false`), програма не падає | EP | Pass |

## Результати виконання
Усі 12 тестів успішно пройдено. Досягнуто покриття коду (Line Coverage) > 80%. Під час ітеративного тестування (TC-01) було виявлено та виправлено логічну помилку в умові валідації граничного значення ваги.
