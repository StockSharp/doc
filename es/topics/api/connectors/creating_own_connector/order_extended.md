# Condiciones de orden extendidas

Al trabajar con algunos exchanges o sistemas de negociación, los campos estándar para registrar una orden pueden no ser suficientes. Por ejemplo, cuando se requiere:

1. Al registrar [órdenes stop](../../orders_management/create_new_stop_order.md).
2. Cuando se necesita especificar propiedades adicionales para establecer reglas de orden personalizadas.

StockSharp proporciona un sistema flexible para trabajar con este tipo de condiciones de orden extendidas.

## Clase base OrderCondition

[OrderCondition](xref:StockSharp.Messages.OrderCondition) es una clase base abstracta para todas las condiciones de orden. Proporciona la funcionalidad básica:

- Diccionario `Parameters` para almacenar parámetros adicionales de la orden
- Método `Clone()` para crear una copia de la condición
- Método `ToString()` sobrescrito para una salida conveniente de la información sobre la condición

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

## Interfaces especializadas

StockSharp define varias interfaces para tipos específicos de condiciones de orden:

- [ITakeProfitOrderCondition](xref:StockSharp.Messages.ITakeProfitOrderCondition) - para órdenes con condición Take-Profit
- [IStopLossOrderCondition](xref:StockSharp.Messages.IStopLossOrderCondition) - para órdenes con condición Stop-Loss
- [IWithdrawOrderCondition](xref:StockSharp.Messages.IWithdrawOrderCondition) - para órdenes de retiro de fondos
- [IRepoOrderCondition](xref:StockSharp.Messages.IRepoOrderCondition) - para órdenes REPO
- [INtmOrderCondition](xref:StockSharp.Messages.INtmOrderCondition) - para órdenes del modo de operaciones negociadas (NDM)

## BaseWithdrawOrderCondition

[BaseWithdrawOrderCondition](xref:StockSharp.Messages.BaseWithdrawOrderCondition) es una clase base para condiciones de orden que admiten el retiro de fondos. Implementa la interfaz `IWithdrawOrderCondition` y contiene campos para la transacción de retiro.

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

La clase `CoinbaseOrderCondition` hereda de `BaseWithdrawOrderCondition`, ya que Coinbase admite el retiro programático de activos. Además, implementa la interfaz `IStopLossOrderCondition`, lo que permite usarla para órdenes stop-loss.

```cs
[Serializable]
[DataContract]
[Display(ResourceType = typeof(LocalizedStrings), Name = LocalizedStrings.CoinbaseKey)]
public class CoinbaseOrderCondition : BaseWithdrawOrderCondition, IStopLossOrderCondition
{
	/// <summary>
	/// Inicializa una nueva instancia de <see cref="CoinbaseOrderCondition"/>.
	/// </summary>
	public CoinbaseOrderCondition()
	{
	}

	/// <summary>
	/// Precio de activación; al alcanzarse, se colocará una orden.
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

## Uso en el adaptador

Al desarrollar su propio adaptador, puede crear su propia clase de condiciones de orden heredándola de `OrderCondition` o de una de sus clases derivadas e implementando las interfaces necesarias. Esto le permitirá agregar soporte para parámetros específicos de su exchange.

Para especificar el tipo de condición de orden admitida por el adaptador, se utiliza el atributo [OrderConditionAttribute](xref:StockSharp.Messages.OrderConditionAttribute).

```cs
[OrderCondition(typeof(CoinbaseOrderCondition))]
public partial class CoinbaseMessageAdapter
```

Este enfoque proporciona flexibilidad al trabajar con varios exchanges y sus requisitos únicos para los parámetros de orden, manteniendo al mismo tiempo la uniformidad dentro de la arquitectura de StockSharp.
