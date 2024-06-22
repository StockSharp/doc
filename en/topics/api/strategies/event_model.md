# Event Model

The event model is built using [IMarketRule](xref:StockSharp.Algo.IMarketRule). [IMarketRule](xref:StockSharp.Algo.IMarketRule) can be used both within and outside [strategies](strategies.md).

In [API](../../api.md), there are several predefined conditions and actions for [IMarketRule](xref:StockSharp.Algo.IMarketRule) tailored for common scenarios. These are added as extension methods in [MarketRuleHelper](xref:StockSharp.Algo.MarketRuleHelper).

- [Using rules](event_model/rules_using.md)
- [Rule synchronization and suspension](event_model/rules_suspension.md)
- [Mutually exclusive rules](event_model/rules_mutually_exclusive.md)
- [Creating custom rules](event_model/rules_create.md)