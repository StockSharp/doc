# 连接器配置: OpenMarkets

从服务提供商获取身份验证凭据，并指定连接参数。

- `ClientId` - 账户或客户端标识符。
- `ClientSecret` - 身份验证凭据。
- `AccountCode` - 账户或客户端标识符。
- `IsTest` - 控制连接器行为的开关。
- `DataSource` - 连接参数。 默认值: `OpenMarketsExtensions.DefaultDataSource`.
- `DefaultExchange` - 连接参数。 默认值: `OpenMarketsExtensions.DefaultExchange`.
- `DefaultDestination` - 连接参数。 默认值: `OpenMarketsExtensions.DefaultExchange`.
- `OrderGiver` - 连接参数。
- `OrderTaker` - 连接参数。
- `DefaultPriceMultiplier` - 连接器的数值参数。 默认值: `0.01m`.
- `DepthPollingInterval` - 时间间隔。 默认值: `TimeSpan.FromSeconds(2)`.
