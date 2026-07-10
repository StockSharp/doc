# チャート

ローソク足をグラフィカルに表示するには、専用の [Chart](xref:StockSharp.Xaml.Charting.Chart) コンポーネント（[チャート構築用コンポーネント](../graphical_user_interface/charts.md) を参照）を使用できます。このコンポーネントは、ローソク足を次のように描画します。

![ローソク足チャートの例](../../../images/sample_candleschart.png)

## ローソク足表示の基本アプローチ

チャートにローソク足を表示する方法は 2 つあります。1 つ目の方法は、データ受信時にローソク足を手動で描画する方法です。

```cs
// CandlesChart - StockSharp.Xaml.Chart
private ChartArea _areaComb;
private ChartCandleElement _candleElement;

// チャートの初期化
private void InitializeChart()
{
	// チャートエリアを作成
	_areaComb = new ChartArea();
	_chart.Areas.Add(_areaComb);
	
	// ローソク足を表すチャート要素を作成
	_candleElement = new ChartCandleElement() { FullTitle = "ローソク足" };
	_areaComb.Elements.Add(_candleElement);
	
	// ローソク足受信イベントを購読
	_connector.CandleReceived += OnCandleReceived;
}

// 5 分足へのサブスクリプションを作成
private void SubscribeToCandles()
{
	var subscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		_security)
	{
		MarketData = 
		{
			// 5 日分の履歴データをリクエスト
			From = DateTime.Today.Subtract(TimeSpan.FromDays(5)),
			To = DateTime.Now
		}
	};
	
	// サブスクリプションを開始
	_connector.Subscribe(subscription);
}

// ローソク足受信イベントのハンドラ
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// ローソク足が完了しているか確認
	if (candle.State == CandleStates.Finished) 
	{
		// 描画用データを作成
		var chartData = new ChartDrawData();
		chartData.Group(candle.OpenTime).Add(_candleElement, candle);
		
		// UI スレッドでチャートに描画
		this.GuiAsync(() => _chart.Draw(chartData));
	}
}
```

## サブスクリプションのチャート要素への自動バインド

2 つ目の方法は、サブスクリプションをチャート要素へ自動バインドする方法です。これにより、受信したデータを自動的に表示できます。

```cs
// 自動バインドを使用したチャートの初期化
private void InitializeChartWithAutoBinding()
{
	// チャートエリアを作成
	var area = new ChartArea();
	_chart.Areas.Add(area);
	
	// ローソク足を表示する要素を作成
	var candleElement = new ChartCandleElement();
	
	// ローソク足へのサブスクリプションを作成
	var subscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		_security)
	{
		MarketData = 
		{
			From = DateTime.Today.Subtract(TimeSpan.FromDays(5)),
			To = DateTime.Now
		}
	};
	
	// 要素をサブスクリプションにバインド
	_chart.AddElement(area, candleElement, subscription);
	
	// サブスクリプションを開始
	_connector.Subscribe(subscription);
}
```

## インジケーターの操作

ローソク足と一緒にチャートへインジケーターを表示するには、[ChartIndicatorElement](xref:StockSharp.Xaml.Charting.ChartIndicatorElement) 型の要素を使用します。

```cs
// チャートにインジケーターを追加
private void AddIndicatorToChart()
{
	// インジケーター用の要素を作成
	var smaElement = new ChartIndicatorElement
	{
		Title = "SMA (14)",
		Color = Colors.Red
	};
	
	// ローソク足と同じエリアに要素を追加
	_areaComb.Elements.Add(smaElement);
	
	// インジケーターを作成
	var sma = new SimpleMovingAverage { Length = 14 };
	
	// インジケーター計算のためにローソク足受信イベントを購読
	_connector.CandleReceived += (subscription, candle) =>
	{
		// インジケーター値を計算
		var indicatorValue = sma.Process(candle);
		
		// 値をチャートに描画
		var chartData = new ChartDrawData();
		chartData.Group(candle.OpenTime).Add(smaElement, indicatorValue);
		
		this.GuiAsync(() => _chart.Draw(chartData));
	};
}
```

## 複数のインジケーターを異なるエリアに表示

インジケーターは、個別のチャートエリアに配置できます。

```cs
// インジケーターを異なるエリアに追加
private void AddIndicatorsToSeparateAreas()
{
	// ローソク足用のメインエリア
	var candleArea = new ChartArea();
	_chart.Areas.Add(candleArea);
	
	// ローソク足用の要素
	var candleElement = new ChartCandleElement();
	candleArea.Elements.Add(candleElement);
	
	// 同じエリア上の SMA 用の要素
	var smaElement = new ChartIndicatorElement { Title = "SMA (14)" };
	candleArea.Elements.Add(smaElement);
	
	// RSI 用の別エリア
	var rsiArea = new ChartArea();
	_chart.Areas.Add(rsiArea);
	
	// RSI 用の要素
	var rsiElement = new ChartIndicatorElement { Title = "RSI (14)" };
	rsiArea.Elements.Add(rsiElement);
	
	// インジケーターを作成
	var sma = new SimpleMovingAverage { Length = 14 };
	var rsi = new RelativeStrengthIndex { Length = 14 };
	
	// ローソク足へのサブスクリプション
	var subscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		_security);
	
	// ローソク足要素をサブスクリプションにバインド
	_chart.AddElement(candleArea, candleElement, subscription);
	
	// サブスクリプションを開始し、インジケーターを処理
	_connector.Subscribe(subscription);
	
	_connector.CandleReceived += (sub, candle) =>
	{
		if (sub != subscription || candle.State != CandleStates.Finished)
			return;
		
		// インジケーター値を計算
		var smaValue = sma.Process(candle);
		var rsiValue = rsi.Process(candle);
		
		// 値をチャートに描画
		var chartData = new ChartDrawData();
		chartData
			.Group(candle.OpenTime)
				.Add(smaElement, smaValue)
				.Add(rsiElement, rsiValue);
		
		this.GuiAsync(() => _chart.Draw(chartData));
	};
}
```

