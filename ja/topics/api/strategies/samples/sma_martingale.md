# マーチンゲール付き移動平均

## 概要

`SmaStrategyMartingaleStrategy` は、2 つの単純移動平均（[SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage)）のクロスオーバーに基づき、マーチンゲール要素を含む取引戦略です。この戦略は、長期 SMA と短期 SMA を使用してエントリーおよびエグジットシグナルを判定し、新しい取引ごとにポジションサイズを増やします。

## 主要コンポーネント

```cs
public class SmaStrategyMartingaleStrategy : Strategy
{
	private readonly StrategyParam<int> _longSmaLength;
	private readonly StrategyParam<int> _shortSmaLength;
	private readonly StrategyParam<DataType> _candleType;

	// 前回のインジケーター値を保存する変数
	private decimal _prevLongValue;
	private decimal _prevShortValue;
	private bool _isFirstValue = true;
}
```

## 戦略パラメーター

この戦略では、次のパラメーターをカスタマイズできます。

- **LongSmaLength** - 長期移動平均期間（既定値は 80）
- **ShortSmaLength** - 短期移動平均期間（既定値は 30）
- **CandleType** - 使用するローソク足タイプ（既定値は 5 分）

すべてのパラメーターは、指定された値範囲で最適化に使用できます。

## 戦略の初期化

[OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) メソッドで、SMA インジケーターが作成され、ローソク足購読が設定され、可視化が準備されます。

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// インジケーターを作成
	var longSma = new SimpleMovingAverage { Length = LongSmaLength };
	var shortSma = new SimpleMovingAverage { Length = ShortSmaLength };

	// IsFormed の自動追跡のために、インジケーターを戦略コレクションに追加
	Indicators.Add(longSma);
	Indicators.Add(shortSma);

	// 購読を作成し、インジケーターをバインド
	var subscription = SubscribeCandles(CandleType);
	subscription
		.Bind(longSma, shortSma, ProcessCandle)
		.Start();

	// チャート上の可視化を設定
	var area = CreateChartArea();
	if (area != null)
	{
		DrawCandles(area, subscription);
		DrawIndicator(area, longSma, System.Drawing.Color.Blue);
		DrawIndicator(area, shortSma, System.Drawing.Color.Red);
		DrawOwnTrades(area);
	}
}
```

## ローソク足の処理

`ProcessCandle` メソッドは、確定済みローソク足ごとに呼び出され、取引ロジックを実装します。

```cs
private void ProcessCandle(ICandleMessage candle, decimal longValue, decimal shortValue)
{
	// 未確定ローソク足をスキップ
	if (candle.State != CandleStates.Finished)
		return;

	// 戦略が取引可能な状態か確認
	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	// 最初の値では、シグナルを生成せずデータのみ保存
	if (_isFirstValue)
	{
		_prevLongValue = longValue;
		_prevShortValue = shortValue;
		_isFirstValue = false;
		return;
	}

	// インジケーター値の現在と前回の比較を取得
	var isShortLessThenLongCurrent = shortValue < longValue;
	var isShortLessThenLongPrevious = _prevShortValue < _prevLongValue;

	// 次のローソク足のために現在値を前回値として保存
	_prevLongValue = longValue;
	_prevShortValue = shortValue;

	// クロスオーバー（シグナル）を確認
	if (isShortLessThenLongPrevious == isShortLessThenLongCurrent)
		return;

	// 新しい注文を出す前にアクティブ注文をキャンセル
	CancelActiveOrders();

	// 取引方向を判定
	var direction = isShortLessThenLongCurrent ? Sides.Sell : Sides.Buy;

	// ポジションサイズを計算（取引ごとにポジションを増やす - マーチンゲール方式）
	var volume = Volume + Math.Abs(Position);

	// 適切な価格で注文を作成して登録
	var price = Security.ShrinkPrice(shortValue);
	RegisterOrder(CreateOrder(direction, price, volume));
}
```

## 取引ロジック

- **買いシグナル**: 短期 SMA が長期 SMA を下から上へクロスする場合
- **売りシグナル**: 短期 SMA が長期 SMA を上から下へクロスする場合
- ポジションサイズは、新しい取引ごとに現在のポジション量だけ増加します（マーチンゲール要素）
- 注文価格は現在の短期 SMA 値に設定され、銘柄のティックサイズに丸められます

## 機能

- 戦略は `GetWorkingSecurities()` メソッドによって、使用する銘柄を自動的に判定します
- 戦略は確定済みローソク足でのみ動作します
- 戦略は SMA 間の現在および前回の関係を比較することで、インジケーターのクロスオーバーを追跡します
- 新しい注文を発注する前に、すべてのアクティブ注文がキャンセルされます
- マーチンゲール原則が実装されています。新しい取引ごとにポジションサイズを増やします
- グラフィック領域が利用可能な場合、インジケーターと約定がチャート上に可視化されます
- 最適な戦略設定を見つけるためのパラメーター最適化がサポートされています
