# Extended Order Conditions

When working with some exchanges or trading systems, standard fields for registering an order may not be enough. For example, when it is required:

1. When registering [stop orders](../../orders_management/create_new_stop_order.md).
2. When it is necessary to specify additional properties to set custom order rules.

StockSharp provides a flexible system for working with such extended order conditions.

## Base Class OrderCondition

[OrderCondition](xref:StockSharp.Messages.OrderCondition) is an abstract base class for all order conditions. It provides the basic functionality:

- `Parameters` dictionary for storing additional order parameters
- `Clone()` method for creating a copy of the condition
- Overridden `ToString()` method for convenient output of information about the condition

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

## Specialized Interfaces

StockSharp defines several interfaces for specific types of order conditions:

- [ITakeProfitOrderCondition](xref:StockSharp.Messages.ITakeProfitOrderCondition) - for orders with Take-Profit condition
- [IStopLossOrderCondition](xref:StockSharp.Messages.IStopLossOrderCondition) - for orders with Stop-Loss condition
- [IWithdrawOrderCondition](xref:StockSharp.Messages.IWithdrawOrderCondition) - for withdrawal orders
- [IRepoOrderCondition](xref:StockSharp.Messages.IRepoOrderCondition) - for REPO orders
- [INtmOrderCondition](xref:StockSharp.Messages.INtmOrderCondition) - for orders of the negotiated deals mode (NDM)

## BaseWithdrawOrderCondition

[BaseWithdrawOrderCondition](xref:StockSharp.Messages.BaseWithdrawOrderCondition) is a base class for order conditions that support funds withdrawal. It implements the `IWithdrawOrderCondition` interface and contains fields for the withdrawal transaction.

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

The `CoinbaseOrderCondition` class is inherited from `BaseWithdrawOrderCondition`, since Coinbase supports programmatic asset withdrawal. In addition, it implements the `IStopLossOrderCondition` interface, which allows it to be used for stop-loss orders.

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

## Usage in the Adapter

When developing your own adapter, you can create your own class of order conditions by inheriting it from `OrderCondition` or one of its descendants and implementing the necessary interfaces. This will allow you to add support for parameters specific to your exchange.

To specify the type of order condition supported by the adapter, the [OrderConditionAttribute](xref:StockSharp.Messages.OrderConditionAttribute) attribute is used.

```cs
[OrderCondition(typeof(CoinbaseOrderCondition))]
public partial class CoinbaseMessageAdapter
```

Such an approach provides flexibility when working with various exchanges and their unique requirements for order parameters while maintaining uniformity within the StockSharp architecture.