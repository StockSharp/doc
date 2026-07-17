# Connector configuration: OpenMarkets

Obtain the credentials from the provider and specify the connection parameters.

- `ClientId` - account or client identifier.
- `ClientSecret` - authentication credential.
- `AccountCode` - account or client identifier.
- `IsTest` - switch controlling connector behavior.
- `DataSource` - connection parameter. Default value: `OpenMarketsExtensions.DefaultDataSource`.
- `DefaultExchange` - connection parameter. Default value: `OpenMarketsExtensions.DefaultExchange`.
- `DefaultDestination` - connection parameter. Default value: `OpenMarketsExtensions.DefaultExchange`.
- `OrderGiver` - connection parameter.
- `OrderTaker` - connection parameter.
- `DefaultPriceMultiplier` - numeric connector parameter. Default value: `0.01m`.
- `DepthPollingInterval` - time interval. Default value: `TimeSpan.FromSeconds(2)`.
