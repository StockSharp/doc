# 扩展订单条件

在使用某些交易所或交易系统时，注册订单的标准字段可能不够。例如，在以下情况下：

1. 在注册[止损单](../../orders_management/create_new_stop_order.md)时。
2. 需要指定额外属性来设置自定义订单规则时。

StockSharp 提供了一套灵活的机制来处理这类扩展订单条件。

## 基类订单条件

[OrderCondition](xref:StockSharp.Messages.OrderCondition) 是所有订单条件的抽象基类。它提供以下基础功能：

- `Parameters` 字典，用于存储额外的订单参数
- `Clone()` 方法，用于创建条件副本
- 重写的 `ToString()` 方法，用于方便地输出条件信息

```cs
public class MyOrderCondition : OrderCondition
{
	public decimal? SpecialPrice
	{
		get => (decimal?)Parameters[nameof(SpecialPrice)];
		set => Parameters[nameof(SpecialPrice)] = value;
	}
}
```

## 专用接口

StockSharp 定义了几种特定类型订单条件的接口：

- [ITakeProfitOrderCondition](xref:StockSharp.Messages.ITakeProfitOrderCondition) - 用于具有止盈条件的订单
- [IStopLossOrderCondition](xref:StockSharp.Messages.IStopLossOrderCondition) - 用于具有止损条件的订单
- [IWithdrawOrderCondition](xref:StockSharp.Messages.IWithdrawOrderCondition) - 用于提现订单
- [IRepoOrderCondition](xref:StockSharp.Messages.IRepoOrderCondition) - 用于回购订单
- [INtmOrderCondition](xref:StockSharp.Messages.INtmOrderCondition) - 用于议价交易模式（NDM）的订单

## 基础提现订单条件

[BaseWithdrawOrderCondition](xref:StockSharp.Messages.BaseWithdrawOrderCondition) 是支持资金提现的订单条件基类。它实现了 `IWithdrawOrderCondition` 接口，并包含提现交易所需的字段。

```cs
public class MyWithdrawCondition : BaseWithdrawOrderCondition
{
	public string DestinationAddress
	{
		get => (string)Parameters[nameof(DestinationAddress)];
		set => Parameters[nameof(DestinationAddress)] = value;
	}
}
```

## CoinbaseOrderCondition

`CoinbaseOrderCondition` 类继承自 `BaseWithdrawOrderCondition`，因为 Coinbase 支持通过程序进行资产提现。此外，它还实现了 `IStopLossOrderCondition` 接口，因此可用于止损订单。

```cs
[Serializable]
[DataContract]
[Display(ResourceType = typeof(LocalizedStrings), Name = LocalizedStrings.CoinbaseKey)]
public class CoinbaseOrderCondition : BaseWithdrawOrderCondition, IStopLossOrderCondition
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CoinbaseOrderCondition"/>.
	/// </summary>
	public CoinbaseOrderCondition()
	{
	}

	/// <summary>
	/// 达到后将下单的激活价格。
	/// </summary>
	[DataMember]
	[Display(
		ResourceType = typeof(LocalizedStrings),
		Name = LocalizedStrings.StopPriceKey,
		Description = LocalizedStrings.StopPriceDescKey,
		GroupName = LocalizedStrings.StopLossKey,
		Order = 0)]
	public decimal? StopPrice
	{
		get => (decimal?)Parameters.TryGetValue(nameof(StopPrice));
		set => Parameters[nameof(StopPrice)] = value;
	}

	decimal? IStopLossOrderCondition.ClosePositionPrice { get; set; }

	decimal? IStopLossOrderCondition.ActivationPrice
	{
		get => StopPrice;
		set => StopPrice = value;
	}

	bool IStopLossOrderCondition.IsTrailing
	{
		get => false;
		set {  }
	}
}
```

## 适配器中的用法

开发自己的适配器时，可以通过继承 `OrderCondition` 或其某个派生类并实现必要接口，创建自己的订单条件类。这样可以为交易所特有的参数添加支持。

要指定适配器支持的订单条件类型，需要使用 [OrderConditionAttribute](xref:StockSharp.Messages.OrderConditionAttribute) 特性。

```cs
[OrderCondition(typeof(CoinbaseOrderCondition))]
public partial class CoinbaseMessageAdapter
```

这种方式既能灵活处理不同交易所对订单参数的特殊要求，又能在 StockSharp 架构内保持统一。
