# スプレッド・クォーティング戦略

## 概要

`MqSpreadStrategy` は、買いと売りのクォートを同時に発注することで市場にスプレッドを作る戦略です。市場の両サイドの注文を管理するために、2 つのクォーティングプロセッサを使用します。

## 主要コンポーネント

```cs
public class MqSpreadStrategy : Strategy
{
	private readonly StrategyParam<MarketPriceTypes> _priceType;
	private readonly StrategyParam<Unit> _priceOffset;
	private readonly StrategyParam<Unit> _bestPriceOffset;

	private QuotingProcessor _buyProcessor;
	private QuotingProcessor _sellProcessor;
}
```

## 戦略パラメーター

この戦略では、次のパラメーターをカスタマイズできます。

- **価格タイプ** - クォーティングに使用する市場価格タイプ（既定値は Following）
- **価格オフセット** - 市場価格からの価格オフセット
- **最良価格からのオフセット** - クォート更新の最小偏差（既定値は 0.1%）

## 戦略の初期化

[OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) メソッドで、戦略は市場時刻の変更を購読します。

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// クォート更新のために市場時刻の変更を購読
	Connector.CurrentTimeChanged += Connector_CurrentTimeChanged;
	Connector_CurrentTimeChanged(new TimeSpan());
}
```

## クォーティングプロセッサの管理

`Connector_CurrentTimeChanged` メソッドは、市場時刻が変化したときに呼び出され、クォーティングプロセッサの作成と更新を管理します。

```cs
private void Connector_CurrentTimeChanged(TimeSpan obj)
{
	// ポジションがゼロで、現在のプロセッサが停止している場合にのみ新しいプロセッサを作成
	if (Position != 0)
		return;

	if (_buyProcessor != null && _buyProcessor.LeftVolume > 0)
		return;

	if (_sellProcessor != null && _sellProcessor.LeftVolume > 0)
		return;

	// 既存プロセッサのリソースを解放
	_buyProcessor?.Dispose();
	_buyProcessor = null;

	_sellProcessor?.Dispose();
	_sellProcessor = null;

	// マーケットクォーティング用の動作を作成
	var buyBehavior = new MarketQuotingBehavior(
		PriceOffset,
		BestPriceOffset,
		PriceType
	);

	var sellBehavior = new MarketQuotingBehavior(
		PriceOffset,
		BestPriceOffset,
		PriceType
	);

	// 買い用プロセッサを作成
	_buyProcessor = new QuotingProcessor(
		buyBehavior,
		Security,
		Portfolio,
		Sides.Buy,
		Volume,
		Volume, // 最大注文出来高
		TimeSpan.Zero, // タイムアウトなし
		this, // Strategy は ISubscriptionProvider を実装
		this, // Strategy は IMarketRuleContainer を実装
		this, // Strategy は ITransactionProvider を実装
		this, // Strategy は ITimeProvider を実装
		this, // Strategy は IMarketDataProvider を実装
		IsFormedAndOnlineAndAllowTrading, // 取引許可を確認
		true, // 板価格を使用
		true  // 板が空の場合は直近約定価格を使用
	)
	{
		Parent = this
	};

	// 売り用プロセッサを作成
	_sellProcessor = new QuotingProcessor(
		sellBehavior,
		Security,
		Portfolio,
		Sides.Sell,
		Volume,
		Volume, // 最大注文出来高
		TimeSpan.Zero, // タイムアウトなし
		this, // Strategy は ISubscriptionProvider を実装
		this, // Strategy は IMarketRuleContainer を実装
		this, // Strategy は ITransactionProvider を実装
		this, // Strategy は ITimeProvider を実装
		this, // Strategy は IMarketDataProvider を実装
		IsFormedAndOnlineAndAllowTrading, // 取引許可を確認
		true, // 板価格を使用
		true  // 板が空の場合は直近約定価格を使用
	)
	{
		Parent = this
	};

	// 新しいクォーティングプロセッサの作成をログ出力
	this.AddInfoLog($"{CurrentTime} に買い/売りスプレッドを作成しました");

	// ログ出力のために買いプロセッサのイベントを購読
	_buyProcessor.OrderRegistered += order =>
		this.AddInfoLog($"買い注文 {order.TransactionId} を価格 {order.Price} で登録しました");

	_buyProcessor.OrderFailed += fail =>
		this.AddInfoLog($"買い注文に失敗しました: {fail.Error.Message}");

	_buyProcessor.OwnTrade += trade =>
		this.AddInfoLog($"買い約定が実行されました: {trade.Trade.Volume}、価格 {trade.Trade.Price}");

	_buyProcessor.Finished += isOk => {
		this.AddInfoLog($"買いクォーティングが正常に完了しました: {isOk}");
		_buyProcessor?.Dispose();
		_buyProcessor = null;
	};

	// ログ出力のために売りプロセッサのイベントを購読
	_sellProcessor.OrderRegistered += order =>
		this.AddInfoLog($"売り注文 {order.TransactionId} を価格 {order.Price} で登録しました");

	_sellProcessor.OrderFailed += fail =>
		this.AddInfoLog($"売り注文に失敗しました: {fail.Error.Message}");

	_sellProcessor.OwnTrade += trade =>
		this.AddInfoLog($"売り約定が実行されました: {trade.Trade.Volume}、価格 {trade.Trade.Price}");

	_sellProcessor.Finished += isOk => {
		this.AddInfoLog($"売りクォーティングが正常に完了しました: {isOk}");
		_sellProcessor?.Dispose();
		_sellProcessor = null;
	};

	// 両方のプロセッサを開始
	_buyProcessor.Start();
	_sellProcessor.Start();
}
```

## リソースの解放

[OnStopped](xref:StockSharp.Algo.Strategies.Strategy.OnStopped) メソッドで、戦略はリソースを解放します。

```cs
protected override void OnStopped()
{
	// メモリリークを防ぐために購読解除
	Connector.CurrentTimeChanged -= Connector_CurrentTimeChanged;

	// プロセッサのリソースを解放
	_buyProcessor?.Dispose();
	_buyProcessor = null;

	_sellProcessor?.Dispose();
	_sellProcessor = null;

	base.OnStopped();
}
```

## 取引ロジック

- 戦略は市場時刻の変更に応答します
- ポジションがゼロで、プロセッサが停止している場合、2 つの新しいプロセッサが作成されます。
  - 買いプロセッサ（Buy）
  - 売りプロセッサ（Sell）
- 両方のプロセッサは同じ出来高で構成され、同じクォーティング設定を使用します
- プロセッサは買い注文と売り注文を同時に発注することで、市場にスプレッドを作ります

## 機能

- モダンなクォーティングプロセッサ [QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor) を [MarketQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingBehavior) とともに使用します
- 買い注文と売り注文を同時に発注することで、市場にスプレッドを作ります
- ポジションがゼロの場合にのみ動作し、不要なリスクの蓄積を防ぎます
- さまざまなクォーティングパラメーター（価格タイプ、オフセット、最小偏差）の設定をサポートします
- クォーティングプロセッサイベントの詳細なログ出力を含みます
- 戦略の停止時および新しいプロセッサ作成時にリソースを適切に管理します
- さまざまな市場価格タイプ（Following、Best、Opposite など）での動作をサポートします
