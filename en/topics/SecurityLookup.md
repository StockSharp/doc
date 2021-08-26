# Instrument search

Some connectors (for example, [OpenECry](OEC.md), [Interactive Brokers](IB.md) or [Sterling](Sterling.md)) do not support the transfer of all the available on the server instruments to the client (usually this is done to reduce the load on the broker server) after the ([IConnector.Connect](../api/StockSharp.BusinessEntities.IConnector.Connect.html)) establish connection. 

To find the instrument you need to call the [IConnector.LookupSecurities](../api/StockSharp.BusinessEntities.IConnector.LookupSecurities.html) method. The instrument passed to it used as a filter. The following search criteria (the exact number depends on the broker system) are available: 

- The [Security.Code](../api/StockSharp.BusinessEntities.Security.Code.html) property sets the instrument or description name mask (for example, «ES» or «e\-mini» or «gold») or the exact name (for example, «esh5»).
- The [Security.Type](../api/StockSharp.BusinessEntities.Security.Type.html) property sets the instrument type.
- The [Security.Board](../api/StockSharp.BusinessEntities.Security.Board.html) property sets the board where the instrument trades (for example, [ExchangeBoard.Bats](../api/StockSharp.BusinessEntities.ExchangeBoard.Bats.html) or [ExchangeBoard.Nasdaq](../api/StockSharp.BusinessEntities.ExchangeBoard.Nasdaq.html)).

Found instruments will be returned through the [Connector.NewSecurity](../api/StockSharp.Algo.Connector.NewSecurity.html) event. 

## Recommended content
