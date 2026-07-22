# SBE 配置

使用 SBE 服务器管理员提供的参数配置客户端连接：

- `Address` - SBE 服务器的 TCP 端点。
- `SenderCompId` - 登录时使用的客户端标识符。
- `TargetCompId` - 目标服务器标识符。
- `Password` - 身份验证凭据。
- `IsSupportNativeCandles` - 启用服务器提供的 K线。禁用时，客户端根据市场数据构建 K线。

服务器使用 [SbeServerSettings](xref:StockSharp.Server.Sbe.SbeServerSettings)：

- `IsEnabled` - 启用 SBE 端点。
- `Address` - 监听端点。默认值为 `127.0.0.1:5002`。
- `HeartBeat` - 会话心跳间隔。默认值为 60 秒。
- `TargetCompId` - 服务器标识符。默认值为 `StockSharp`。

客户端和服务器应使用相同的 SBE 架构版本。
