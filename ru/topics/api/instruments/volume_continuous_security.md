# Непрерывный фьючерс по объему (VolumeContinuousSecurity)

## Обзор

Класс `VolumeContinuousSecurity` представляет непрерывный фьючерсный контракт, в котором переход (ролловер) между контрактами происходит на основе объема торгов или открытого интереса. Это отличается от `ExpirationContinuousSecurity`, где переключение выполняется по заранее заданным датам экспирации.

Оба класса наследуются от `ContinuousSecurity`, который в свою очередь наследует `BasketSecurity`.

## Отличие от ExpirationContinuousSecurity

| Характеристика | ExpirationContinuousSecurity | VolumeContinuousSecurity |
|---|---|---|
| Условие ролловера | Дата экспирации (фиксированная) | Порог объема или открытого интереса |
| Настройка | Словарь `SecurityId -> DateTime` | Список `SecurityId` + `VolumeLevel` |
| Предсказуемость | Переключение по расписанию | Переключение по рыночным условиям |
| Код корзины | `CE` | `CV` |

`ExpirationContinuousSecurity` требует ручного задания дат перехода для каждого контракта. `VolumeContinuousSecurity` автоматически переключается на следующий контракт, когда его объем торгов (или открытый интерес) превышает заданный порог.

## Основные свойства

```csharp
public class VolumeContinuousSecurity : ContinuousSecurity
{
    // Список внутренних инструментов (контрактов), упорядоченных по очереди ролловера
    public SynchronizedList<SecurityId> InnerSecurities { get; }

    // Использовать открытый интерес вместо объема для определения ролловера
    public bool IsOpenInterest { get; set; }

    // Порог объема, при достижении которого происходит переключение на следующий контракт
    public Unit VolumeLevel { get; set; }
}
```

Свойство `VolumeLevel` имеет тип `Unit`, что позволяет задавать как абсолютные, так и процентные значения.

## Пример использования

```csharp
using StockSharp.Algo;
using StockSharp.Messages;

// Создать непрерывный фьючерс по объему
var continuous = new VolumeContinuousSecurity
{
    Id = "RTS-CONT@FORTS",
    Board = ExchangeBoard.Forts,
};

// Добавить контракты в порядке ролловера
continuous.InnerSecurities.AddRange(new[]
{
    "RTS-3.26@FORTS".ToSecurityId(),
    "RTS-6.26@FORTS".ToSecurityId(),
    "RTS-9.26@FORTS".ToSecurityId(),
});

// Установить порог объема для переключения
continuous.VolumeLevel = new Unit(10000);

// Или использовать открытый интерес
continuous.IsOpenInterest = true;
continuous.VolumeLevel = new Unit(50000);
```

## Пример с ExpirationContinuousSecurity для сравнения

```csharp
using StockSharp.Algo;
using StockSharp.Messages;

// Непрерывный фьючерс по дате экспирации
var expContinuous = new ExpirationContinuousSecurity
{
    Id = "RTS-CONT-EXP@FORTS",
    Board = ExchangeBoard.Forts,
};

// Указать точные даты перехода для каждого контракта
expContinuous.ExpirationJumps.Add(
    "RTS-3.26@FORTS".ToSecurityId(),
    new DateTime(2026, 3, 15)
);
expContinuous.ExpirationJumps.Add(
    "RTS-6.26@FORTS".ToSecurityId(),
    new DateTime(2026, 6, 15)
);
```

## Когда использовать

`VolumeContinuousSecurity` подходит для ситуаций, когда:

- Точные даты ролловера заранее не известны
- Требуется переключение на основании ликвидности (объема торгов или открытого интереса)
- Необходим более адаптивный переход, реагирующий на рыночные условия

`ExpirationContinuousSecurity` предпочтителен, когда даты экспирации известны заранее и требуется детерминированный ролловер.
