# 持仓保护

## 介绍

这种对SMA策略的修改实现了使用本地保护控制器保护未平仓头寸的机制。这种方法允许灵活的风险管理，并在满足某些条件时自动平仓。

## 持仓保护的关键组成部分

### 保护控制器

该策略使用两个关键对象来保护头寸：

```cs
// Declaration of protective controllers
private readonly ProtectiveController _protectiveController = new();
private IProtectivePositionController _posController;

// This code initializes the main protective controller and creates a placeholder for
// a specific position controller. ProtectiveController manages all positions,
// while IProtectivePositionController is responsible for a specific position.
```

- `_protectiveController`：用于管理所有持仓保护的主控制器。
- `_posController`：特定位置的控制器。

### 保护初始化

在开设新持仓或修改现有持仓时，将初始化保护控制器：

```cs
// Initialization of the protective controller for a new position
this.WhenOwnTradeReceived()
	.Do(t =>
	{
		// ... (other code)

		if (TakeValue.IsSet() || StopValue.IsSet())
		{
			_posController ??= _protectiveController.GetController(
				security.ToSecurityId(),
				portfolio.Name,
				new LocalProtectiveBehaviourFactory(security.PriceStep, security.Decimals),
				TakeValue, StopValue, true, default, default, true);
		}

		var info = _posController?.Update(t.Trade.Price, t.GetPosition());

		if (info is not null)
			ActiveProtection(info.Value);
	})
	.Apply(this);

// This code creates and initializes a protective controller for a new position
// upon receiving information about a new trade. It also updates the information
// about the position in the controller and activates protection if necessary.
```

这会为特定位置创建一个带有指定止盈和止损参数的控制器。

### 更新位置信息

```cs
var info = _posController?.Update(t.Trade.Price, t.GetPosition());

if (info is not null)
	ActiveProtection(info.Value);
```

这允许控制器跟踪位置的当前状态，并在必要时调整保护性指令。

### 检查保护的激活条件

在处理新数据的方法中（e.g.，当接收到新K线时），会检查启动保护性订单的条件：

```cs
// Checking protection activation conditions in the ProcessCandle method
var info = _posController?.TryActivate(candle.ClosePrice, CurrentTime);

if (info is not null)
	ActiveProtection(info.Value);

// This code checks if a protective order needs to be activated based on
// the current price (in this case, the candle's closing price) and time.
// If the conditions are met, the ActiveProtection method is called.
```

这里，烛线的收盘价被用作当前价格，但它可以是任何相关的价格值（e.g。，最后一次交易的价格或订单簿中的当前买卖价差）。

### 启动保护令

如果满足启动保护令的条件，将触发相应的逻辑：

```cs
// Method for activating a protective order
private void ActiveProtection((bool isTake, Sides side, decimal price, decimal volume, OrderCondition condition) info)
{
	// sending a protective (position-closing) order as a regular order
	RegisterOrder(this.CreateOrder(info.side, info.price, info.volume));
}

// This method creates and registers an order to close the position
// based on the information received from the protective controller.
```

此方法根据保护控制器返回的参数创建并注册一个平仓订单。

## 与服务器端止损订单的比较

### 服务器端止损单的优势

1. 止损单（止损和止盈）会直接发送给经纪商。
2. 经纪商独立监控止损条件的达成情况。
3. 当触发止损时，经纪商会自动下达市价单或限价单。

### 本地方法的优势

1. **灵活性**：能够实现标准服务器端拦截无法提供的复杂保护逻辑。
2. **保密性**：关于止损水平的信息不会传送给经纪商，这在某些市场中可能很重要。
3. **反应速度**：对不断变化的市场条件可能有更快的反应。
4. **适应性**：能够根据市场数据或策略逻辑动态调整保护级别。
5. **独立于经纪商/交易所的实现**：本地方法的工作方式相同，无论经纪商或交易所是否支持所有必要类型的保护性订单。
6. **在历史数据上测试**：能够在历史数据上完全测试带有持仓保护的策略，而服务器端止损无法实现这一点。

### 地方方法的缺点

1. **依赖交易终端功能**：如果终端断开连接，保护将不起作用。
2. **系统负载**：需要在客户端进行持续计算。
3. **延迟**：在触发保护条件后，下达订单可能会出现延迟。

### 服务器端止损订单的缺点

1. **依赖经纪商/交易所的实现**：并非所有经纪商或交易所都支持所有类型的保护性订单，这可能会限制策略的功能性。
2. **无法在历史数据上进行全面测试**：在历史数据上测试时，服务器端止损无法被准确模拟，这使得评估策略的实际有效性变得困难。
3. **有限的灵活性**：通常只提供基本类型的止损订单，这限制了实施复杂保护机制的可能性。

## 结论

在SMA策略中使用本地保护控制器可以有效管理未平仓头寸的风险。这种方法在设置保护参数和迅速应对市场变化方面提供了灵活性，这对于成功交易至关重要。