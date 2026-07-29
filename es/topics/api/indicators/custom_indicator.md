# Indicador personalizado

Para crear su propio indicador, necesita implementar la interfaz [IIndicator](xref:StockSharp.Algo.Indicators.IIndicator). Como ejemplo, puede consultar el código fuente de otros indicadores ubicados en el repositorio [GitHub/StockSharp](https://github.com/StockSharp/StockSharp). Así es como se ve la implementación de la media móvil simple [SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage):

```cs
/// <summary>
/// Media móvil simple.
/// </summary>
[DisplayName("SMA")]
[Description("Media móvil simple.")]
public class SimpleMovingAverage : LengthIndicator<decimal>
{
	/// <summary>
	/// Crea <see cref="SimpleMovingAverage"/>.
	/// </summary>
	public SimpleMovingAverage()
	{
		Length = 32;
	}

	/// <summary>
	/// Valor de entrada del proceso.
	/// </summary>
	/// <param name="input">Valor de entrada.</param>
	/// <returns>Valor resultante.</returns>
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


[SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage) hereda del [LengthIndicator\<TResult\>](xref:StockSharp.Algo.Indicators.LengthIndicator`1), del cual deben heredar todos los indicadores con un parámetro de duración de período.

## Propiedades y métodos importantes de los indicadores

Al crear un indicador personalizado, se debe prestar especial atención a las siguientes propiedades y métodos:

### NumValuesToInitialize

La propiedad [NumValuesToInitialize](xref:StockSharp.Algo.Indicators.IIndicator.NumValuesToInitialize) indica cuántos valores requiere el indicador para la inicialización (formación o "preparación"). Este valor se utiliza para determinar cuándo se puede considerar que el indicador está formado y listo para usar:

```cs
/// <inheritdoc />
public override int NumValuesToInitialize => Length;
```

Para indicadores más complejos que constan de múltiples componentes, este valor generalmente se determina como el máximo de todas las partes constituyentes:

```cs
/// <inheritdoc />
public override int NumValuesToInitialize => _shortEma.NumValuesToInitialize.Max(_longEma.NumValuesToInitialize);
```

### Measure

La propiedad [Measure](xref:StockSharp.Algo.Indicators.IIndicator.Measure) define el tipo de medida y dimensionalidad que proporciona el indicador:

```cs
/// <inheritdoc />
public override IndicatorMeasures Measure => IndicatorMeasures.Percent;
```

Tipos de medición disponibles:
- `IndicatorMeasures.Price` - indicador mide el precio (por ejemplo, medias móviles)
- `IndicatorMeasures.Percent`: el indicador utiliza una escala porcentual de 0 a 100 (por ejemplo, RSI)
- `IndicatorMeasures.MinusOnePlusOne`: el indicador utiliza una escala de -1 a +1
- `IndicatorMeasures.Volume` - indicador mide el volumen (por ejemplo, OBV)

Esta propiedad es de vital importancia para mostrar correctamente los indicadores en un gráfico. Cuando se superponen varios indicadores con diferentes dimensiones en el mismo panel, se crean ejes Y separados para indicadores con diferentes tipos `Measure`. Esto permite visualizar visualmente todos los indicadores en su escala natural, incluso si uno tiene valores en miles (por ejemplo, precio), mientras que otro se mide en fracciones de unidad (por ejemplo, oscilador).

### Guardar y cargar

Los métodos [Save](xref:StockSharp.Algo.Indicators.BaseIndicator.Save(Ecng.Serialization.SettingsStorage)) y [Load](xref:StockSharp.Algo.Indicators.BaseIndicator.Load(Ecng.Serialization.SettingsStorage)) son necesarios para guardar y cargar la configuración del indicador:

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

## Indicadores compuestos

Algunos indicadores son compuestos y utilizan otros indicadores en sus cálculos. Por lo tanto, los indicadores se pueden reutilizar entre sí, como se demuestra en el ejemplo de implementación del indicador de volatilidad de Chaikin [ChaikinVolatility](xref:StockSharp.Algo.Indicators.ChaikinVolatility):

