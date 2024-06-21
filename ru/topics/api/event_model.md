# Событийная модель

Событийная модель в [S#](../api.md) построена с применением [IMarketRule](xref:StockSharp.Algo.IMarketRule). [IMarketRule](xref:StockSharp.Algo.IMarketRule) может использоваться как внутри [стратегий](strategies.md), так и вне ее.

В [S#](../../api.md) для [IMarketRule](xref:StockSharp.Algo.IMarketRule) уже есть ряд предопределенных условий и действий для наиболее распространенных сценариев, которые добавлены как методы-расширения в [MarketRuleHelper](xref:StockSharp.Algo.MarketRuleHelper).

- [Использование правил](event_model/rules_using.md)
- [Синхронизация и приостановка правил](event_model/rules_suspension.md)
- [Взаимоисключающие правила](event_model/rules_mutually_exclusive.md)
- [Собственное правило](event_model/rules_create.md)

## См. также

[Создание стратегии](strategies/creating_strategies.md)
