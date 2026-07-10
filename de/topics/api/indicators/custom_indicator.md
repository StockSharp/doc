# Benutzerdefinierter Indikator

Um einen eigenen Indikator zu erstellen, müssen Sie das Interface [IIndicator](xref:StockSharp.Algo.Indicators.IIndicator) implementieren. Als Beispiel können Sie sich den Quellcode anderer Indikatoren im Repository [GitHub/StockSharp](https://github.com/StockSharp/StockSharp) ansehen. So sieht die Implementierung des einfachen gleitenden Durchschnitts [SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage) aus:

```cs
/// <summary>
/// Einfacher gleitender Durchschnitt.
/// </summary>
[DisplayName("SMA")]
[Description("Einfacher gleitender Durchschnitt.")]
public class SimpleMovingAverage : LengthIndicator<decimal>
{
	/// <summary>
	/// <see cref="SimpleMovingAverage"/> erstellen.
	/// </summary>
	public SimpleMovingAverage()
	{
		Length = 32;
	}

	/// <summary>
	/// Eingabewert verarbeiten.
	/// </summary>
	/// <param name="input">Eingabewert.</param>
	/// <returns>Ergebniswert.</returns>
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


[SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage) erbt von [LengthIndicator\<TResult\>](xref:StockSharp.Algo.Indicators.LengthIndicator`1), von dem alle Indikatoren mit einem Parameter für die Periodeenlänge erben müssen.

## Wichtige Indikatoreigenschaften und -methoden

Beim Erstellen eines benutzerdefinierten Indikators sollten die folgenden Eigenschaften und Methoden besonders beachtet werden:

### NumValuesToInitialize

Die Eigenschaft [NumValuesToInitialize](xref:StockSharp.Algo.Indicators.IIndicator.NumValuesToInitialize) gibt an, wie viele Werte der Indikator zur Initialisierung benötigt (Formierung bzw. "Aufwärmphase"). Dieser Wert wird verwendet, um zu bestimmen, wann der Indikator als formiert und einsatzbereit gilt:

```cs
/// <inheritdoc />
public override int NumValuesToInitialize => Length;
```

Bei komplexeren Indikatoren, die aus mehreren Komponenten bestehen, wird dieser Wert normalerweise als Maximum aller Bestandteile bestimmt:

```cs
/// <inheritdoc />
public override int NumValuesToInitialize => _shortEma.NumValuesToInitialize.Max(_longEma.NumValuesToInitialize);
```

### Measure

Die Eigenschaft [Measure](xref:StockSharp.Algo.Indicators.IIndicator.Measure) definiert Messart und Dimension, die der Indikator bereitstellt:

```cs
/// <inheritdoc />
public override IndicatorMeasures Measure => IndicatorMeasures.Percent;
```

Verfügbare Messarten:
- `IndicatorMeasures.Price` - der Indikator misst Preise (z. B. gleitende Durchschnitte)
- `IndicatorMeasures.Percent` - der Indikator verwendet eine Prozentskala von 0 bis 100 (z. B. RSI)
- `IndicatorMeasures.MinusOnePlusOne` - der Indikator verwendet eine Skala von -1 bis +1
- `IndicatorMeasures.Volume` - der Indikator misst Volumen (z. B. OBV)

Diese Eigenschaft ist entscheidend für die korrekte Anzeige von Indikatoren in einem Chart. Wenn mehrere Indikatoren mit unterschiedlichen Dimensionen im selben Panel überlagert werden, werden für Indikatoren mit unterschiedlichen `Measure`-Typen separate Y-Achsen erstellt. Dadurch können alle Indikatoren in ihrer natürlichen Skala angezeigt werden, selbst wenn ein Indikator Werte im Tausenderbereich hat (z. B. Preis), während ein anderer in Bruchteilen einer Einheit gemessen wird (z. B. Oszillator).

### Save und Load

Die Methoden [Save](xref:StockSharp.Algo.Indicators.BaseIndicator.Save(Ecng.Serialization.SettingsStorage)) und [Load](xref:StockSharp.Algo.Indicators.BaseIndicator.Load(Ecng.Serialization.SettingsStorage)) sind erforderlich, um Indikatoreinstellungen zu speichern und zu laden:

