# 自定义指标

要创建您自己的指标，您需要实现 [IIndicator](xref:StockSharp.Algo.Indicators.IIndicator) 接口。作为示例，您可以查看位于 [GitHub/StockSharp](https://github.com/StockSharp/StockSharp) 仓库中的其他指标源代码。下面是简单移动平均 [SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage) 的实现示例：

```cs
/// <summary>
/// 简单移动平均。
/// </summary>
[DisplayName("SMA")]
[Description("简单移动平均线。")]
public class SimpleMovingAverage : LengthIndicator<decimal>
{
	/// <summary>
	/// 创建 <see cref="SimpleMovingAverage"/>。
	/// </summary>
	public SimpleMovingAverage()
	{
		Length = 32;
	}

	/// <summary>
	/// 处理输入值。
	/// </summary>
	/// <param name="input">输入值。</param>
	/// <returns>结果值。</returns>
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


[SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage) 继承自 [LengthIndicator\<TResult\>](xref:StockSharp.Algo.Indicators.LengthIndicator`1)，所有具有周期长度参数的指标都必须继承自它。

## 重要指标属性和方法

在创建自定义指标时，应该特别注意以下属性和方法：

### 要初始化的值数量

[NumValuesToInitialize](xref:StockSharp.Algo.Indicators.IIndicator.NumValuesToInitialize) 属性表示指标初始化（形成或“预热”）所需的值数量。该值用于确定何时可以认为指标已形成并可以使用：

```cs
/// <inheritdoc />
public override int NumValuesToInitialize => Length;
```

对于由多个组成部分构成的更复杂的指标，该值通常确定为所有组成部分的最大值：

```cs
/// <inheritdoc />
public override int NumValuesToInitialize => _shortEma.NumValuesToInitialize.Max(_longEma.NumValuesToInitialize);
```

### 测量

[Measure](xref:StockSharp.Algo.Indicators.IIndicator.Measure) 属性定义了指标提供的测量类型和维度：

```cs
/// <inheritdoc />
public override IndicatorMeasures Measure => IndicatorMeasures.Percent;
```

可用的测量类型：
- `IndicatorMeasures.Price` - 指标衡量价格（例如， 移动平均线）
- `IndicatorMeasures.Percent` - 指标使用从 0 到 100 的百分比刻度 (例如， RSI)
- `IndicatorMeasures.MinusOnePlusOne` - 指标使用从 -1 到 +1 的刻度
- `IndicatorMeasures.Volume` - 指标用于测量交易量 (例如， OBV)

该属性对于在图表上正确显示指标至关重要。当在同一面板上叠加具有不同维度的多个指标时，会为具有不同 `Measure` 类型的指标创建独立的 Y 轴。这可以让所有指标以其自然的比例进行可视化显示，即使一个指标的数值以千为单位（e.g，价格），而另一个指标以单位的小数部分为单位（e.g，振荡器）。

### 保存与加载

[保存](xref:StockSharp.Algo.Indicators.BaseIndicator.Save(Ecng.Serialization.SettingsStorage)) 和 [加载](xref:StockSharp.Algo.Indicators.BaseIndicator.Load(Ecng.Serialization.SettingsStorage)) 方法对于保存和加载指标设置是必要的：

```cs
/// <inheritdoc />
public override void Save(SettingsStorage storage)
{
	base.Save(storage);

	storage.SetValue(nameof(Short周期), Short周期);
	storage.SetValue(nameof(Long周期), Long周期);
}

/// <inheritdoc />
public override void Load(SettingsStorage storage)
{
	base.Load(storage);

	Short周期 = storage.GetValue<int>(nameof(Short周期));
	Long周期 = storage.GetValue<int>(nameof(Long周期));
}
```

## 综合指标

有些指标是复合指标，并在其计算中使用其他指标。因此，指标可以相互重用，如Chaikin波动率指标 [Chaikin波动率](xref:StockSharp.Algo.Indicators.ChaikinVolatility) 的示例实现中所示：

```cs
/// <summary>
/// Chaikin 波动率。
/// </summary>
[DisplayName("波动率")]
[Description("Chaikin 波动率。")]
public class Chaikin波动率 : BaseIndicator<IIndicatorValue>
{
	/// <summary>
	/// 创建 <see cref="Chaikin波动率"/>。
	/// </summary>
	public Chaikin波动率()
	{
		Ema = new ExponentialMovingAverage();
		Roc = new RateOfChange();
	}

	/// <summary>
	/// 移动平均。
	/// </summary>
	[ExpandableObject]
	[DisplayName("MA")]
	[Description("移动平均。")]
	[Category("主要")]
	public ExponentialMovingAverage Ema { get; private set; }

	/// <summary>
	/// 变化率。
	/// </summary>
	[ExpandableObject]
	[DisplayName("ROC")]
	[Description("变动率。")]
	[Category("主要")]
	public RateOfChange Roc { get; private set; }

	/// <summary>
	/// 指标是否已形成。
	/// </summary>
	public override bool IsFormed
	{
		get { return Roc.IsFormed; }
	}

	/// <summary>
	/// 处理输入值。
	/// </summary>
	/// <param name="input">输入值。</param>
	/// <returns>结果值。</returns>
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

## 多线指标

最后一种指标是那些不仅由其他指标组成，而且还可以同时以多种状态（多条线）图形显示的指标。例如，[平均趋向指数](xref:StockSharp.Algo.Indicators.AverageDirectionalIndex)：

```cs
/// <summary>
/// Welles Wilder 平均趋向指数。
/// </summary>
[DisplayName("ADX")]
[Description("Welles Wilder 平均趋向指数。")]
public class AverageDirectionalIndex : BaseComplexIndicator
{
	/// <summary>
	/// 创建 <see cref="AverageDirectionalIndex"/>。
	/// </summary>
	public AverageDirectionalIndex()
		: this(new DirectionalIndex { Length = 14 }, new WilderMovingAverage { Length = 14 })
	{
	}

