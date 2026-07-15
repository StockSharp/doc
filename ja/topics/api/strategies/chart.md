# ストラテジーでのチャートの操作

StockSharp では、[Strategy](xref:StockSharp.Algo.Strategies.Strategy) クラスが、チャート上で取引活動を可視化するための便利なインターフェイスを提供します。この記事では、ストラテジーからチャートへアクセスする方法、エリア（ChartArea）の作成、さまざまな要素（ローソク足、インジケーター、約定）の追加、データの描画について説明します。

## チャートへのアクセス

### GetChart メソッド

ストラテジーからチャートへアクセスするには、[Strategy.GetChart()](xref:StockSharp.Algo.Strategies.Strategy.GetChart) メソッドを使用します。

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);
	
	// チャートの取得
	_chart = GetChart();
	
	// チャートの利用可否を確認
	if (_chart != null)
	{
		// チャートの初期化
		InitializeChart();
	}
	else
	{
		// たとえばコンソールモードで実行している場合、チャートは利用できません
		LogInfo("チャートは利用できません。可視化は無効です。");
	}
}
```

[GetChart()](xref:StockSharp.Algo.Strategies.Strategy.GetChart) メソッドは、チャート機能へのアクセスを提供する [IChart](xref:StockSharp.Charting.IChart) インターフェイスを返します。たとえばコンソールモードやクラウドテストでストラテジーを実行している場合、チャートが利用できないことがあるため、結果が `null` かどうかを確認することが重要です。

### SetChart メソッド

場合によっては、チャートを外部から設定できます。このためには、[Strategy.SetChart](xref:StockSharp.Algo.Strategies.Strategy.SetChart(StockSharp.Charting.IChart)) メソッドを使用します。

```cs
// 外部ソースからチャートを設定
public void ConfigureVisualization(IChart chart)
{
	SetChart(chart);
	
	if (chart != null)
	{
		InitializeChart();
	}
}
```

## チャートエリアの作成

チャートへのアクセスを取得した後、さまざまなデータを表示するために 1 つ以上のエリアを作成できます。[CreateChartArea](xref:StockSharp.Algo.Strategies.Strategy.CreateChartArea) メソッドを使用します。

```cs
private void InitializeChart()
{
	// ローソク足とインジケーター用のメインエリアを作成
	_mainArea = CreateChartArea();
	
	// 出来高用の追加エリアを作成
	_volumeArea = CreateChartArea();
	
	// エリアを設定し、要素を追加
	ConfigureChartElements();
}
```

[IChart.AddArea](xref:StockSharp.Charting.ChartingInterfacesExtensions.AddArea(StockSharp.Charting.IChart)) メソッドを直接使用することもできます。

```cs
private void InitializeChart()
{
	// 必要に応じて既存のエリアをクリア
	foreach (var area in _chart.Areas.ToArray())
		_chart.RemoveArea(area);
	
	// ローソク足とインジケーター用のメインエリアを作成
	_mainArea = _chart.AddArea();
	
	// 出来高用の追加エリアを作成
	_volumeArea = _chart.AddArea();
	
	// エリアを設定し、要素を追加
	ConfigureChartElements();
}
```

## チャートへの要素の追加

チャートエリアを作成した後、データを表示するためにさまざまな要素を追加できます。StockSharp は、ローソク足、インジケーター、約定、注文など、さまざまな種類の要素をサポートしています。

### ローソク足の追加

ローソク足を表示するには、チャートエリアの [AddCandles](xref:StockSharp.Charting.ChartingInterfacesExtensions.AddCandles(StockSharp.Charting.IChartArea)) メソッドを使用します。

```cs
private void ConfigureChartElements()
{
	// メインエリアへローソク足要素を追加
	_candleElement = _mainArea.AddCandles();
	
	// ローソク足表示を設定
	_candleElement.DrawStyle = ChartCandleDrawStyles.CandleStick; // ローソク足
	_candleElement.AntiAliasing = true; // スムージング
	_candleElement.UpFillColor = Color.Green; // 上昇ローソク足の実体色
	_candleElement.DownFillColor = Color.Red; // 下落ローソク足の実体色
	_candleElement.UpBorderColor = Color.DarkGreen; // 上昇ローソク足の枠線色
	_candleElement.DownBorderColor = Color.DarkRed; // 下落ローソク足の枠線色
	_candleElement.StrokeThickness = 1; // 線の太さ
	_candleElement.ShowAxisMarker = true; // Y 軸マーカーを表示
}
```

[IChartCandleElement](xref:StockSharp.Charting.IChartCandleElement) インターフェイスは、ローソク足表示を設定するための多くのプロパティを提供します。

- **描画スタイル** - ローソク足の表示スタイル:
  - **CandleStick** - ローソク足
  - **Ohlc** - バー
  - **LineOpen/LineHigh/LineLow/LineClose** - 各価格に対応する線
  - **ボックス出来高** - 出来高ボックス
  - **ClusterProfile** - クラスタープロファイル
  - **Area** - エリア
  - **PnF** - ポイントアンドフィギュアチャート

- **色設定**:
  - **UpFillColor/DownFillColor** - 上昇/下落ローソク足の実体色
  - **UpBorderColor/DownBorderColor** - 上昇/下落ローソク足の枠線色
  - **線色** - ライン型チャートの線色
  - **領域色** - Area 型のエリア色

- **その他の設定**:
  - **線の太さ** - 線の太さ
  - **アンチエイリアス** - スムージング
  - **軸マーカーを表示** - Y 軸マーカーを表示

### インジケーターの追加

インジケーターを表示するには、[DrawIndicator](xref:StockSharp.Algo.Strategies.Strategy.DrawIndicator(StockSharp.Charting.IChartArea,StockSharp.Algo.Indicators.IIndicator,System.Nullable{System.Drawing.Color},System.Nullable{System.Drawing.Color})) メソッドを使用します。

```cs
// インジケーターの作成
_sma = new SimpleMovingAverage { Length = SmaLength };
_bollinger = new BollingerBands
{
	Length = BollingerLength,
	Deviation = BollingerDeviation
};

