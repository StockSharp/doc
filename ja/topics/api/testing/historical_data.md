# 履歴データ

履歴データでのテストにより、パターンを見つけるための市場分析と[ストラテジーパラメーターの最適化](optimization.md)の両方を行えます。主な処理は [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) クラスによって実行されます。このクラスは、特別な [API](../market_data_storage/api.md) を通じてローカルリポジトリに保存されたデータを取得します。追加パラメーターについては、[テスト設定](extended_settings.md) セクションで説明しています。

テストは、さまざまな種類の市場データを使用して実行できます。
- ティック約定 ([ITickTradeMessage](xref:StockSharp.Messages.ITickTradeMessage))
- 板情報 ([IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage))
- さまざまな時間枠のローソク足
- [OrderLog](xref:StockSharp.Messages.IOrderLogMessage)
- [Level1](xref:StockSharp.Messages.Level1ChangeMessage) (最良気配の買値と売値)
- 複数データ型の組み合わせ

テスト期間の保存済み板情報がない場合は、[MarketDepthGenerator](xref:StockSharp.Algo.Testing.MarketDepthGenerator) を使用して約定に基づいて生成するか、[OrderLogMarketDepthBuilder](xref:StockSharp.Messages.OrderLogMarketDepthBuilder) を使用して注文ログから再構築できます。

