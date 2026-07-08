# Modelo de Eventos

O modelo de eventos é construído com [IMarketRule](xref:StockSharp.Algo.IMarketRule). [IMarketRule](xref:StockSharp.Algo.IMarketRule) pode ser usado tanto dentro como fora de [estratégias](../strategies.md).

Na [API](../../api.md), existem várias condições e acções predefinidas para [IMarketRule](xref:StockSharp.Algo.IMarketRule), adaptadas a cenários comuns. Estas são adicionadas como métodos de extensão em [MarketRuleHelper](xref:StockSharp.Algo.MarketRuleHelper).

- [Utilizar Regras](event_model/rules_using.md)
- [Sincronização e suspensão de regras](event_model/rules_suspension.md)
- [Regras mutuamente exclusivas](event_model/rules_mutually_exclusive.md)
- [Criar regras personalizadas](event_model/rules_create.md)