// ストラテジーコレクションへインジケーターを追加
Indicators.Add(_sma);
Indicators.Add(_bollinger);

// インジケーターの可視化
_smaElement = DrawIndicator(_mainArea, _sma, Color.Blue);
_bollingerUpperElement = DrawIndicator(_mainArea, _bollinger, Color.Purple);
_bollingerLowerElement = DrawIndicator(_mainArea, _bollinger, Color.Purple);
_bollingerMiddleElement = DrawIndicator(_mainArea, _bollinger, Color.Gray);
```

[DrawIndicator](xref:StockSharp.Algo.Strategies.Strategy.DrawIndicator(StockSharp.Charting.IChartArea,StockSharp.Algo.Indicators.IIndicator,System.Nullable{System.Drawing.Color},System.Nullable{System.Drawing.Color})) メソッドは、インジケーター要素を自動的に作成し、指定されたチャートエリアへ追加します。表示用の色と追加色を指定できます。

チャートエリアの [AddIndicator](xref:StockSharp.Charting.ChartingInterfacesExtensions.AddIndicator(StockSharp.Charting.IChartArea,StockSharp.Algo.Indicators.IIndicator)) メソッドを通じて、インジケーター要素を直接追加することもできます。

```cs
// チャートエリアを通じて SMA を直接追加
var smaElement = _mainArea.AddIndicator(_sma);
smaElement.Color = Color.Blue;
smaElement.StrokeThickness = 2;
smaElement.DrawStyle = DrawStyles.Line;
smaElement.AntiAliasing = true;
smaElement.ShowAxisMarker = true;
smaElement.AutoAssignYAxis = true; // Y 軸を自動的に割り当て
```

[IChartIndicatorElement](xref:StockSharp.Charting.IChartIndicatorElement) インターフェイスは、設定用に次のプロパティを提供します。

- **色** - メインインジケーター色
- **追加色** - 追加色（2 本の線を持つインジケーター用）
- **線の太さ** - 線の太さ
- **アンチエイリアス** - スムージング
- **描画スタイル** - 描画スタイル（線、点、ヒストグラムなど）
- **軸マーカーを表示** - Y 軸マーカーを表示
- **AutoAssignYAxis** - Y 軸を自動的に割り当て

### 約定の追加

約定を表示するには、[DrawOwnTrades](xref:StockSharp.Algo.Strategies.Strategy.DrawOwnTrades(StockSharp.Charting.IChartArea)) メソッドを使用します。

```cs
// 約定を表示する要素を追加
_tradesElement = DrawOwnTrades(_mainArea);

