# Написание коннектора с ИИ

Пошаговое руководство по созданию коннектора к бирже на базе StockSharp с помощью ИИ-инструментов.

## Подготовка

### 1. Изучите API биржи

Перед началом работы подготовьте:
- Документацию REST/WebSocket API биржи
- Тестовые API-ключи (sandbox/testnet)
- Список поддерживаемых типов данных (свечи, стакан, тики, сделки)
- Список поддерживаемых типов заявок (лимитная, рыночная, стоп)

### 2. Создайте проект

```bash
dotnet new classlib -n StockSharp.MyExchange --framework net10.0
cd StockSharp.MyExchange
dotnet add package StockSharp.Messages
dotnet add package StockSharp.Algo
```

### 3. Подготовьте контекст для ИИ

Создайте файл `CLAUDE.md`:

```markdown
# Правила проекта — коннектор к бирже

- Фреймворк: StockSharp 5.x, .NET 10
- Коннектор реализуется как MessageAdapter
- Наследовать от AsyncMessageAdapter для async/await
- Все HTTP-запросы через HttpClient с CancellationToken
- WebSocket-подписки через native клиент или ClientWebSocket
- Маппинг типов: биржевые типы → StockSharp Messages
- Обработка ошибок: SendOutError() для ошибок подключения
- Все строки — в ресурсах локализации (или хотя бы const)
```

## Архитектура коннектора

Коннектор в StockSharp — это `MessageAdapter`, который:
1. Принимает входящие сообщения (запросы) от ядра
2. Обрабатывает их (вызывает API биржи)
3. Отправляет обратно ответные сообщения (результаты)

```
Ядро StockSharp → [Message] → MessageAdapter → [HTTP/WS] → Биржа
Биржа → [HTTP/WS] → MessageAdapter → [Message] → Ядро StockSharp
```

## Пошаговый пример

### Шаг 1: Базовая структура адаптера

Промпт:

```
Создай базовый MessageAdapter для коннектора к криптовалютной бирже MyExchange
на StockSharp:
- Наследуй от AsyncMessageAdapter
- Реализуй подключение/отключение (ConnectMessage, DisconnectMessage)
- Добавь настройки: ApiKey, Secret, демо-режим
- Используй HttpClient для REST API
- Базовый URL API: https://api.myexchange.com/v1
```

### Шаг 2: Поиск инструментов

Промпт:

```
Добавь в адаптер обработку SecurityLookupMessage:
- Запрос GET /api/v1/symbols возвращает JSON список инструментов
- Каждый инструмент имеет: symbol, baseAsset, quoteAsset,
  minQty, maxQty, tickSize, status
- Маппинг: symbol → SecurityId, baseAsset/quoteAsset → в имя,
  tickSize → SecurityMessage.PriceStep
- Отправлять SecurityMessage для каждого инструмента
- В конце отправить SubscriptionFinishedMessage
```

### Шаг 3: Рыночные данные

Промпт:

```
Добавь в адаптер подписку на рыночные данные:

1. Свечи (MarketDataTypes.CandleTimeFrame):
   - REST: GET /api/v1/klines?symbol={}&interval={}&limit=1000
   - WebSocket: подписка на канал kline_{symbol}_{interval}
   - Маппинг интервалов: 1m, 5m, 15m, 1h, 4h, 1d

2. Стакан (MarketDataTypes.MarketDepth):
   - WebSocket: подписка на канал depth_{symbol}
   - Парсинг bids/asks в QuoteChangeMessage

3. Тики (MarketDataTypes.Trades):
   - WebSocket: подписка на канал trades_{symbol}
   - Парсинг в ExecutionMessage с ExecutionTypes.Tick
```

### Шаг 4: Торговые операции

Промпт:

```
Добавь в адаптер поддержку торговых операций:

1. Регистрация заявки (OrderRegisterMessage):
   - POST /api/v1/order с параметрами: symbol, side, type, quantity, price
   - Возврат ExecutionMessage с ExecutionTypes.Transaction

2. Отмена заявки (OrderCancelMessage):
   - DELETE /api/v1/order/{orderId}
   - Возврат ExecutionMessage со статусом OrderStates.Done

3. Получение портфеля (PortfolioLookupMessage):
   - GET /api/v1/account
   - Парсинг балансов в PositionChangeMessage

4. WebSocket для обновлений заявок:
   - Канал orders_{listenKey}
   - Парсинг обновлений статуса заявок
```

### Шаг 5: Проверка кода

