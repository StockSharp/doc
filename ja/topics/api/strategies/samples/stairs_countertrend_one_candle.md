# 1 本ローソク足逆張り戦略

## 概要

`OneCandleCountertrendStrategy` は、1 本のローソク足の分析に基づいて判断を行うシンプルな逆張り戦略です。

## 主要コンポーネント

```cs
public class OneCandleCountertrendStrategy : Strategy
{
	private readonly StrategyParam<DataType> _candleType;
}
```

## 戦略パラメーター

この戦略では、次のパラメーターをカスタマイズできます。

- **ローソク足タイプ** - 使用するローソク足タイプ（既定値は 5 分）

## 戦略の初期化

[OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) メソッドで、ローソク足購読が作成され、可視化が準備されます。

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// 購読を作成
	var subscription = SubscribeCandles(CandleType);
	
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
}
```

## ローソク足の処理

`ProcessCandle` メソッドは、確定済みローソク足ごとに呼び出され、取引ロジックを実装します。

```cs
private void ProcessCandle(ICandleMessage candle)
{
	// ローソク足が確定しているか確認
	if (candle.State != CandleStates.Finished)
		return;

	// 戦略が取引可能な状態か確認
	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	// 逆張り戦略: 陰線で買い、陽線で売り
	if (candle.OpenPrice < candle.ClosePrice && Position >= 0)
	{
		// 陽線 - 売り
		SellMarket(Volume + Math.Abs(Position));
	}
	else if (candle.OpenPrice > candle.ClosePrice && Position <= 0)
	{
		// 陰線 - 買い
		BuyMarket(Volume + Math.Abs(Position));
	}
}
```

## 取引ロジック

- **売りシグナル**: 陽線（終値が始値より上）で、ショートポジションがない場合
- **買いシグナル**: 陰線（終値が始値より下）で、ロングポジションがない場合
- ポジション出来高は、新しい取引ごとに現在のポジション量だけ増加します

## 機能

- 戦略は `GetWorkingSecurities()` メソッドによって、使用する銘柄を自動的に判定します
- 戦略は確定済みローソク足でのみ動作します
- 戦略はポジションエントリーに成行注文を使用します
- 戦略は 1 本のローソク足に基づくシンプルな逆張り検出ロジックを適用します
- グラフィック領域が利用可能な場合、ローソク足と約定がチャート上に可視化されます
