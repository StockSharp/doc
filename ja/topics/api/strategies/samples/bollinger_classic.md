# クラシック Bollinger ストラテジー

## 概要

`BollingerStrategyClassicStrategy` は、[BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands) インジケーターに基づくストラテジーです。価格が Bollinger Bands の上限または下限に到達したときにポジションを開きます。

## 主なコンポーネント

このストラテジーは [Strategy](xref:StockSharp.Algo.Strategies.Strategy) から継承し、設定にパラメーターを使用します。

```cs
public class BollingerStrategyClassicStrategy : Strategy
{
	private readonly StrategyParam<int> _bollingerLength;
	private readonly StrategyParam<decimal> _bollingerDeviation;
	private readonly StrategyParam<DataType> _candleType;

	private BollingerBands _bollingerBands;
}
```

## ストラテジー パラメーター

このストラテジーでは、次のパラメーターをカスタマイズできます。

- **BollingerLength** - Bollinger Bands インジケーターの期間 (既定値 20)
- **BollingerDeviation** - 標準偏差の乗数 (既定値 2.0)
- **CandleType** - 使用するローソク足の種類 (既定値 5 分足)

すべてのパラメーターは、指定された値範囲で最適化に使用できます。

## ストラテジーの初期化

[OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) メソッドでは、Bollinger Bands インジケーターが作成され、ローソク足サブスクリプションが設定され、可視化が準備されます。

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
	// 価格が上限バンドに到達するか、それを上回ったときに売る
	if (candle.ClosePrice >= typed.UpBand && Position >= 0)
	{
		SellMarket(Volume + Math.Abs(Position));
	}
	// 価格が下限バンドに到達するか、それを下回ったときに買う
	else if (candle.ClosePrice <= typed.LowBand && Position <= 0)
	{
		BuyMarket(Volume + Math.Abs(Position));
	}
}
```

## 取引ロジック

- **売りシグナル**: ショート ポジションがないときに、ローソク足の終値が Bollinger Band の上限に到達するか、それを上回る
- **買いシグナル**: ロング ポジションがないときに、ローソク足の終値が Bollinger Band の下限に到達するか、それを下回る
- ポジション数量は、新しい取引ごとに現在のポジション量だけ増加します

## 特徴

- このストラテジーは `GetWorkingSecurities()` メソッドを通じて、動作対象の銘柄を自動的に判定します
- このストラテジーは完了したローソク足のみを使用します
- グラフィック領域が利用可能な場合、インジケーターと取引はチャート上に可視化されます
- 最適なストラテジー設定を見つけるためのパラメーター最適化がサポートされています
