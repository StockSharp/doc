# 型変換

型変換コンポーネントは、StockSharp で使用されるデータ型と特定の取引所固有の形式との互換性を確保するうえで重要な役割を果たします。

## 主な機能

1. StockSharp の型（例: [Sides](xref:StockSharp.Messages.Sides)、[OrderTypes](xref:StockSharp.Messages.OrderTypes)、[TimeInForce](xref:StockSharp.Messages.TimeInForce)）を、取引所で使用される文字列表現へ変換します。
2. 取引所から受け取ったデータを StockSharp の型へ逆変換します。
3. StockSharp 形式と取引所形式の間で銘柄識別子を変換します。
4. 時刻形式とタイムフレームを変換します。

## 実装例

以下は、型変換用の拡張メソッドを持つクラスの例です。

```cs
static class Extensions
{
	// StockSharp の注文サイドを取引所の文字列表現へ変換
	public static string ToNative(this Sides side)
	{
		return side switch
		{
			Sides.Buy => "buy",
			Sides.Sell => "sell",
			_ => throw new ArgumentOutOfRangeException(nameof(side), side, LocalizedStrings.InvalidValue),
		};
	}

	// 取引所の注文サイド文字列表現を StockSharp の型へ変換
	public static Sides ToSide(this string side)
		=> side?.ToLowerInvariant() switch
		{
			"buy" or "bid" => Sides.Buy,
			"sell" or "ask" or "offer" => Sides.Sell,
			_ => throw new ArgumentOutOfRangeException(nameof(side), side, LocalizedStrings.InvalidValue),
		};

	// StockSharp の注文種別を取引所の文字列表現へ変換
	public static string ToNative(this OrderTypes? type)
	{
		return type switch
		{
			null => null,
			OrderTypes.Limit => "limit",
			OrderTypes.Market => "market",
			OrderTypes.Conditional => "stop",
			_ => throw new ArgumentOutOfRangeException(nameof(type), type, LocalizedStrings.InvalidValue),
		};
	}

	// 取引所の注文種別文字列表現を StockSharp の型へ変換
	public static OrderTypes ToOrderType(this string type)
		=> type?.ToLowerInvariant() switch
		{
			"limit" => OrderTypes.Limit,
			"market" => OrderTypes.Market,
			"stop" or "stop limit" => OrderTypes.Conditional,
			_ => throw new ArgumentOutOfRangeException(nameof(type), type, LocalizedStrings.InvalidValue),
		};

	// その他の変換メソッド...

	// StockSharp のタイムフレームを取引所の文字列表現にマッピングする辞書
	public static readonly PairSet<TimeSpan, string> TimeFrames = new()
	{
		{ TimeSpan.FromMinutes(1), "ONE_MINUTE" },
		{ TimeSpan.FromMinutes(5), "FIVE_MINUTE" },
		// その他のタイムフレーム...
	};

	// StockSharp のタイムフレームを取引所の文字列表現へ変換
	public static string ToNative(this TimeSpan timeFrame)
		=> TimeFrames.TryGetValue(timeFrame) ?? throw new ArgumentOutOfRangeException(nameof(timeFrame), timeFrame, LocalizedStrings.InvalidValue);

	// 取引所のタイムフレーム文字列表現を TimeSpan へ変換
	public static TimeSpan ToTimeFrame(this string name)
		=> TimeFrames.TryGetKey2(name) ?? throw new ArgumentOutOfRangeException(nameof(name), name, LocalizedStrings.InvalidValue);
}
```

## 推奨事項

- 変換関数を便利に使用できるように、拡張メソッドを使用してください。
- `null` や不明な値を含む、すべての可能な列挙値を処理してください。
- より簡潔で読みやすいコードにするために、`switch` 式（C# 8.0 以降）を使用してください。
- 無効な値のチェックを追加し、明確なエラーメッセージを持つ例外をスローしてください。
- 特に複雑な、または頻繁に変更されるマッピング（例: タイムフレーム）では、値のマッピングに辞書を使用することを検討してください。

型変換を適切に実装すると、コネクターの他の部分でのデータ処理が大幅に簡素化され、形式の不一致に関連するエラーの可能性が低減されます。
