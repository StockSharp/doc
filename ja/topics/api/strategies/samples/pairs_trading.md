# ペアトレーディング戦略

## 概要

`PairsTradingStrategy` は、関連する 2 つの銘柄間の統計的裁定に基づくペアトレーディング戦略です。2 つの資産価格のスプレッドを追跡し、スプレッドが平均から大きく乖離したときに、平均回帰を期待してポジションを建てます。

## 主要コンポーネント

この戦略は [Strategy](xref:StockSharp.Algo.Strategies.Strategy) を継承し、設定にパラメーターを使用します。

```cs
public class PairsTradingStrategy : Strategy
{
	private readonly StrategyParam<int> _spreadLength;
	private readonly StrategyParam<decimal> _entryThreshold;
	private readonly StrategyParam<decimal> _exitThreshold;
	private readonly StrategyParam<DataType> _candleType;

	// 各銘柄の最新価格
	private decimal? _lastPrice1;
	private decimal? _lastPrice2;
}
```

## 戦略パラメーター

この戦略では、次のパラメーターをカスタマイズできます。

- **SpreadLength** - スプレッドの平均と標準偏差を計算する期間（既定値は 20）
- **EntryThreshold** - ポジションにエントリーするための Z-Score しきい値（既定値は 2.0）
- **ExitThreshold** - ポジションを決済するための Z-Score しきい値（既定値は 0.5）
- **CandleType** - 使用するローソク足タイプ（既定値は 5 分）

すべてのパラメーターは、指定された値範囲で最適化に使用できます。

## 戦略の初期化

[OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) メソッドで、インジケーターが作成され、2 つの銘柄のローソク足購読が設定されます。

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// ペアトレーディング用に 2 つの銘柄を取得
	var securities = GetWorkingSecurities().ToArray();
	if (securities.Length < 2)
		throw new InvalidOperationException("2つの銘柄を指定する必要があります。");

	var sec1 = securities[0].sec;
	var sec2 = securities[1].sec;

	// スプレッドの平均と標準偏差を計算するインジケーター
	var sma = new SimpleMovingAverage { Length = SpreadLength };
	var stdDev = new StandardDeviation { Length = SpreadLength };

	_lastPrice1 = null;
	_lastPrice2 = null;

	// 1 つ目の銘柄のローソク足を購読
	SubscribeCandles(CandleType, security: sec1)
		.Bind(c =>
		{
			if (c.State != CandleStates.Finished)
				return;

			_lastPrice1 = c.ClosePrice;
		})
		.Start();

	// スプレッド処理を含めて 2 つ目の銘柄のローソク足を購読
	SubscribeCandles(CandleType, security: sec2)
		.Bind(c =>
		{
			if (c.State != CandleStates.Finished)
				return;

			_lastPrice2 = c.ClosePrice;

			if (_lastPrice1 == null)
				return;

			ProcessSpread(_lastPrice1.Value, _lastPrice2.Value, sma, stdDev);
		})
		.Start();
}
```

## スプレッド処理

`ProcessSpread` メソッドは、スプレッドの Z-Score を計算し、取引シグナルを生成します。

```cs
private void ProcessSpread(decimal price1, decimal price2,
	SimpleMovingAverage sma, StandardDeviation stdDev)
{
	// 価格差としてスプレッドを計算
	var spread = price1 - price2;

	// インジケーターを処理
	var smaValue = sma.Process(new DecimalIndicatorValue(sma, spread));
	var devValue = stdDev.Process(new DecimalIndicatorValue(stdDev, spread));

	if (!sma.IsFormed || !stdDev.IsFormed)
		return;

	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	var mean = smaValue.ToDecimal();
	var dev = devValue.ToDecimal();

	if (dev == 0)
		return;

	// Z-Score を計算: 平均からのスプレッド乖離を標準偏差単位で表す
	var zScore = (spread - mean) / dev;

	// スプレッドが高すぎる: 1 つ目の銘柄を売り、2 つ目を買う
	if (zScore > EntryThreshold && Position >= 0)
	{
		SellMarket(Volume + Math.Abs(Position));
	}
	// スプレッドが低すぎる: 1 つ目の銘柄を買い、2 つ目を売る
	else if (zScore < -EntryThreshold && Position <= 0)
	{
		BuyMarket(Volume + Math.Abs(Position));
	}
	// 平均への回帰: ポジションを決済
	else if (Math.Abs(zScore) < ExitThreshold && Position != 0)
	{
		ClosePosition();
	}
}
```

## 取引ロジック

- **売りシグナル**: スプレッドの Z-Score がエントリーしきい値（既定値 2.0）を上回り、ショートポジションがない場合
- **買いシグナル**: スプレッドの Z-Score が負のエントリーしきい値（既定値 -2.0）を下回り、ロングポジションがない場合
- **ポジション決済**: Z-Score の絶対値が決済しきい値（既定値 0.5）を下回り、スプレッドが平均へ回帰していることを示す場合

## 機能

- 戦略は `GetWorkingSecurities()` メソッドで取得した 2 つの銘柄を使用します
- スプレッドは、2 つの銘柄のローソク足終値の差として計算されます
- Z-Score は、平均からのスプレッド乖離を正規化するために使用されます
- 戦略は古典的な平均回帰の考え方を実装します
- 戦略は確定済みローソク足でのみ動作します
- 最適な戦略設定を見つけるためのパラメーター最適化がサポートされています
