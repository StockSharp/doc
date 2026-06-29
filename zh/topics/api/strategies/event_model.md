# 事件模型

事件模型是使用 [IMarketRule](xref:StockSharp.Algo.IMarketRule) 构建的。[IMarketRule](xref:StockSharp.Algo.IMarketRule) 可以在 [策略](../strategies.md) 内部和外部使用。

在 [API](../../api.md) 中，有几个针对常见场景为 [IMarketRule](xref:StockSharp.Algo.IMarketRule) 定制的预定义条件和操作。这些作为扩展方法添加在 [MarketRuleHelper](xref:StockSharp.Algo.MarketRuleHelper) 中。

- [使用规则](event_model/rules_using.md)
- [规则同步与暂停](event_model/rules_suspension.md)
- [互斥规则](event_model/rules_mutually_exclusive.md)
- [创建自定义规则](event_model/rules_create.md)