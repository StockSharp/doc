# Modelo de eventos

El modelo de eventos se construye mediante [IMarketRule](xref:StockSharp.Algo.IMarketRule). [IMarketRule](xref:StockSharp.Algo.IMarketRule) se puede usar tanto dentro como fuera de las [estrategias](../strategies.md).

En [API](../../api.md), existen varias condiciones y acciones predefinidas para [IMarketRule](xref:StockSharp.Algo.IMarketRule) adaptadas a escenarios comunes. Se agregan como métodos de extensión en [MarketRuleHelper](xref:StockSharp.Algo.MarketRuleHelper).

- [Uso de reglas](event_model/rules_using.md)
- [Sincronización y suspensión de reglas](event_model/rules_suspension.md)
- [Reglas mutuamente excluyentes](event_model/rules_mutually_exclusive.md)
- [Creación de reglas personalizadas](event_model/rules_create.md)
