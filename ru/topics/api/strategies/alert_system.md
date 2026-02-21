# Система оповещений

## Обзор

Стратегии в StockSharp имеют встроенную систему оповещений, позволяющую отправлять уведомления различных типов: всплывающие окна, звуковые сигналы, записи в лог и сообщения в Telegram. Оповещения полезны для информирования трейдера о важных событиях -- входах в позицию, достижении уровней, ошибках и прочих торговых сигналах.

При бэктестинге оповещения, кроме типа `Log`, автоматически пропускаются, чтобы не создавать помех при тестировании.

## Типы оповещений

Перечисление `AlertNotifications` определяет доступные типы:

| Тип | Описание |
|-----|----------|
| `Sound` | Звуковой сигнал |
| `Popup` | Всплывающее окно |
| `Log` | Запись в лог-файл |
| `Telegram` | Сообщение в Telegram |

## Методы

### Alert

Базовый метод отправки оповещения с указанием типа, заголовка и текста:

```csharp
// С заголовком и текстом
Alert(AlertNotifications type, string caption, string message);

// С автоматическим заголовком (используется имя стратегии)
Alert(AlertNotifications type, string message);
```

### AlertPopup

Отправляет всплывающее уведомление. Заголовком является имя стратегии:

```csharp
AlertPopup(string message);
```

### AlertSound

Отправляет звуковое уведомление:

```csharp
AlertSound(string message);
```

### AlertLog

Отправляет уведомление в лог. Этот тип работает и при бэктестинге:

```csharp
AlertLog(string message);
```

## Настройка сервиса оповещений

Для работы оповещений необходимо, чтобы в окружении стратегии был зарегистрирован сервис `IAlertNotificationService`. Это выполняется через метод расширения:

```csharp
strategy.SetAlertService(alertService);
```

Получить текущий сервис можно через:

```csharp
var service = strategy.GetAlertService();
```

В графических приложениях (Designer, терминал) сервис обычно регистрируется автоматически.

## Пример использования

```csharp
public class AlertStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;
    private readonly StrategyParam<decimal> _priceLevel;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public decimal PriceLevel
    {
        get => _priceLevel.Value;
        set => _priceLevel.Value = value;
    }

    public AlertStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
        _priceLevel = Param(nameof(PriceLevel), 100m);
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        var subscription = SubscribeCandles(CandleType);

        subscription
            .Bind(ProcessCandle)
            .Start();

        // Оповещение о запуске стратегии
        AlertLog("Стратегия запущена, отслеживаемый уровень: " + PriceLevel);
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        // Цена пересекла уровень снизу вверх
        if (candle.OpenPrice < PriceLevel && candle.ClosePrice >= PriceLevel)
        {
            AlertPopup("Цена пересекла уровень " + PriceLevel + " вверх!");
            AlertSound("Пробой уровня!");
            BuyMarket();
        }

        // Цена пересекла уровень сверху вниз
        if (candle.OpenPrice > PriceLevel && candle.ClosePrice <= PriceLevel)
        {
            Alert(AlertNotifications.Telegram, "Торговый сигнал",
                "Цена пробила уровень " + PriceLevel + " вниз");
            SellMarket();
        }
    }
}
```

В этом примере стратегия использует разные типы оповещений для разных ситуаций: `AlertPopup` и `AlertSound` для немедленного привлечения внимания трейдера, а `Alert` с типом `Telegram` для удаленного уведомления.
