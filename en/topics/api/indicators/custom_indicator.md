# Custom Indicator

To create your own indicator, you need to implement the [IIndicator](xref:StockSharp.Algo.Indicators.IIndicator) interface. As an example, you can look at the source code of other indicators located in the [GitHub/StockSharp](https://github.com/StockSharp/StockSharp) repository. Here's what the implementation of the Simple Moving Average [SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage) looks like:

```cs
/// <summary>
/// Simple Moving Average.
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
	/// Process input value.
	/// </summary>
	/// <param name="input">Input value.</param>
	/// <returns>Resulting value.</returns>
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


[SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage) inherits from the [LengthIndicator\<TResult\>](xref:StockSharp.Algo.Indicators.LengthIndicator`1), from which all indicators with a period length parameter must inherit.

## Important Indicator Properties and Methods

When creating a custom indicator, special attention should be paid to the following properties and methods:

### NumValuesToInitialize

The [NumValuesToInitialize](xref:StockSharp.Algo.Indicators.IIndicator.NumValuesToInitialize) property indicates how many values the indicator requires for initialization (formation or "warm-up"). This value is used to determine when the indicator can be considered formed and ready to use:

```cs
/// <inheritdoc />
public override int NumValuesToInitialize => Length;
```

For more complex indicators consisting of multiple components, this value is usually determined as the maximum of all constituent parts:

```cs
/// <inheritdoc />
public override int NumValuesToInitialize => _shortEma.NumValuesToInitialize.Max(_longEma.NumValuesToInitialize);
```

### Measure

The [Measure](xref:StockSharp.Algo.Indicators.IIndicator.Measure) property defines the type of measurement and dimensionality provided by the indicator:

```cs
/// <inheritdoc />
public override IndicatorMeasures Measure => IndicatorMeasures.Percent;
```

Available measurement types:
- `IndicatorMeasures.Price` - indicator measures price (e.g., moving averages)
- `IndicatorMeasures.Percent` - indicator uses percentage scale from 0 to 100 (e.g., RSI)
- `IndicatorMeasures.MinusOnePlusOne` - indicator uses scale from -1 to +1
- `IndicatorMeasures.Volume` - indicator measures volume (e.g., OBV)

This property is critically important for correctly displaying indicators on a chart. When multiple indicators with different dimensions are overlaid on the same panel, separate Y-axes are created for indicators with different `Measure` types. This allows visually displaying all indicators in their natural scale, even if one has values in thousands (e.g., price), while another is measured in fractions of a unit (e.g., oscillator).

### Save and Load

The [Save](xref:StockSharp.Algo.Indicators.BaseIndicator.Save(Ecng.Serialization.SettingsStorage)) and [Load](xref:StockSharp.Algo.Indicators.BaseIndicator.Load(Ecng.Serialization.SettingsStorage)) methods are necessary for saving and loading indicator settings:

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

## Composite Indicators

Some indicators are composite and use other indicators in their calculations. Therefore, indicators can be reused from each other, as demonstrated in the example implementation of the Chaikin Volatility indicator [ChaikinVolatility](xref:StockSharp.Algo.Indicators.ChaikinVolatility):

```cs
/// <summary>
/// Chaikin Volatility.
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
	/// Moving Average.
	/// </summary>
	[ExpandableObject]
	[DisplayName("MA")]
	[Description("Moving Average.")]
	[Category("Main")]
	public ExponentialMovingAverage Ema { get; private set; }

	/// <summary>
	/// Rate of Change.
	/// </summary>
	[ExpandableObject]
	[DisplayName("ROC")]
	[Description("Rate of Change.")]
	[Category("Main")]
	public RateOfChange Roc { get; private set; }

	/// <summary>
	/// Is the indicator formed.
	/// </summary>
	public override bool IsFormed
	{
		get { return Roc.IsFormed; }
	}

	/// <summary>
	/// Process input value.
	/// </summary>
	/// <param name="input">Input value.</param>
	/// <returns>Resulting value.</returns>
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

## Indicators with Multiple Lines

The last type of indicators is those that not only consist of other indicators but are also graphically displayed with multiple states simultaneously (multiple lines). For example, [AverageDirectionalIndex](xref:StockSharp.Algo.Indicators.AverageDirectionalIndex):

```cs
/// <summary>
/// Welles Wilder's Average Directional Index.
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
	/// Welles Wilder's Directional Movement Index.
	/// </summary>
	[Browsable(false)]
	public DirectionalIndex Dx { get; private set; }

	/// <summary>
	/// Moving Average.
	/// </summary>
	[Browsable(false)]
	public LengthIndicator<decimal> MovingAverage { get; private set; }

	/// <summary>
	/// Period length.
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

Such indicators should inherit from the [BaseComplexIndicator](xref:StockSharp.Algo.Indicators.BaseComplexIndicator) class and pass the indicator's components to [BaseComplexIndicator.InnerIndicators](xref:StockSharp.Algo.Indicators.BaseComplexIndicator.InnerIndicators). In addition, each complex indicator must declare its own value type derived from `ComplexIndicatorValue`.

## Example of a Complex Indicator with SaveLoad Implementation

Below is an example of implementing the Percentage Volume Oscillator (PVO), which demonstrates the implementation of `NumValuesToInitialize`, `Measure`, and `Save` and `Load` methods:

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
	/// Short period.
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
	/// Long period.
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

This example demonstrates:
1. Implementation of `NumValuesToInitialize` for a complex indicator
2. Specifying measurement type through the `Measure` property
3. Implementation of a dedicated value type for the complex indicator
4. Correct implementations of `Save` and `Load` methods for saving and loading parameters
5. Overriding `ToString()` for convenient display of indicator configuration
