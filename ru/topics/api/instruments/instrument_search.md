# Поиск инструмента

Некоторые коннекторы (например, [OpenECry](../connectors/stock_market/openecry.md), [Interactive Brokers](../connectors/stock_market/interactive_brokers.md) или [Sterling](../connectors/stock_market/sterling.md)) не поддерживают после вызова соединения ([IConnector.Connect](xref:StockSharp.BusinessEntities.IConnector.Connect)) передачу всех имеющихся на сервере инструментов на клиент (как правило, это сделано для уменьшения нагрузки на сервер брокера). 

Для поиска инструмента необходимо вызывать метод [Connector.Subscribe](xref:StockSharp.Algo.Connector.Subscribe(StockSharp.BusinessEntities.Subscription))**(**[StockSharp.BusinessEntities.Subscription](xref:StockSharp.BusinessEntities.Subscription) subscription **)**. Передаваемая в него подписка должна быть создана на основе [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage), поля которого используются в качестве фильтра. Например: 

- Свойство [SecurityMessage.SecurityId](xref:StockSharp.Messages.SecurityMessage.SecurityId) с указанием [SecurityId.SecurityCode](xref:StockSharp.Messages.SecurityId.SecurityCode) задает маску имени инструмента или описания (например, «ES» или «e\-mini» или «gold») или точное название (например, «esh5»).
- Свойство [SecurityMessage.SecurityType](xref:StockSharp.Messages.SecurityMessage.SecurityType) задает тип инструмента.
- Свойство [SecurityMessage.SecurityId](xref:StockSharp.Messages.SecurityMessage.SecurityId) с указанием [SecurityId.BoardCode](xref:StockSharp.Messages.SecurityId.BoardCode) задает, на какой площадке торгуется инструмент (например, [ExchangeBoard.Forts](xref:StockSharp.BusinessEntities.ExchangeBoard.Forts) или [ExchangeBoard.Nasdaq](xref:StockSharp.BusinessEntities.ExchangeBoard.Nasdaq)).

Найденные инструменты будут возвращены через событие [Connector.NewSecurity](xref:StockSharp.Algo.Connector.NewSecurity). 
