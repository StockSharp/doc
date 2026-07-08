# ストラテジーにおける高レベル API

StockSharp は、取引ストラテジーで一般的なタスクを扱いやすくするための高レベル API セットを提供します。これらのインターフェイスにより、技術的な詳細ではなく取引ロジックに集中した、より簡潔なコードを記述できます。

## 簡略化されたサブスクリプション管理

サブスクリプションを扱う高レベルメソッドは、サブスクリプションのライフサイクル管理とデータ処理の複雑さを隠蔽します。

### SubscribeCandles メソッド

サブスクリプションを手動で作成してイベントハンドラーを設定する代わりに、[SubscribeCandles](xref:StockSharp.Algo.Strategies.Strategy.SubscribeCandles(System.TimeSpan,System.Boolean,StockSharp.BusinessEntities.Security)) メソッドを使用できます。

```cs
// ローソク足サブスクリプションを 1 行で作成して設定
var subscription = SubscribeCandles(CandleType);
```

このメソッドは [ISubscriptionHandler\<ICandleMessage\>](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1) 型のオブジェクトを返し、以後のサブスクリプション設定に便利なインターフェイスを提供します。

### インジケーターとサブスクリプションの自動バインド

高レベル API により、インジケーターをデータサブスクリプションに簡単にバインドできます。

```cs
var longSma = new SMA { Length = Long };
var shortSma = new SMA { Length = Short };

subscription
	// インジケーターをローソク足サブスクリプションにバインド
	.Bind(longSma, shortSma, OnProcess)
	// 処理を開始
	.Start();
```

#### Strategy.Indicators コレクションへのインジケーターの自動追加

インジケーターをサブスクリプションにリンクするために [Bind](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.Bind(StockSharp.Algo.Indicators.IIndicator,StockSharp.Algo.Indicators.IIndicator,System.Action{`0,System.Decimal,System.Decimal})) メソッドを使用する場合、これらのインジケーターを追加で [Strategy.Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) コレクションへ追加する**必要はありません**。従来のアプローチ（[インジケーターのドキュメント](indicators.md)で説明）では通常この追加を行いますが、システムは自動的に次を行います。

1. インジケーターを [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) コレクションに追加します
2. インジケーターの形成状態を追跡します
3. ストラテジーの [IsFormed](xref:StockSharp.Algo.Strategies.Strategy.IsFormed) 状態を更新します

これによりコードが大幅に簡略化され、エラーの可能性が低下します。

一部のインジケーターにまだデータがない場合（`IIndicatorValue.IsEmpty` が `true`）でもインジケーター値を受け取りたい場合は、`BindWithEmpty` メソッドを使用します。この場合、ハンドラー引数の型は `decimal?` である必要があります。また、`BindEx` を使用して生の `IIndicatorValue` オブジェクトを直接確認することもできます。

#### BindEx を使用した生のインジケーター値の処理

インジケーターが標準的ではない値（単なる数値以外）を返す場合、[BindEx](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.BindEx(StockSharp.Algo.Indicators.IIndicator,System.Action{`0,StockSharp.Algo.Indicators.IIndicatorValue},System.Boolean)) メソッドを使用できます。このメソッドでは、元の [IIndicatorValue](xref:StockSharp.Algo.Indicators.IIndicatorValue) オブジェクトにアクセスできます。

```cs
subscription
	.BindEx(indicator, OnProcessWithRawValue)
	.Start();

// ハンドラーは元の IIndicatorValue を受け取る
private void OnProcessWithRawValue(ICandleMessage candle, IIndicatorValue value)
{
	// IIndicatorValue プロパティへのアクセス
	if (value.IsFinal)
	{
		// ブール値を返すインジケーターの場合
		var boolValue = value.GetValue<bool>();
		
		// または特定のインジケーターに固有の他のデータ型
		// ...
	}
}
```

