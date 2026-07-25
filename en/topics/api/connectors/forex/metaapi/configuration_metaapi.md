# Connector configuration: MetaApi

Create a MetaApi token, deploy the trading account, and specify the connection parameters.

- `Token` - MetaApi access token.
- `AccountId` - identifier of the deployed MetaApi account.
- `Region` - account region. API tokens resolve it automatically, so set it explicitly only for account-scoped tokens.
- `Domain` - MetaApi domain. The default is `agiliumtrade.agiliumtrade.ai`.
- `SynchronizationTimeout` - how long to wait for the server-side terminal synchronization. The default is 2 minutes, and shorter values are raised to 10 seconds.

The connection completes once MetaApi reports the terminal state as synchronized, so the timeout must cover the initial synchronization of the account history.

Pending-order and protection parameters are passed through [MetaApiOrderCondition](xref:StockSharp.MetaApi.MetaApiOrderCondition): the activation price, the stop-loss and take-profit prices, and the magic number, comment, and client identifier stored with the position.

## See also

[Official MetaApi documentation](https://metaapi.cloud/docs/client/)
