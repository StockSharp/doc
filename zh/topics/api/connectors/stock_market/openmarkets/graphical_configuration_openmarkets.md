# 图形化配置: OpenMarkets

在所有 StockSharp 产品中，均通过[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)配置连接。

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

## 另请参阅

[连接器](../../../connectors.md)

[图形化配置](../../graphical_configuration.md)

[保存和加载设置](../../save_and_load_settings.md)

[创建自定义连接器](../../creating_own_connector.md)
