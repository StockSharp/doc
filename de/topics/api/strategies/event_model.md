# Ereignismodell

Das Ereignismodell wird mit [IMarketRule](xref:StockSharp.Algo.IMarketRule) aufgebaut. [IMarketRule](xref:StockSharp.Algo.IMarketRule) kann sowohl innerhalb als auch außerhalb von [Strategien](../strategies.md) verwendet werden.

In der [API](../../api.md) gibt es mehrere vordefinierte Bedingungen und Aktionen für [IMarketRule](xref:StockSharp.Algo.IMarketRule), die auf häufige Szenarien zugeschnitten sind. Sie werden als Erweiterungsmethoden in [MarketRuleHelper](xref:StockSharp.Algo.MarketRuleHelper) hinzugefügt.

- [Regeln verwenden](event_model/rules_using.md)
- [Regelsynchronisierung und Aussetzung](event_model/rules_suspension.md)
- [Gegenseitig ausschließende Regeln](event_model/rules_mutually_exclusive.md)
- [Benutzerdefinierte Regeln erstellen](event_model/rules_create.md)
