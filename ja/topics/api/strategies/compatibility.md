# StockSharp プラットフォームとのストラテジー互換性

StockSharp で取引ストラテジーを開発する際は、[Designer](../../designer.md)、[Shell](../../shell.md)、[Runner](../../runner.md)、および [クラウドバックテスト](../../designer/backtesting/cloud_backtesting.md) など、さまざまなプラットフォームとの互換性を考慮することが重要です。以下の推奨事項に従うことで、すべての環境で正しく動作するストラテジーを作成できます。

## ストラテジーコンストラクターのパラメーター

### コンストラクター内のパラメーターを避ける

StockSharp プラットフォーム、特にクラウドバックテストとの互換性を確保するため、**ストラテジーコンストラクターにパラメーターを追加しないでください**。

```cs
// 正しい: パラメーターなしのコンストラクター
public class SmaStrategy : Strategy
{
	public SmaStrategy()
	{
		// パラメーターの初期化
	}
}

// 誤り: パラメーター付きコンストラクター
public class SmaStrategy : Strategy
{
	public SmaStrategy(int longLength, int shortLength) // この方法は使用しないでください
	{
		// ...
	}
}
```

StockSharp プラットフォームは、パラメーターなしコンストラクターを使用してストラテジーインスタンスを作成します。ストラテジーがパラメーター付きコンストラクターを必要とする場合、正しく初期化されません。

## 通常のプロパティではなく StrategyParam を使用する

### StrategyParam の利点

通常の C# プロパティを作成してから `Save` メソッドと `Load` メソッドをオーバーライドする代わりに、カスタマイズ可能なすべてのパラメーターに [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) を使用します。

```cs
// 正しい: StrategyParam を使用
private readonly StrategyParam<int> _longSmaLength;

public int LongSmaLength
{
	get => _longSmaLength.Value;
	set => _longSmaLength.Value = value;
}

public SmaStrategy()
{
	_longSmaLength = Param(nameof(LongSmaLength), 80)
						.SetDisplay("Long SMA length", string.Empty, "基本設定");
}

// 誤り: 通常のプロパティを使用
private int _longSmaLength = 80; // この方法は使用しないでください

public int LongSmaLength
{
	get => _longSmaLength;
	set => _longSmaLength = value;
}
```

[StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) を通じて作成されたパラメーターは、自動的に次のように扱われます。
- プラットフォームのユーザーインターフェイスに表示される
- `Save` メソッドと `Load` メソッドをオーバーライドしなくても保存および読み込みされる
- 最適化で使用される
- クラウドバックテストへ送信されるときに正しくシリアライズされる

## ユーザーインターフェイスの操作

### 直接 UI へアクセスする代わりに抽象化を使用する

ユーザーインターフェイス要素へ直接アクセスする代わりに、StockSharp が提供する抽象化を使用します。

```cs
// 正しい方法: IChart を使用
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);
	
	// ランタイム環境が提供するチャートを取得
	_chart = GetChart();
	
	if (_chart != null)
	{
		// チャートが利用可能（例: Designer または Shell）
		InitChart();
	}
	else
	{
		// チャートは利用不可（例: Runner またはクラウドバックテスト）
		// ストラテジーは可視化なしで動作を継続
	}
}

private void InitChart()
{
	// 抽象インターフェイスを通じてチャートを設定
	_chart.ClearAreas();
	var area = _chart.AddArea();
	_chartCandleElement = area.AddCandles();
	// ...
}
```

[Strategy.GetChart()](xref:StockSharp.Algo.Strategies.Strategy.GetChart) メソッドは、現在のランタイム環境でチャートが利用可能な場合に [IChart](xref:StockSharp.Charting.IChart) インターフェイスを返します。グラフィカルインターフェイスがないコンソール [Runner](../../runner.md) またはクラウドバックテストでストラテジーを実行している場合、このメソッドは `null` を返します。

[IChart](xref:StockSharp.Charting.IChart) インターフェイスは、チャートを操作するためのメソッドを提供します。
- [AddArea](xref:StockSharp.Charting.IChart.AddArea(StockSharp.Charting.IChartArea)) - チャートにエリアを追加する
- [RemoveArea](xref:StockSharp.Charting.IChart.RemoveArea(StockSharp.Charting.IChartArea)) - エリアを削除する
- [AddElement](xref:StockSharp.Charting.IChart.AddElement(StockSharp.Charting.IChartArea,StockSharp.Charting.IChartElement)) - チャートに要素を追加する
- [RemoveElement](xref:StockSharp.Charting.IChart.RemoveElement(StockSharp.Charting.IChartArea,StockSharp.Charting.IChartElement)) - 要素を削除する
- [Reset](xref:StockSharp.Charting.IChart.Reset(System.Collections.Generic.IEnumerable{StockSharp.Charting.IChartElement})) - 要素の値をリセットする

### チャートの利用可否を確認する

チャートを使用する前に、必ず利用可否を確認してください。

