# Собственный индикатор

Для того, чтобы создать свой собственный индикатор, необходимо реализовать интерфейс [IIndicator](xref:StockSharp.Algo.Indicators.IIndicator). В качестве примера можно взять исходные коды других индикаторов, которые находятся в репозитории [GitHub\/StockSharp](https://github.com/StockSharp/StockSharp). Вот как выглядит код реализации индикатора простой скользящей средней [SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage): 

```cs
/// <summary>
/// Простая скользящая средняя.
/// </summary>
[DisplayName("SMA")]
[Description("Простая скользящая средняя.")]
public class SimpleMovingAverage : LengthIndicator<decimal>
{
	/// <summary>
	/// Создать <see cref="SimpleMovingAverage"/>.
	/// </summary>
	public SimpleMovingAverage()
	{
		Length = 32;
	}
	/// <summary>
	/// Обработать входное значение.
	/// </summary>
	/// <param name="input">Входное значение.</param>
	/// <returns>Результирующее значение.</returns>
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

[SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage) наследуется от класса [LengthIndicator\<TResult\>](xref:StockSharp.Algo.Indicators.LengthIndicator`1), от которого необходимо наследовать все индикаторы, имеющие в качестве параметра длину периода. 

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
public override int NumValuesToInitialize => _shortEma.NumValuesToInitialize.Max(_longEma.NumValuesToInitialize);
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
/// Волатильность Чайкина.
/// </summary>
/// <remarks>
/// http://www2.wealth-lab.com/WL5Wiki/Volatility.ashx
/// http://www.incrediblecharts.com/indicators/chaikin_volatility.php
/// </remarks>
[DisplayName("Волатильность")]
[Description("Волатильность Чайкина.")]
public class ChaikinVolatility : BaseIndicator<IIndicatorValue>
{
	/// <summary>
	/// Создать <see cref="ChaikinVolatility"/>.
	/// </summary>
	public ChaikinVolatility()
	{
		Ema = new ExponentialMovingAverage();
		Roc = new RateOfChange();
	}
	/// <summary>
	/// Скользящая средняя.
	/// </summary>
	[ExpandableObject]
	[DisplayName("MA")]
	[Description("Скользящая средняя.")]
	[Category("Основное")]
	public ExponentialMovingAverage Ema { get; private set; }
	/// <summary>
	/// Скорость изменения.
	/// </summary>
	[ExpandableObject]
	[DisplayName("ROC")]
	[Description("Скорость изменения.")]
	[Category("Основное")]
	public RateOfChange Roc { get; private set; }
	/// <summary>
	/// Сформирован ли индикатор.
	/// </summary>
	public override bool IsFormed
	{
		get { return Roc.IsFormed; }
	}
	/// <summary>
	/// Обработать входное значение.
	/// </summary>
	/// <param name="input">Входное значение.</param>
	/// <returns>Результирующее значение.</returns>
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

## Индикаторы с несколькими линиями

Последний вид индикаторов \- это те, которые не просто состоят из других индикаторов, но так же графически отображаются несколькими состояниями одновременно (несколькими линиями). Например, [AverageDirectionalIndex](xref:StockSharp.Algo.Indicators.AverageDirectionalIndex): 

```cs
/// <summary>
/// Индекс среднего направления движения Welles Wilder.
/// </summary>
[DisplayName("ADX")]
[Description("Индекс среднего направления движения Welles Wilder.")]
public class AverageDirectionalIndex : BaseComplexIndicator
{
	/// <summary>
	/// Создать <see cref="AverageDirectionalIndex"/>.
	/// </summary>
	public AverageDirectionalIndex()
		: this(new DirectionalIndex { Length = 14 }, new WilderMovingAverage { Length = 14 })
	{
	}
	/// <summary>
	/// Создать <see cref="AverageDirectionalIndex"/>.
	/// </summary>
	/// <param name="dx">Индекса направленного движения Welles Wilder.</param>
	/// <param name="movingAverage">Скользящая средняя.</param>
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
	/// Индекса направленного движения Welles Wilder.
	/// </summary>
	[Browsable(false)]
	public DirectionalIndex Dx { get; private set; }
	/// <summary>
	/// Скользящая средняя.
	/// </summary>
	[Browsable(false)]
	public LengthIndicator<decimal> MovingAverage { get; private set; }
	/// <summary>
	/// Длина периода.
	/// </summary>
	[DisplayName("Период")]
	[Description("Период индикатора.")]
	[Category("Основные")]
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

Такие индикаторы должны наследоваться от класса [BaseComplexIndicator](xref:StockSharp.Algo.Indicators.BaseComplexIndicator), и передавать в [BaseComplexIndicator.InnerIndicators](xref:StockSharp.Algo.Indicators.BaseComplexIndicator.InnerIndicators) составные части индикатора. [BaseComplexIndicator](xref:StockSharp.Algo.Indicators.BaseComplexIndicator) будет обрабатывать данные части один за другим. Если [BaseComplexIndicator.Mode](xref:StockSharp.Algo.Indicators.BaseComplexIndicator.Mode) установлено в [ComplexIndicatorModes.Sequence](xref:StockSharp.Algo.Indicators.ComplexIndicatorModes.Sequence), то результирующее значение первого индикатора будет передано в качестве входного значения второму, и так далее до конца. Если же установлено значение [ComplexIndicatorModes.Parallel](xref:StockSharp.Algo.Indicators.ComplexIndicatorModes.Parallel), то результаты вложенных индикаторов игнорируются.

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
public class PercentageVolumeOscillator : BaseComplexIndicator
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

		var result = new ComplexIndicatorValue(this, input.Time);

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
}
```

Этот пример демонстрирует:
1. Реализацию `NumValuesToInitialize` для комплексного индикатора
2. Указание типа измерения через свойство `Measure`
3. Корректные реализации методов `Save` и `Load` для сохранения и загрузки параметров
4. Переопределение `ToString()` для удобного отображения конфигурации индикатора