// 約定表示を設定
_tradesElement.BuyBrush = Color.Green;  // 買いの色
_tradesElement.SellBrush = Color.Red;   // 売りの色
_tradesElement.PointSize = 10;          // 点のサイズ
```

### 注文の追加

注文を表示するには、[DrawOrders](xref:StockSharp.Algo.Strategies.Strategy.DrawOrders(StockSharp.Charting.IChartArea)) メソッドを使用します。

```cs
// 注文を表示する要素を追加
_ordersElement = DrawOrders(_mainArea);

// 注文表示を設定
_ordersElement.ActiveBrush = Color.Blue;     // アクティブな注文の色
_ordersElement.CanceledBrush = Color.Gray;   // キャンセル済み注文の色
_ordersElement.DoneBrush = Color.Green;      // 完了済み注文の色
_ordersElement.ErrorColor = Color.Red;       // エラー色
_ordersElement.PointSize = 8;                // 点のサイズ
```

[IChartOrderElement](xref:StockSharp.Charting.IChartOrderElement) インターフェイスは、設定用に次のプロパティを提供します。

- **有効な注文の色** - アクティブな注文の色
- **キャンセル済み注文の色** - キャンセル済み注文の色
- **完了済み注文の色** - 完了済み注文の色
- **エラー色** - エラー色
- **エラー境界色** - エラー枠線色
- **フィルター** - 注文表示フィルター

## チャート上へのデータ描画

すべてのチャート要素を設定した後、データの描画へ進めます。データの種類に応じて異なるメソッドを使用します。

### ローソク足とインジケーターの描画

データを描画する最も効率的な方法は、[IChart.Draw](xref:StockSharp.Charting.IThemeableChart.Draw(StockSharp.Charting.IChartDrawData)) メソッドを [IChartDrawData](xref:StockSharp.Charting.IChartDrawData) オブジェクトとともに使用することです。

```cs
private void ProcessCandle(ICandleMessage candle)
{
	// インジケーターでローソク足を処理
	var smaValue = _sma.Process(candle);
	var bollingerValue = _bollinger.Process(candle);
	
	// チャートが利用できない場合、描画をスキップ
	if (_chart == null)
		return;
	
	// 描画用データを作成
	var drawData = _chart.CreateData();
	
	// ローソク足の時刻でデータをグループ化
	var group = drawData.Group(candle.OpenTime);
	
	// ローソク足を追加
	group.Add(_candleElement, 
		candle.DataType, 
		candle.SecurityId, 
		candle.OpenPrice, 
		candle.HighPrice, 
		candle.LowPrice, 
		candle.ClosePrice, 
		candle.PriceLevels, 
		candle.State);
	
	// インジケーター値を追加
	group.Add(_smaElement, smaValue);
	
	if (bollingerValue != null)
	{
		group.Add(_bollingerUpperElement, bollingerValue);
		group.Add(_bollingerMiddleElement, bollingerValue);
		group.Add(_bollingerLowerElement, bollingerValue);
	}
	
	// チャート上にデータを描画
	_chart.Draw(drawData);
}
```

[IChart.CreateData](xref:StockSharp.Charting.IThemeableChart.CreateData) メソッドは、異なるチャート要素に対してデータをグループ化して追加するために使用される [IChartDrawData](xref:StockSharp.Charting.IChartDrawData) オブジェクトを作成します。データのグループ化は、[Group](xref:StockSharp.Charting.IChartDrawData.Group(System.DateTimeOffset)) メソッドを使用してタイムスタンプ単位で行われます。

異なる種類のデータを追加するには、[Add](xref:StockSharp.Charting.IChartDrawData.IChartDrawDataItem.Add(StockSharp.Charting.IChartCandleElement,StockSharp.Messages.DataType,StockSharp.Messages.SecurityId,System.Decimal,System.Decimal,System.Decimal,System.Decimal,StockSharp.Messages.CandlePriceLevel[],StockSharp.Messages.CandleStates)) メソッドの各種オーバーロードが、[IChartDrawDataItem](xref:StockSharp.Charting.IChartDrawData.IChartDrawDataItem) オブジェクトに対して使用されます。

### 約定と注文の描画

約定と注文の描画には、通常、新しい約定を受信したとき、または注文が変更されたときにトリガーされる自動メカニズムが使用されます。ただし、手動描画が必要な場合は、次のコードを使用できます。

```cs
// 約定を描画
var tradeDrawData = _chart.CreateData();
var tradeGroup = tradeDrawData.Group(trade.Time);
tradeGroup.Add(_tradesElement, trade.Id, trade.StringId, trade.Side, trade.Price, trade.Volume);
_chart.Draw(tradeDrawData);

