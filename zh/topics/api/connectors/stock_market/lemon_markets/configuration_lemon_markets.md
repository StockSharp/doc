# 连接器配置: lemon.markets

从服务提供商获取身份验证凭据，并指定连接参数。

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
