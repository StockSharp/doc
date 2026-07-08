# Condições estendidas de ordem

Ao trabalhar com algumas bolsas ou sistemas de negociação, os campos padrão para registrar uma ordem podem não ser suficientes. Por exemplo, quando é necessário:

1. Ao registrar [ordens stop](../../orders_management/create_new_stop_order.md).
2. Quando é necessário especificar propriedades adicionais para definir regras de ordem personalizadas.

O StockSharp oferece um sistema flexível para trabalhar com essas condições estendidas de ordem.

## Classe Base OrderCondition

[OrderCondition](xref:StockSharp.Messages.OrderCondition) é uma classe base abstrata para todas as condições de ordem. Ela fornece a funcionalidade básica:

- Dicionário `Parameters` para armazenar parâmetros adicionais da ordem
- Método `Clone()` para criar uma cópia da condição
- Método `ToString()` sobrescrito para exibição conveniente das informações sobre a condição

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

## Interfaces Especializadas

O StockSharp define várias interfaces para tipos específicos de condições de ordem:

- [ITakeProfitOrderCondition](xref:StockSharp.Messages.ITakeProfitOrderCondition) - para ordens com condição Take-Profit
- [IStopLossOrderCondition](xref:StockSharp.Messages.IStopLossOrderCondition) - para ordens com condição Stop-Loss
- [IWithdrawOrderCondition](xref:StockSharp.Messages.IWithdrawOrderCondition) - para ordens de retirada
- [IRepoOrderCondition](xref:StockSharp.Messages.IRepoOrderCondition) - para ordens REPO
- [INtmOrderCondition](xref:StockSharp.Messages.INtmOrderCondition) - para ordens no modo de negociações negociadas (NDM)

## BaseWithdrawOrderCondition

[BaseWithdrawOrderCondition](xref:StockSharp.Messages.BaseWithdrawOrderCondition) é uma classe base para condições de ordem que suportam a retirada de fundos. Ela implementa a interface `IWithdrawOrderCondition` e contém campos para a transação de retirada.

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

A classe `CoinbaseOrderCondition` herda de `BaseWithdrawOrderCondition`, já que a Coinbase suporta retirada programática de ativos. Além disso, ela implementa a interface `IStopLossOrderCondition`, o que permite seu uso para ordens stop-loss.

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
	/// Preço de ativação; ao ser atingido, uma ordem será colocada.
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

## Uso no Adaptador

Ao desenvolver seu próprio adaptador, você pode criar sua própria classe de condições de ordem herdando-a de `OrderCondition` ou de uma de suas descendentes e implementando as interfaces necessárias. Isso permitirá adicionar suporte a parâmetros específicos da sua bolsa.

Para especificar o tipo de condição de ordem suportado pelo adaptador, é usado o atributo [OrderConditionAttribute](xref:StockSharp.Messages.OrderConditionAttribute).

```cs
[OrderCondition(typeof(CoinbaseOrderCondition))]
public partial class CoinbaseMessageAdapter
```

Essa abordagem proporciona flexibilidade ao trabalhar com diversas bolsas e seus requisitos exclusivos de parâmetros de ordem, mantendo a uniformidade dentro da arquitetura do StockSharp.
