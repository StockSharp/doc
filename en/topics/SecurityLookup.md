# Instrument search

Some connectors (for example, [OpenECry](OEC.md), [Interactive Brokers](IB.md) or [Sterling](Sterling.md)) do not support the transfer of all the available on the server instruments to the client (usually this is done to reduce the load on the broker server) after the ([IConnector.Connect](xref:StockSharp.BusinessEntities.IConnector.Connect)) establish connection. 

To find the instrument you need to call the [IConnector.LookupSecurities](xref:StockSharp.BusinessEntities.IConnector.LookupSecurities) method. The instrument passed to it used as a filter. The following search criteria (the exact number depends on the broker system) are available: 

- The [Security.Code](xref:StockSharp.BusinessEntities.Security.Code) property sets the instrument or description name mask (for example, «ES» or «e\-mini» or «gold») or the exact name (for example, «esh5»).
- The [Security.Type](xref:StockSharp.BusinessEntities.Security.Type) property sets the instrument type.
- The [Security.Board](xref:StockSharp.BusinessEntities.Security.Board) property sets the board where the instrument trades (for example, [ExchangeBoard.Bats](xref:StockSharp.BusinessEntities.ExchangeBoard.Bats) or [ExchangeBoard.Nasdaq](xref:StockSharp.BusinessEntities.ExchangeBoard.Nasdaq)).

Found instruments will be returned through the [Connector.NewSecurity](xref:StockSharp.Algo.Connector.NewSecurity) event. 

## Recommended content