```cs
private void DrawCandlesAndIndicators(ICandleMessage candle, IIndicatorValue longSma, IIndicatorValue shortSma)
{
	if (_chart == null) return; // 重要な確認
	
	var data = _chart.CreateData();
	data.Group(candle.OpenTime)
		.Add(_chartCandleElement, candle)
		.Add(_longSmaIndicatorElement, longSma)
		.Add(_shortSmaIndicatorElement, shortSma);
	_chart.Draw(data);
}
```

## スレッドと同期

### 追加スレッドの作成を避ける

StockSharp では、データ処理のために**追加スレッドを作成する必要はありません**。すべてのイベント（マーケットデータ、トランザクション）は単一スレッドで届きます。

```cs
// 正しい: 標準イベントハンドラーを使用
private void ProcessCandle(ICandleMessage candle)
{
	// メインスレッドでローソク足を処理
	var longSmaIsFormedPrev = _longSma.IsFormed;
	var ls = _longSma.Process(candle);
	var ss = _shortSma.Process(candle);
	
	// ...
}

// 誤り: 追加スレッドを作成
private void ProcessCandle(ICandleMessage candle)
{
	// これは行わないでください
	Task.Run(() => {
		var longSmaIsFormedPrev = _longSma.IsFormed;
		// ...
	});
}
```

### 同期オブジェクトを避ける

すべてのイベントは単一スレッドで処理されるため、**同期オブジェクトを使用する必要はありません**。

```cs
// 正しい: 同期なしの通常処理
private void ProcessCandle(ICandleMessage candle)
{
	var ls = _longSma.Process(candle);
	var ss = _shortSma.Process(candle);
	// ...
}

// 誤り: 不要な同期
private readonly object _syncLock = new object(); // 不要

private void ProcessCandle(ICandleMessage candle)
{
	lock (_syncLock) // 不要
	{
		var ls = _longSma.Process(candle);
		// ...
	}
}
```

## 外部リソース

### StockSharp インフラストラクチャを使用する

外部リソース（ファイル、データベース、ネットワーク）へ直接アクセスする代わりに、StockSharp プラットフォームが提供する機能を使用します。

```cs
// 正しい: データ保存に組み込みメカニズムを使用
protected override void OnStopped()
{
	// データはストラテジーパラメーターを通じて自動的に保存されます
	base.OnStopped();
}

// 誤り: 外部リソースへ直接アクセス
protected override void OnStopped()
{
	// これは行わないでください
	File.WriteAllText("results.txt", $"PnL: {PnL}");
	
	// またはこれ
	using (var connection = new SqlConnection("..."))
	{
		// ...
	}
	
	base.OnStopped();
}
```

### データストレージ

ストラテジーの結果を保存するには、次を使用します。

- 設定と構成には [ストラテジーパラメーター](parameters.md)
- [Designer](../../designer.md) と [Shell](../../shell.md) の組み込みストレージメカニズム
- 取引メトリクスの収集には [統計](xref:StockSharp.Algo.Statistics.StatisticManager)

### Save メソッドと Load メソッド

[Strategy.Save](xref:StockSharp.Algo.Strategies.Strategy.Save(Ecng.Serialization.SettingsStorage)) メソッドと [Strategy.Load](xref:StockSharp.Algo.Strategies.Strategy.Load(Ecng.Serialization.SettingsStorage)) メソッドは、設定やパラメーターではない追加のストラテジーデータを保存するために専用設計されています。これは、ストラテジー状態の復元に必要なデータを保存する理想的な場所です。

```cs
public override void Save(SettingsStorage settings)
{
	base.Save(settings); // 最初にストラテジーパラメーターを保存
	
	// 次にカスタムデータを保存
	settings.SetValue("CustomState", _customState);
	settings.SetValue("LastSignalTime", _lastSignalTime);
}

public override void Load(SettingsStorage settings)
{
	base.Load(settings); // 最初にストラテジーパラメーターを読み込み
	
	// 次にカスタムデータを読み込み
	if (settings.Contains("CustomState"))
		_customState = settings.GetValue<string>("CustomState");
	
	if (settings.Contains("LastSignalTime"))
		_lastSignalTime = settings.GetValue<DateTimeOffset>("LastSignalTime");
}
```

ただし、主な設定可能パラメーターは、上で説明したように引き続き [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) を通じて実装する必要があります。これにより、ユーザーインターフェイスでの自動表示が保証されるためです。

## マーケットデータ購読

### 直接購読ではなくルールを使用する

マーケットデータ処理には、[イベントモデル](event_model.md) とルールを使用することが推奨されます。

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	_shortSma = new SimpleMovingAverage { Length = ShortSmaLength };
	_longSma = new SimpleMovingAverage { Length = LongSmaLength };

	Indicators.Add(_shortSma);
	Indicators.Add(_longSma);
	
	var subscription = new Subscription(Series, Security);

	// 正しい: データ処理にルールを使用
	Connector
		.WhenCandlesFinished(subscription)
		.Do(ProcessCandle)
		.Apply(this);

	Subscribe(subscription);
}
```

ルールには、通常のイベントハンドラーに比べていくつかの重要な利点があります。

1. **自動購読解除** - ストラテジーが停止したとき、またはルールが不要になったとき、ルールはイベントから自動的に購読解除されます。購読を手動で管理する必要はありません。

2. **高レベル API** - ルールは、標準イベントハンドラーよりも理解しやすく便利なインターフェイスを提供します。たとえば、`CandleReceived` イベントを購読して後続でローソク足の状態を確認するよりも、`WhenCandlesFinished` のほうがはるかに明確です。

3. **条件の結合** - ルールは `And`、`Or` などの演算子を使用して結合でき、複雑なアクティブ化条件を作成できます。

```cs
// ルール結合の例
var tickSub = new Subscription(DataType.Ticks, Security);

