# マルチタイムフレーム戦略

## 概要

`MultiTimeframeStrategy` は、取引判断に 2 つのタイムフレームを使用する戦略です。時間足ローソク足は移動平均のクロスオーバーによってトレンド方向を判定し、5 分足ローソク足と [RelativeStrengthIndex](xref:StockSharp.Algo.Indicators.RelativeStrengthIndex) インジケーターは、トレンド方向への精密なエントリーに使用されます。

## 主要コンポーネント

この戦略は [Strategy](xref:StockSharp.Algo.Strategies.Strategy) を継承し、設定にパラメーターを使用します。

```cs
public class MultiTimeframeStrategy : Strategy
{
	private readonly StrategyParam<int> _fastSmaLength;
	private readonly StrategyParam<int> _slowSmaLength;
	private readonly StrategyParam<int> _rsiLength;
	private readonly StrategyParam<decimal> _takeProfit;
	private readonly StrategyParam<decimal> _stopLoss;

	// 上位タイムフレームのトレンド方向
	private Sides? _hourlyTrend;
}
```

## 戦略パラメーター

この戦略では、次のパラメーターをカスタマイズできます。

- **FastSmaLength** - 時間足チャートの高速移動平均期間（既定値は 10）
- **SlowSmaLength** - 時間足チャートの低速移動平均期間（既定値は 30）
- **RsiLength** - 5 分足チャートの RSI 期間（既定値は 14）
- **TakeProfit** - テイクプロフィット幅（パーセント、既定値は 2）
- **StopLoss** - ストップロス幅（パーセント、既定値は 1）

すべてのパラメーターは、指定された値範囲で最適化に使用できます。

## 戦略の初期化

[OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) メソッドで、インジケーターが作成され、2 つのタイムフレームのローソク足購読が設定されます。

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	var fastSma = new SimpleMovingAverage { Length = FastSmaLength };
	var slowSma = new SimpleMovingAverage { Length = SlowSmaLength };
	var rsi = new RelativeStrengthIndex { Length = RsiLength };

	_hourlyTrend = null;

	// トレンド検出用の時間足ローソク足（SMA クロスオーバー）
	SubscribeCandles(TimeSpan.FromHours(1))
		.Bind(fastSma, slowSma, ProcessHourlyCandle)
		.Start();

	// 精密なエントリー用の 5 分足ローソク足（RSI）
	SubscribeCandles(TimeSpan.FromMinutes(5))
		.Bind(rsi, ProcessEntryCandle)
		.Start();

	// ポジション保護（テイクプロフィットとストップロス）を設定
	StartProtection(
		new Unit(TakeProfit, UnitTypes.Percent),
		new Unit(StopLoss, UnitTypes.Percent)
	);

	// チャート上の可視化を設定
	var area = CreateChartArea();
	if (area != null)
	{
		DrawIndicator(area, fastSma, System.Drawing.Color.Blue);
		DrawIndicator(area, slowSma, System.Drawing.Color.Red);
		DrawOwnTrades(area);
	}
}
```

## 時間足ローソク足の処理

`ProcessHourlyCandle` メソッドは、上位タイムフレームのトレンド方向を判定します。

```cs
private void ProcessHourlyCandle(ICandleMessage candle, decimal fastValue, decimal slowValue)
{
	if (candle.State != CandleStates.Finished)
		return;

	// 移動平均クロスオーバーでトレンドを判定
	_hourlyTrend = fastValue > slowValue ? Sides.Buy : Sides.Sell;
}
```

## 5 分足ローソク足の処理

`ProcessEntryCandle` メソッドは、トレンド方向の RSI シグナルに基づくポジションエントリーを実装します。

```cs
private void ProcessEntryCandle(ICandleMessage candle, decimal rsiValue)
{
	if (candle.State != CandleStates.Finished)
		return;

	if (_hourlyTrend == null || !IsFormedAndOnlineAndAllowTrading())
		return;

	// 買い: 上昇トレンドかつ RSI が売られ過ぎゾーン
	if (_hourlyTrend == Sides.Buy && rsiValue < 30 && Position <= 0)
	{
		BuyMarket(Volume + Math.Abs(Position));
	}
	// 売り: 下降トレンドかつ RSI が買われ過ぎゾーン
	else if (_hourlyTrend == Sides.Sell && rsiValue > 70 && Position >= 0)
	{
		SellMarket(Volume + Math.Abs(Position));
	}
}
```

## 取引ロジック

- **トレンド検出**: 時間足チャートで高速 SMA が低速 SMA を上回る場合は上昇トレンド、下回る場合は下降トレンドを示します
- **買いシグナル**: 時間足チャートが上昇トレンドで、5 分足チャートの RSI < 30、かつロングポジションがない場合
- **売りシグナル**: 時間足チャートが下降トレンドで、5 分足チャートの RSI > 70、かつショートポジションがない場合
- **ポジション保護**: `StartProtection` による自動テイクプロフィットとストップロス

## 機能

- 戦略は 2 つのタイムフレームを使用します。トレンドには時間足、エントリーには 5 分足を使用します
- ポジションエントリーは、上位タイムフレームのトレンド方向にのみ実行されます
- RSI は、最適なエントリーポイント（売られ過ぎ/買われ過ぎ）を見つけるためのフィルターとして使用されます
- ポジションはストップロスとテイクプロフィットで自動的に保護されます
- 戦略は確定済みローソク足でのみ動作します
- グラフィック領域が利用可能な場合、インジケーターと約定がチャート上に可視化されます
- 最適な戦略設定を見つけるためのパラメーター最適化がサポートされています
