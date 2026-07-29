# クォーティング付き逆張り戦略

## 概要

`StairsCountertrendStrategy` は、特定の長さで確立されたトレンドに逆らってポジションを建てる逆張り取引戦略であり、より正確な市場エントリーのためにクォーティングメカニズムを使用します。

## 主要コンポーネント

```cs
public class StairsCountertrendStrategy : Strategy
{
	private readonly StrategyParam<DataType> _candleDataType;
	private readonly StrategyParam<int> _length;
	private QuotingProcessor _quotingProcessor;

	private int _bullLength;
	private int _bearLength;
}
```

## 戦略パラメーター

この戦略では、次のパラメーターをカスタマイズできます。

- **CandleDataType** - 使用するローソク足タイプ（既定値は 1 分）
- `Length` - トレンドを識別するための同一方向の連続ローソク足数（既定値は 5）

Length パラメーターは、2 から 10 まで、ステップ 1 の範囲で最適化に使用できます。

## 戦略の初期化

[OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) メソッドで、カウンターがリセットされ、ローソク足購読が作成され、可視化が準備されます。

```cs
protected override void OnStarted2(DateTime time)
{
	// 開始時にカウンターをリセット
	_bullLength = 0;
	_bearLength = 0;

	// ローソク足購読を作成
	var subscription = SubscribeCandles(CandleDataType);

	subscription
		.Bind(ProcessCandle)
		.Start();

	// チャート上の可視化を設定
	var area = CreateChartArea();
	if (area != null)
	{
		DrawCandles(area, subscription);
		DrawOwnTrades(area);
	}

	base.OnStarted2(time);
}
```

## ローソク足の処理

`ProcessCandle` メソッドは、確定済みローソク足ごとに呼び出され、トレンド検出とクォーティングプロセッサ管理のロジックを実装します。

```cs
private void ProcessCandle(ICandleMessage candle)
{
	if (candle.State != CandleStates.Finished)
		return;

	// 陽線または陰線を識別
	if (candle.OpenPrice < candle.ClosePrice)
	{
		_bullLength++;
		_bearLength = 0;

		this.AddInfoLog($"陽線を検出しました。連続数: {_bullLength}");
	}
	else if (candle.OpenPrice > candle.ClosePrice)
	{
		_bullLength = 0;
		_bearLength++;

		this.AddInfoLog($"陰線を検出しました。連続数: {_bearLength}");
	}

	// 方向変更が必要な場合は既存プロセッサを停止
	if (_quotingProcessor != null)
	{
		// プロセッサのクリアが必要か確認（トレンド変化またはポジション）
		var shouldClearProcessor = false;

		// 上昇トレンドでショートポジションがない場合は売りが必要
		if (_bullLength >= Length && Position >= 0)
			shouldClearProcessor = true;
		// 下降トレンドでロングポジションがない場合は買いが必要
		else if (_bearLength >= Length && Position <= 0)
			shouldClearProcessor = true;

		if (shouldClearProcessor)
		{
			_quotingProcessor?.Dispose();
			_quotingProcessor = null;
		}
	}

	// 必要に応じて新しいクォーティングプロセッサを作成
	if (_quotingProcessor == null && IsFormedAndOnlineAndAllowTrading())
	{
		if (_bullLength >= Length && Position >= 0)
		{
			// 上昇トレンド - ショートポジションを建てる
			CreateQuotingProcessor(Sides.Sell);
			this.AddInfoLog($"{_bullLength} 本の陽線後に売りクォーティングを開始します");
		}
		else if (_bearLength >= Length && Position <= 0)
		{
			// 下降トレンド - ロングポジションを建てる
			CreateQuotingProcessor(Sides.Buy);
			this.AddInfoLog($"{_bearLength} 本の陰線後に買いクォーティングを開始します");
		}
	}
}
```

## クォーティングプロセッサの作成

`CreateQuotingProcessor` メソッドは、指定された方向のクォーティングプロセッサを作成します。

```cs
private void CreateQuotingProcessor(Sides side)
{
	// マーケットクォーティング用の動作を作成
	var behavior = new MarketQuotingBehavior(
		0, // 価格オフセットなし
		new Unit(0.1m, UnitTypes.Percent), // 最小偏差として 0.1% を使用
		MarketPriceTypes.Following // 市場価格に追随
	);

	// クォーティングプロセッサを作成
	_quotingProcessor = new(
		behavior,
		Security,
		Portfolio,
		side,
		Volume, // クォーティング出来高
		Volume, // 最大注文出来高
		TimeSpan.Zero, // タイムアウトなし
		this, // Strategy は ISubscriptionProvider を実装
		this, // Strategy は IMarketRuleContainer を実装
		this, // Strategy は ITransactionProvider を実装
		this, // Strategy は ITimeProvider を実装
		this, // Strategy は IMarketDataProvider を実装
		IsFormedAndOnlineAndAllowTrading, // 取引許可を確認
		true, // 板価格を使用
		true // 板が空の場合は直近約定価格を使用
	)
	{
		Parent = this
	};

	// プロセッサイベントを購読
	_quotingProcessor.OrderRegistered += order =>
		this.AddInfoLog($"注文 {order.TransactionId} を価格 {order.Price} で登録しました");

	_quotingProcessor.OrderFailed += fail =>
		this.AddInfoLog($"注文に失敗しました: {fail.Error.Message}");

	_quotingProcessor.OwnTrade += trade =>
		this.AddInfoLog($"約定が実行されました: {trade.Trade.Volume}、価格 {trade.Trade.Price}");

	_quotingProcessor.Finished += isOk =>
	{
		_quotingProcessor?.Dispose();
		_quotingProcessor = null;
	};

	// プロセッサを初期化
	_quotingProcessor.Start();
}
```

## 取引ロジック

- **売りシグナル**: `Length` 本の連続した陽線（終値が始値より上）で、ショートポジションがない場合
- **買いシグナル**: `Length` 本の連続した陰線（終値が始値より下）で、ロングポジションがない場合
- クォーティングプロセッサは、市場価格に追随しながら市場エントリーに使用されます

## 機能

- 戦略は `GetWorkingSecurities()` メソッドによって、使用する銘柄を自動的に判定します
- 戦略は確定済みローソク足でのみ動作します
- より効率的な市場エントリーのため、成行注文の代わりにクォーティングを使用します
- 戦略は逆張りアプローチを適用し、確立されたトレンドに逆らってポジションを建てます
- デバッグのため、主要イベントの詳細なログ出力が実装されています
- トレンド方向が変化した場合、または目標に到達した場合、クォーティングプロセッサは自動的にクリアされます
- チャート上でのローソク足と約定の可視化がサポートされています
- 戦略設定のため、連続長パラメーターの最適化が実装されています