// 注文を描画
var orderDrawData = _chart.CreateData();
var orderGroup = orderDrawData.Group(order.Time);
orderGroup.Add(_ordersElement, order.Id, order.StringId, order.Side, order.Price, order.Volume);
_chart.Draw(orderDrawData);
```

## ストラテジーでのチャート描画の完全な例

以下は、チャート設定と描画を含むストラテジーの完全な例です。

```cs
public class SmaStrategy : Strategy
{
	private readonly StrategyParam<int> _smaLength;
	private readonly StrategyParam<int> _bollingerLength;
	private readonly StrategyParam<decimal> _bollingerDeviation;
	
	private SimpleMovingAverage _sma;
	private BollingerBands _bollinger;
	
	private IChart _chart;
	private IChartArea _mainArea;
	private IChartArea _volumeArea;
	
	private IChartCandleElement _candleElement;
	private IChartIndicatorElement _smaElement;
	private IChartIndicatorElement _bollingerUpperElement;
	private IChartIndicatorElement _bollingerMiddleElement;
	private IChartIndicatorElement _bollingerLowerElement;
	private IChartOrderElement _ordersElement;
	private IChartTradeElement _tradesElement;
	
	public SmaStrategy()
	{
		_smaLength = Param(nameof(SmaLength), 20);
		_bollingerLength = Param(nameof(BollingerLength), 20);
		_bollingerDeviation = Param(nameof(BollingerDeviation), 2m);
	}
	
	public int SmaLength
	{
		get => _smaLength.Value;
		set => _smaLength.Value = value;
	}
	
	public int BollingerLength
	{
		get => _bollingerLength.Value;
		set => _bollingerLength.Value = value;
	}
	
