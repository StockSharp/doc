# 持仓保护

## 介绍

这种对SMA策略的修改实现了使用本地保护控制器保护未平仓持仓的机制。这种方法允许灵活的风险管理，并在满足某些条件时自动平仓。

## 持仓保护的关键组成部分

### 保护控制器

该策略使用两个关键对象来保护持仓：

```cs
// 声明保护控制器
private readonly ProtectiveController _protectiveController = new();
private IProtectivePositionController _posController;

// 此代码初始化主保护控制器，并为以下内容创建占位符
// 特定持仓控制器。ProtectiveController 管理所有持仓，
// while IProtectivePositionController is responsible for a specific position.
```

- `_protectiveController`：用于管理所有持仓保护的主控制器。
- `_posController`：特定位置的控制器。

### 保护初始化

在开设新持仓或修改现有持仓时，将初始化保护控制器：

```cs
// 为新持仓初始化保护控制器
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

// 此代码为新持仓创建并初始化保护控制器
// 在收到新成交信息后执行。它还会更新信息
// 更新控制器中的持仓信息，并在需要时激活保护。
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

在处理新数据的方法中（例如，当接收到新K线时），会检查启动保护性订单的条件：

```cs
// 在 ProcessCandle 方法中检查保护激活条件
var info = _posController?.TryActivate(candle.ClosePrice, CurrentTime);

if (info is not null)
	ActiveProtection(info.Value);

// 此代码根据当前价格（在本例中为蜡烛的收盘价）和时间
// 检查是否需要激活保护订单。
// 如果满足条件，则调用 ActiveProtection 方法。
```

这里，蜡烛的收盘价被用作当前价格，但它可以是任何相关的价格值（例如，最后一次交易的价格或订单簿中的当前买卖价差）。

### 启动保护令

如果满足启动保护令的条件，将触发相应的逻辑：

```cs
// 激活保护订单的方法
private void ActiveProtection((bool isTake, Sides side, decimal price, decimal volume, OrderCondition condition) info)
{
	// 将保护性（平仓）订单作为普通订单发送
	RegisterOrder(this.CreateOrder(info.side, info.price, info.volume));
}

// 此方法创建并注册用于平仓的订单
// 基于从保护控制器收到的信息。
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

在SMA策略中使用本地保护控制器可以有效管理未平仓持仓的风险。这种方法在设置保护参数和迅速应对市场变化方面提供了灵活性，这对于成功交易至关重要。
