# Aster 配置

要使用该连接器，请在交易所账户中生成 **API Key** 和 **Secret**，并在连接设置中指定它们。

主要设置：

- **Key** 和 **Secret**。
- **Section**：`Spot` 或 `Derivatives`。
- **Derivatives mode**：`Legacy` 或 `V3 Agent`。
- **现货 REST / 现货 WS** 端点。
- **衍生品 REST / 衍生品 WS** 端点。
- **Demo** 模式。

官方 API 文档：

- [Spot API 概览](https://asterdex.github.io/aster-api-website/spot/spot-api-overview/)
- [Spot 账户和交易 API](https://asterdex.github.io/aster-api-website/spot/spot-account-and-trading-api/)
- [Spot websocket 市场数据](https://asterdex.github.io/aster-api-website/spot/websocket-market-data/)
- [Spot websocket 账户信息](https://asterdex.github.io/aster-api-website/spot/websocket-account-info/)
- [Futures v3 一般信息](https://asterdex.github.io/aster-api-website/futures-v3/general-info/)
- [Futures 用户数据流](https://asterdex.github.io/aster-api-website/futures/user-data-streams/)
- [Aster 代码端点](https://asterdex.github.io/aster-api-website/asterCode/endpoints/)

> [!TIP]
> Aster derivatives 有两个协议系列。启用交易前，请选择正确的 **Derivatives mode**。
