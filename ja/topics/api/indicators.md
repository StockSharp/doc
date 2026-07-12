# インジケーター

[S#](../api.md) は、140 種類を超える標準のテクニカル分析インジケーターを提供します。これにより、インジケーターを一から作成するのではなく、既製のインジケーターを使用できます。また、[カスタムインジケーター](indicators/custom_indicator.md) セクションに示すように、既存のインジケーターを基に独自のインジケーターを作成することもできます。インジケーターを操作するためのすべての基本クラスとインジケーター本体は、[StockSharp.Algo.Indicators](xref:StockSharp.Algo.Indicators) 名前空間に配置されています。

## インジケーターを取引アルゴリズムへ統合する

1. まず、インジケーターを作成する必要があります。インジケーターは通常の .NET オブジェクトと同じように作成します。

   ```cs
   var longSma = new SimpleMovingAverage { Length = 80 };
   var shortSma = new SimpleMovingAverage { Length = 30 };
   
   // インジケーターを戦略コレクションに追加することを推奨します
   Indicators.Add(longSma);
   Indicators.Add(shortSma);
   ```

2. 次に、インジケーター用にマーケットデータを処理する必要があります。最も効率的な方法は、[Process](xref:StockSharp.Algo.Indicators.IIndicator.Process(StockSharp.Algo.Indicators.IIndicatorValue)) メソッドが返す結果を使用することです。

   ```cs
   private void ProcessCandle(ICandleMessage candle)
   {
       // ローソク足をインジケーターで処理し、結果をすぐに保存します
       var longValue = longSma.Process(candle);
       var shortValue = shortSma.Process(candle);
       
       // 取引判断に結果を使用します
       if (shortValue.GetValue<decimal>() > longValue.GetValue<decimal>())
       {
           // 買いシグナル
           BuyAtMarket();
       }
   }
   ```

   インジケーターは入力として [IIndicatorValue](xref:StockSharp.Algo.Indicators.IIndicatorValue) を受け取ります。[SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage) のように単純な数値を扱うインジケーターもあれば、[MedianPrice](xref:StockSharp.Algo.Indicators.MedianPrice) のように完全なローソク足を必要とするインジケーターもあります。したがって、入力値は [DecimalIndicatorValue](xref:StockSharp.Algo.Indicators.DecimalIndicatorValue) または [CandleIndicatorValue](xref:StockSharp.Algo.Indicators.CandleIndicatorValue) のいずれかにキャストする必要があります。インジケーターの結果値は、入力値と同じ規則に従います。

3. インジケーターの結果値と入力値の両方には [IIndicatorValue.IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal) プロパティがあり、その値が確定していて、この時点ではインジケーターが変化しないことを示します。たとえば、[SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage) インジケーターはローソク足の終値に基づいて形成されますが、現在時点では最終的な終値は不明で変化しています。この場合、[IIndicatorValue.IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal) の結果値は false になります。完了したローソク足をインジケーターに渡すと、[IIndicatorValue.IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal) の入力値と結果値はいずれも true になります。

4. **推奨される方法**: [Process](xref:StockSharp.Algo.Indicators.IIndicator.Process(StockSharp.Algo.Indicators.IIndicatorValue)) メソッドの呼び出しから取得した値を直接使用し、後で [GetCurrentValue](xref:StockSharp.Algo.Indicators.IndicatorHelper.GetCurrentValue(StockSharp.Algo.Indicators.IIndicator)) を呼び出すことは避けます。

   ```cs
   // 2 つの移動平均を使用する戦略の例
   private void ProcessCandle(ICandleMessage candle)
   {
       // ローソク足をインジケーターで処理し、結果をすぐに保存します
       var longValue = _longSma.Process(candle);
       var shortValue = _shortSma.Process(candle);
       
       // チャートに描画します
       DrawCandlesAndIndicators(candle, longValue, shortValue);
       
       if (!IsFormedAndOnlineAndAllowTrading()) 
           return;
           
       // 取得した値を比較に使用します
       var isShortLessCurrent = shortValue.GetValue<decimal>() < longValue.GetValue<decimal>();
       var isShortLessPrev = _shortSma.GetValue(1) < _longSma.GetValue(1);
       
       // クロスオーバーが発生したか確認します
       if (isShortLessCurrent == isShortLessPrev) 
           return;
       
       var volume = Volume + Math.Abs(Position);
       
       // シグナルに基づく取引アクション
       if (isShortLessCurrent)
           SellMarket(volume);
       else
           BuyMarket(volume);
   }
   ```

   この方法には、次の利点があります。
   - ストリーミングデータ処理モデル（受信 → 処理 → 結果の使用）に一致します
   - 蓄積された値のコンテナーへ繰り返しアクセスすることを避けられるため、より効率的です
   - Process の呼び出しとその後の GetCurrentValue 呼び出しの間で発生しうる同期の問題を排除できます

5. 推奨されない方法（効率が低い）:

   ```cs
   // 最適ではない方法
   foreach (var candle in candles)
   {
       // ローソク足を処理しますが、戻り値は無視します
       _longSma.Process(candle);
       _shortSma.Process(candle);
   }
   
   // 後で GetCurrentValue() 経由で値を取得しようとします
   var isShortLessThenLong = _shortSma.GetCurrentValue() < _longSma.GetCurrentValue();
   ```
   
   この方法では、過去のインジケーター値のコンテナーに追加でアクセスすることになり、遅延が発生し、データ処理のストリーミングモデルが乱れます。

6. すべてのインジケーターには [BaseIndicator.IsFormed](xref:StockSharp.Algo.Indicators.BaseIndicator.IsFormed) プロパティがあり、インジケーターが使用可能な状態かどうかを示します。たとえば、[SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage) インジケーターには期間があり、インジケーターの期間と同じ数のローソク足を処理するまでは、そのインジケーターは使用可能な状態ではないと見なされます。そして、[BaseIndicator.IsFormed](xref:StockSharp.Algo.Indicators.BaseIndicator.IsFormed) プロパティは false になります。

## 完全な移動平均戦略の例

以下は、インジケーターを正しく使用し、ローソク足を処理して Process メソッドの結果を利用する戦略の例です。

```cs
public class SmaStrategy : Strategy
{
	private readonly StrategyParam<DataType> _series;
	private readonly StrategyParam<int> _longSmaLength;
	private readonly StrategyParam<int> _shortSmaLength;

	private SimpleMovingAverage _longSma;
	private SimpleMovingAverage _shortSma;

	private IChartIndicatorElement _longSmaIndicatorElement;
	private IChartIndicatorElement _shortSmaIndicatorElement;
	private IChartCandleElement _chartCandleElement;
	private IChartTradeElement _tradesElem;
	private IChart _chart;

	public SmaStrategy()
	{
		base.Name = "SMA戦略";

		// 戦略パラメーターを初期化します
		_longSmaLength = Param(nameof(LongSmaLength), 80);
		_shortSmaLength = Param(nameof(ShortSmaLength), 30);
		_series = Param(nameof(Series), DataType.TimeFrame(TimeSpan.FromMinutes(15)));
	}

	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);

		// インジケーターを作成します
		_shortSma = new SimpleMovingAverage { Length = _shortSmaLength.Value };
		_longSma = new SimpleMovingAverage { Length = _longSmaLength.Value };

		// インジケーターを戦略コレクションに追加します
		Indicators.Add(_shortSma);
		Indicators.Add(_longSma);

		// チャートを初期化します
		_chart = GetChart();
		if (_chart != null)
		{
			InitChart();
		}
		
		// ローソク足をサブスクライブします
		var subscription = new Subscription(_series.Value, Security);

		Connector
			.WhenCandlesFinished(subscription)
			.Do(ProcessCandle)
			.Apply(this);

		Connector.Subscribe(subscription);
	}

	private void ProcessCandle(ICandleMessage candle)
	{
		// ローソク足をインジケーターで処理し、結果を保存します
		var longValue = _longSma.Process(candle);
		var shortValue = _shortSma.Process(candle);
		
		// チャートに描画します
		DrawCandlesAndIndicators(candle, longValue, shortValue);
		
		// 取引条件を確認します
		if (!IsFormedAndOnlineAndAllowTrading()) 
			return;

		// 現在と前回のインジケーター値を比較します
		var isShortLessCurrent = shortValue.GetValue<decimal>() < longValue.GetValue<decimal>();
		var isShortLessPrev = _shortSma.GetValue(1) < _longSma.GetValue(1);

		// クロスオーバーを確認します
		if (isShortLessCurrent == isShortLessPrev) 
			return;

		var volume = Volume + Math.Abs(Position);

		// シグナルに基づく取引アクション
		if (isShortLessCurrent)
			SellMarket(volume);
		else
			BuyMarket(volume);
	}

	private void DrawCandlesAndIndicators(ICandleMessage candle, IIndicatorValue longSma, IIndicatorValue shortSma)
	{
		if (_chart == null) return;
		var data = _chart.CreateData();
		data.Group(candle.OpenTime)
			.Add(_chartCandleElement, candle)
			.Add(_longSmaIndicatorElement, longSma)
			.Add(_shortSmaIndicatorElement, shortSma);
		_chart.Draw(data);
	}

	// 簡潔にするため、その他のチャート初期化メソッドは省略しています
}
```

この例は、StockSharp のストリーミングモデルでインジケーターを扱う正しい方法を示しています。