	/// <summary>
	/// 创建 <see cref="AverageDirectionalIndex"/>。
	/// </summary>
	/// <param name="dx">Welles Wilder 的方向运动指数。</param>
	/// <param name="movingAverage">移动平均。</param>
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
	/// Welles Wilder 趋向运动指数。
	/// </summary>
	[Browsable(false)]
	public DirectionalIndex Dx { get; private set; }

	/// <summary>
	/// 移动平均。
	/// </summary>
	[Browsable(false)]
	public LengthIndicator<decimal> MovingAverage { get; private set; }

	/// <summary>
	/// 周期长度。
	/// </summary>
	[DisplayName("周期")]
	[Description("指标周期。")]
	[Category("主要")]
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

此类指标应继承自 [BaseComplexIndicator](xref:StockSharp.Algo.Indicators.BaseComplexIndicator) 类，并将指标的组件传递给 [BaseComplexIndicator.InnerIndicators](xref:StockSharp.Algo.Indicators.BaseComplexIndicator.InnerIndicators)。此外，每个复杂指标必须声明其自己派生自 `ComplexIndicatorValue` 的值类型。

## 具有保存加载实现的复杂指标示例

以下是实现百分比成交量振荡器（PVO）的示例，演示了 `NumValuesToInitialize`、`Measure`、`Save` 和 `Load` 方法的实现：

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
	/// 初始化 <see cref="PercentageVolumeOscillator"/> 的新实例。
	/// </summary>
	public PercentageVolumeOscillator()
		: this(new(), new())
	{
		Short周期 = 12;
		Long周期 = 26;
	}

	/// <summary>
	/// 初始化 <see cref="PercentageVolumeOscillator"/> 的新实例。
	/// </summary>
	/// <param name="shortEma">短期 EMA。</param>
	/// <param name="longEma">长期 EMA。</param>
	public PercentageVolumeOscillator(ExponentialMovingAverage shortEma, ExponentialMovingAverage longEma)
		: base(shortEma, longEma)
	{
		_shortEma = shortEma;
		_longEma = longEma;
	}

	/// <summary>
	/// 短周期。
	/// </summary>
	[Display(
		ResourceType = typeof(LocalizedStrings),
		Name = LocalizedStrings.Short周期Key,
		Description = LocalizedStrings.ShortMaDescKey,
		GroupName = LocalizedStrings.GeneralKey)]
	public int Short周期
	{
		get => _shortEma.Length;
		set => _shortEma.Length = value;
	}

	/// <summary>
	/// 长周期。
	/// </summary>
	[Display(
		ResourceType = typeof(LocalizedStrings),
		Name = LocalizedStrings.Long周期Key,
		Description = LocalizedStrings.LongMaDescKey,
		GroupName = LocalizedStrings.GeneralKey)]
	public int Long周期
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

		storage.SetValue(nameof(Short周期), Short周期);
		storage.SetValue(nameof(Long周期), Long周期);
	}

	/// <inheritdoc />
	public override void Load(SettingsStorage storage)
	{
		base.Load(storage);

		Short周期 = storage.GetValue<int>(nameof(Short周期));
		Long周期 = storage.GetValue<int>(nameof(Long周期));
	}

	/// <inheritdoc />
	public override string ToString() => base.ToString() + $" S={Short周期},L={Long周期}";

	/// <inheritdoc />
	protected override PercentageVolumeOscillatorValue CreateValue(DateTimeOffset time)
		=> new(this, time);
}
```

```cs
/// <summary>
/// <see cref="PercentageVolumeOscillator"/> 指标值。
/// </summary>
public class PercentageVolumeOscillatorValue : ComplexIndicatorValue<PercentageVolumeOscillator>
{
	/// <summary>
	/// 初始化 <see cref="PercentageVolumeOscillatorValue"/> 类的新实例。
	/// </summary>
	/// <param name="indicator">Indicator.</param>
	/// <param name="time">Value time.</param>
	public PercentageVolumeOscillatorValue(PercentageVolumeOscillator indicator, DateTimeOffset time)
			: base(indicator, time)
	{
	}
}
```

这个例子演示了：
1. 复杂指标的 `NumValuesToInitialize` 实施
2. 通过 `Measure` 属性指定测量类型
3. 为复合指标实现专用值类型
4. `Save` 和 `Load` 方法用于保存和加载参数的正确实现
5. 覆盖 `ToString()` 以便于指示器配置的显示
