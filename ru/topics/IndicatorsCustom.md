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

[SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage) наследуется от класса [LengthIndicator\`1](xref:StockSharp.Algo.Indicators.LengthIndicator`1), от которого необходимо наследовать все индикаторы, имеющие в качестве параметра длину периода. 

Некоторые индикаторы являются составными, и используют в своих рассчетах другие индикаторы. Поэтому индикаторы можно переиспользовать друг из друга, как показано в качестве примера реализация индикатора волатильности Чайкина [ChaikinVolatility](xref:StockSharp.Algo.Indicators.ChaikinVolatility): 

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