	public decimal BollingerDeviation
	{
		get => _bollingerDeviation.Value;
		set => _bollingerDeviation.Value = value;
	}
	
	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);
		
		// インジケーターの作成
		_sma = new SimpleMovingAverage { Length = SmaLength };
		_bollinger = new BollingerBands
		{
			Length = BollingerLength,
			Deviation = BollingerDeviation
		};
		
		// インジケーターをストラテジーコレクションへ追加
		Indicators.Add(_sma);
		Indicators.Add(_bollinger);
		
		// チャートの取得
		_chart = GetChart();
		
		// 利用可能な場合、チャートを初期化
		if (_chart != null)
		{
			InitializeChart();
		}
		
		// ローソク足を購読
		var subscription = new Subscription(
			DataType.TimeFrame(TimeSpan.FromMinutes(5)),
			Security);
		
		subscription
			.WhenCandlesFinished(this)
			.Do(ProcessCandle)
			.Apply(this);
		
		Subscribe(subscription);
	}
	
	private void InitializeChart()
	{
		// 既存のエリアをクリア
		foreach (var area in _chart.Areas.ToArray())
			_chart.RemoveArea(area);
		
		// ローソク足とインジケーター用のメインエリアを作成
		_mainArea = _chart.AddArea();
		
		// 出来高用の追加エリアを作成
		_volumeArea = _chart.AddArea();
		
		// チャート要素を設定
		ConfigureChartElements();
	}
	
	private void ConfigureChartElements()
	{
		// ローソク足を表示する要素を追加
		_candleElement = _mainArea.AddCandles();
		_candleElement.DrawStyle = ChartCandleDrawStyles.CandleStick;
		_candleElement.AntiAliasing = true;
		_candleElement.UpFillColor = Color.Green;
		_candleElement.DownFillColor = Color.Red;
		_candleElement.UpBorderColor = Color.DarkGreen;
		_candleElement.DownBorderColor = Color.DarkRed;
		_candleElement.StrokeThickness = 1;
		_candleElement.ShowAxisMarker = true;
		
		// インジケーター用の要素を追加
		_smaElement = _mainArea.AddIndicator(_sma);
		_smaElement.Color = Color.Blue;
		_smaElement.StrokeThickness = 2;
		
		_bollingerUpperElement = _mainArea.AddIndicator(_bollinger);
		_bollingerUpperElement.Color = Color.Purple;
		_bollingerUpperElement.StrokeThickness = 1;
		
		_bollingerMiddleElement = _mainArea.AddIndicator(_bollinger);
		_bollingerMiddleElement.Color = Color.Gray;
		_bollingerMiddleElement.StrokeThickness = 1;
		
		_bollingerLowerElement = _mainArea.AddIndicator(_bollinger);
		_bollingerLowerElement.Color = Color.Purple;
		_bollingerLowerElement.StrokeThickness = 1;
		
		// 注文と約定用の要素を追加
		_ordersElement = DrawOrders(_mainArea);
		_tradesElement = DrawOwnTrades(_mainArea);
	}
	
	private void ProcessCandle(ICandleMessage candle)
	{
		// インジケーターでローソク足を処理
		var smaValue = _sma.Process(candle);
		var bollingerValue = _bollinger.Process(candle);
		
		// チャートが利用できない場合、描画をスキップ
		if (_chart == null)
			return;
		
		// チャート上にデータを描画
		var drawData = _chart.CreateData();
		var group = drawData.Group(candle.OpenTime);
		
		// ローソク足を追加
		group.Add(_candleElement, 
			candle.DataType, 
			candle.SecurityId, 
			candle.OpenPrice, 
			candle.HighPrice, 
			candle.LowPrice, 
			candle.ClosePrice, 
			candle.PriceLevels, 
			candle.State);
		
		// インジケーター値を追加
		group.Add(_smaElement, smaValue);
		
		if (bollingerValue != null)
		{
			group.Add(_bollingerUpperElement, bollingerValue);
			group.Add(_bollingerMiddleElement, bollingerValue);
			group.Add(_bollingerLowerElement, bollingerValue);
		}
		
		// チャート上にデータを描画
		_chart.Draw(drawData);
		
		// 取引ロジック
		if (!IsFormed)
			return;
			
		// ... 取引ロジックの実装 ...
	}
}
```

## まとめ

StockSharp ストラテジーでチャートを使用すると、取引活動を可視化でき、取引ストラテジーの開発、デバッグ、監視が大幅に簡単になります。[Strategy](xref:StockSharp.Algo.Strategies.Strategy) クラスは、チャートを操作するための多くのメソッドを提供しており、さまざまな要素の追加やデータ描画を容易に行えます。

グラフィカルインターフェイスを持つストラテジーを開発する場合、たとえばコンソールモードやクラウドテストで実行しているときにはチャートが利用できない可能性があることを常に考慮してください。そのため、[GetChart()](xref:StockSharp.Algo.Strategies.Strategy.GetChart) メソッドの結果が `null` かどうかを確認し、可視化なしでストラテジーが動作するための代替シナリオを用意することが重要です。

## 関連項目

- [ストラテジー内のインジケーター](indicators.md)
- [ストラテジーでの取引操作](trading_operations.md)