[BindEx](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.BindEx(StockSharp.Algo.Indicators.IIndicator,System.Action{`0,StockSharp.Algo.Indicators.IIndicatorValue},System.Boolean)) メソッドは、特に次のケースで有用です。

- ブール値を返すインジケーター（例: [Fractals](xref:StockSharp.Algo.Indicators.Fractals)）を扱う場合
- インジケーター値型の追加プロパティ（例: [IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal) フラグ）へアクセスする場合
- 構造化データを返すインジケーターを扱う場合

#### 複合インジケーター（IComplexIndicator）の処理

複数の内部インジケーターを含む複合インジケーター（例: [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands)、[MACD](xref:StockSharp.Algo.Indicators.MovingAverageConvergenceDivergence)）の場合、API は `Bind` メソッドと `BindEx` メソッドの特別なオーバーロードを提供します。

```cs
// 複合インジケーターを作成
var bollinger = new BollingerBands 
{ 
	Length = 20, 
	Deviation = 2 
};

// 複合インジケーターをサブスクリプションにバインド
subscription
	.BindEx(bollinger, OnProcessBollinger)
	.Start();

// ハンドラーは BollingerBandsValue インスタンスを受け取る
private void OnProcessBollinger(ICandleMessage candle, IIndicatorValue value)
{
	var typed = (BollingerBandsValue)value;

	// Bollinger band 値を使用
	if (candle.ClosePrice >= typed.UpBand && Position >= 0)
		SellMarket(Volume + Math.Abs(Position));
	else if (candle.ClosePrice <= typed.LowBand && Position <= 0)
		BuyMarket(Volume + Math.Abs(Position));
}
```

より柔軟に処理するには、複合インジケーター値へ直接アクセスできる [BindEx](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.BindEx(StockSharp.Algo.Indicators.IIndicator,System.Action{`0,StockSharp.Algo.Indicators.IIndicatorValue},System.Boolean)) を使用できます。

```cs
subscription.BindEx(bollinger, (candle, indicatorValue) =>
{
	var typed = (BollingerBandsValue)indicatorValue;

	if (candle.ClosePrice >= typed.UpBand && Position >= 0)
		SellMarket(Volume + Math.Abs(Position));
	else if (candle.ClosePrice <= typed.LowBand && Position <= 0)
		BuyMarket(Volume + Math.Abs(Position));
});
```