tickSub
	.WhenTickTradeReceived(this)
	.And(Portfolio.WhenChanged(Connector))
	.Do(() => {
		// 新しい約定があり、かつポートフォリオ残高が変化した場合にのみ実行されるコード
	})
	.Apply(this);

Subscribe(tickSub);
```

4. **ライフサイクル管理** - ルールは 1 回限りにしたり（`Once()`）、キャンセル条件を設定したり（`Until()`）、遅延アクションを追加したりできます。

## 互換性のあるストラテジー例

以下は、すべての推奨事項に従い、すべての StockSharp プラットフォームで正しく動作するストラテジーの例です。

```cs
public class SmaStrategy : Strategy
{
	private readonly StrategyParam<DataType> _series;
	private readonly StrategyParam<int> _longSmaLength;
	private readonly StrategyParam<int> _shortSmaLength;

	public DataType Series
	{
		get => _series.Value;
		set => _series.Value = value;
	}

	public int LongSmaLength
	{
		get => _longSmaLength.Value;
		set => _longSmaLength.Value = value;
	}

	public int ShortSmaLength
	{
		get => _shortSmaLength.Value;
		set => _shortSmaLength.Value = value;
	}

	private SimpleMovingAverage _longSma;
	private SimpleMovingAverage _shortSma;
	private IChart _chart;
	private IChartCandleElement _chartCandleElement;
	private IChartIndicatorElement _longSmaIndicatorElement;
	private IChartIndicatorElement _shortSmaIndicatorElement;

	public SmaStrategy()
	{
		_longSmaLength = Param(nameof(LongSmaLength), 80)
							.SetDisplay("Long SMA length", string.Empty, "基本設定")
							.SetCanOptimize(true);
							
		_shortSmaLength = Param(nameof(ShortSmaLength), 30)
							.SetDisplay("Short SMA length", string.Empty, "基本設定")
							.SetCanOptimize(true);
							
		_series = Param(nameof(Series), TimeSpan.FromMinutes(15).TimeFrame())
					.SetDisplay("Series", string.Empty, "基本設定");
	}

	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);

		_longSma = new SimpleMovingAverage { Length = LongSmaLength };
		_shortSma = new SimpleMovingAverage { Length = ShortSmaLength };

		Indicators.Add(_shortSma);
		Indicators.Add(_longSma);
		
		// 利用可能な場合、チャートを初期化
		_chart = GetChart();
		if (_chart != null)
			InitChart();
		
		var subscription = new Subscription(Series, Security);

		Connector
			.WhenCandlesFinished(subscription)
			.Do(ProcessCandle)
			.Apply(this);

		Subscribe(subscription);
	}

	private void InitChart()
	{
		_chart.ClearAreas();
		var area = _chart.AddArea();
		
		_chartCandleElement = area.AddCandles();
		
		_longSmaIndicatorElement = area.AddIndicator(_longSma);
		_longSmaIndicatorElement.Color = System.Drawing.Color.Brown;
		_longSmaIndicatorElement.DrawStyle = DrawStyles.Line;
		
		_shortSmaIndicatorElement = area.AddIndicator(_shortSma);
		_shortSmaIndicatorElement.Color = System.Drawing.Color.Blue;
		_shortSmaIndicatorElement.DrawStyle = DrawStyles.Line;
	}

	private void ProcessCandle(ICandleMessage candle)
	{
		var ls = _longSma.Process(candle);
		var ss = _shortSma.Process(candle);
		
		// 利用可能な場合、チャートへ描画
		if (_chart != null)
		{
			var data = _chart.CreateData();
			data.Group(candle.OpenTime)
				.Add(_chartCandleElement, candle)
				.Add(_longSmaIndicatorElement, ls)
				.Add(_shortSmaIndicatorElement, ss);
			_chart.Draw(data);
		}
		
		if (!_longSma.IsFormed)
			return;
			
		var isShortLessCurrent = _shortSma.GetCurrentValue() < _longSma.GetCurrentValue();
		var isShortLessPrev = _shortSma.GetValue(1) < _longSma.GetValue(1);

		if (isShortLessCurrent == isShortLessPrev)
			return;
			
		// 取引ロジック
		var volume = Volume + Math.Abs(Position);

		if (isShortLessCurrent)
			SellMarket(volume);
		else
			BuyMarket(volume);
	}
}
```

## 関連項目

- [ストラテジーパラメーター](parameters.md)
- [イベントモデル](event_model.md)
- [ストラテジーのログ記録](logging.md)

