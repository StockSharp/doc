# Graphic renderer of indicators

Some indicators require a special drawing style, for example, two lines for the [BollingerBands](../api/StockSharp.Algo.Indicators.BollingerBands.html) indicator. Or points for the [Fractals](../api/StockSharp.Algo.Indicators.Fractals.html)indicator. In this case, a graphical indicator renderer must be explicitly specified in [ChartIndicatorElement](../api/StockSharp.Xaml.Charting.ChartIndicatorElement.html)

```cs
			var chartIndicatorElement = new ChartIndicatorElement()
			{
				IndicatorPainter = new BollingerBandsPainter(),
			};
		
```

Consider how to create your own IndicatorPainter on the example of the [Fractals](../api/StockSharp.Algo.Indicators.Fractals.html) indicator.

All IndicatorPainter must be inherited from the [BaseChartIndicatorPainter\`1](../api/StockSharp.Xaml.Charting.IndicatorPainters.BaseChartIndicatorPainter`1.html) base class or the [IChartIndicatorPainter](../api/StockSharp.Xaml.Charting.IChartIndicatorPainter.html) interface:

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

Set [ChartLineElement](../api/StockSharp.Xaml.Charting.ChartLineElement.html) chart elements that will represent upper and lower fractals:

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

In the [FractalsPainter](../api/StockSharp.Xaml.Charting.IndicatorPainters.FractalsPainter.html) designer, we set them the values and basic properties \- the color, thickness, and drawing style. Then add them as the chart child element:

```cs
	...
		/// <summary>
		/// Create instance.
		/// </summary>
		public FractalsPainter()
		{
			Up = new ChartLineElement { Color = Colors.Green };
			Down = new ChartLineElement { Color = Colors.Red };
			Up.Style = Down.Style = ChartIndicatorDrawStyles.Dot;
			Up.StrokeThickness = Down.StrokeThickness = 4;
			AddChildElement(Up);
			AddChildElement(Down);
		}
	...
		
```

Override the [OnDraw](../api/StockSharp.Xaml.Charting.IndicatorPainters.BaseChartIndicatorPainter`1.OnDraw.html) method in which we draw the indicator using the [DrawValues](../api/StockSharp.Xaml.Charting.IndicatorPainters.BaseChartIndicatorPainter`1.DrawValues.html) method:

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

Full [FractalsPainter](../api/StockSharp.Xaml.Charting.IndicatorPainters.FractalsPainter.html) code:

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
			Up.Style = Down.Style = ChartIndicatorDrawStyles.Dot;
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