```cs
/// <summary>
/// Volatilidad de Chaikin.
/// </summary>
[DisplayName("Volatilidad")]
[Description("Volatilidad de Chaikin.")]
public class ChaikinVolatility : BaseIndicator<IIndicatorValue>
{
	/// <summary>
	/// Crea <see cref="ChaikinVolatility"/>.
	/// </summary>
	public ChaikinVolatility()
	{
		Ema = new ExponentialMovingAverage();
		Roc = new RateOfChange();
	}

	/// <summary>
	/// Media móvil.
	/// </summary>
	[ExpandableObject]
	[DisplayName("MA")]
	[Description("Media móvil.")]
	[Category("Principal")]
	public ExponentialMovingAverage Ema { get; private set; }

	/// <summary>
	/// Tasa de cambio.
	/// </summary>
	[ExpandableObject]
	[DisplayName("ROC")]
	[Description("Tasa de cambio.")]
	[Category("Principal")]
	public RateOfChange Roc { get; private set; }

	/// <summary>
	/// ¿Está formado el indicador?
	/// </summary>
	public override bool IsFormed
	{
		get { return Roc.IsFormed; }
	}

	/// <summary>
	/// Valor de entrada del proceso.
	/// </summary>
	/// <param name="input">Valor de entrada.</param>
	/// <returns>Valor resultante.</returns>
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

## Indicadores con varias líneas

El último tipo de indicadores son aquellos que no sólo constan de otros indicadores sino que también se muestran gráficamente con múltiples estados simultáneamente (múltiples líneas). Por ejemplo, [AverageDirectionalIndex](xref:StockSharp.Algo.Indicators.AverageDirectionalIndex):

```cs
/// <summary>
/// Índice direccional promedio de Welles Wilder.
/// </summary>
[DisplayName("ADX")]
[Description("Índice direccional medio de Welles Wilder.")]
public class AverageDirectionalIndex : BaseComplexIndicator
{
	/// <summary>
	/// Crea <see cref="AverageDirectionalIndex"/>.
	/// </summary>
	public AverageDirectionalIndex()
		: this(new DirectionalIndex { Length = 14 }, new WilderMovingAverage { Length = 14 })
	{
	}

	/// <summary>
	/// Crea <see cref="AverageDirectionalIndex"/>.
	/// </summary>
	/// <param name="dx">Índice de movimiento direccional de Welles Wilder.</param>
	/// <param name="movingAverage">Media móvil.</param>
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
	/// Índice de movimiento direccional de Welles Wilder.
	/// </summary>
	[Browsable(false)]
	public DirectionalIndex Dx { get; private set; }

	/// <summary>
	/// Media móvil.
	/// </summary>
	[Browsable(false)]
	public LengthIndicator<decimal> MovingAverage { get; private set; }

	/// <summary>
	/// Longitud Período.
	/// </summary>
	[DisplayName("Período")]
	[Description("Período del indicador.")]
	[Category("Principal")]
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

Dichos indicadores deben heredar de la clase [BaseComplexIndicator](xref:StockSharp.Algo.Indicators.BaseComplexIndicator) y pasar los componentes del indicador a [BaseComplexIndicator.InnerIndicators](xref:StockSharp.Algo.Indicators.BaseComplexIndicator.InnerIndicators). Además, cada indicador complejo debe declarar su propio tipo de valor derivado de `ComplexIndicatorValue`.

## Ejemplo de indicador complejo con implementación de SaveLoad

A continuación se muestra un ejemplo de implementación del oscilador porcentual de volumen (PVO), que demuestra la implementación de los métodos `NumValuesToInitialize`, `Measure`, `Save` y `Load`:

```cs
/// <summary>
/// Oscilador porcentual de volumen (PVO).
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
	/// Inicializa una nueva instancia de <see cref="PercentageVolumeOscillator"/>.
	/// </summary>
	public PercentageVolumeOscillator()
		: this(new(), new())
	{
		ShortPeriod = 12;
		LongPeriod = 26;
	}

	/// <summary>
	/// Inicializa una nueva instancia de <see cref="PercentageVolumeOscillator"/>.
	/// </summary>
	/// <param name="shortEma">EMA de corto plazo.</param>
	/// <param name="longEma">EMA de largo plazo.</param>
	public PercentageVolumeOscillator(ExponentialMovingAverage shortEma, ExponentialMovingAverage longEma)
		: base(shortEma, longEma)
	{
		_shortEma = shortEma;
		_longEma = longEma;
	}

	/// <summary>
	/// Corto período.
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
	/// Largo período.
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
/// Valor del indicador <see cref="PercentageVolumeOscillator"/>.
/// </summary>
public class PercentageVolumeOscillatorValue : ComplexIndicatorValue<PercentageVolumeOscillator>
{
	/// <summary>
	/// Inicializa una nueva instancia de la clase <see cref="PercentageVolumeOscillatorValue"/>.
	/// </summary>
	/// <param name="indicator">Indicador.</param>
	/// <param name="time">Hora del valor.</param>
	public PercentageVolumeOscillatorValue(PercentageVolumeOscillator indicator, DateTimeOffset time)
			: base(indicator, time)
	{
	}
}
```

Este ejemplo demuestra:
1. Implementación de `NumValuesToInitialize` para un indicador complejo
2. Especificación del tipo de medición a través de la propiedad `Measure`
3. Implementación de un tipo de valor dedicado para el indicador complejo
4. Implementaciones correctas de los métodos `Save` y `Load` para guardar y cargar parámetros
5. Anulación de `ToString()` para una visualización cómoda de la configuración del indicador
