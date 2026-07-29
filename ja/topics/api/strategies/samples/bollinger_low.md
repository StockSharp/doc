# 下限バンドに焦点を当てた Bollinger ストラテジー

## 概要

`BollingerStrategyLowBandStrategy` は、[BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands) インジケーターに基づくストラテジーです。価格がボリンジャーバンドの下限に到達したときにショート ポジションを開き、価格が中央線に到達したときにそれをクローズします。

## 主なコンポーネント

このストラテジーは [Strategy](xref:StockSharp.Algo.Strategies.Strategy) から継承し、設定にパラメーターを使用します。

```cs
public class BollingerStrategyLowBandStrategy : Strategy
{
	private readonly StrategyParam<int> _bollingerLength;
	private readonly StrategyParam<decimal> _bollingerDeviation;
	private readonly StrategyParam<DataType> _candleType;

	private BollingerBands _bollingerBands;
}
```

## ストラテジー パラメーター

このストラテジーでは、次のパラメーターをカスタマイズできます。

- **BollingerLength** - ボリンジャーバンド インジケーターの期間 (既定値 20)
- **BollingerDeviation** - 標準偏差の乗数 (既定値 2.0)
- `CandleType` - 使用するローソク足の種類 (既定値 5 分足)

すべてのパラメーターは、指定された値範囲で最適化に使用できます。

## ストラテジーの初期化

[OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) メソッドでは、ボリンジャーバンド インジケーターが作成され、ローソク足サブスクリプションが設定され、可視化が準備されます。

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// インジケーターを作成する
	_bollingerBands = new BollingerBands
	{
		Length = BollingerLength,
		Width = BollingerDeviation
	};

	// サブスクリプションを作成し、インジケーターをバインドする
	var subscription = SubscribeCandles(CandleType);
	subscription
		.BindEx(_bollingerBands, ProcessCandle)
		.Start();

	// チャート上の可視化を設定する
	var area = CreateChartArea();
	if (area != null)
	{
		DrawCandles(area, subscription);
		DrawIndicator(area, _bollingerBands, System.Drawing.Color.Purple);
		DrawOwnTrades(area);
	}
}
```

## ローソク足の処理

`ProcessCandle` メソッドは、完了した各ローソク足に対して呼び出され、取引ロジックを実装します。

```cs
private void ProcessCandle(ICandleMessage candle, IIndicatorValue bollingerValue)
{
	// 未完了のローソク足をスキップする
	if (candle.State != CandleStates.Finished)
		return;

	// ストラテジーが取引可能な状態か確認する
	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	var typed = (BollingerBandsValue)bollingerValue;

	// 取引ロジック:
	// 価格が下限バンドに触れたときに売る (ポジションがない場合のみ)
	if (candle.ClosePrice <= typed.LowBand && Position == 0)
	{
		SellMarket(Volume);
	}
	// 価格が中央線に到達したときにポジションをクローズするために買う (ショート ポジションがある場合のみ)
	else if (candle.ClosePrice >= typed.MiddleBand && Position < 0)
	{
		BuyMarket(Math.Abs(Position));
	}
}
```

## 取引ロジック

- **売りシグナル**: オープン ポジションがないときに、ローソク足の終値がボリンジャーバンドの下限に到達するか、それを下回る
- **買いシグナル** (ショート ポジションのクローズ): ショート ポジションがあるときに、ローソク足の終値がボリンジャーバンドの中央線に到達するか、それを上回る
- ポジション数量は、オープン時は固定され、クローズ時は現在のポジション全体と等しくなります

## 特徴

- このストラテジーは `GetWorkingSecurities()` メソッドを通じて、動作対象の銘柄を自動的に判定します
- このストラテジーは完了したローソク足のみを使用します
- このストラテジーはボリンジャーバンド インジケーターの下限バンドと中央線のみを使用します
- ショート ポジションのみが開かれます
- グラフィック領域が利用可能な場合、インジケーターと取引はチャート上に可視化されます
- 最適なストラテジー設定を見つけるためのパラメーター最適化がサポートされています
