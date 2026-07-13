# ローソク足

[S#](../api.md) は、次の種類のローソク足をサポートします:

- [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage) - 時間間隔、つまりタイムフレームに基づくローソク足です。一般的な間隔 (分、時間、日足) とカスタム間隔の両方を設定できます。たとえば、21 秒、4.5 分などです。
- [RangeCandleMessage](xref:StockSharp.Messages.RangeCandleMessage) - 価格レンジローソク足です。許容範囲を超える価格の約定が発生すると、新しいローソク足が作成されます。許容範囲は、毎回最初の約定価格に基づいて形成されます。
- [VolumeCandleMessage](xref:StockSharp.Messages.VolumeCandleMessage) - 約定の合計出来高が指定された上限を超えるまでローソク足が形成されます。新しい約定が許容出来高を超える場合、その約定は新しいローソク足に含まれます。
- [TickCandleMessage](xref:StockSharp.Messages.TickCandleMessage) - [VolumeCandleMessage](xref:StockSharp.Messages.VolumeCandleMessage) と同じですが、制限として出来高ではなく約定数を使用します。
- [PnFCandleMessage](xref:StockSharp.Messages.PnFCandleMessage) - ポイントアンドフィギュアチャートのローソク足 (X-O チャート) です。
- [RenkoCandleMessage](xref:StockSharp.Messages.RenkoCandleMessage) - Renko ローソク足です。

ローソク足の扱い方は、*Samples\/02\_Candles\/01\_Realtime* フォルダーにある例で示されています。

次の画像は、[TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage) と [RangeCandleMessage](xref:StockSharp.Messages.RangeCandleMessage) のチャートを示しています:

![時間枠ローソク足の例](../../images/sample_timeframecandles.png)

![レンジローソク足の例](../../images/sample_rangecandles.png)

## データ取得の開始

1. ローソク足を取得するには、[Subscription](xref:StockSharp.BusinessEntities.Subscription) クラスを使用してサブスクリプションを作成します:

```cs
// 5 分足ローソク足へのサブスクリプションを作成
var subscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),  // タイムフレーム指定付きのデータ型
	security)  // 銘柄
{
	// MarketData プロパティを通じて追加パラメーターを設定
	MarketData = 
	{
		// 履歴データをリクエストする期間 (直近 30 日)
		From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Now
	}
};
```

2. ローソク足を受信するには、処理対象の新しい値が出現したことを通知する [Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived) イベントをサブスクライブします:

```cs
// ローソク足受信イベントをサブスクライブ
_connector.CandleReceived += OnCandleReceived;

// ローソク足受信イベントハンドラー
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// ここで subscription は作成したサブスクリプションオブジェクト
	// 受信したローソク足
	
	// ローソク足が自分のサブスクリプションに属しているか確認
	if (subscription == _candleSubscription)
	{
		// チャートにローソク足を描画
		Chart.Draw(_candleElement, candle);
	}
}
```

> [!TIP]
> [Chart](xref:StockSharp.Xaml.Charting.Chart) グラフィカルコンポーネントは、ローソク足の表示に使用されます。

3. 次に、[Connector.Subscribe](xref:StockSharp.Algo.Connector.Subscribe(StockSharp.BusinessEntities.Subscription)) メソッドを通じてサブスクリプションを開始します:

```cs
// サブスクリプションを開始
_connector.Subscribe(subscription);
```

この後、[Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived) イベントが呼び出され始めます。

4. [Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived) イベントは、新しいローソク足が出現したときだけでなく、現在のローソク足が変化したときにも呼び出されます。

**"完了済み"** ローソク足のみを表示する必要がある場合は、受信したローソク足の [ICandleMessage.State](xref:StockSharp.Messages.ICandleMessage.State) プロパティを確認する必要があります:

```cs
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// ローソク足が自分のサブスクリプションに属しているか確認
	if (subscription != _candleSubscription)
		return;
	
	// ローソク足が完了しているか確認
	if (candle.State == CandleStates.Finished) 
	{
		// 描画用データを作成
		var chartData = new ChartDrawData();
		chartData.Group(candle.OpenTime).Add(_candleElement, candle);
		
		// チャートにローソク足を描画
		this.GuiAsync(() => Chart.Draw(chartData));
	}
}
```

5. サブスクリプションには追加パラメーターを設定できます:

- **ローソク足構築モード** - 既製データをリクエストするか、別のデータ型から構築するかを決定します:

```cs
// 既製データのみをリクエスト
subscription.MarketData.BuildMode = MarketDataBuildModes.Load;

// 別のデータ型からのみ構築
subscription.MarketData.BuildMode = MarketDataBuildModes.Build;

// 既製データをリクエストし、利用できない場合は構築
subscription.MarketData.BuildMode = MarketDataBuildModes.LoadAndBuild;
```

- **ローソク足構築元** - 直接利用できない場合に、どのデータ型からローソク足を構築するかを示します:

```cs
// ティック約定からローソク足を構築
subscription.MarketData.BuildFrom = DataType.Ticks;

// 板情報からローソク足を構築
subscription.MarketData.BuildFrom = DataType.MarketDepth;

// Level1 からローソク足を構築
subscription.MarketData.BuildFrom = DataType.Level1;
```

- **ローソク足構築用フィールド** - 特定のデータ型では指定が必要です:

```cs
// Level1 の最良買気配価格からローソク足を構築
subscription.MarketData.BuildField = Level1Fields.BestBidPrice;

// Level1 の最良売気配価格からローソク足を構築
subscription.MarketData.BuildField = Level1Fields.BestAskPrice;

// 板情報内のスプレッド中央値からローソク足を構築
subscription.MarketData.BuildField = Level1Fields.SpreadMiddle;
```

- **出来高プロファイル** - ローソク足の出来高プロファイル計算:

```cs
// 出来高プロファイル計算を有効化
subscription.MarketData.IsCalcVolumeProfile = true;
```

## さまざまなローソク足タイプへのサブスクリプション例

### 標準タイムフレームのローソク足

```cs
// 5 分足ローソク足
var timeFrameSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	security);
_connector.Subscribe(timeFrameSubscription);
```

### 履歴ローソク足のみを読み込む

```cs
// リアルタイムへ移行せず、履歴ローソク足のみを読み込む
var historicalSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	security)
{
	MarketData =
	{
		From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Today,  // 終了日を指定
		BuildMode = MarketDataBuildModes.Load  // 既製データのみを読み込む
	}
};
_connector.Subscribe(historicalSubscription);
```

### ティックから非標準タイムフレームのローソク足を構築する

```cs
// ティックから構築される 21 秒タイムフレームのローソク足
var customTimeFrameSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromSeconds(21)),
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks
	}
};
_connector.Subscribe(customTimeFrameSubscription);
```

### 板情報データからローソク足を構築する

```cs
// 板情報内のスプレッド中央値から構築されるローソク足
var depthBasedSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(1)),
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.MarketDepth,
		BuildField = Level1Fields.SpreadMiddle
	}
};
_connector.Subscribe(depthBasedSubscription);
```

### 出来高プロファイル付きローソク足

```cs
// 出来高プロファイル計算付き 5 分足ローソク足
var volumeProfileSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.LoadAndBuild,
		BuildFrom = DataType.Ticks,
		IsCalcVolumeProfile = true
	}
};
_connector.Subscribe(volumeProfileSubscription);
```

### 出来高ローソク足

```cs
// 出来高ローソク足 (各ローソク足に出来高 1000 コントラクトを含む)
var volumeCandleSubscription = new Subscription(
	DataType.Volume(1000m),  // ローソク足タイプと出来高を指定
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks
	}
};
_connector.Subscribe(volumeCandleSubscription);
```

### ティック数ローソク足

```cs
// ティック数ローソク足 (各ローソク足に 1000 約定を含む)
var tickCandleSubscription = new Subscription(
	DataType.Tick(1000),  // ローソク足タイプと約定数を指定
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks
	}
};
_connector.Subscribe(tickCandleSubscription);
```

### 価格レンジローソク足

```cs
// レンジ 0.1 単位の価格レンジローソク足
var rangeCandleSubscription = new Subscription(
	DataType.Range(0.1m),  // ローソク足タイプと価格レンジを指定
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks
	}
};
_connector.Subscribe(rangeCandleSubscription);
```

### Renko ローソク足

```cs
// ステップ 0.1 の Renko ローソク足
var renkoCandleSubscription = new Subscription(
	DataType.Renko(0.1m),  // ローソク足タイプとブロックサイズを指定
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks
	}
};
_connector.Subscribe(renkoCandleSubscription);
```

### ポイントアンドフィギュアローソク足 (P&F)

```cs
// ポイントアンドフィギュアローソク足
var pnfCandleSubscription = new Subscription(
	DataType.PnF(new PnfArg { BoxSize = 0.1m, ReversalAmount = 1 }),  // P&F パラメーターを指定
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks
	}
};
_connector.Subscribe(pnfCandleSubscription);
```

## 次のステップ

[チャート](candles/chart.md)

[カスタムローソク足タイプ](candles/custom_type_of_candle.md)