## 注文と約定のチャート表示

注文と約定をチャートに表示するには、専用の要素を使用します。

```cs
// 注文と約定を表示する要素を追加
private void AddOrdersAndTradesToChart()
{
	// 注文と約定を表示する要素を作成
	var orderElement = new ChartOrderElement();
	var tradeElement = new ChartTradeElement();
	
	// 要素をチャートエリアに追加
	_areaComb.Elements.Add(orderElement);
	_areaComb.Elements.Add(tradeElement);
	
	// 注文および約定受信イベントを購読
	_connector.OrderReceived += (subscription, order) =>
	{
		if (order.Security != _security)
			return;
		
		// 注文をチャートに描画
		var chartData = new ChartDrawData();
		chartData.Group(order.Time).Add(orderElement, order);
		
		this.GuiAsync(() => _chart.Draw(chartData));
	};
	
	_connector.OwnTradeReceived += (subscription, trade) =>
	{
		if (trade.Order.Security != _security)
			return;
		
		// 約定をチャートに描画
		var chartData = new ChartDrawData();
		chartData.Group(trade.Time).Add(tradeElement, trade);
		
		this.GuiAsync(() => _chart.Draw(chartData));
	};
}
```

## チャート外観の設定

チャート外観のさまざまな側面を設定できます。

```cs
// チャート外観の設定
private void ConfigureChartAppearance()
{
	// チャートエリアの設定
	_areaComb.Height = 300;
	_areaComb.BackgroundMajorGridColor = Colors.Gray;
	_areaComb.BackgroundMinorGridColor = Colors.LightGray;
	
	// ローソク足要素の設定
	_candleElement.DrawStyle = ChartCandleDrawStyles.CandleStick;
	_candleElement.UpBrush = Brushes.Green;
	_candleElement.DownBrush = Brushes.Red;
	_candleElement.StrokeThickness = 1;
	
	// チャート全体の設定
	_chart.IsAutoRange = true;            // 自動スケーリング
	_chart.IsManualVerticalValues = false; // 垂直値の自動計算
	_chart.BidEnabled = false;            // 最良買気配価格の表示を無効化
	_chart.AskEnabled = false;            // 最良売気配価格の表示を無効化
}
```

## チャートのズームとスクロール

チャートのズームとスクロールを管理します。

```cs
// ズームとスクロールの設定
private void ConfigureChartZoomAndScroll()
{
	// 表示する開始日と終了日を設定
	_chart.SetXRange(DateTime.Today.AddDays(-10), DateTime.Today);
	
	// Y 軸範囲を設定
	_chart.SetYRange(100, 150);
	
	// ズーム制御用ボタン
	zoomInButton.Click += (s, e) => _chart.ZoomIn();
	zoomOutButton.Click += (s, e) => _chart.ZoomOut();
	
	// スクロール用ボタン
	scrollLeftButton.Click += (s, e) => _chart.ScrollLeft();
	scrollRightButton.Click += (s, e) => _chart.ScrollRight();
	
	// ズームを自動にリセット
	resetZoomButton.Click += (s, e) => _chart.IsAutoRange = true;
}
```

## チャートを画像にエクスポート

チャートをファイルに保存するには、次のようにします。

```cs
// チャートを画像にエクスポート
private void ExportChartToImage()
{
	// 画像保存用のオブジェクトを作成
	var saveFileDialog = new SaveFileDialog
	{
		Filter = "PNG 画像|*.png|JPEG 画像|*.jpg|BMP 画像|*.bmp",
		Title = "チャート画像を保存"
	};
	
	if (saveFileDialog.ShowDialog() == true)
	{
		// チャートから画像を作成
		var rtb = new RenderTargetBitmap(
			(int)_chart.ActualWidth, 
			(int)_chart.ActualHeight, 
			96, 96, 
			PixelFormats.Pbgra32);
		
		rtb.Render(_chart);
		
		// 選択された形式で画像を保存
		BitmapEncoder encoder;
		
		switch (Path.GetExtension(saveFileDialog.FileName).ToLower())
		{
			case ".jpg":
				encoder = new JpegBitmapEncoder();
				break;
			case ".bmp":
				encoder = new BmpBitmapEncoder();
				break;
			default:
				encoder = new PngBitmapEncoder();
				break;
		}
		
		encoder.Frames.Add(BitmapFrame.Create(rtb));
		
		using (var fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
		{
			encoder.Save(fileStream);
		}
	}
}
```

## チャートのクリア

チャート上のデータをクリアするには、次のようにします。

```cs
// チャートまたはその要素をクリア
private void ClearChart()
{
	// チャート全体をクリア
	_chart.Reset();
	
	// 特定のエリアをクリア
	_areaComb.Reset();
	
	// 特定の要素をクリア
	_candleElement.Reset();
}
```

チャートにローソク足を表示する例は、[ローソク足](../candles.md) セクションにあります。

## 関連項目

[チャート構築用コンポーネント](../graphical_user_interface/charts.md)