После генерации каждого шага попросите ИИ:

```
Проверь сгенерированный адаптер на соответствие StockSharp API:
1. Все ли типы сообщений обрабатываются?
2. Правильно ли используется CancellationToken?
3. Есть ли обработка ошибок HTTP?
4. Отправляются ли SubscriptionFinishedMessage после завершения?
5. Корректно ли работает реконнект при разрыве WebSocket?
```

## Ключевые моменты реализации

### MessageAdapter — что обрабатывать

| Входящее сообщение | Что делать | Ответное сообщение |
|-------------------|------------|-------------------|
| `ConnectMessage` | Подключиться к API | `ConnectMessage` (ответ) |
| `DisconnectMessage` | Отключиться | `DisconnectMessage` (ответ) |
| `SecurityLookupMessage` | Запросить инструменты | `SecurityMessage` × N |
| `MarketDataMessage` (подписка) | Подписаться на данные | `SubscriptionResponseMessage` |
| `OrderRegisterMessage` | Создать заявку | `ExecutionMessage` |
| `OrderCancelMessage` | Отменить заявку | `ExecutionMessage` |
| `PortfolioLookupMessage` | Запросить портфели | `PortfolioMessage`, `PositionChangeMessage` |

### Async-паттерн

```csharp
public class MyExchangeAdapter : AsyncMessageAdapter
{
    private HttpClient _httpClient;

    protected override ValueTask OnConnectAsync(ConnectMessage msg, CancellationToken token)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://api.myexchange.com/v1/");
        _httpClient.DefaultRequestHeaders.Add("X-API-KEY", Key.To<string>());

        SendOutMessage(new ConnectMessage());
        return default;
    }

    protected override ValueTask OnSecurityLookupAsync(SecurityLookupMessage msg, CancellationToken token)
    {
        // ... запрос инструментов
    }

    // ... остальные методы
}
```

### Подпись запросов

Большинство бирж требуют HMAC-подпись для приватных запросов:

```csharp
private string SignRequest(string payload)
{
    using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(Secret.To<string>()));
    var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
    return Convert.ToHexString(hash).ToLowerInvariant();
}
```

## Чек-лист проверки коннектора

### Подключение
- [ ] Подключение/отключение работает корректно
- [ ] Обрабатываются ошибки аутентификации
- [ ] Работает переподключение при обрыве

### Инструменты
- [ ] Загружается список инструментов
- [ ] Правильно маппятся: SecurityId, PriceStep, VolumeStep
- [ ] Отправляется `SubscriptionFinishedMessage`

### Рыночные данные
- [ ] Свечи: загрузка истории + подписка на новые
- [ ] Стакан: корректная глубина, обновления
- [ ] Тики: правильное время, объём, направление

### Торговля
- [ ] Лимитные заявки: создание, отмена
- [ ] Рыночные заявки: создание
- [ ] Обновление статуса заявки
- [ ] Баланс портфеля обновляется

### Общее
- [ ] Все `CancellationToken` пробрасываются
- [ ] Ошибки логируются через `SendOutError()`
- [ ] Нет утечек ресурсов (WebSocket, HttpClient)
- [ ] Компилируется без ошибок и предупреждений

## Примеры промптов

### Добавление нового типа данных

```
Добавь в мой коннектор поддержку Level 1 данных (BestBid/BestAsk):
- WebSocket канал: ticker_{symbol}
- Парсинг bid, ask, last, volume
- Отправка Level1ChangeMessage с полями:
  Level1Fields.BestBidPrice, Level1Fields.BestAskPrice,
  Level1Fields.LastTradePrice, Level1Fields.Volume
```

### Обработка rate limits

```
Добавь в мой коннектор обработку rate limits:
- API возвращает заголовки X-RateLimit-Remaining и X-RateLimit-Reset
- При достижении лимита: ждать до Reset, логировать предупреждение
- Использовать SemaphoreSlim для ограничения параллельных запросов
```

## Советы

1. **Начинайте с read-only** — сначала реализуйте подключение, инструменты и рыночные данные. Торговые операции добавляйте после проверки
2. **Используйте sandbox** — тестируйте на тестовом окружении биржи
3. **Сверяйте с существующими коннекторами** — дайте ИИ код существующего коннектора StockSharp как образец
4. **Логируйте всё** — при отладке коннектора подробные логи бесценны
5. **Обрабатывайте edge cases** — переподключение, изменение инструментов, нестандартные типы заявок
