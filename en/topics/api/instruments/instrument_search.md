# Instrument search

Some connectors (for example, [OpenECry](../connectors/stock_market/openecry.md), [Interactive Brokers](../connectors/stock_market/interactive_brokers.md) or [Sterling](../connectors/stock_market/sterling.md)) do not support the transfer of all the available on the server instruments to the client (usually this is done to reduce the load on the broker server) after the ([IConnector.Connect](xref:StockSharp.BusinessEntities.IConnector.Connect)) establish connection. 

To find the instrument you need to call the [Connector.Subscribe](xref:StockSharp.Algo.Connector.Subscribe(StockSharp.BusinessEntities.Subscription))**(**[StockSharp.BusinessEntities.Subscription](xref:StockSharp.BusinessEntities.Subscription) subscription **)** method. Передаваемая в него подписка должна быть создана на основе [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage), поля которого используются в качестве фильтра. Например: The subscription passed to it should be based on [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage), and used as a filter. For example, search criterias: 

- The [SecurityMessage.SecurityId](xref:StockSharp.Messages.SecurityMessage.SecurityId) with specified [SecurityId.SecurityCode](xref:StockSharp.Messages.SecurityId.SecurityCode) property sets the instrument or description name mask (for example, «ES» or «e\-mini» or «gold») or the exact name (for example, «esh5»).
- The [SecurityMessage.SecurityType](xref:StockSharp.Messages.SecurityMessage.SecurityType) property sets the instrument type.
- The [SecurityMessage.SecurityId](xref:StockSharp.Messages.SecurityMessage.SecurityId) with specified [SecurityId.BoardCode](xref:StockSharp.Messages.SecurityId.BoardCode) property sets the board where the instrument trades (for example, [ExchangeBoard.Bats](xref:StockSharp.BusinessEntities.ExchangeBoard.Bats) or [ExchangeBoard.Nasdaq](xref:StockSharp.BusinessEntities.ExchangeBoard.Nasdaq)).

Found instruments will be returned through the [Connector.NewSecurity](xref:StockSharp.Algo.Connector.NewSecurity) event. 
