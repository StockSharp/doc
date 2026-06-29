# 扩展订单条件

在使用某些交易所或交易系统时，注册订单的标准字段可能不够。例如，当需要时：

1. 在注册[止损单](../../orders_management/create_new_stop_order.md)时。
2. 当有必要指定额外属性以设置自定义排序规则时。

StockSharp 提供了一个灵活的系统来处理这种扩展订单条件。

## 基类订单条件

[OrderCondition](xref:StockSharp.Messages.OrderCondition) 是所有订单条件的抽象基类。它提供了基本功能：

- `Parameters` 字典，用于存储额外的订单参数
- `Clone()` 创建条件副本的方法
- 重写 `ToString()` 方法以便方便地输出有关条件的信息

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
- [IStopLossOrderCondition](xref:StockSharp.Messages.IStopLossOrderCondition) - 适用于具有止损条件的订单
- [IWithdrawOrderCondition](xref:StockSharp.Messages.IWithdrawOrderCondition) - 用于提现订单
- [IRepoOrderCondition](xref:StockSharp.Messages.IRepoOrderCondition) - 用于回购订单
- [INtmOrderCondition](xref:StockSharp.Messages.INtmOrderCondition) - 用于议价交易模式（NDM）的订单

## 基础提现订单条件

[BaseWithdrawOrderCondition](xref:StockSharp.Messages.BaseWithdrawOrderCondition) 是支持资金提取的订单条件的基类。它实现了 `IWithdrawOrderCondition` 接口，并包含用于提款交易的字段。

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

## Coinbase订单条件

`CoinbaseOrderCondition` 类继承自 `BaseWithdrawOrderCondition`，因为 Coinbase 支持程序化的资产提取。此外，它还实现了 `IStopLossOrderCondition` 接口，使其可用于止损订单。

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
	/// Activation price, when reached an order will be placed.
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

在开发您自己的适配器时，您可以通过从 `OrderCondition` 或其某个子类继承并实现必要的接口来创建您自己的订单条件类。这将允许您添加对您交易所特定参数的支持。

要指定适配器支持的订单条件类型，使用 [OrderConditionAttribute](xref:StockSharp.Messages.OrderConditionAttribute) 属性。

```cs
[OrderCondition(typeof(CoinbaseOrderCondition))]
public partial class CoinbaseMessageAdapter
```

这种方法在处理不同交易所及其订单参数的独特要求时提供了灵活性，同时在 StockSharp 架构内保持一致性。