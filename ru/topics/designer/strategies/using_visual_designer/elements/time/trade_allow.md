# Разрешена ли торговля

![Designer TradeAllowedDiagramElement 00](../../../../../../images/designer_tradealloweddiagramelement_00.png)

Кубик используется для проверки разрешена ли торговля в текущий момент. Проверяются следующие условия:

- Все подписки стратегии на маркет-данные должны быть в состоянии [Онлайн](../../../../../api/market_data/subscriptions.md) (получать данные реального времени).
- Все индикаторы должны быть [сформированы](../../../../../api/indicators.md).
- В случае [live торговли](../../../../live_execution/getting_started.md) пришедшенее значение в триггер должно иметь метку времени большее, чем время запуска стратегии.

### Входящие сокеты

Входящие сокеты

- **Триггер** \- сигнал, с помощью которого определяется момент, когда необходимо выполнять проверку.

### Исходящие сокеты

Исходящие сокеты

- **Флаг** \- флаг, который определяет активна ли торговая сессия.

## См. также

[Текущее время](current_time.md)