複合インジケーター向けの [BindEx](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.BindEx(StockSharp.Algo.Indicators.IIndicator,System.Action{`0,StockSharp.Algo.Indicators.IIndicatorValue},System.Boolean)) メソッドは、自動的に次を行います。

1. 入力データを複合インジケーターで処理します
2. 結果の `IIndicatorValue` を指定されたハンドラーへ渡します

個別のフィールドを扱うには、値をそのインジケーター専用の**値型**へキャストします。

### `Bind` メソッドは、サブスクリプションデータとインジケーターの間の接続を確立します。新しいローソク足を受信すると:

1. ローソク足は自動的にインジケーターへ処理のために送信されます
2. 処理結果が指定されたハンドラー（例では `OnProcess` メソッド）へ渡されます
3. すべての同期処理と状態管理コードは開発者から隠蔽されます

ハンドラーは、すぐに使用できる値を単純な `decimal` 型として受け取ります。このメソッドは、バインドされたすべてのインジケーターがデータを返した場合にのみ呼び出されます。

```cs
private void OnProcess(ICandleMessage candle, decimal longValue, decimal shortValue)
{
	// 準備済みのインジケーター値を直接扱う
	var isShortLessThenLong = shortValue < longValue;
	
	// 取引ロジックはクリーンな数値を使用する
	// IIndicatorValue から値を抽出する必要はない
	// ...
}
```

これによりコードが大幅に簡略化され、開発者が次を行う必要がなくなるため、可読性も向上します。
- ローソク足受信イベントを手動で処理する
- データをインジケーターへ手動で渡す
- インジケーター結果から値を抽出する

## 簡略化されたチャート管理

### 自動可視化

高レベル API は、サブスクリプションとインジケーターをチャート要素へバインドするためのシンプルなメソッドを提供します。

```cs
var area = CreateChartArea();

// GUI なしで実行している場合、area は null になる可能性がある
if (area != null)
{
	// ローソク足をチャートエリアへ自動バインド
	DrawCandles(area, subscription);

	// 色をカスタマイズしてインジケーターを描画
	DrawIndicator(area, shortSma, System.Drawing.Color.Coral);
	DrawIndicator(area, longSma);
	
	// 自己取引を描画
	DrawOwnTrades(area);
	
	// 注文を描画
	DrawOrders(area);
}
```

#### DrawCandles メソッド

[DrawCandles](xref:StockSharp.Algo.Strategies.Strategy.DrawCandles(StockSharp.Charting.IChartArea,StockSharp.BusinessEntities.Subscription)) メソッドは、ローソク足サブスクリプションをチャートのローソク足表示要素へ自動的にリンクします。

```cs
// ローソク足を表示するチャート要素を作成
IChartCandleElement candles = DrawCandles(area, subscription);

// 追加の要素パラメーターを設定可能
candles.DrawOpenClose = true;  // 始値/終値ラインを表示
candles.DrawHigh = true;       // 高値を表示
candles.DrawLow = true;        // 安値を表示
```

このメソッドは、さらにカスタマイズ可能な [IChartCandleElement](xref:StockSharp.Charting.IChartCandleElement) チャート要素を返します。

#### DrawIndicator メソッド

[DrawIndicator](xref:StockSharp.Algo.Strategies.Strategy.DrawIndicator(StockSharp.Charting.IChartArea,StockSharp.Algo.Indicators.IIndicator,System.Nullable{System.Drawing.Color},System.Nullable{System.Drawing.Color})) メソッドは、インジケーター値を表示するためのチャート要素を作成して設定します。

```cs
// デフォルト色でインジケーターをチャートへ簡単に追加
IChartIndicatorElement smaElem = DrawIndicator(area, sma);

// 指定した主要色でインジケーターを追加
IChartIndicatorElement rsiFast = DrawIndicator(area, rsi, System.Drawing.Color.Red);

// 指定した主要色と副次色でインジケーターを追加
IChartIndicatorElement bollingerElem = DrawIndicator(
	area, 
	bollinger, 
	System.Drawing.Color.Blue,    // 主要色
	System.Drawing.Color.Gray     // 副次色（2 本目のライン用）
);

// 追加の要素設定
smaElem.DrawStyle = DrawStyles.Line;           // 描画スタイル: ライン
rsiFast.DrawStyle = DrawStyles.Dot;            // 描画スタイル: ドット
bollingerElem.DrawStyle = DrawStyles.Dashdot;  // 描画スタイル: ダッシュドット
```

このメソッドは、カスタマイズ可能な [IChartIndicatorElement](xref:StockSharp.Charting.IChartIndicatorElement) チャート要素を返します。複数の値を持つインジケーター（例: [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands)）では、主要色が最初の値に、副次色が 2 番目の値に適用されます。

#### DrawOwnTrades メソッド

[DrawOwnTrades](xref:StockSharp.Algo.Strategies.Strategy.DrawOwnTrades(StockSharp.Charting.IChartArea)) メソッドは、ストラテジー自身の取引をチャートに表示する要素を作成します。

```cs
// 取引を表示する要素を作成
IChartTradeElement trades = DrawOwnTrades(area);

// 要素設定
trades.BuyColor = System.Drawing.Color.Green;   // 買い取引用の色
trades.SellColor = System.Drawing.Color.Red;    // 売り取引用の色
trades.FullTitle = "My Strategy Trades";        // 要素タイトル
```

このメソッドは、ストラテジーによって実行されたすべての取引の表示を自動的に設定します。取引は、売買方向（buy/sell）を考慮し、約定した位置にマーカーとしてチャート上に表示されます。

#### DrawOrders メソッド

[DrawOrders](xref:StockSharp.Algo.Strategies.Strategy.DrawOrders(StockSharp.Charting.IChartArea)) メソッドは、チャート上に注文を表示する要素を作成します。

```cs
// 注文を表示する要素を作成
IChartOrderElement orders = DrawOrders(area);

// 要素設定
orders.BuyPendingColor = System.Drawing.Color.DarkGreen;   // アクティブな買い注文の色
orders.SellPendingColor = System.Drawing.Color.DarkRed;    // アクティブな売り注文の色
orders.BuyColor = System.Drawing.Color.Green;              // 約定済み買い注文の色
orders.SellColor = System.Drawing.Color.Red;               // 約定済み売り注文の色
orders.CancelColor = System.Drawing.Color.Gray;            // キャンセル済み注文の色
```

このメソッドは、ストラテジーによって発注されたすべての注文の表示を自動的に設定します。注文は、異なる注文状態ごとに色分けされ、価格水準にマーカーとして表示されます。

#### CreateChartArea メソッド

[CreateChartArea](xref:StockSharp.Algo.Strategies.Strategy.CreateChartArea) メソッドは、ストラテジーチャート上に新しいエリアを作成します。

```cs
// ローソク足とインジケーター用の最初のエリアを作成
var mainArea = CreateChartArea();
DrawCandles(mainArea, subscription);
DrawIndicator(mainArea, sma);

// 個別インジケーター（例: RSI）用の 2 番目のエリアを作成
var secondArea = CreateChartArea();
DrawIndicator(secondArea, rsi);
```

チャートをエリアに分割することで、異なるデータ型をより視覚的に表示できます。たとえば、価格とは異なる値範囲を持つインジケーター（RSI、stochastic など）は、別エリアに表示する方が適しています。

高レベル可視化メソッドの利点:
- `ChartDrawData` オブジェクトを手動で作成する必要がありません
- 時刻ごとのデータグループ化を管理する必要がありません
- チャートを更新するために `chart.Draw()` を呼び出す必要がありません
- サブスクリプションとチャート要素の間でデータが自動同期されます
- グラフィカル要素の外観管理が簡略化されます

新しいデータを受信すると、システムはチャートを自動的に更新するため、開発者は可視化の技術的な詳細に集中せずに済みます。

## ポジション保護

### StartProtection メソッド

オープンポジションを保護するため、StockSharp は高レベルの [StartProtection](xref:StockSharp.Algo.Strategies.Strategy.StartProtection(StockSharp.Messages.Unit,StockSharp.Messages.Unit,System.Boolean,System.Nullable{System.TimeSpan},System.Nullable{System.TimeSpan},System.Boolean)) メソッドを提供します。

```cs
// Take Profit と Stop Loss レベルでポジション保護を開始
StartProtection(TakeValue, StopValue);
```

このメソッドは、すべてのオープンポジションに対する保護を自動的に設定します。
- 価格変化を追跡します
- Take Profit または Stop Loss レベルに達したとき、ポジションをクローズする注文を自動作成します
- さまざまな種類の測定単位（絶対値、パーセンテージ、ポイント）をサポートします
- 適応的なポジション保護のためにトレーリングストップを使用できます

追加パラメーターを指定した例:

```cs
// トレーリングストップと成行注文で保護を開始
StartProtection(
	takeProfit: new Unit(50, UnitTypes.Absolute), // Take Profit
	stopLoss: new Unit(2, UnitTypes.Percent),     // パーセンテージでの Stop Loss
	isStopTrailing: true,                         // トレーリングストップを有効化
	useMarketOrders: true                         // 成行注文を使用
);
```

## 高レベル API の利点

StockSharp ストラテジーの高レベル API には、次の利点があります。

1. **コード量の削減** - 一般的なタスクを実行するために必要なコード行数が少なくなります

2. **責務の分離** - 取引ロジックが、データ処理や可視化の技術的詳細から分離されます

3. **可読性の向上** - コードがより理解しやすく表現力のあるものになり、ビジネスロジックに集中できます

4. **エラー発生確率の低減** - 定型作業の自動化により、多くの典型的なエラーが排除されます

5. **クリーンなデータ型での処理** - 複雑なオブジェクトを扱う代わりに、単純なデータ型（例: `decimal`）で操作できます

## 高レベル API を使用したストラテジー例

以下は、高レベル API の使用を示す完全なストラテジー例です。

```cs
public class SmaStrategy : Strategy
{
	private bool? _isShortLessThenLong;

	public SmaStrategy()
	{
		_candleType = Param(nameof(CandleType), DataType.TimeFrame(TimeSpan.FromMinutes(1)));
		_long = Param(nameof(Long), 80);
		_short = Param(nameof(Short), 30);
		_takeValue = Param(nameof(TakeValue), new Unit(50, UnitTypes.Absolute));
		_stopValue = Param(nameof(StopValue), new Unit(2, UnitTypes.Percent));
	}

	private readonly StrategyParam<DataType> _candleType;
	public DataType CandleType
	{
		get => _candleType.Value;
		set => _candleType.Value = value;
	}

	private readonly StrategyParam<int> _long;
	public int Long
	{
		get => _long.Value;
		set => _long.Value = value;
	}

	private readonly StrategyParam<int> _short;
	public int Short
	{
		get => _short.Value;
		set => _short.Value = value;
	}

	private readonly StrategyParam<Unit> _takeValue;
	public Unit TakeValue
	{
		get => _takeValue.Value;
		set => _takeValue.Value = value;
	}

	private readonly StrategyParam<Unit> _stopValue;
	public Unit StopValue
	{
		get => _stopValue.Value;
		set => _stopValue.Value = value;
	}

	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);

		// インジケーターを作成
		var longSma = new SMA { Length = Long };
		var shortSma = new SMA { Length = Short };

		// ローソク足サブスクリプションを作成し、インジケーターにバインド
		var subscription = SubscribeCandles(CandleType);
		subscription
			.Bind(longSma, shortSma, OnProcess)
			.Start();

		// 可視化を設定
		var area = CreateChartArea();
		if (area != null)
		{
			DrawCandles(area, subscription);
			DrawIndicator(area, shortSma, System.Drawing.Color.Coral);
			DrawIndicator(area, longSma);
			DrawOwnTrades(area);
		}

		// ポジション保護を開始
		StartProtection(TakeValue, StopValue);
	}

	private void OnProcess(ICandleMessage candle, decimal longValue, decimal shortValue)
	{
		// 完成したローソク足のみ処理
		if (candle.State != CandleStates.Finished)
			return;

		// インジケータークロスオーバーに基づく取引ロジック
		var isShortLessThenLong = shortValue < longValue;

		if (_isShortLessThenLong == null)
		{
			_isShortLessThenLong = isShortLessThenLong;
		}
		else if (_isShortLessThenLong != isShortLessThenLong)
		{
			// クロスオーバーが発生
			var direction = isShortLessThenLong ? Sides.Sell : Sides.Buy;
			var volume = Position == 0 ? Volume : Position.Abs().Min(Volume) * 2;
			var priceStep = GetSecurity().PriceStep ?? 1;
			var price = candle.ClosePrice + (direction == Sides.Buy ? priceStep : -priceStep);

			// 注文を発注
			if (direction == Sides.Buy)
				BuyLimit(price, volume);
			else
				SellLimit(price, volume);

			// 現在のインジケーター位置を保存
			_isShortLessThenLong = isShortLessThenLong;
		}
	}
}
```

## まとめ

StockSharp の高レベル API は、取引ストラテジーの開発を大幅に簡略化し、開発者が技術的詳細ではなく取引ロジックに集中できるようにします。データ処理や可視化の細かな調整が不要な典型的なユースケースで特に有用です。

ストラテジーパラメーターシステム、イベントモデル、ポジション保護の仕組みと組み合わせることで、高レベル API は StockSharp をアルゴリズム取引向けの強力で便利なツールにし、初心者にも経験豊富な開発者にも適したものにします。
