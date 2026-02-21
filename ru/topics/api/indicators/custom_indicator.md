# Собственный индикатор

Для того, чтобы создать свой собственный индикатор, необходимо реализовать интерфейс [IIndicator](xref:StockSharp.Algo.Indicators.IIndicator). В качестве примера можно взять исходные коды других индикаторов, которые находятся в репозитории [GitHub\/StockSharp](https://github.com/StockSharp/StockSharp). Вот как выглядит код реализации индикатора простой скользящей средней [SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage): 

```cs
/// <summary>
/// Simple moving average.
/// </summary>
[Display(
	ResourceType = typeof(LocalizedStrings),
	Name = LocalizedStrings.SMAKey,
	Description = LocalizedStrings.SimpleMovingAverageKey)]
[Doc("topics/api/indicators/list_of_indicators/sma.html")]
public class SimpleMovingAverage : DecimalLengthIndicator
{
	/// <summary>
	/// Initializes a new instance of the <see cref="SimpleMovingAverage"/>.
	/// </summary>
	public SimpleMovingAverage()
	{
		Length = 32;

		Buffer.Stats = CircularBufferStats.Sum;
	}

	/// <inheritdoc />
	protected override decimal? OnProcessDecimal(IIndicatorValue input)
	{
		var newValue = input.ToDecimal(Source);

		if (input.IsFinal)
		{
			Buffer.PushBack(newValue);
			return Buffer.Sum / Length;
		}

		return (Buffer.SumNoFirst + newValue) / Length;
	}
}
```

[SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage) наследуется от класса [DecimalLengthIndicator](xref:StockSharp.Algo.Indicators.DecimalLengthIndicator), от которого необходимо наследовать все индикаторы, имеющие в качестве параметра длину периода и работающие с числовыми значениями типа `decimal`. Метод `OnProcessDecimal` возвращает `decimal?` \- если вернуть `null`, будет создано пустое значение индикатора. Буфер `Buffer` является циклическим (`CircularBuffer`), а метод `PushBack` автоматически удаляет самый старый элемент при превышении вместимости.

## Важные свойства и методы индикаторов

При создании собственного индикатора особое внимание следует уделить следующим свойствам и методам:

### NumValuesToInitialize

Свойство [NumValuesToInitialize](xref:StockSharp.Algo.Indicators.IIndicator.NumValuesToInitialize) указывает, сколько значений требуется индикатору для инициализации (формирования или "прогрева"). Это значение используется для определения, когда индикатор можно считать сформированным и готовым к использованию:

```cs
/// <inheritdoc />
public override int NumValuesToInitialize => Length;
```

Для более сложных индикаторов, состоящих из нескольких компонентов, это значение обычно определяется как максимальное из всех составных частей:

```cs
/// <inheritdoc />
public override int NumValuesToInitialize => ShortEma.NumValuesToInitialize.Max(LongEma.NumValuesToInitialize);
```

### Measure

Свойство [Measure](xref:StockSharp.Algo.Indicators.IIndicator.Measure) определяет тип измерения и размерность, которую предоставляет индикатор:

```cs
/// <inheritdoc />
public override IndicatorMeasures Measure => IndicatorMeasures.Percent;
```

Доступные типы измерений:
- `IndicatorMeasures.Price` - индикатор измеряет цену (например, скользящие средние)
- `IndicatorMeasures.Percent` - индикатор использует процентную шкалу от 0 до 100 (например, RSI)
- `IndicatorMeasures.MinusOnePlusOne` - индикатор использует шкалу от -1 до +1
- `IndicatorMeasures.Volume` - индикатор измеряет объем (например, OBV)

Это свойство критически важно для правильного отображения индикаторов на графике. Когда на одну и ту же панель накладываются несколько индикаторов с разными размерностями, для индикаторов с отличающимся типом `Measure` создаются отдельные оси Y. Это позволяет визуально отображать все индикаторы в их естественном масштабе, даже если один из них имеет значения в тысячах (например, цена), а другой измеряется в долях единицы (например, осциллятор).

### Save и Load

Методы [Save](xref:StockSharp.Algo.Indicators.BaseIndicator.Save(Ecng.Serialization.SettingsStorage)) и [Load](xref:StockSharp.Algo.Indicators.BaseIndicator.Load(Ecng.Serialization.SettingsStorage)) необходимы для сохранения и загрузки настроек индикатора:

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

В этих методах необходимо сохранять и загружать все настраиваемые параметры индикатора. Это обеспечивает корректное сохранение и восстановление состояния индикатора при сохранении и загрузке стратегии. 

Важно вызывать базовые методы `base.Save()` и `base.Load()` для обработки общих параметров индикатора, унаследованных от базового класса.

## Составные индикаторы

Некоторые индикаторы являются составными, и используют в своих расчетах другие индикаторы. Поэтому индикаторы можно переиспользовать друг из друга, как показано в качестве примера реализация индикатора волатильности Чайкина [ChaikinVolatility](xref:StockSharp.Algo.Indicators.ChaikinVolatility):

```cs
/// <summary>
/// Chaikin volatility.
/// </summary>
/// <remarks>
/// https://doc.stocksharp.com/topics/api/indicators/list_of_indicators/chv.html
/// </remarks>
[Display(
	ResourceType = typeof(LocalizedStrings),
	Name = LocalizedStrings.ChaikinVolatilityKey,
	Description = LocalizedStrings.ChaikinVolatilityIndicatorKey)]
[IndicatorIn(typeof(CandleIndicatorValue))]
[Doc("topics/api/indicators/list_of_indicators/chv.html")]
public class ChaikinVolatility : BaseIndicator
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ChaikinVolatility"/>.
	/// </summary>
	public ChaikinVolatility()
	{
		Ema = new();
		Roc = new();

		AddResetTracking(Ema);
		AddResetTracking(Roc);
	}

	/// <summary>
	/// Moving Average.
	/// </summary>
	[TypeConverter(typeof(ExpandableObjectConverter))]
	[Display(
		ResourceType = typeof(LocalizedStrings),
		Name = LocalizedStrings.MAKey,
		Description = LocalizedStrings.MovingAverageKey,
		GroupName = LocalizedStrings.GeneralKey)]
	public ExponentialMovingAverage Ema { get; }

	/// <summary>
	/// Rate of change.
	/// </summary>
	[TypeConverter(typeof(ExpandableObjectConverter))]
	[Display(
		ResourceType = typeof(LocalizedStrings),
		Name = LocalizedStrings.ROCKey,
		Description = LocalizedStrings.RateOfChangeKey,
		GroupName = LocalizedStrings.GeneralKey)]
	public RateOfChange Roc { get; }

	/// <inheritdoc />
	protected override bool CalcIsFormed() => Roc.IsFormed;

	/// <inheritdoc />
	public override int NumValuesToInitialize => Ema.NumValuesToInitialize + Roc.NumValuesToInitialize - 1;

	/// <inheritdoc />
	public override IndicatorMeasures Measure => IndicatorMeasures.Percent;

	/// <inheritdoc />
	protected override IIndicatorValue OnProcess(IIndicatorValue input)
	{
		var candle = input.ToCandle();

		var emaValue = Ema.Process(input, candle.GetLength());

		if (Ema.IsFormed)
		{
			var val = Roc.Process(emaValue);
			return new DecimalIndicatorValue(this, val.ToDecimal(Source), input.Time);
		}

		return new DecimalIndicatorValue(this, input.Time);
	}

	/// <inheritdoc />
	public override void Load(SettingsStorage storage)
	{
		base.Load(storage);

		Ema.LoadIfNotNull(storage, nameof(Ema));
		Roc.LoadIfNotNull(storage, nameof(Roc));
	}

	/// <inheritdoc />
	public override void Save(SettingsStorage storage)
	{
		base.Save(storage);

		storage.SetValue(nameof(Ema), Ema.Save());
		storage.SetValue(nameof(Roc), Roc.Save());
	}
}
```

## Индикаторы с несколькими линиями

Последний вид индикаторов \- это те, которые не просто состоят из других индикаторов, но так же графически отображаются несколькими состояниями одновременно (несколькими линиями). Например, [AverageDirectionalIndex](xref:StockSharp.Algo.Indicators.AverageDirectionalIndex): 

```cs
/// <summary>
/// Welles Wilder Average Directional Index.
/// </summary>
[Display(
	ResourceType = typeof(LocalizedStrings),
	Name = LocalizedStrings.AdxKey,
	Description = LocalizedStrings.AverageDirectionalIndexKey)]
[Doc("topics/api/indicators/list_of_indicators/adx.html")]
[IndicatorOut(typeof(IAverageDirectionalIndexValue))]
public class AverageDirectionalIndex : BaseComplexIndicator<IAverageDirectionalIndexValue>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="AverageDirectionalIndex"/>.
	/// </summary>
	public AverageDirectionalIndex()
		: this(new DirectionalIndex { Length = 14 }, new WilderMovingAverage { Length = 14 })
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="AverageDirectionalIndex"/>.
	/// </summary>
	/// <param name="dx">Welles Wilder Directional Movement Index.</param>
	/// <param name="movingAverage">Moving Average.</param>
	public AverageDirectionalIndex(DirectionalIndex dx, DecimalLengthIndicator movingAverage)
		: base(dx, movingAverage)
	{
		Dx = dx;
		MovingAverage = movingAverage;
		Mode = ComplexIndicatorModes.Sequence;
	}

	/// <inheritdoc />
	public override IndicatorMeasures Measure => IndicatorMeasures.Percent;

	/// <summary>
	/// Welles Wilder Directional Movement Index.
	/// </summary>
	[Browsable(false)]
	public DirectionalIndex Dx { get; }

	/// <summary>
	/// Moving Average.
	/// </summary>
	[Browsable(false)]
	public DecimalLengthIndicator MovingAverage { get; }

	/// <summary>
	/// Period length.
	/// </summary>
	[Display(
		ResourceType = typeof(LocalizedStrings),
		Name = LocalizedStrings.PeriodKey,
		Description = LocalizedStrings.IndicatorPeriodKey,
		GroupName = LocalizedStrings.GeneralKey)]
	public int Length
	{
		get => MovingAverage.Length;
		set
		{
			MovingAverage.Length = Dx.Length = value;
			Reset();
		}
	}

	/// <inheritdoc />
	public override void Load(SettingsStorage storage)
	{
		base.Load(storage);
		Length = storage.GetValue<int>(nameof(Length));
	}

	/// <inheritdoc />
	public override void Save(SettingsStorage storage)
	{
		base.Save(storage);
		storage.SetValue(nameof(Length), Length);
	}

	/// <inheritdoc />
	protected override IAverageDirectionalIndexValue CreateValue(DateTime time)
		=> new AverageDirectionalIndexValue(this, time);
}

/// <summary>
/// <see cref="AverageDirectionalIndex"/> indicator value.
/// </summary>
public interface IAverageDirectionalIndexValue : IComplexIndicatorValue
{
	/// <summary>
	/// Gets the <see cref="AverageDirectionalIndex.Dx"/> value.
	/// </summary>
	IDirectionalIndexValue Dx { get; }

	/// <summary>
	/// Gets the <see cref="AverageDirectionalIndex.MovingAverage"/> value.
	/// </summary>
	IIndicatorValue MovingAverageValue { get; }

	/// <summary>
	/// Gets the <see cref="AverageDirectionalIndex.MovingAverage"/> value.
	/// </summary>
	[Browsable(false)]
	decimal? MovingAverage { get; }
}

/// <summary>
/// AverageDirectionalIndex indicator value implementation.
/// </summary>
public class AverageDirectionalIndexValue(AverageDirectionalIndex indicator, DateTime time)
	: ComplexIndicatorValue<AverageDirectionalIndex>(indicator, time), IAverageDirectionalIndexValue
{
	/// <inheritdoc />
	public IDirectionalIndexValue Dx => (IDirectionalIndexValue)this[TypedIndicator.Dx];

	/// <inheritdoc />
	public IIndicatorValue MovingAverageValue => this[TypedIndicator.MovingAverage];

	/// <inheritdoc />
	public decimal? MovingAverage => MovingAverageValue.ToNullableDecimal(TypedIndicator.Source);
}
```

Такие индикаторы должны наследоваться от класса [BaseComplexIndicator\<TValue\>](xref:StockSharp.Algo.Indicators.BaseComplexIndicator`1), где `TValue` \- интерфейс значения, реализующий `IComplexIndicatorValue`. Составные части индикатора передаются через конструктор в `base(...)`. Каждый составной индикатор обязан реализовать собственный интерфейс значения и класс, производный от `ComplexIndicatorValue<T>`, а также переопределить метод `CreateValue(DateTime)`. Атрибут `[IndicatorOut]` указывает на интерфейс значения. [BaseComplexIndicator](xref:StockSharp.Algo.Indicators.BaseComplexIndicator`1) будет обрабатывать данные части один за другим. Если [BaseComplexIndicator.Mode](xref:StockSharp.Algo.Indicators.BaseComplexIndicator`1.Mode) установлено в [ComplexIndicatorModes.Sequence](xref:StockSharp.Algo.Indicators.ComplexIndicatorModes.Sequence), то результирующее значение первого индикатора будет передано в качестве входного значения второму, и так далее до конца. Если же установлено значение [ComplexIndicatorModes.Parallel](xref:StockSharp.Algo.Indicators.ComplexIndicatorModes.Parallel), то результаты вложенных индикаторов игнорируются.

## Пример комплексного индикатора с реализацией SaveLoad

Ниже приведен пример реализации индикатора Percentage Volume Oscillator (PVO), который демонстрирует реализацию свойств `NumValuesToInitialize`, `Measure`, а также методов `Save` и `Load`:

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
[IndicatorOut(typeof(IPercentageVolumeOscillatorValue))]
public class PercentageVolumeOscillator : BaseComplexIndicator<IPercentageVolumeOscillatorValue>
{
	/// <summary>
	/// Short EMA.
	/// </summary>
	[Browsable(false)]
	public ExponentialMovingAverage ShortEma { get; }

	/// <summary>
	/// Long EMA.
	/// </summary>
	[Browsable(false)]
	public ExponentialMovingAverage LongEma { get; }

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
		ShortEma = shortEma;
		LongEma = longEma;
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
		get => ShortEma.Length;
		set => ShortEma.Length = value;
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
		get => LongEma.Length;
		set => LongEma.Length = value;
	}

	/// <inheritdoc />
	public override IndicatorMeasures Measure => IndicatorMeasures.Volume;

	/// <inheritdoc />
	public override int NumValuesToInitialize => ShortEma.NumValuesToInitialize.Max(LongEma.NumValuesToInitialize);

	/// <inheritdoc />
	protected override bool CalcIsFormed() => ShortEma.IsFormed && LongEma.IsFormed;

	/// <inheritdoc />
	protected override IIndicatorValue OnProcess(IIndicatorValue input)
	{
		var volume = input.ToCandle().TotalVolume;

		var result = new PercentageVolumeOscillatorValue(this, input.Time);

		var shortValue = ShortEma.Process(input, volume);
		var longValue = LongEma.Process(input, volume);

		result.Add(ShortEma, shortValue);
		result.Add(LongEma, longValue);

		if (LongEma.IsFormed)
		{
			var den = longValue.ToDecimal(Source);
			var pvo = den == 0 ? 0 : ((shortValue.ToDecimal(Source) - den) / den) * 100;
			result.Add(this, new DecimalIndicatorValue(this, pvo, input.Time) { IsFinal = input.IsFinal });
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
	protected override IPercentageVolumeOscillatorValue CreateValue(DateTime time)
		=> new PercentageVolumeOscillatorValue(this, time);
}
```

```cs
/// <summary>
/// <see cref="PercentageVolumeOscillator"/> indicator value.
/// </summary>
public interface IPercentageVolumeOscillatorValue : IComplexIndicatorValue
{
	/// <summary>
	/// Gets the short EMA value.
	/// </summary>
	IIndicatorValue ShortEmaValue { get; }

	/// <summary>
	/// Gets the short EMA value.
	/// </summary>
	[Browsable(false)]
	decimal? ShortEma { get; }

	/// <summary>
	/// Gets the long EMA value.
	/// </summary>
	IIndicatorValue LongEmaValue { get; }

	/// <summary>
	/// Gets the long EMA value.
	/// </summary>
	[Browsable(false)]
	decimal? LongEma { get; }
}

/// <summary>
/// Percentage Volume Oscillator indicator value implementation.
/// </summary>
public class PercentageVolumeOscillatorValue(PercentageVolumeOscillator indicator, DateTime time)
	: ComplexIndicatorValue<PercentageVolumeOscillator>(indicator, time), IPercentageVolumeOscillatorValue
{
	/// <inheritdoc />
	public IIndicatorValue ShortEmaValue => this[TypedIndicator.ShortEma];
	/// <inheritdoc />
	public decimal? ShortEma => ShortEmaValue.ToNullableDecimal(TypedIndicator.Source);

	/// <inheritdoc />
	public IIndicatorValue LongEmaValue => this[TypedIndicator.LongEma];
	/// <inheritdoc />
	public decimal? LongEma => LongEmaValue.ToNullableDecimal(TypedIndicator.Source);
}
```

Этот пример демонстрирует:
1. Реализацию `NumValuesToInitialize` для комплексного индикатора
2. Указание типа измерения через свойство `Measure`
3. Реализацию интерфейса значения `IPercentageVolumeOscillatorValue` и класса `PercentageVolumeOscillatorValue` с primary constructor синтаксисом
4. Использование `[IndicatorOut(typeof(IPercentageVolumeOscillatorValue))]` с интерфейсом (не конкретным классом)
5. Корректные реализации методов `Save` и `Load` для сохранения и загрузки параметров
6. Переопределение `CreateValue(DateTime)` для создания экземпляра значения индикатора
