# ティックデータとスプレッドのローソク足への圧縮

## はじめに

API は、ティックデータとスプレッド（最良買気配/売気配価格）をローソク足へ圧縮するための強力なツールを提供します。この機能は、履歴データの分析やカスタムインジケーターの構築に特に役立ちます。

データ圧縮の主要な拡張メソッドは、`CandleHelper` クラスにあります。このクラスの完全なソースコードは [GitHub で入手できます](https://github.com/StockSharp/StockSharp/blob/master/Algo/Candles/CandleHelper.cs)。

利用可能なすべてのメソッドとそのパラメーターを完全に理解するために、このファイルを確認することを推奨します。

## 圧縮メソッド

### ティックデータのローソク足への圧縮

```cs
// ティックに対する ToCandles の使用例
var tickStorage = storageRegistry.GetTickMessageStorage(securityId, Drive, StorageFormat);
var trades = tickStorage.LoadAsync(from, to);
var candles = trades.ToCandles(mdMsg, candleBuilderProvider: candleBuilderProvider);

// このコードはストレージからティックデータを読み込み、ローソク足へ変換します。
// mdMsg - 作成されるローソク足のパラメーター（タイプ、時間枠など）を持つメッセージ。
// candleBuilderProvider - 特定のローソク足ビルダー実装を提供するプロバイダー。
```

### スプレッドデータのローソク足への圧縮

```cs
// スプレッドデータに対する ToCandles の使用例
var depthStorage = storageRegistry.GetQuoteMessageStorage(securityId, Drive, StorageFormat);
var depths = depthStorage.LoadAsync(from, to);
var candles = depths.ToCandles(mdMsg, Level1Fields.SpreadMiddle, candleBuilderProvider: candleBuilderProvider);

// ここではスプレッドデータを読み込み、ローソク足へ変換します。
// Level1Fields.SpreadMiddle は、ローソク足の構築にスプレッドの中央値を使用することを示します。
// 最良買気配価格または最良売気配価格には、それぞれ Level1Fields.BestBid または Level1Fields.BestAsk も使用できます。
```

## 圧縮パラメーター

データを圧縮する際には、次のパラメーターを指定できます。

- `series`: 作成されるローソク足のタイプとパラメーターを定義するローソク足シリーズ。
- `type`: ローソク足形成に使用するデータのタイプ（例: 最良買気配、最良売気配、またはスプレッド中央値）。
- `candleBuilderProvider`: ローソク足ビルダーのプロバイダー（省略可能なパラメーター）。

## 使用例

```cs
private IEnumerable<CandleMessage> InternalGetCandles(SecurityId securityId, DateTime? from, DateTime? to)
{
	// ...（初期化コード）

	switch (type)
	{
		case BuildTypes.Ticks:
			return StorageRegistry
					.GetTickMessageStorage(securityId, Drive, StorageFormat)
					.LoadAsync(from, to)
					.ToCandles(mdMsg, candleBuilderProvider: candleBuilderProvider);

		case BuildTypes.OrderLog:
			// ...（オーダーログからローソク足を構築するコード）

		case BuildTypes.Depths:
			return StorageRegistry
					.GetQuoteMessageStorage(securityId, Drive, StorageFormat)
					.LoadAsync(from, to)
					.ToCandles(mdMsg, Convert(extraType), candleBuilderProvider: candleBuilderProvider);

		// ...（その他のケース）
	}
}

// このメソッドは、ソースデータのタイプに応じたローソク足構築のさまざまな方法を示します。
// ティック、オーダーログ、スプレッド、およびその他のソースからの構築に対応しています。
```

## 追加機能

### さまざまなソースからのローソク足構築

API では、ティックとスプレッドだけでなく、他のデータソースからもローソク足を構築できます。

```cs
// さまざまなソースからローソク足を構築する例
switch (type)
{
	case BuildTypes.Ticks:
		// ...（ティック用のコード）

	case BuildTypes.OrderLog:
		// ...（オーダーログ用のコード）

	case BuildTypes.Depths:
		// ...（スプレッド用のコード）

	case BuildTypes.Level1:
		// ...（Level1 用のコード）

	case BuildTypes.SmallerTimeFrame:
		return candleBuilderProvider
				.GetCandleMessageBuildableStorage(StorageRegistry, securityId, mdMsg.GetTimeFrame(), Drive, StorageFormat)
				.LoadAsync(from, to);

	// ...（その他のケース）
}

// このコードは、ティック、オーダーログ、スプレッド、Level1 データ、さらにはより小さい時間枠のローソク足など、異なるデータソースからローソク足を構築する方法を示します。
```

## まとめ

API のデータ圧縮メソッドは、市場データを扱うための柔軟なツールを提供します。ティックデータとスプレッドデータを、さまざまなタイプおよび時間間隔のローソク足へ効率的に変換できるため、市場分析や取引戦略の開発に特に有用です。
