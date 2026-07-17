# 图形化配置: lemon.markets

在所有 StockSharp 产品中，均通过[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)配置连接。

- `ApiKey` - 身份验证凭据。
- `IsDemo` - 控制连接器行为的开关。 默认值: `true`.
- `AccountId` - 账户或客户端标识符。
- `SecuritiesAccountId` - 账户或客户端标识符。
- `DataPrivacyPrincipal` - 连接参数。
- `DataPrivacyJustification` - 连接参数。 默认值: `app_usage-stocksharp`.
- `PersonId` - 账户或客户端标识符。
- `DefaultFeeAmount` - 连接器的数值参数。
- `IsAppropriatenessConsentAccepted` - 控制连接器行为的开关。
- `PollingInterval` - 时间间隔。 默认值: `TimeSpan.FromSeconds(10)`.

## 另请参阅

[连接器](../../../connectors.md)

[图形化配置](../../graphical_configuration.md)

[保存和加载设置](../../save_and_load_settings.md)

[创建自定义连接器](../../creating_own_connector.md)
