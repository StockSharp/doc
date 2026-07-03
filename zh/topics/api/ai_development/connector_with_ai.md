# 使用 AI 编写连接器

本章节分步介绍如何借助 AI 工具为 StockSharp 创建交易所连接器。

## 准备工作

### 1. 研究交易所 API

开始之前，请准备：

- 交易所的 REST/WebSocket API 文档
- 测试 API 密钥（沙盒／测试网）
- 支持的数据类型列表（K线、市场深度、逐笔成交、成交）
- 支持的订单类型列表（限价、市价、止损）

### 2. 创建项目

```bash
dotnet new classlib -n StockSharp.MyExchange --framework net10.0
cd StockSharp.MyExchange
dotnet add package StockSharp.Messages
dotnet add package StockSharp.Algo
```

### 3. 为 AI 准备上下文

创建 `CLAUDE.md` 文件：

```markdown
# Project Rules — Exchange Connector

- Framework: StockSharp 5.x, .NET 10
- Connector is implemented as a MessageAdapter
- Inherit from AsyncMessageAdapter for async/await
- All HTTP requests via HttpClient with CancellationToken
- WebSocket subscriptions via native client or ClientWebSocket
- Type mapping: exchange types → StockSharp Messages
- Error handling: SendOutError() for connection errors
- All strings in localization resources (or at least const)
```

## 连接器架构

StockSharp 连接器是一个 `MessageAdapter`，其工作流程如下：

1. 接收核心发送的传入消息（请求）
2. 处理消息（调用交易所 API）
3. 发回响应消息（结果）

```
StockSharp Core → [Message] → MessageAdapter → [HTTP/WS] → Exchange
Exchange → [HTTP/WS] → MessageAdapter → [Message] → StockSharp Core
```

## 分步示例

### 第 1 步：适配器基本结构

提示词：

```
Create a basic MessageAdapter for a cryptocurrency exchange connector
called MyExchange using StockSharp:
- Inherit from AsyncMessageAdapter
- Implement connect/disconnect (ConnectMessage, DisconnectMessage)
- Add settings: ApiKey, Secret, demo mode
- Use HttpClient for REST API
- Base API URL: https://api.myexchange.com/v1
```

### 第 2 步：查询交易品种

提示词：

```
Add SecurityLookupMessage handling to the adapter:
- Request GET /api/v1/symbols returns a JSON list of instruments
- Each instrument has: symbol, baseAsset, quoteAsset,
  minQty, maxQty, tickSize, status
- Mapping: symbol → SecurityId, baseAsset/quoteAsset → name,
  tickSize → SecurityMessage.PriceStep
- Send SecurityMessage for each instrument
- Send SubscriptionFinishedMessage at the end
```

### 第 3 步：市场数据

提示词：

```
Add market data subscription to the adapter:

1. Candles (MarketDataTypes.CandleTimeFrame):
   - REST: GET /api/v1/klines?symbol={}&interval={}&limit=1000
   - WebSocket: subscribe to channel kline_{symbol}_{interval}
   - Interval mapping: 1m, 5m, 15m, 1h, 4h, 1d

2. Order book (MarketDataTypes.MarketDepth):
   - WebSocket: subscribe to channel depth_{symbol}
   - Parse bids/asks into QuoteChangeMessage

3. Ticks (MarketDataTypes.Trades):
   - WebSocket: subscribe to channel trades_{symbol}
   - Parse into ExecutionMessage with ExecutionTypes.Tick
```

### 第 4 步：交易操作

提示词：

```
Add trading operation support to the adapter:

1. Order registration (OrderRegisterMessage):
   - POST /api/v1/order with params: symbol, side, type, quantity, price
   - Return ExecutionMessage with ExecutionTypes.Transaction

2. Order cancellation (OrderCancelMessage):
   - DELETE /api/v1/order/{orderId}
   - Return ExecutionMessage with status OrderStates.Done

3. Portfolio retrieval (PortfolioLookupMessage):
   - GET /api/v1/account
   - Parse balances into PositionChangeMessage

4. WebSocket for order updates:
   - Channel orders_{listenKey}
   - Parse order status updates
```

