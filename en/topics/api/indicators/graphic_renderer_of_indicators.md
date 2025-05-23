# Graphic Renderer of Indicators

Some indicators require a special drawing style, such as two lines for the [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands) indicator. Or points for the [Fractals](xref:StockSharp.Algo.Indicators.Fractals) indicator. In these cases, you need to explicitly specify the graphic renderer of indicators in [ChartIndicatorElement](xref:StockSharp.Xaml.Charting.ChartIndicatorElement).

```cs
var chartIndicatorElement = new ChartIndicatorElement()
{
	IndicatorPainter = new BollingerBandsPainter(),
};
```

Let's consider how to create a custom IndicatorPainter using the [Fractals](xref:StockSharp.Algo.Indicators.Fractals) indicator as an example.

All IndicatorPainters must inherit from the base class [BaseChartIndicatorPainter\<TIndicator\>](xref:StockSharp.Xaml.Charting.IndicatorPainters.BaseChartIndicatorPainter`1) or implement the interface [IChartIndicatorPainter](xref:StockSharp.Charting.IChartIndicatorPainter):

```cs
/// <summary>
/// The chart element for <see cref="Fractals"/>.
/// </summary>
[Indicator(typeof(Fractals))]
public class FractalsPainter : BaseChartIndicatorPainter<Fractals>
{
	...
}
```

Let's define the chart elements [ChartLineElement](xref:StockSharp.Xaml.Charting.ChartLineElement) that will represent the upper and lower fractals:

```cs
/// <summary>
/// The chart element for <see cref="Fractals"/>.
/// </summary>
[Indicator(typeof(Fractals))]
public class FractalsPainter : BaseChartIndicatorPainter<Fractals>
{
	/// <summary>
	/// <see cref="Fractals.Up"/> dots color.
	/// </summary>
	[Display(
		ResourceType = typeof(LocalizedStrings),
		Name = LocalizedStrings.Str2035Key,
		Description = LocalizedStrings.Str2036Key)]
	public ChartLineElement Up { get; }

	/// <summary>
	/// <see cref="Fractals.Down"/> dots color.
	/// </summary>
	[Display(
		ResourceType = typeof(LocalizedStrings),
		Name = LocalizedStrings.Str2037Key,
		Description = LocalizedStrings.Str2038Key)]
	public ChartLineElement Down { get; }
	...
}
```

In the constructor of [FractalsPainter](xref:StockSharp.Xaml.Charting.IndicatorPainters.FractalsPainter), we set their values and main properties such as color, thickness, and drawing style. After that, we add them as child elements of the chart:

```cs
...

/// <summary>
/// Create instance.
/// </summary>
public FractalsPainter()
{
	Up = new ChartLineElement { Color = Colors.Green };
	Down = new ChartLineElement { Color = Colors.Red };
	Up.Style = Down.Style = DrawStyles.Dot;
	Up.StrokeThickness = Down.StrokeThickness = 4;
	AddChildElement(Up);
	AddChildElement(Down);
}

...
```

Override the [OnDraw](xref:StockSharp.Xaml.Charting.IndicatorPainters.BaseChartIndicatorPainter`1.OnDraw(`0,System.Collections.Generic.IDictionary{StockSharp.Algo.Indicators.IIndicator,System.Collections.Generic.IList{StockSharp.Xaml.Charting.ChartDrawData.IndicatorData}})) method, in which we draw the indicator using the [DrawValues](xref:StockSharp.Xaml.Charting.IndicatorPainters.BaseChartIndicatorPainter`1.DrawValues(System.Collections.Generic.IList{StockSharp.Xaml.Charting.ChartDrawData.IndicatorData},StockSharp.Charting.IChartElement,System.Func{StockSharp.Xaml.Charting.ChartDrawData.IndicatorData,System.Double})) method:

```cs
...
/// <inheritdoc />
protected override bool OnDraw(Fractals ind, IDictionary<IIndicator, IList<ChartDrawData.IndicatorData>> data)
{
	var result = false;
	result |= DrawValues(data[ind.Down], Down);
	result |= DrawValues(data[ind.Up], Up);
	return result;
}
...
```

Complete code of [FractalsPainter](xref:StockSharp.Xaml.Charting.IndicatorPainters.FractalsPainter):

```cs
/// <summary>
/// The chart element for <see cref="Fractals"/>.
/// </summary>
[Indicator(typeof(Fractals))]
public class FractalsPainter : BaseChartIndicatorPainter<Fractals>
{
	/// <summary>
	/// <see cref="Fractals.Up"/> dots color.
	/// </summary>
	[Display(
		ResourceType = typeof(LocalizedStrings),
		Name = LocalizedStrings.Str2035Key,
		Description = LocalizedStrings.Str2036Key)]
	public ChartLineElement Up { get; }
	/// <summary>
	/// <see cref="Fractals.Down"/> dots color.
	/// </summary>
	[Display(
		ResourceType = typeof(LocalizedStrings),
		Name = LocalizedStrings.Str2037Key,
		Description = LocalizedStrings.Str2038Key)]
	public ChartLineElement Down { get; }
	/// <summary>
	/// Create instance.
	/// </summary>
	public FractalsPainter()
	{
		Up = new ChartLineElement { Color = Colors.Green };
		Down = new ChartLineElement { Color = Colors.Red };
		Up.Style = Down.Style = DrawStyles.Dot;
		Up.StrokeThickness = Down.StrokeThickness = 4;
		AddChildElement(Up);
		AddChildElement(Down);
	}
	/// <inheritdoc />
	protected override bool OnDraw(Fractals ind, IDictionary<IIndicator, IList<ChartDrawData.IndicatorData>> data)
	{
		var result = false;
		result |= DrawValues(data[ind.Down], Down);
		result |= DrawValues(data[ind.Up], Up);
		return result;
	}
	#region IPersistable
	/// <inheritdoc />
	public override void Load(SettingsStorage storage)
	{
		base.Load(storage);
		Up.Load(storage.GetValue<SettingsStorage>(nameof(Up)));
		Down.Load(storage.GetValue<SettingsStorage>(nameof(Down)));
	}
	/// <inheritdoc />
	public override void Save(SettingsStorage storage)
	{
		base.Save(storage);
		storage.SetValue(nameof(Up), Up.Save());
		storage.SetValue(nameof(Down), Down.Save());
	}
	#endregion
}
```