```cs
/// <inheritdoc />
public override void Save(SettingsStorage storage)
{
	base.Save(storage);

	storage.SetValue(nameof(ShortPeriode), ShortPeriode);
	storage.SetValue(nameof(LongPeriode), LongPeriode);
}

/// <inheritdoc />
public override void Load(SettingsStorage storage)
{
	base.Load(storage);

	ShortPeriode = storage.GetValue<int>(nameof(ShortPeriode));
	LongPeriode = storage.GetValue<int>(nameof(LongPeriode));
}
```

## Zusammengesetzte Indikatoren

Einige Indikatoren sind zusammengesetzt und verwenden andere Indikatoren in ihren Berechnungen. Daher können Indikatoren gegenseitig wiederverwendet werden, wie im Beispiel der Implementierung des Chaikin-Volatilität-Indikators [ChaikinVolatilität](xref:StockSharp.Algo.Indicators.ChaikinVolatility) gezeigt:

```cs
/// <summary>
/// Chaikin-Volatilität.
/// </summary>
[DisplayName("Volatilität")]
[Description("Chaikin-Volatilität.")]
public class ChaikinVolatilität : BaseIndicator<IIndicatorValue>
{
	/// <summary>
	/// <see cref="ChaikinVolatilität"/> erstellen.
	/// </summary>
	public ChaikinVolatilität()
	{
		Ema = new ExponentialMovingAverage();
		Roc = new RateOfChange();
	}

	/// <summary>
	/// Gleitender Durchschnitt.
	/// </summary>
	[ExpandableObject]
	[DisplayName("MA")]
	[Description("Gleitender Durchschnitt.")]
	[Category("Hauptgruppe")]
	public ExponentialMovingAverage Ema { get; private set; }

	/// <summary>
	/// Änderungsrate.
	/// </summary>
	[ExpandableObject]
	[DisplayName("ROC")]
	[Description("Änderungsrate.")]
	[Category("Hauptgruppe")]
	public RateOfChange Roc { get; private set; }

	/// <summary>
	/// Gibt an, ob der Indikator formiert ist.
	/// </summary>
	public override bool IsFormed
	{
		get { return Roc.IsFormed; }
	}

	/// <summary>
	/// Eingabewert verarbeiten.
	/// </summary>
	/// <param name="input">Eingabewert.</param>
	/// <returns>Ergebniswert.</returns>
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

## Indikatoren mit mehreren Linien

Der letzte Typ von Indikatoren besteht nicht nur aus anderen Indikatoren, sondern wird auch gleichzeitig mit mehreren Zuständen grafisch dargestellt (mehrere Linien). Ein Beispiel ist [AverageDirectionalIndex](xref:StockSharp.Algo.Indicators.AverageDirectionalIndex):

```cs
/// <summary>
/// Welles Wilders Average Directional Index.
/// </summary>
[DisplayName("ADX")]
[Description("Welles Wilders durchschnittlicher Richtungsindex.")]
public class AverageDirectionalIndex : BaseComplexIndicator
{
	/// <summary>
	/// <see cref="AverageDirectionalIndex"/> erstellen.
	/// </summary>
	public AverageDirectionalIndex()
		: this(new DirectionalIndex { Length = 14 }, new WilderMovingAverage { Length = 14 })
	{
	}

	/// <summary>
	/// <see cref="AverageDirectionalIndex"/> erstellen.
	/// </summary>
	/// <param name="dx">Welles Wilders Directional Movement Index.</param>
	/// <param name="movingAverage">Gleitender Durchschnitt.</param>
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
	/// Welles Wilders Directional Movement Index.
	/// </summary>
	[Browsable(false)]
	public DirectionalIndex Dx { get; private set; }

	/// <summary>
	/// Gleitender Durchschnitt.
	/// </summary>
	[Browsable(false)]
	public LengthIndicator<decimal> MovingAverage { get; private set; }