### 第 5 步：代码审查

每完成一个步骤后，都可以要求 AI 进行检查：

```
Review the generated adapter for StockSharp API compliance:
1. Are all message types handled?
2. Is CancellationToken used correctly?
3. Is HTTP error handling in place?
4. Are SubscriptionFinishedMessage sent after completion?
5. Does WebSocket reconnection work on disconnect?
```

## 关键实现细节

### MessageAdapter — 需要处理的内容

| 传入消息 | 操作 | 响应消息 |
|-----------------|--------|-----------------|
| `ConnectMessage` | 连接 API | `ConnectMessage`（响应） |
| `DisconnectMessage` | 断开连接 | `DisconnectMessage`（响应） |
| `SecurityLookupMessage` | 请求交易品种列表 | `SecurityMessage` × N |
| `MarketDataMessage`（订阅） | 订阅数据 | `SubscriptionResponseMessage` |
| `OrderRegisterMessage` | 创建订单 | `ExecutionMessage` |
| `OrderCancelMessage` | 撤销订单 | `ExecutionMessage` |
| `PortfolioLookupMessage` | 请求投资组合 | `PortfolioMessage`、`PositionChangeMessage` |

### 异步模式

```csharp
public class MyExchangeAdapter : AsyncMessageAdapter
{
    private HttpClient _httpClient;

    protected override ValueTask OnConnectAsync(ConnectMessage msg, CancellationToken token)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://api.myexchange.com/v1/");
        _httpClient.DefaultRequestHeaders.Add("X-API-KEY", Key.To<string>());

        SendOutMessage(new ConnectMessage());
        return default;
    }

    protected override ValueTask OnSecurityLookupAsync(SecurityLookupMessage msg, CancellationToken token)
    {
        // ... request instruments
    }

    // ... other methods
}
```

### 请求签名

大多数交易所要求对私有请求进行 HMAC 签名：

```csharp
private string SignRequest(string payload)
{
    using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(Secret.To<string>()));
    var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
    return Convert.ToHexString(hash).ToLowerInvariant();
}
```

## 连接器检查清单

### 连接

- [ ] 连接和断开连接功能正常
- [ ] 正确处理身份验证错误
- [ ] 断开连接后能够重新连接

### 交易品种

- [ ] 成功加载交易品种列表
- [ ] 正确映射 SecurityId、PriceStep 和 VolumeStep
- [ ] 已发送 `SubscriptionFinishedMessage`

### 市场数据

- [ ] K线：加载历史数据并订阅新K线
- [ ] 市场深度：档位和更新均正确
- [ ] 逐笔成交：时间、数量和方向均正确

### 交易

- [ ] 限价订单：创建和撤销
- [ ] 市价订单：创建
- [ ] 更新订单状态
- [ ] 更新投资组合余额

### 通用检查

- [ ] 所有 `CancellationToken` 均已向下传递
- [ ] 通过 `SendOutError()` 记录错误
- [ ] 不存在资源泄漏（WebSocket、HttpClient）
- [ ] 编译时无错误或警告

## 提示词示例

### 添加新的数据类型

```
Add Level 1 data support (BestBid/BestAsk) to my connector:
- WebSocket channel: ticker_{symbol}
- Parse bid, ask, last, volume
- Send Level1ChangeMessage with fields:
  Level1Fields.BestBidPrice, Level1Fields.BestAskPrice,
  Level1Fields.LastTradePrice, Level1Fields.Volume
```

### 处理速率限制

```
Add rate limit handling to my connector:
- API returns headers X-RateLimit-Remaining and X-RateLimit-Reset
- When limit is reached: wait until Reset, log a warning
- Use SemaphoreSlim to limit concurrent requests
```

## 建议

1. **先实现只读功能** — 首先实现连接、交易品种和市场数据，验证无误后再添加交易操作
2. **使用沙盒环境** — 在交易所的测试环境中进行验证
3. **参考现有连接器** — 将现有 StockSharp 连接器代码提供给 AI 作为参考
4. **记录所有信息** — 详细日志对于调试连接器非常重要
5. **处理边界情况** — 包括重新连接、交易品种变更和非标准订单类型
