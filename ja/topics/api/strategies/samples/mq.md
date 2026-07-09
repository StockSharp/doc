# クォーティング ストラテジー

## 概要

`MqStrategy` は、市場ポジションを管理するためにクォーティング メカニズムを使用するストラテジーです。現在のポジションに基づいてクォーティング プロセッサを作成し、市場状況の変化へ適応的に対応できるようにします。

## 主なコンポーネント

```cs
public class MqStrategy : Strategy
{
	private readonly StrategyParam<MarketPriceTypes> _priceType;
	private readonly StrategyParam<Unit> _priceOffset;
	private readonly StrategyParam<Unit> _bestPriceOffset;

	private QuotingProcessor _quotingProcessor;
}
```

## ストラテジー パラメーター

このストラテジーでは、次のパラメーターをカスタマイズできます。

- **PriceType** - クォーティングに使用する市場価格タイプ (既定値 Following)
- **PriceOffset** - 市場価格からの価格オフセット
- **BestPriceOffset** - クォート更新の最小乖離 (既定値 0.1%)

## ストラテジーの初期化

[OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) メソッドでは、ストラテジーは市場時刻の変更を購読します。

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// クォート更新のために市場時刻の変更を購読する
	Connector.CurrentTimeChanged += Connector_CurrentTimeChanged;
	Connector_CurrentTimeChanged(default);
}
```

## クォーティング プロセッサの管理

`Connector_CurrentTimeChanged` メソッドは市場時刻が変化したときに呼び出され、クォーティング プロセッサの作成と更新を管理します。

```cs
private void Connector_CurrentTimeChanged(TimeSpan obj)
{
	// 現在のプロセッサが停止しているか存在しない場合のみ新しいプロセッサを作成する
	if (_quotingProcessor != null && _quotingProcessor.LeftVolume > 0)
		return;

	// 古いプロセッサが存在する場合はそのリソースを解放する
	_quotingProcessor?.Dispose();
	_quotingProcessor = null;

	// 現在のポジションに基づいてクォーティング側を判定する
	var side = Position <= 0 ? Sides.Buy : Sides.Sell;

	// 新しいクォーティング動作を作成する
	var behavior = new MarketQuotingBehavior(
		PriceOffset,
		BestPriceOffset,
		PriceType
	);

	// クォーティング数量を計算する
	var quotingVolume = Volume + Math.Abs(Position);

	// プロセッサを作成して初期化する
	_quotingProcessor = new QuotingProcessor(
		behavior,
		Security,
		Portfolio,
		side,
		quotingVolume,
		Volume, // 最大注文数量
		TimeSpan.Zero, // タイムアウトなし
		this, // Strategy は ISubscriptionProvider を実装
		this, // Strategy は IMarketRuleContainer を実装
		this, // Strategy は ITransactionProvider を実装
		this, // Strategy は ITimeProvider を実装
		this, // Strategy は IMarketDataProvider を実装
		IsFormedAndOnlineAndAllowTrading, // 取引許可を確認する
		true, // オーダーブック価格を使用する
		true  // オーダーブックが空の場合は直近約定価格を使用する
	)
	{
		Parent = this
	};

	// ロギングのためにプロセッサ イベントを購読する
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

	// プロセッサを開始する
	_quotingProcessor.Start();
}
```

## リソース解放

[OnStopped](xref:StockSharp.Algo.Strategies.Strategy.OnStopped) メソッドでは、ストラテジーはリソースを解放します。

```cs
protected override void OnStopped()
{
	// メモリ リークを防ぐために購読解除する
	Connector.CurrentTimeChanged -= Connector_CurrentTimeChanged;

	// 現在のプロセッサが存在する場合はそのリソースを解放する
	_quotingProcessor?.Dispose();
	_quotingProcessor = null;

	base.OnStopped();
}
```

## 取引ロジック

- このストラテジーは市場時刻の変化に応答します
- クォーティング方向は現在のポジションに基づいて決定されます。
  - ポジション <= 0 の場合、Buy クォートが作成されます
  - ポジション > 0 の場合、Sell クォートが作成されます
- クォーティング数量は、基本数量に現在ポジションの絶対値を加えたものとして計算されます
- クォーティングには、[QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor) と [MarketQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingBehavior) が使用されます

## 特徴

- レガシーなクォーティング ストラテジーではなく、最新のクォーティング プロセッサを使用します
- クォーティング方向を変えることで、ポジション変化に適応的に対応します
- さまざまなクォーティング パラメーター (価格タイプ、オフセット、最小乖離) の設定をサポートします
- クォーティング プロセッサ イベントの詳細なロギングを含みます
- ストラテジー停止時および新しいプロセッサ作成時に、リソースを適切に管理します
- さまざまな種類の市場価格 (Following、Best、Opposite など) での動作をサポートします
