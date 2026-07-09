# 报价参考

## 概览

本节包含 StockSharp 中报价系统架构和组件的完整参考。描述了所有可用的报价行为、操作类型和接口。有关基本介绍，请参见 [报价算法](quoting.md)。

## 架构

### 报价策略（已弃用）

[QuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.QuotingStrategy) 类已被弃用。建议使用 [QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor) 代替。

已弃用类的主要参数：

- `QuotingSide` -- 报价方向（买入/卖出）
- `QuotingVolume` -- 报价数量
- `TimeOut` -- 执行超时
- `UseBidAsk` -- 使用订单簿价格
- `UseLastTradePrice` -- 使用最后交易价格

### 报价引擎

[QuotingEngine](xref:StockSharp.Algo.Strategies.Quoting.QuotingEngine) —— 报价系统的功能核心。它计算剩余量和超时，并返回操作建议而不产生副作用。

### 报价行为算法

[QuotingBehaviorAlgo](xref:StockSharp.Algo.Strategies.Quoting.QuotingBehaviorAlgo) -- `IPositionModifyAlgo` 接口在算法持仓管理中的实现。

主要方法：

| 方法 | 描述 |
|--------|-------------|
| `UpdateMarketData(time, price, volume)` | 更新市场数据 |
| `UpdateOrderBook(depth)` | 更新订单簿 |
| `GetNextAction()` | 获取下一个报价操作 |

支持 VWAP 和 TWAP 模式。

### 报价处理器

[QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor)——策略中执行报价的主要处理器。管理订单生命周期：下单、修改、取消。

## 报价操作类型

处理器返回类型为 [QuotingAction](xref:StockSharp.Algo.Strategies.Quoting.QuotingAction) 的动作：

| 类型 | 描述 |
|------|-------------|
| `None(reason)` | 无需操作 |
| `PlaceOrder(price, volume)` | 下新订单 |
| `ModifyOrder(price, volume)` | 修改现有订单 |
| `CancelOrder()` | 取消订单 |
| `Finish(success, reason)` | 完成报价 |

## 报价行为

StockSharp 提供 10 种报价行为，每种都实现了 [IQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.IQuotingBehavior) 接口：

| # | 类 | 描述 | 关键参数 |
|---|-------|-------------|----------------|
| 1 | [按价格报价行为](xref:StockSharp.Algo.Strategies.Quoting.BestByPriceQuotingBehavior) | 来自订单簿的最佳价格 | 最佳价格偏移 |
| 2 | [LastTradeQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LastTradeQuotingBehavior) | 最后交易价格 | 最佳价格偏移 |
| 3 | [限制报价行为](xref:StockSharp.Algo.Strategies.Quoting.LimitQuotingBehavior) | 固定限价 | 限价 |
| 4 | [市场报价行为](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingBehavior) | 带偏移的市场价格 | 价格偏移, 价格类型（跟随/相反/中间） |
| 5 | [波动率报价行为](xref:StockSharp.Algo.Strategies.Quoting.VolatilityQuotingBehavior) | 期权波动率（布莱克-斯科尔斯） | IV范围，模型 |
| 6 | [理论价格报价行为](xref:StockSharp.Algo.Strategies.Quoting.TheorPriceQuotingBehavior) | 期权理论价格 | 理论价格偏移 |
| 7 | [按成交量最佳报价行为](xref:StockSharp.Algo.Strategies.Quoting.BestByVolumeQuotingBehavior) | 订单簿中的累计成交量 | VolumeExchange |
| 8 | [LevelQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LevelQuotingBehavior) | 订单簿深度级别 | 级别（范围\<int\>），自身级别 |
| 9 | [VWAP报价行为](xref:StockSharp.Algo.Strategies.Quoting.VWAPQuotingBehavior) | VWAP -- 成交量加权平均价 | 最佳价格偏移 |
| 10 | [TWAP报价行为](xref:StockSharp.Algo.Strategies.Quoting.TWAPQuotingBehavior) | TWAP -- 时间加权平均 | 时间间隔，价格缓冲大小（默认 10） |

## IQuotingBehavior 接口

[IQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.IQuotingBehavior) 接口定义了两个关键方法：

### 计算最佳价格

计算下订单的最佳价格：

```cs
decimal? CalculateBestPrice(
	Security security,
	IMarketDataProvider provider,
	Sides quotingDirection,
	decimal? bestBid,
	decimal? bestAsk,
	decimal? lastTradePrice,
	decimal? lastTradeVolume,
	IEnumerable<QuoteChange> bids,
	IEnumerable<QuoteChange> asks);
```

### 是否需要报价

确定是否需要更新当前订单：

```cs
decimal? NeedQuoting(
	Security security,
	IMarketDataProvider provider,
	DateTimeOffset currentTime,
	decimal? currentPrice,
	decimal? currentVolume,
	decimal? newVolume,
	decimal? bestPrice);
```

如果不需要更新，则返回 `null`，或返回下订单的新价格。

## 使用示例

### 带偏移的市场报价

```cs
var behavior = new MarketQuotingBehavior
{
	PriceOffset = new Unit(2, UnitTypes.Absolute),
	PriceType = MarketPriceTypes.Following,
	BestPriceOffset = new Unit(0.5m),
};
```

### 成交量加权平均价格报价

```cs
var behavior = new VWAPQuotingBehavior
{
	BestPriceOffset = new Unit(1),
};
```

### 期权波动性报价

```cs
var behavior = new VolatilityQuotingBehavior
{
	IVRange = new Range<decimal>(0.2m, 0.3m),
	Model = new BlackScholes(option, connector),
};
```

### 带处理器的完整示例

```cs
// 选择报价行为
var behavior = new BestByPriceQuotingBehavior
{
	BestPriceOffset = new Unit(0.01m),
};

// 创建处理器
var processor = new QuotingProcessor(
	behavior,
	Security,
	Portfolio,
	Sides.Buy,
	volume: 10,
	maxOrderVolume: 10,
	timeout: TimeSpan.FromMinutes(5),
	subscriptionProvider: this,
	ruleContainer: this,
	transactionProvider: this,
	timeProvider: this,
	marketDataProvider: this,
	isTradingAllowed: IsFormedAndOnlineAndAllowTrading,
	useBidAsk: true,
	useLastTradePrice: true)
{
	Parent = this,
};

processor.Finished += isOk =>
{
	this.AddInfoLog($"报价已完成: {isOk}");
	processor?.Dispose();
};

processor.Start();
```

## 另请参阅

- [报价算法](quoting.md)
- [报价策略](samples/mq.md)
- [波动率报价](../options/volatility_trading.md)
