# チャートへのインジケーター追加

次の例は、チャートに描画するためのインジケーターを追加する方法を示しています。

```cs
private readonly Connector _connector = new Connector();
private Security _security;
private Subscription _candleSubscription;
private SimpleMovingAverage _sma;
readonly TimeSpan _timeFrame = TimeSpan.FromMinutes(1);
private ChartArea _area;
private ChartCandleElement _candlesElem;
private ChartIndicatorElement _longMaElem;

// チャートとインジケーターの初期化
private void InitializeChart()
{
	// _chart - StockSharp.Xaml.Charting.Chart
	// チャート領域の作成
	_area = new ChartArea();
	_chart.Areas.Add(_area);
	
	// ローソク足を表すチャート要素の作成
	_candlesElem = new ChartCandleElement();
	_area.Elements.Add(_candlesElem);
	
	// インジケーターを表すチャート要素の作成
	_longMaElem = new ChartIndicatorElement
	{
		Title = "長期"
	};
	_area.Elements.Add(_longMaElem);
	
	// インジケーターの作成
	_sma = new SimpleMovingAverage() { Length = 80 };
	
	// ローソク足受信イベントへのサブスクライブ
	_connector.CandleReceived += OnCandleReceived;
}

// ローソク足をサブスクライブするメソッド
private void SubscribeToCandles()
{
	// 指定したタイムフレームでローソク足のサブスクリプションを作成
	_candleSubscription = new Subscription(
		DataType.TimeFrame(_timeFrame),
		_security)
	{
		MarketData = 
		{
			// 30日分の履歴データをリクエスト
			From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
			To = DateTime.Now,
			// 完成したローソク足のみを受信
			IsFinishedOnly = true
		}
	};
	
	// サブスクリプションを開始
	_connector.Subscribe(_candleSubscription);
}

// ローソク足受信イベントのハンドラー
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// ローソク足がこのサブスクリプションに属しているか確認
	if (subscription != _candleSubscription)
		return;
	
	// ローソク足の状態を確認
	if (candle.State != CandleStates.Finished)
		return;
	
	// インジケーターでローソク足を処理
	var longValue = _sma.Process(candle);
	
	// 描画用データの作成
	var data = new ChartDrawData();
	data
		.Group(candle.OpenTime)
			.Add(_candlesElem, candle)
			.Add(_longMaElem, longValue);
	
	// UI スレッドでチャートに描画
	this.GuiAsync(() => _chart.Draw(data));
}

// ウィンドウを閉じるときにサブスクライブ解除するメソッド
private void UnsubscribeFromCandles()
{
	if (_candleSubscription != null)
	{
		_connector.CandleReceived -= OnCandleReceived;
		_connector.UnSubscribe(_candleSubscription);
		_candleSubscription = null;
	}
}
```

![indicators chart](../../../images/indicators_chart.png)

## 複数インジケーターを扱う例

```cs
private readonly Connector _connector = new Connector();
private Security _security;
private Subscription _candleSubscription;
private SimpleMovingAverage _shortSma;
private SimpleMovingAverage _longSma;
private ChartArea _mainArea;
private ChartArea _indicatorArea;
private ChartCandleElement _candlesElem;
private ChartIndicatorElement _shortSmaElem;
private ChartIndicatorElement _longSmaElem;
private RelativeStrengthIndex _rsi;
private ChartIndicatorElement _rsiElem;

// チャートとインジケーターの初期化
private void InitializeChartWithMultipleIndicators()
{
	// ローソク足と移動平均用のメイン領域を作成
	_mainArea = new ChartArea();
	_chart.Areas.Add(_mainArea);
	
	// RSI 用の領域を作成
	_indicatorArea = new ChartArea();
	_chart.Areas.Add(_indicatorArea);
	
	// チャート要素の作成
	_candlesElem = new ChartCandleElement();
	_shortSmaElem = new ChartIndicatorElement { Title = "SMA (短期)" };
	_longSmaElem = new ChartIndicatorElement { Title = "SMA (長期)" };
	_rsiElem = new ChartIndicatorElement { Title = "RSI" };
	
	// 要素の色を設定
	_shortSmaElem.Color = Colors.Red;
	_longSmaElem.Color = Colors.Blue;
	_rsiElem.Color = Colors.Green;
	
	// 各領域に要素を追加
	_mainArea.Elements.Add(_candlesElem);
	_mainArea.Elements.Add(_shortSmaElem);
	_mainArea.Elements.Add(_longSmaElem);
	_indicatorArea.Elements.Add(_rsiElem);
	
	// インジケーターの作成
	_shortSma = new SimpleMovingAverage { Length = 9 };
	_longSma = new SimpleMovingAverage { Length = 20 };
	_rsi = new RelativeStrengthIndex { Length = 14 };
	
	// ローソク足受信イベントへのサブスクライブ
	_connector.CandleReceived += OnCandleReceivedMultipleIndicators;
	
	// ローソク足のサブスクリプションを作成
	_candleSubscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		_security)
	{
		MarketData = 
		{
			From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
			To = DateTime.Now,
			IsFinishedOnly = true
		}
	};
	
	// サブスクリプションを開始
	_connector.Subscribe(_candleSubscription);
}

// 複数インジケーター用のローソク足受信イベントのハンドラー
private void OnCandleReceivedMultipleIndicators(Subscription subscription, ICandleMessage candle)
{
	// ローソク足がこのサブスクリプションに属しているか確認
	if (subscription != _candleSubscription)
		return;
	
	if (candle.State != CandleStates.Finished)
		return;
	
	// インジケーターでローソク足を処理
	var shortSmaValue = _shortSma.Process(candle);
	var longSmaValue = _longSma.Process(candle);
	var rsiValue = _rsi.Process(candle);
	
	// 描画用データの作成
	var data = new ChartDrawData();
	data
		.Group(candle.OpenTime)
			.Add(_candlesElem, candle)
			.Add(_shortSmaElem, shortSmaValue)
			.Add(_longSmaElem, longSmaValue)
			.Add(_rsiElem, rsiValue);
	
	// UI スレッドでチャートに描画
	this.GuiAsync(() => _chart.Draw(data));
}
```

## 関連項目

[チャートを構築するためのコンポーネント](../graphical_user_interface/charts.md)
