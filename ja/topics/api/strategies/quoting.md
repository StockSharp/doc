# クォーティングアルゴリズム

## 概要

クォーティングアルゴリズムは、最良の執行価格を実現することを目的として、市場に注文を自動的に発注および更新するための仕組みです。積極的な成行注文を出す代わりに、クォーティングでは指値注文を使用します。これにより、スリッページを最小化し、取引コストを削減できます。

## 主要コンポーネント

StockSharp は、クォーティングのために 2 つの主要コンポーネントを提供します。

1. **[QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor)** - マーケットデータと注文状態を分析し、クォーティングアクションを推奨するメインプロセッサーです。

2. **[IQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.IQuotingBehavior)** - 最適価格の計算や注文更新の必要性の判断など、クォーティング動作を定義するインターフェイスです。

## クォーティングを使用するストラテジーの例

ドキュメントには、クォーティング機構の使用方法を示すストラテジー例が用意されています。

- **MqStrategy** - 市場でポジションを管理するためにクォーティング機構を使用するストラテジー例です。
- **MqSpreadStrategy** - 買い気配と売り気配を同時に提示することで市場にスプレッドを作成するストラテジー例です。
- **StairsCountertrendStrategy** - より正確な市場エントリーのためにクォーティングを使用する逆張りストラテジー例です。

これらの例は、クォーティング機構を独自の取引アルゴリズムへ統合する方法を理解するのに役立ちます。

## クォーティング動作

StockSharp はさまざまなクォーティング動作をサポートしています。

- **[MarketQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingBehavior)** - カスタマイズ可能なオフセットとタイプを使用し、市場価格に基づくクォーティングです。
- **[BestByPriceQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.BestByPriceQuotingBehavior)** - カスタマイズ可能なオフセットを使用し、最良価格に基づくクォーティングです。
- **[LimitQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LimitQuotingBehavior)** - 固定価格でのクォーティングです。
- **[BestByVolumeQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.BestByVolumeQuotingBehavior)** - 数量に基づく最良価格でのクォーティングです。
- **[LevelQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LevelQuotingBehavior)** - 板情報内の指定レベルに基づくクォーティングです。
- **[LastTradeQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LastTradeQuotingBehavior)** - 直近約定価格に基づくクォーティングです。
- **[TheorPriceQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.TheorPriceQuotingBehavior)** - 理論価格に基づくオプションのクォーティングです。
- **[VolatilityQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.VolatilityQuotingBehavior)** - ボラティリティに基づくオプションのクォーティングです。

## 独自ストラテジーでの使用

### 手順 1: Quoting Behavior を作成する

```csharp
// マーケットクォーティング用の動作を作成
var behavior = new MarketQuotingBehavior(
	new Unit(0.01m), // 最良気配からの価格オフセット
	new Unit(0.1m, UnitTypes.Percent), // 気配更新の最小偏差
	MarketPriceTypes.Following // クォーティング用の市場価格タイプ
);
```

### 手順 2: プロセッサーを作成して初期化する

```csharp
// クォーティングプロセッサーを作成
_quotingProcessor = new QuotingProcessor(
	behavior,
	Security, // 銘柄
	Portfolio, // ポートフォリオ
	Sides.Buy, // クォーティング方向
	Volume, // クォーティング数量
	Volume, // 最大注文数量
	TimeSpan.Zero, // タイムアウトなし
	this, // Strategy は ISubscriptionProvider を実装
	this, // Strategy は IMarketRuleContainer を実装
	this, // Strategy は ITransactionProvider を実装
	this, // Strategy は ITimeProvider を実装
	this, // Strategy は IMarketDataProvider を実装
	IsFormedAndOnlineAndAllowTrading, // 取引許可を確認
	true, // 板情報価格を使用
	true  // 板情報が空の場合は直近約定価格を使用
)
{
	Parent = this
};
```

### 手順 3: プロセッサーイベントを購読する

```csharp
// ログ記録と処理のためにプロセッサーイベントを購読
_quotingProcessor.OrderRegistered += order =>
	this.AddInfoLog($"注文 {order.TransactionId} を価格 {order.Price} で登録しました");

_quotingProcessor.OrderFailed += fail =>
	this.AddInfoLog($"注文に失敗しました: {fail.Error.Message}");

_quotingProcessor.OwnTrade += trade =>
	this.AddInfoLog($"約定が実行されました: {trade.Trade.Volume}、価格 {trade.Trade.Price}");

_quotingProcessor.Finished += isOk => {
	this.AddInfoLog($"クォーティングが正常に完了しました: {isOk}");
	_quotingProcessor?.Dispose();
	_quotingProcessor = null;
};
```

### 手順 4: プロセッサーを開始する

```csharp
// プロセッサーを開始
_quotingProcessor.Start();
```

### 手順 5: リソースを解放する

ストラテジー停止時には、プロセッサーリソースのクリーンアップを忘れないでください。

```csharp
protected override void OnStopped()
{
	// 現在のプロセッサーのリソースを解放
	_quotingProcessor?.Dispose();
	_quotingProcessor = null;
	
	base.OnStopped();
}
```

## 使用する利点

1. **スリッページの低減** - クォーティングは、成行注文と比べてより良い執行価格を得るのに役立ちます。

2. **設定の柔軟性** - さまざまな市場状況に対応する各種クォーティング動作。

3. **自動更新** - プロセッサーは市場変化を自動的に追跡し、必要に応じて注文を更新します。

4. **完全な制御** - 価格オフセット、最小偏差、市場価格タイプなどのクォーティングパラメーターを設定できます。

5. **リスク管理** - 執行時間を制限するタイムアウトを設定できます。

## 適用ユースケース

- **マーケットメイキング** - 両側気配を提示して市場に流動性を作る。
- **アルゴリズム取引** - 自動売買ストラテジーで執行価格を改善する。
- **逆張り取引** - トレンドに逆らう、より正確な市場エントリー。
- **裁定取引** - 価格差を利用するため、複数市場で同時にクォーティングする。

ストラテジーにクォーティング機構を実装すると、注文執行を大きく改善し、効率を高めることができます。
