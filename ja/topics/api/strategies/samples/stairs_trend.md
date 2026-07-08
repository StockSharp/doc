# Stairs トレンド戦略

## 概要

`StairsTrendStrategy` は、連続するローソク足の分析に基づいてトレンドを判定する取引戦略です。この戦略は、特定の長さで持続的なトレンドが形成されたときにポジションを建てます。

## 主要コンポーネント

```cs
public class StairsTrendStrategy : Strategy
{
	private readonly StrategyParam<int> _lengthParam;
	private readonly StrategyParam<DataType> _candleType;
	
	private int _bullLength;
	private int _bearLength;
}
```

## 戦略パラメーター

この戦略では、次のパラメーターをカスタマイズできます。

- **Length** - トレンドを識別するための同一方向の連続ローソク足数（既定値は 3）
- **CandleType** - 使用するローソク足タイプ（既定値は 5 分）

Length パラメーターは、2 から 10 まで、ステップ 1 の範囲で最適化に使用できます。

## 戦略の初期化

[OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) メソッドで、カウンターがリセットされ、ローソク足購読が作成され、可視化が準備されます。

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);
	
	// カウンターをリセット
	_bullLength = 0;
	_bearLength = 0;

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

	// ローソク足の方向に基づいてカウンターを更新
	if (candle.OpenPrice < candle.ClosePrice)
	{
		// 陽線
		_bullLength++;
		_bearLength = 0;
	}
	else if (candle.OpenPrice > candle.ClosePrice)
	{
		// 陰線
		_bullLength = 0;
		_bearLength++;
	}

	// トレンド戦略:
	// Length 本の連続した陽線の後に買う
	if (_bullLength >= Length && Position <= 0)
	{
		BuyMarket(Volume + Math.Abs(Position));
	}
	// Length 本の連続した陰線の後に売る
	else if (_bearLength >= Length && Position >= 0)
	{
		SellMarket(Volume + Math.Abs(Position));
	}
}
```

## 取引ロジック

- **買いシグナル**: `Length` 本の連続した陽線（終値が始値より上）で、ロングポジションがない場合
- **売りシグナル**: `Length` 本の連続した陰線（終値が始値より下）で、ショートポジションがない場合
- ポジション出来高は、新しい取引ごとに現在のポジション量だけ増加します

## 機能

- 戦略は `GetWorkingSecurities()` メソッドによって、使用する銘柄を自動的に判定します
- 戦略は確定済みローソク足でのみ動作します
- 戦略はポジションエントリーに成行注文を使用します
- 戦略は一連のローソク足に基づくシンプルなトレンド検出ロジックを適用します
- 反対方向のローソク足が現れると、ローソク足カウンターはリセットされます
- グラフィック領域が利用可能な場合、ローソク足と約定がチャート上に可視化されます
- 最適な戦略設定を見つけるため、連続長の最適化がサポートされています
