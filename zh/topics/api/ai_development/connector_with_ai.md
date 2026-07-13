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
# 项目规则 — 交易所连接器

- 框架：StockSharp 5.x、.NET 10
- 连接器实现为 MessageAdapter
- 为 async/await 继承 AsyncMessageAdapter
- 所有 HTTP 请求都通过带 CancellationToken 的 HttpClient 发送
- WebSocket 订阅通过原生客户端或 ClientWebSocket 实现
- 类型映射：交易所类型 → StockSharp Messages
- 错误处理：连接错误使用 SendOutError()
- 所有字符串放入本地化资源（或至少定义为 const）
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
使用 StockSharp 为名为 MyExchange 的加密货币交易所连接器
创建基础 MessageAdapter：
- 继承 AsyncMessageAdapter
- 实现连接/断开连接（ConnectMessage、DisconnectMessage）
- 添加设置：ApiKey、Secret、demo 模式
- REST API 使用 HttpClient
- API 基础 URL：https://api.myexchange.com/v1
```

### 第 2 步：查询交易品种

提示词：

```
为适配器添加 SecurityLookupMessage 处理：
- GET /api/v1/symbols 请求返回交易品种的 JSON 列表
- 每个交易品种包含：symbol、baseAsset、quoteAsset、
  minQty, maxQty, tickSize, status
- 映射：symbol → SecurityId，baseAsset/quoteAsset → name，
  tickSize → SecurityMessage.PriceStep
- 为每个交易品种发送 SecurityMessage
- 最后发送 SubscriptionFinishedMessage
```

### 第 3 步：市场数据

提示词：

```
为适配器添加市场数据订阅：

1. K线 (MarketDataTypes.CandleTimeFrame):
   - REST: GET /api/v1/klines?symbol={}&interval={}&limit=1000
   - WebSocket：订阅 kline_{symbol}_{interval} 频道
   - 周期间隔映射：1m、5m、15m、1h、4h、1d

2. 订单簿 (MarketDataTypes.MarketDepth):
   - WebSocket：订阅 depth_{symbol} 频道
   - 将 bids/asks 解析为 QuoteChangeMessage

3. 逐笔成交 (MarketDataTypes.Trades):
   - WebSocket：订阅 trades_{symbol} 频道
   - 解析为带有 ExecutionTypes.Tick 的 ExecutionMessage
```

### 第 4 步：交易操作

提示词：

```
为适配器添加交易操作支持：

1. 订单注册 (OrderRegisterMessage):
   - 使用参数 symbol、side、type、quantity、price 调用 POST /api/v1/order
   - 返回带有 ExecutionTypes.Transaction 的 ExecutionMessage

2. 订单取消 (OrderCancelMessage):
   - DELETE /api/v1/order/{orderId}
   - 返回状态为 OrderStates.Done 的 ExecutionMessage

3. 投资组合获取 (PortfolioLookupMessage):
   - GET /api/v1/account
   - 将余额解析为 PositionChangeMessage

4. 用于订单更新的 WebSocket:
   - 频道 orders_{listenKey}
   - 解析订单状态更新
```

### 第 5 步：代码审查

每完成一个步骤后，都可以要求 AI 进行检查：

```
检查生成的适配器是否符合 StockSharp API：
1. 是否处理了所有消息类型？
2. CancellationToken 是否正确使用？
3. 是否实现了 HTTP 错误处理？
4. 完成后是否发送 SubscriptionFinishedMessage？
5. WebSocket 断开后是否能重新连接？
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
        // ... 请求交易品种
    }

    // ... 其他方法
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
为我的连接器添加 Level 1 数据（BestBid/BestAsk）支持：
- WebSocket 通道：ticker_{symbol}
- 解析 bid、ask、last、volume
- 发送包含以下字段的 Level1ChangeMessage：
  Level1Fields.BestBidPrice, Level1Fields.BestAskPrice,
  Level1Fields.LastTradePrice, Level1Fields.Volume
```

### 处理速率限制

```
为我的连接器添加频率限制处理：
- API 返回 X-RateLimit-Remaining 和 X-RateLimit-Reset 响应头
- 达到限制时：等待到 Reset，并记录警告
- 使用 SemaphoreSlim 限制并发请求数
```

## 建议

1. **先实现只读功能** — 首先实现连接、交易品种和市场数据，验证无误后再添加交易操作
2. **使用沙盒环境** — 在交易所的测试环境中进行验证
3. **参考现有连接器** — 将现有 StockSharp 连接器代码提供给 AI 作为参考
4. **记录所有信息** — 详细日志对于调试连接器非常重要
5. **处理边界情况** — 包括重新连接、交易品种变更和非标准订单类型
