# 拡張注文条件

一部の取引所や取引システムを扱う場合、注文登録の標準フィールドだけでは不十分なことがあります。たとえば、次のような場合です。

1. [ストップ注文](../../orders_management/create_new_stop_order.md)を登録する場合。
2. カスタムの注文ルールを設定するために追加プロパティを指定する必要がある場合。

StockSharp は、このような拡張注文条件を扱うための柔軟な仕組みを提供しています。

## 基底クラス OrderCondition

[OrderCondition](xref:StockSharp.Messages.OrderCondition) は、すべての注文条件の抽象基底クラスです。基本機能を提供します。

- 追加の注文パラメーターを保存するための `Parameters` ディクショナリ
- 条件のコピーを作成するための `Clone()` メソッド
- 条件に関する情報を出力しやすくするためにオーバーライドされた `ToString()` メソッド

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

## 特殊化されたインターフェイス

StockSharp は、特定の種類の注文条件向けにいくつかのインターフェイスを定義しています。

- [ITakeProfitOrderCondition](xref:StockSharp.Messages.ITakeProfitOrderCondition) - Take-Profit 条件を持つ注文用
- [IStopLossOrderCondition](xref:StockSharp.Messages.IStopLossOrderCondition) - Stop-Loss 条件を持つ注文用
- [IWithdrawOrderCondition](xref:StockSharp.Messages.IWithdrawOrderCondition) - 出金注文用
- [IRepoOrderCondition](xref:StockSharp.Messages.IRepoOrderCondition) - REPO 注文用
- [INtmOrderCondition](xref:StockSharp.Messages.INtmOrderCondition) - 相対取引モード (NDM) の注文用

## BaseWithdrawOrderCondition

[BaseWithdrawOrderCondition](xref:StockSharp.Messages.BaseWithdrawOrderCondition) は、資金の出金をサポートする注文条件の基底クラスです。`IWithdrawOrderCondition` インターフェイスを実装し、出金トランザクション用のフィールドを含みます。

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

Coinbase はプログラムによる資産出金をサポートしているため、`CoinbaseOrderCondition` クラスは `BaseWithdrawOrderCondition` から継承されています。さらに、Stop-Loss 注文で使用できるようにするため、`IStopLossOrderCondition` インターフェイスを実装しています。

```cs
[Serializable]
[DataContract]
[Display(ResourceType = typeof(LocalizedStrings), Name = LocalizedStrings.CoinbaseKey)]
public class CoinbaseOrderCondition : BaseWithdrawOrderCondition, IStopLossOrderCondition
{
	/// <summary>
	/// <see cref="CoinbaseOrderCondition"/> の新しいインスタンスを初期化します。
	/// </summary>
	public CoinbaseOrderCondition()
	{
	}

	/// <summary>
	/// 到達すると注文が発注される有効化価格。
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

## アダプターでの使用

独自のアダプターを開発する場合、`OrderCondition` またはその派生クラスのいずれかを継承し、必要なインターフェイスを実装することで、独自の注文条件クラスを作成できます。これにより、対象の取引所固有のパラメーターをサポートできます。

アダプターがサポートする注文条件の型を指定するには、[OrderConditionAttribute](xref:StockSharp.Messages.OrderConditionAttribute) 属性を使用します。

```cs
[OrderCondition(typeof(CoinbaseOrderCondition))]
public partial class CoinbaseMessageAdapter
```

このアプローチにより、さまざまな取引所とそれぞれ固有の注文パラメーター要件に柔軟に対応しながら、StockSharp アーキテクチャ内での統一性を維持できます。