履歴テスト用のデータは、事前に特別な [S#](../../api.md) 形式でダウンロードして保存しておく必要があります。これは、[コネクター](../connectors.md) と [Storage API](../market_data_storage/api.md) を使用して手動で行うことも、専用の [Hydra](../../hydra.md) アプリケーションを設定して実行することでも行えます。

## 履歴テストの主な段階

### 1. データストレージの設定

最初のステップは、[IStorageRegistry](xref:StockSharp.Algo.Storages.IStorageRegistry) オブジェクトを作成することです。[HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) はこのオブジェクトを通じて履歴データへアクセスします。

```csharp
// 履歴データにアクセスするためのストレージ
var storageRegistry = new StorageRegistry
{
	// 履歴データディレクトリのパスを設定
	DefaultDrive = new LocalMarketDataDrive(HistoryPath.Folder)
};
```

> [!CAUTION]
> [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive) コンストラクターには、特定銘柄のディレクトリではなく、**すべての銘柄**の履歴が保存されているルートディレクトリへのパスを渡します。たとえば、HistoryData.zip アーカイブを *C:\\MarketData\\AAPL@NASDAQ\\* ディレクトリに展開した場合、[LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive) には *C:\\MarketData\\* パスを渡す必要があります。詳細は [API](../market_data_storage/api.md) セクションを参照してください。

### 2. 銘柄とポートフォリオの作成

```csharp
// テスト用の銘柄を作成
var security = new Security
{
	Id = SecId.Text, // ID of the instrument corresponds to the name of the folder with historical data
	Code = secCode,
	Board = board,
};

// テスト用ポートフォリオ
var portfolio = new Portfolio
{
	Name = "test account",
	BeginValue = 1000000,
};
```

### 3. エミュレーションコネクターの作成

```csharp
// エミュレーション用コネクタを作成
var connector = new HistoryEmulationConnector(
	new[] { security },
	new[] { portfolio })
{
	EmulationAdapter =
	{
		Emulator =
		{
			Settings =
			{
				// 履歴価格が指値注文価格に到達した場合に注文をマッチング
				// デフォルトではオフ。価格が指値注文価格を通過する必要があります
// (より厳格なテストモード)
				MatchOnTouch = false,
				
				// 約定手数料
				CommissionRules = new ICommissionRule[]
				{
					new CommissionPerTradeRule { Value = 0.01m },
				}
			}
		}
	},
	UseExternalCandleSource = emulationInfo.UseCandle != null,
	CreateDepthFromOrdersLog = emulationInfo.UseOrderLog,
	CreateTradesFromOrdersLog = emulationInfo.UseOrderLog,
	HistoryMessageAdapter =
	{
		StorageRegistry = storageRegistry,
		// テスト範囲を設定
		StartDate = startTime,
		StopDate = stopTime,
		OrderLogMarketDepthBuilders =
		{
			{ secId, new ItchOrderLogMarketDepthBuilder(secId) }
		}
	},
	// 市場時刻の更新間隔を設定
	MarketTimeChangedInterval = timeFrame,
};
```

### 4. イベントの購読とデータ生成の設定

接続時には、テストパラメーターに応じて必要なデータを受信するよう設定します。

```csharp
connector.SecurityReceived += (subscr, s) =>
{
	if (s != security)
		return;
		
	// Level1 値を埋める
	connector.EmulationAdapter.SendInMessage(level1Info);
	
	// テスト設定に応じて必要なデータを購読
	if (emulationInfo.UseMarketDepth)
	{
		connector.Subscribe(new(DataType.MarketDepth, security));
		
		// 板情報を生成する必要がある場合
		if (generateDepths || emulationInfo.UseCandle != null)
		{
			// 戦略で必要だが履歴板情報がない場合、
			// 直近価格に基づくジェネレータを使用
			connector.RegisterMarketDepth(new TrendMarketDepthGenerator(connector.GetSecurityId(security))
			{
				Interval = TimeSpan.FromSeconds(1), // 板情報の更新頻度 - 1 秒
				MaxAsksDepth = maxDepth,
				MaxBidsDepth = maxDepth,
				UseTradeVolume = true,
				MaxVolume = maxVolume,
				MinSpreadStepCount = 2,
				MaxSpreadStepCount = 5,
				MaxPriceStepCount = 3
			});
		}
	}
	
	if (emulationInfo.UseOrderLog)
	{
		connector.Subscribe(new(DataType.OrderLog, security));
	}
	
	if (emulationInfo.UseTicks)
	{
		connector.Subscribe(new(DataType.Ticks, security));
	}
	
	if (emulationInfo.UseLevel1)
	{
		connector.Subscribe(new(DataType.Level1, security));
	}
	
	// エミュレーション開始前に戦略を開始
	strategy.Start();
	
	// 履歴データの読み込みを開始
	connector.Start();
};
```

### 5. ストラテジーの作成と設定

```csharp
// 期間 80 と 10 の移動平均に基づく取引戦略を作成
var strategy = new SmaStrategy
{
	LongSma = 80,
	ShortSma = 10,
	Volume = 1,
	Portfolio = portfolio,
	Security = security,
	Connector = connector,
	LogLevel = DebugLogCheckBox.IsChecked == true ? LogLevels.Debug : LogLevels.Info,
	// デフォルト間隔は 1 分で、数か月の範囲では細かすぎます
	UnrealizedPnLInterval = ((stopTime - startTime).Ticks / 1000).To<TimeSpan>()
};

// ローソク足構築に使用するデータ型を設定
if (emulationInfo.UseCandle != null)
{
	strategy.CandleType = emulationInfo.UseCandle;
	
	if (strategy.CandleType != TimeSpan.FromMinutes(1).TimeFrame())
	{
		strategy.BuildFrom = TimeSpan.FromMinutes(1).TimeFrame();
	}
}
else if (emulationInfo.UseTicks)
	strategy.BuildFrom = DataType.Ticks;
else if (emulationInfo.UseLevel1)
{
	strategy.BuildFrom = DataType.Level1;
	strategy.BuildField = emulationInfo.BuildField;
}
else if (emulationInfo.UseOrderLog)
	strategy.BuildFrom = DataType.OrderLog;
else if (emulationInfo.UseMarketDepth)
	strategy.BuildFrom = DataType.MarketDepth;
```

### 6. 結果の可視化

テスト結果を視覚的に表示するため、P&L とポジションの変化を購読します。

```csharp
var pnlCurve = equity.CreateCurve(LocalizedStrings.PnL + " " + emulationInfo.StrategyName, Colors.Green, Colors.Red, DrawStyles.Area);
var realizedPnLCurve = equity.CreateCurve(LocalizedStrings.PnLRealized + " " + emulationInfo.StrategyName, Colors.Black, DrawStyles.Line);
var unrealizedPnLCurve = equity.CreateCurve(LocalizedStrings.PnLUnreal + " " + emulationInfo.StrategyName, Colors.DarkGray, DrawStyles.Line);
var commissionCurve = equity.CreateCurve(LocalizedStrings.Commission + " " + emulationInfo.StrategyName, Colors.Red, DrawStyles.DashedLine);

strategy.PnLReceived2 += (s, pf, t, r, u, c) =>
{
	var data = equity.CreateData();

	data
		.Group(t)
		.Add(pnlCurve, r - (c ?? 0))
		.Add(realizedPnLCurve, r)
		.Add(unrealizedPnLCurve, u ?? 0)
		.Add(commissionCurve, c ?? 0);

	equity.Draw(data);
};

var posItems = pos.CreateCurve(emulationInfo.StrategyName, emulationInfo.CurveColor, DrawStyles.Line);

strategy.PositionReceived += (s, p) =>
{
	var data = pos.CreateData();

	data
		.Group(p.LocalTime)
		.Add(posItems, p.CurrentValue);

	pos.Draw(data);
};

// 進捗更新を購読
connector.ProgressChanged += steps => this.GuiAsync(() => progressBar.Value = steps);
```

### 7. テストの開始

```csharp
// エミュレーションを開始
connector.Connect();
```

## 履歴テストの最新実装

[S#](../../api.md) の最新バージョンでは、履歴テストの例が大幅に近代化され、さまざまな種類の市場データを使用してストラテジーをテストできるようになりました。

- ティック (約定)
- 板情報
- さまざまな時間枠のローソク足
- 注文ログ
- Level1 データ (最良価格)
- 複数データ型の組み合わせ

データ型ごとに、チャートと統計を含む個別のタブが作成されます。

```csharp
// テストモードを作成
_settings = new[]
{
	(
		TicksCheckBox,
		TicksProgress,
		TicksParameterGrid,
		// ticks
		new EmulationInfo
		{
			UseTicks = true,
			CurveColor = Colors.DarkGreen,
			StrategyName = LocalizedStrings.Ticks
		},
		TicksChart,
		TicksEquity,
		TicksPosition
	),

	(
		TicksAndDepthsCheckBox,
		TicksAndDepthsProgress,
		TicksAndDepthsParameterGrid,
		// ティック + 板情報
		new EmulationInfo
		{
			UseTicks = true,
			UseMarketDepth = true,
			CurveColor = Colors.Red,
			StrategyName = LocalizedStrings.TicksAndDepths
		},
		TicksAndDepthsChart,
		TicksAndDepthsEquity,
		TicksAndDepthsPosition
	),
	
	// その他のデータ型の組み合わせ
};
```

このアプローチにより、異なるデータソースを使用した場合のストラテジー性能を視覚的に比較できます。

## 改良された SMA ストラテジー

移動平均 (SMA) ストラテジーは再設計され、データ購読とローソク足処理に、より新しいアプローチを使用するようになりました。

```csharp
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// 必要な種類のローソク足購読を作成
	var dt = CandleTimeFrame is null
		? CandleType
		: DataType.Create(CandleType.MessageType, CandleTimeFrame);

	var subscription = new Subscription(dt, Security)
	{
		MarketData =
		{
			IsFinishedOnly = true,
			BuildFrom = BuildFrom,
			BuildMode = BuildFrom is null ? MarketDataBuildModes.LoadAndBuild : MarketDataBuildModes.Build,
			BuildField = BuildField,
		}
	};

	// インジケーターを作成
	var longSma = new SMA { Length = LongSma };
	var shortSma = new SMA { Length = ShortSma };

	// ローソク足を購読してインジケーターにバインド
	SubscribeCandles(subscription)
		.Bind(longSma, shortSma, OnProcess)
		.Start();

	// チャート表示を設定
	var area = CreateChartArea();

	if (area != null)
	{
		DrawCandles(area, subscription);
		DrawIndicator(area, shortSma, System.Drawing.Color.Coral);
		DrawIndicator(area, longSma);
		DrawOwnTrades(area);
	}

	// ポジション保護を設定
	StartProtection(TakeValue, StopValue);
}
```

ローソク足処理と売買判断は、専用メソッドに分離されています。

```csharp
private void OnProcess(ICandleMessage candle, decimal longValue, decimal shortValue)
{
	LogInfo(LocalizedStrings.SmaNewCandleLog, candle.OpenTime, candle.OpenPrice, candle.HighPrice, candle.LowPrice, candle.ClosePrice, candle.TotalVolume, candle.SecurityId);

	// ローソク足が完了しているか確認
	if (candle.State != CandleStates.Finished)
		return;

	// インジケーターのクロスを分析
	var isShortLessThenLong = shortValue < longValue;

	if (_isShortLessThenLong == null)
	{
		_isShortLessThenLong = isShortLessThenLong;
	}
	else if (_isShortLessThenLong != isShortLessThenLong) // クロスが発生
	{
		// short が long より小さい場合は売り、それ以外は買い
		var direction = isShortLessThenLong ? Sides.Sell : Sides.Buy;

		// ポジション開始または反転用の数量を計算
		var volume = Position == 0 ? Volume : Position.Abs().Min(Volume) * 2;

		// ローソク足の終値を使用
		var price = candle.ClosePrice;

		if (direction == Sides.Buy)
			BuyLimit(price, volume);
		else
			SellLimit(price, volume);

		_isShortLessThenLong = isShortLessThenLong;
	}
}
```

## 追加のテスト設定

[S#](../../api.md) では、以下を含む拡張テスト設定を利用できます。

- 指定パラメーターによる板情報の生成
- 手数料設定
- 価格スリッページ設定
- 約定遅延エミュレーション

これらの設定については、[テスト設定](extended_settings.md) セクションで詳しく説明しています。
