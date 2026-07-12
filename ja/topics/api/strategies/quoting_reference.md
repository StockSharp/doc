# クォーティング リファレンス

## 概要

このセクションでは、StockSharp におけるクォーティング システムのアーキテクチャとコンポーネントの完全なリファレンスを提供します。利用可能なすべてのクォーティング動作、アクション種別、およびインターフェイスについて説明します。基本的な導入については、[クォーティング アルゴリズム](quoting.md)を参照してください。

## アーキテクチャ

### QuotingStrategy (非推奨)

[QuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.QuotingStrategy) クラスは非推奨です。代わりに [QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor) を使用することを推奨します。

非推奨クラスの主なパラメーター:

- `QuotingSide` -- クォーティング方向（買い/売り）
- `QuotingVolume` -- クォーティング数量
- `TimeOut` -- 実行タイムアウト
- `UseBidAsk` -- オーダーブック価格を使用する
- `UseLastTradePrice` -- 直近約定価格を使用する

### QuotingEngine

[QuotingEngine](xref:StockSharp.Algo.Strategies.Quoting.QuotingEngine) -- クォーティング システムの機能的な中核です。残数量とタイムアウトを計算し、副作用なしでアクションの推奨を返します。

### QuotingBehaviorAlgo

[QuotingBehaviorAlgo](xref:StockSharp.Algo.Strategies.Quoting.QuotingBehaviorAlgo) -- アルゴリズムによるポジション管理のための `IPositionModifyAlgo` インターフェイス実装です。

主なメソッド:

| メソッド | 説明 |
|--------|-------------|
| `UpdateMarketData(time, price, volume)` | マーケット データを更新する |
| `UpdateOrderBook(depth)` | オーダーブックを更新する |
| `GetNextAction()` | 次のクォーティング アクションを取得する |

VWAP モードと TWAP モードをサポートします。

### QuotingProcessor

[QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor) -- ストラテジー内でクォーティングを実行するためのメイン プロセッサです。注文のライフサイクル、つまり発注、変更、キャンセルを管理します。

## QuotingAction の種類

プロセッサは [QuotingAction](xref:StockSharp.Algo.Strategies.Quoting.QuotingAction) 型のアクションを返します。

| 種類 | 説明 |
|------|-------------|
| `None(reason)` | アクションは不要 |
| `PlaceOrder(price, volume)` | 新しい注文を発注する |
| `ModifyOrder(price, volume)` | 既存の注文を変更する |
| `CancelOrder()` | 注文をキャンセルする |
| `Finish(success, reason)` | クォーティングを終了する |

## クォーティング動作

StockSharp は 10 種類のクォーティング動作を提供しており、それぞれが [IQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.IQuotingBehavior) インターフェイスを実装します。

| # | クラス | 説明 | 主なパラメーター |
|---|-------|-------------|----------------|
| 1 | [BestByPriceQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.BestByPriceQuotingBehavior) | オーダーブックからの最良価格 | BestPriceOffset |
| 2 | [LastTradeQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LastTradeQuotingBehavior) | 直近約定価格 | BestPriceOffset |
| 3 | [LimitQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LimitQuotingBehavior) | 固定の指値価格 | LimitPrice |
| 4 | [MarketQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingBehavior) | オフセット付き市場価格 | PriceOffset, PriceType (Following/Opposite/Middle) |
| 5 | [VolatilityQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.VolatilityQuotingBehavior) | オプション ボラティリティ (Black-Scholes) | IVRange, Model |
| 6 | [TheorPriceQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.TheorPriceQuotingBehavior) | オプション理論価格 | TheorPriceOffset |
| 7 | [BestByVolumeQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.BestByVolumeQuotingBehavior) | オーダーブック内の累積数量 | VolumeExchange |
| 8 | [LevelQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LevelQuotingBehavior) | オーダーブック深度レベル | Level (Range\<int\>), OwnLevel |
| 9 | [VWAPQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.VWAPQuotingBehavior) | VWAP -- 出来高加重平均 | BestPriceOffset |
| 10 | [TWAPQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.TWAPQuotingBehavior) | TWAP -- 時間加重平均 | TimeInterval, PriceBufferSize (既定値 10) |

## IQuotingBehavior インターフェイス

[IQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.IQuotingBehavior) インターフェイスは、2 つの主要なメソッドを定義します。

### CalculateBestPrice

注文を発注するための最良価格を計算します。

```cs
decimal? CalculateBestPrice(
	Security security,
	IMarketDataProvider provider,
	Sides quotingDirection,
	decimal? bestBid,
	decimal? bestAsk,
	decimal? lastTradePrice,
	decimal? lastTradeVolume,
	IEnumerable<QuoteChange> bids,
	IEnumerable<QuoteChange> asks);
```

### NeedQuoting

現在の注文を更新する必要があるかどうかを判定します。

```cs
decimal? NeedQuoting(
	Security security,
	IMarketDataProvider provider,
	DateTimeOffset currentTime,
	decimal? currentPrice,
	decimal? currentVolume,
	decimal? newVolume,
	decimal? bestPrice);
```

更新が不要な場合は `null` を返し、そうでない場合は注文を発注するための新しい価格を返します。

## 使用例

### オフセット付きマーケット クォーティング

```cs
var behavior = new MarketQuotingBehavior
{
	PriceOffset = new Unit(2, UnitTypes.Absolute),
	PriceType = MarketPriceTypes.Following,
	BestPriceOffset = new Unit(0.5m),
};
```

### VWAP クォーティング

```cs
var behavior = new VWAPQuotingBehavior
{
	BestPriceOffset = new Unit(1),
};
```

### オプション ボラティリティ クォーティング

```cs
var behavior = new VolatilityQuotingBehavior
{
	IVRange = new Range<decimal>(0.2m, 0.3m),
	Model = new BlackScholes(option, connector),
};
```

### Processor を使用した完全な例

```cs
// クォーティング動作を選択する
var behavior = new BestByPriceQuotingBehavior
{
	BestPriceOffset = new Unit(0.01m),
};

// プロセッサを作成する
var processor = new QuotingProcessor(
	behavior,
	Security,
	Portfolio,
	Sides.Buy,
	volume: 10,
	maxOrderVolume: 10,
	timeout: TimeSpan.FromMinutes(5),
	subscriptionProvider: this,
	ruleContainer: this,
	transactionProvider: this,
	timeProvider: this,
	marketDataProvider: this,
	isTradingAllowed: IsFormedAndOnlineAndAllowTrading,
	useBidAsk: true,
	useLastTradePrice: true)
{
	Parent = this,
};

processor.Finished += isOk =>
{
	this.AddInfoLog($"クォーティング完了: {isOk}");
	processor?.Dispose();
};

processor.Start();
```

## 関連項目

- [クォーティング アルゴリズム](quoting.md)
- [クォーティング ストラテジー](samples/mq.md)
- [ボラティリティ クォーティング](../options/volatility_trading.md)
