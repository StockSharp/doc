# カスタムインジケーター

独自のインジケーターを作成するには、[IIndicator](xref:StockSharp.Algo.Indicators.IIndicator) インターフェイスを実装する必要があります。例として、[GitHub/StockSharp](https://github.com/StockSharp/StockSharp) リポジトリにある他のインジケーターのソースコードを参照できます。Simple Moving Average [SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage) の実装は次のようになります。

```cs
/// <summary>
/// 単純移動平均。
/// </summary>
[DisplayName("SMA")]
[Description("Simple Moving Average.")]
public class SimpleMovingAverage : LengthIndicator<decimal>
{
	/// <summary>
	/// Create <see cref="SimpleMovingAverage"/>.
	/// </summary>
	public SimpleMovingAverage()
	{
		Length = 32;
	}

	/// <summary>
	/// 入力値を処理。
	/// </summary>
	/// <param name="input">Input value.</param>
	/// <returns>結果値。</returns>
	protected override IIndicatorValue OnProcess(IIndicatorValue input)
	{
		var newValue = input.GetValue<decimal>();
		if (input.IsFinal)
		{
			Buffer.Add(newValue);
			if (Buffer.Count > Length)
				Buffer.RemoveAt(0);
		}
		
		if (input.IsFinal)
			return new DecimalIndicatorValue(this, Buffer.Sum() / Length);
		
		return new DecimalIndicatorValue(this, (Buffer.Skip(1).Sum() + newValue) / Length);
	}
}
```


[SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage) は [LengthIndicator\<TResult\>](xref:StockSharp.Algo.Indicators.LengthIndicator`1) を継承しています。期間長パラメーターを持つすべてのインジケーターは、このクラスを継承する必要があります。

## 重要なインジケータープロパティとメソッド

カスタムインジケーターを作成するときは、次のプロパティとメソッドに特に注意する必要があります。

### NumValuesToInitialize

[NumValuesToInitialize](xref:StockSharp.Algo.Indicators.IIndicator.NumValuesToInitialize) プロパティは、インジケーターが初期化（形成、または「ウォームアップ」）に必要とする値の数を示します。この値は、インジケーターが形成済みで使用可能な状態と見なせるタイミングを判断するために使用されます。

```cs
/// <inheritdoc />
public override int NumValuesToInitialize => Length;
```

複数のコンポーネントで構成されるより複雑なインジケーターでは、この値は通常、すべての構成要素の最大値として決定されます。

```cs
/// <inheritdoc />
public override int NumValuesToInitialize => _shortEma.NumValuesToInitialize.Max(_longEma.NumValuesToInitialize);
```

### Measure

[Measure](xref:StockSharp.Algo.Indicators.IIndicator.Measure) プロパティは、インジケーターが提供する測定タイプと次元を定義します。

```cs
/// <inheritdoc />
public override IndicatorMeasures Measure => IndicatorMeasures.Percent;
```

利用可能な測定タイプ:
- `IndicatorMeasures.Price` - インジケーターが価格を測定します（例: 移動平均）
- `IndicatorMeasures.Percent` - インジケーターが 0 から 100 までのパーセントスケールを使用します（例: RSI）
- `IndicatorMeasures.MinusOnePlusOne` - インジケーターが -1 から +1 までのスケールを使用します
- `IndicatorMeasures.Volume` - インジケーターが出来高を測定します（例: OBV）

このプロパティは、チャート上でインジケーターを正しく表示するために非常に重要です。異なる次元を持つ複数のインジケーターが同じパネルに重ねて表示される場合、`Measure` タイプが異なるインジケーターには個別の Y 軸が作成されます。これにより、一方の値が数千単位（例: 価格）で、もう一方が単位の小数（例: オシレーター）で測定される場合でも、すべてのインジケーターをそれぞれ自然なスケールで視覚的に表示できます。

### Save と Load

[Save](xref:StockSharp.Algo.Indicators.BaseIndicator.Save(Ecng.Serialization.SettingsStorage)) および [Load](xref:StockSharp.Algo.Indicators.BaseIndicator.Load(Ecng.Serialization.SettingsStorage)) メソッドは、インジケーター設定の保存と読み込みに必要です。

```cs
/// <inheritdoc />
public override void Save(SettingsStorage storage)
{
	base.Save(storage);

	storage.SetValue(nameof(ShortPeriod), ShortPeriod);
	storage.SetValue(nameof(LongPeriod), LongPeriod);
}

/// <inheritdoc />
public override void Load(SettingsStorage storage)
{
	base.Load(storage);

	ShortPeriod = storage.GetValue<int>(nameof(ShortPeriod));
	LongPeriod = storage.GetValue<int>(nameof(LongPeriod));
}
```

## 複合インジケーター

一部のインジケーターは複合型で、計算に他のインジケーターを使用します。そのため、[ChaikinVolatility](xref:StockSharp.Algo.Indicators.ChaikinVolatility) インジケーターの実装例に示すように、インジケーター同士で再利用できます。

```cs
/// <summary>
/// Chaikin ボラティリティ。
/// </summary>
[DisplayName("Volatility")]
[Description("Chaikin Volatility.")]
public class ChaikinVolatility : BaseIndicator<IIndicatorValue>
{
	/// <summary>
	/// Create <see cref="ChaikinVolatility"/>.
	/// </summary>
	public ChaikinVolatility()
	{
		Ema = new ExponentialMovingAverage();
		Roc = new RateOfChange();
	}

	/// <summary>
	/// 移動平均。
	/// </summary>
	[ExpandableObject]
	[DisplayName("MA")]
	[Description("Moving Average.")]
	[Category("Main")]
	public ExponentialMovingAverage Ema { get; private set; }

	/// <summary>
	/// 変化率。
	/// </summary>
	[ExpandableObject]
	[DisplayName("ROC")]
	[Description("Rate of Change.")]
	[Category("Main")]
	public RateOfChange Roc { get; private set; }

	/// <summary>
	/// インジケーターが形成済みか。
	/// </summary>
	public override bool IsFormed
	{
		get { return Roc.IsFormed; }
	}

	/// <summary>
	/// 入力値を処理。
	/// </summary>
	/// <param name="input">Input value.</param>
	/// <returns>結果値。</returns>
	protected override IIndicatorValue OnProcess(IIndicatorValue input)
	{
		var candle = input.GetValue<Candle>();
		var emaValue = Ema.Process(input.SetValue(this, candle.HighPrice - candle.LowPrice));
		
		if (Ema.IsFormed)
		{
			return Roc.Process(emaValue);
		}
		
		return input;
	}
}
```

## 複数ラインを持つインジケーター

最後の種類は、他のインジケーターで構成されるだけでなく、複数の状態（複数ライン）を同時にグラフィカルに表示するインジケーターです。たとえば、[AverageDirectionalIndex](xref:StockSharp.Algo.Indicators.AverageDirectionalIndex) があります。

```cs
/// <summary>
/// Welles Wilder の平均方向性指数。
/// </summary>
[DisplayName("ADX")]
[Description("Welles Wilder's Average Directional Index.")]
public class AverageDirectionalIndex : BaseComplexIndicator
{
	/// <summary>
	/// Create <see cref="AverageDirectionalIndex"/>.
	/// </summary>
	public AverageDirectionalIndex()
		: this(new DirectionalIndex { Length = 14 }, new WilderMovingAverage { Length = 14 })
	{
	}

	/// <summary>
	/// Create <see cref="AverageDirectionalIndex"/>.
	/// </summary>
	/// <param name="dx">Welles Wilder's Directional Movement Index.</param>
	/// <param name="movingAverage">Moving Average.</param>
	public AverageDirectionalIndex(DirectionalIndex dx, LengthIndicator<decimal> movingAverage)
	{
		if (dx == null)
			throw new ArgumentNullException(nameof(dx));
		if (movingAverage == null)
			throw new ArgumentNullException(nameof(movingAverage));
		
		InnerIndicators.Add(Dx = dx);
		InnerIndicators.Add(MovingAverage = movingAverage);
		Mode = ComplexIndicatorModes.Sequence;
	}

	/// <summary>
	/// Welles Wilder の方向性指数。
	/// </summary>
	[Browsable(false)]
	public DirectionalIndex Dx { get; private set; }

	/// <summary>
	/// 移動平均。
	/// </summary>
	[Browsable(false)]
	public LengthIndicator<decimal> MovingAverage { get; private set; }

	/// <summary>
	/// 期間の長さ。
	/// </summary>
	[DisplayName("Period")]
	[Description("Indicator period.")]
	[Category("Main")]
	public virtual int Length
	{
		get { return MovingAverage.Length; }
		set
		{
			MovingAverage.Length = Dx.Length = value;
			Reset();
		}
	}
}
```

このようなインジケーターは [BaseComplexIndicator](xref:StockSharp.Algo.Indicators.BaseComplexIndicator) クラスを継承し、インジケーターのコンポーネントを [BaseComplexIndicator.InnerIndicators](xref:StockSharp.Algo.Indicators.BaseComplexIndicator.InnerIndicators) に渡す必要があります。さらに、各複合インジケーターは `ComplexIndicatorValue` から派生した独自の値型を宣言する必要があります。

## SaveLoad 実装を含む複合インジケーターの例

以下は Percentage Volume Oscillator (PVO) の実装例です。`NumValuesToInitialize`、`Measure`、および `Save` と `Load` メソッドの実装を示しています。

```cs
/// <summary>
/// Percentage Volume Oscillator (PVO).
/// </summary>
[Display(
	ResourceType = typeof(LocalizedStrings),
	Name = LocalizedStrings.PVOKey,
	Description = LocalizedStrings.PercentageVolumeOscillatorKey)]
[IndicatorIn(typeof(CandleIndicatorValue))]
[Doc("topics/api/indicators/list_of_indicators/percentage_volume_oscillator.html")]
[IndicatorOut(typeof(PercentageVolumeOscillatorValue))]
public class PercentageVolumeOscillator : BaseComplexIndicator<PercentageVolumeOscillatorValue>
{
	private readonly ExponentialMovingAverage _shortEma;
	private readonly ExponentialMovingAverage _longEma;

	/// <summary>
	/// Initializes a new instance of the <see cref="PercentageVolumeOscillator"/>.
	/// </summary>
	public PercentageVolumeOscillator()
		: this(new(), new())
	{
		ShortPeriod = 12;
		LongPeriod = 26;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="PercentageVolumeOscillator"/>.
	/// </summary>
	/// <param name="shortEma">The short-term EMA.</param>
	/// <param name="longEma">The long-term EMA.</param>
	public PercentageVolumeOscillator(ExponentialMovingAverage shortEma, ExponentialMovingAverage longEma)
		: base(shortEma, longEma)
	{
		_shortEma = shortEma;
		_longEma = longEma;
	}

	/// <summary>
	/// 短期期間。
	/// </summary>
	[Display(
		ResourceType = typeof(LocalizedStrings),
		Name = LocalizedStrings.ShortPeriodKey,
		Description = LocalizedStrings.ShortMaDescKey,
		GroupName = LocalizedStrings.GeneralKey)]
	public int ShortPeriod
	{
		get => _shortEma.Length;
		set => _shortEma.Length = value;
	}

	/// <summary>
	/// 長期期間。
	/// </summary>
	[Display(
		ResourceType = typeof(LocalizedStrings),
		Name = LocalizedStrings.LongPeriodKey,
		Description = LocalizedStrings.LongMaDescKey,
		GroupName = LocalizedStrings.GeneralKey)]
	public int LongPeriod
	{
		get => _longEma.Length;
		set => _longEma.Length = value;
	}

	/// <inheritdoc />
	public override IndicatorMeasures Measure => IndicatorMeasures.Volume;

	/// <inheritdoc />
	public override int NumValuesToInitialize => _shortEma.NumValuesToInitialize.Max(_longEma.NumValuesToInitialize);

	/// <inheritdoc />
	protected override bool CalcIsFormed() => _shortEma.IsFormed && _longEma.IsFormed;

	/// <inheritdoc />
	protected override IIndicatorValue OnProcess(IIndicatorValue input)
	{
		var volume = input.ToCandle().TotalVolume;

		var result = new PercentageVolumeOscillatorValue(this, input.Time);

		var shortValue = _shortEma.Process(input, volume);
		var longValue = _longEma.Process(input, volume);

		result.Add(_shortEma, shortValue);
		result.Add(_longEma, longValue);

		if (_longEma.IsFormed)
		{
			var den = longValue.ToDecimal();
			var pvo = den == 0 ? 0 : ((shortValue.ToDecimal() - den) / den) * 100;
			result.Add(this, new DecimalIndicatorValue(this, pvo, input.Time));
		}

		return result;
	}

	/// <inheritdoc />
	public override void Save(SettingsStorage storage)
	{
		base.Save(storage);

		storage.SetValue(nameof(ShortPeriod), ShortPeriod);
		storage.SetValue(nameof(LongPeriod), LongPeriod);
	}

	/// <inheritdoc />
	public override void Load(SettingsStorage storage)
	{
		base.Load(storage);

		ShortPeriod = storage.GetValue<int>(nameof(ShortPeriod));
		LongPeriod = storage.GetValue<int>(nameof(LongPeriod));
	}

	/// <inheritdoc />
	public override string ToString() => base.ToString() + $" S={ShortPeriod},L={LongPeriod}";

	/// <inheritdoc />
	protected override PercentageVolumeOscillatorValue CreateValue(DateTimeOffset time)
		=> new(this, time);
}
```

```cs
/// <summary>
/// <see cref="PercentageVolumeOscillator"/> indicator value.
/// </summary>
public class PercentageVolumeOscillatorValue : ComplexIndicatorValue<PercentageVolumeOscillator>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="PercentageVolumeOscillatorValue"/> class.
	/// </summary>
	/// <param name="indicator">Indicator.</param>
	/// <param name="time">Value time.</param>
	public PercentageVolumeOscillatorValue(PercentageVolumeOscillator indicator, DateTimeOffset time)
			: base(indicator, time)
	{
	}
}
```

この例は次の内容を示しています。
1. 複合インジケーターに対する `NumValuesToInitialize` の実装
2. `Measure` プロパティによる測定タイプの指定
3. 複合インジケーター専用の値型の実装
4. パラメーターを保存および読み込むための `Save` と `Load` メソッドの正しい実装
5. インジケーター構成を便利に表示するための `ToString()` のオーバーライド

