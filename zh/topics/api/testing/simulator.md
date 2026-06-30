# 实时市场数据测试

实时市场数据测试涉及与交易所的实际连接进行交易（“实时”报价），但不在交易所下真实订单。所有注册订单都会被拦截，并根据市场订单簿模拟其执行。这种测试可以在开发交易模拟器时使用，或者在用真实报价短期检查交易算法时使用。

要模拟使用真实数据进行交易，您需要使用 [RealTimeEmulationTrader\<TAdapter\>](xref:StockSharp.Algo.Testing.RealTimeEmulationTrader`1)，它作为特定交易系统连接器（[Binance](../connectors/crypto_exchanges/binance.md)、[Interactive Brokers](../connectors/stock_market/interactive_brokers.md) 等）的“包装器”。

## 创建仿真连接器

要创建一个模拟连接器，首先创建一个用于接收市场数据的常规连接器，然后在其基础上创建一个模拟连接器：

```csharp
// Create a regular connector for receiving market data
private readonly Connector _realConnector = new();

// Create an emulation connector
_emuConnector = new RealTimeEmulationTrader<IMessageAdapter>(_realConnector.Adapter, _realConnector, _emuPf, false);

// Configure emulation parameters
var settings = _emuConnector.EmulationAdapter.Emulator.Settings;
settings.TimeZone = TimeHelper.Est;
settings.ConvertTime = true;
```

为了进行交易模拟，您需要使用一个特殊的投资组合：

```csharp
private readonly Portfolio _emuPf = Portfolio.CreateSimulator();
```

## 订阅事件

像普通连接器一样，仿真连接器在接收市场数据和执行交易时会生成事件：

```csharp
// Subscribe to connector events
_emuConnector.Connected += () =>
{
	// update gui labels
	this.GuiAsync(() => { ChangeConnectStatus(true); });
};

_emuConnector.Disconnected += () =>
{
	// update gui labels
	this.GuiAsync(() => { ChangeConnectStatus(false); });
};

_emuConnector.ConnectionError += error => this.GuiAsync(() =>
{
	// update gui labels
	ChangeConnectStatus(false);
	MessageBox.Show(this, error.ToString(), LocalizedStrings.ErrorConnection);
});

_emuConnector.OrderBookReceived += OnDepth;
_emuConnector.PositionReceived += (sub, p) => PortfolioGrid.Positions.TryAdd(p);
_emuConnector.OwnTradeReceived += (s, t) => TradeGrid.Trades.TryAdd(t);
_emuConnector.OrderReceived += (s, o) =>
{
	if (!_fistTimeOrders.Add(o))
		return;

	_bufferOrders.Add(o);
	OrderGrid.Orders.Add(o);
};

// Subscribe to order registration errors
_emuConnector.OrderRegisterFailReceived += (s, f) => OrderGrid.AddRegistrationFail(f);

_emuConnector.CandleReceived += (s, candle) =>
{
	if (s == _candlesSubscription)
		_buffer.Add(candle);
};
```

## 订阅市场数据

要处理市场数据，您需要订阅相应的数据类型：

```csharp
// Subscribe to order books, ticks, and Level1 for the emulation connector
_emuConnector.Subscribe(new(DataType.MarketDepth, security));
_emuConnector.Subscribe(new(DataType.Ticks, security));
_emuConnector.Subscribe(new(DataType.Level1, security));

// Subscribe to order books for the real connector (needed for emulation)
_realConnector.Subscribe(new(DataType.MarketDepth, security));

// Subscribe to candles
_candlesSubscription = new(CandleDataTypeEdit.DataType, security)
{
	From = DateTimeOffset.UtcNow - TimeSpan.FromDays(10),
};
_emuConnector.Subscribe(_candlesSubscription);
```

## 订单注册与管理

订单通过模拟连接器注册，方式类似于常规连接器：

```csharp
// Order registration
_emuConnector.RegisterOrder(order);

// Order cancellation
_emuConnector.CancelOrder(order);

// Order replacement
_emuConnector.ReRegisterOrder(order, newPrice, order.Balance);
```

## 配置仿真参数

您可以使用 [MarketEmulatorSettings](xref:StockSharp.Algo.Testing.MarketEmulatorSettings) 属性来配置仿真参数：

```csharp
var settings = _emuConnector.EmulationAdapter.Emulator.Settings;

// Set timezone
settings.TimeZone = TimeHelper.Est;

// Convert time
settings.ConvertTime = true;

// Match orders on price touch
settings.MatchOnTouch = false;

// Emulate order execution latency
settings.Latency = TimeSpan.FromMilliseconds(100);
```

## 界面示例

SampleRealTimeEmulation 示例演示了同时显示来自真实连接器和仿真连接器的数据的能力：

![sample realtime emulation](../../../images/sample_realtime_emulation.png)

应用程序界面包含以下元素：
- 用于显示蜡烛和订单的图表
- 订购并拥有贸易表
- 真实市场和模拟订单簿
- 用于创建和取消订单的控制

## 优点和局限性

实时市场数据测试具有以下优势：
- 使用真实市场数据而不承担金融风险
- 在非常接近真实交易的条件下测试算法
- 能够实时将结果与真实市场进行比较

限制：
- 测试速度受真实数据速率的限制
- 无法在历史时期进行测试
- 依赖所接收市场数据的质量和完整性