	/// <summary>
	/// Periodeenlänge.
	/// </summary>
	[DisplayName("Periode")]
	[Description("Indikatorperiode.")]
	[Category("Hauptgruppe")]
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

Solche Indikatoren sollten von der Klasse [BaseComplexIndicator](xref:StockSharp.Algo.Indicators.BaseComplexIndicator) erben und die Komponenten des Indikators an [BaseComplexIndicator.InnerIndicators](xref:StockSharp.Algo.Indicators.BaseComplexIndicator.InnerIndicators) übergeben. Außerdem muss jeder komplexe Indikator einen eigenen Werttyp deklarieren, der von `ComplexIndicatorValue` abgeleitet ist.

## Beispiel eines komplexen Indikators mit SaveLoad-Implementierung

Unten ist ein Beispiel für die Implementierung des Percentage Volume Oscillator (PVO), das die Implementierung von `NumValuesToInitialize`, `Measure` sowie der Methoden `Save` und `Load` demonstriert:

```cs
/// <summary>
/// Prozentualer Volumenoszillator (PVO).
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
	/// Initialisiert eine neue Instanz von <see cref="PercentageVolumeOscillator"/>.
	/// </summary>
	public PercentageVolumeOscillator()
		: this(new(), new())
	{
		ShortPeriode = 12;
		LongPeriode = 26;
	}

	/// <summary>
	/// Initialisiert eine neue Instanz von <see cref="PercentageVolumeOscillator"/>.
	/// </summary>
	/// <param name="shortEma">Die kurzfristige EMA.</param>
	/// <param name="longEma">Die langfristige EMA.</param>
	public PercentageVolumeOscillator(ExponentialMovingAverage shortEma, ExponentialMovingAverage longEma)
		: base(shortEma, longEma)
	{
		_shortEma = shortEma;
		_longEma = longEma;
	}

	/// <summary>
	/// Kurze Periodee.
	/// </summary>
	[Display(
		ResourceType = typeof(LocalizedStrings),
		Name = LocalizedStrings.ShortPeriodeKey,
		Description = LocalizedStrings.ShortMaDescKey,
		GroupName = LocalizedStrings.GeneralKey)]
	public int ShortPeriode
	{
		get => _shortEma.Length;
		set => _shortEma.Length = value;
	}

	/// <summary>
	/// Länge Periodee.
	/// </summary>
	[Display(
		ResourceType = typeof(LocalizedStrings),
		Name = LocalizedStrings.LongPeriodeKey,
		Description = LocalizedStrings.LongMaDescKey,
		GroupName = LocalizedStrings.GeneralKey)]
	public int LongPeriode
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

		storage.SetValue(nameof(ShortPeriode), ShortPeriode);
		storage.SetValue(nameof(LongPeriode), LongPeriode);
	}

	/// <inheritdoc />
	public override void Load(SettingsStorage storage)
	{
		base.Load(storage);

		ShortPeriode = storage.GetValue<int>(nameof(ShortPeriode));
		LongPeriode = storage.GetValue<int>(nameof(LongPeriode));
	}

	/// <inheritdoc />
	public override string ToString() => base.ToString() + $" S={ShortPeriode},L={LongPeriode}";

	/// <inheritdoc />
	protected override PercentageVolumeOscillatorValue CreateValue(DateTimeOffset time)
		=> new(this, time);
}
```

```cs
/// <summary>
/// Wert des Indikators <see cref="PercentageVolumeOscillator"/>.
/// </summary>
public class PercentageVolumeOscillatorValue : ComplexIndicatorValue<PercentageVolumeOscillator>
{
	/// <summary>
	/// Initialisiert eine neue Instanz der Klasse <see cref="PercentageVolumeOscillatorValue"/>.
	/// </summary>
	/// <param name="indicator">Indikator.</param>
	/// <param name="time">Zeit des Werts.</param>
	public PercentageVolumeOscillatorValue(PercentageVolumeOscillator indicator, DateTimeOffset time)
			: base(indicator, time)
	{
	}
}
```

Dieses Beispiel demonstriert:
1. Implementierung von `NumValuesToInitialize` für einen komplexen Indikator
2. Angabe der Messart über die Eigenschaft `Measure`
3. Implementierung eines eigenen Werttyps für den komplexen Indikator
4. Korrekte Implementierungen der Methoden `Save` und `Load` zum Speichern und Laden von Parametern
5. Überschreiben von `ToString()` für eine bequeme Anzeige der Indikatorkonfiguration
