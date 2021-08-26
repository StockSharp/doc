# Графический рендерер индикаторов 

Некоторые индикаторы требуют особого стиля прорисовки как, например, для индикатора [BollingerBands](../api/StockSharp.Algo.Indicators.BollingerBands.html) это две линии. Или для индикатора [Fractals](../api/StockSharp.Algo.Indicators.Fractals.html) это точки. В этом случае необходимо явно указывать в [ChartIndicatorElement](../api/StockSharp.Xaml.Charting.ChartIndicatorElement.html) графический рендерер индикаторов.

```cs
			var chartIndicatorElement = new ChartIndicatorElement()
			{
				IndicatorPainter = new BollingerBandsPainter(),
			};
		
```

Рассмотрим на примере индикатора [Fractals](../api/StockSharp.Algo.Indicators.Fractals.html) как создать собственный IndicatorPainter. 

Все IndicatorPainter должны быть унаследованны от базового класса [BaseChartIndicatorPainter\`1](../api/StockSharp.Xaml.Charting.IndicatorPainters.BaseChartIndicatorPainter`1.html) или интерфейса [IChartIndicatorPainter](../api/StockSharp.Xaml.Charting.IChartIndicatorPainter.html):

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

Зададим элементы графика [ChartLineElement](../api/StockSharp.Xaml.Charting.ChartLineElement.html) которые будут представлять верхний и нижний фракталы:

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

В конструкторе [FractalsPainter](../api/StockSharp.Xaml.Charting.IndicatorPainters.FractalsPainter.html) зададим им значения и основные свойства цвет, толщину и стиль прорисовки. После чего добавляем их как дочерний элемент графика:

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

Переопределяем метод [OnDraw](../api/StockSharp.Xaml.Charting.IndicatorPainters.BaseChartIndicatorPainter`1.OnDraw.html) в котором с помощью метода [DrawValues](../api/StockSharp.Xaml.Charting.IndicatorPainters.BaseChartIndicatorPainter`1.DrawValues.html) прорисовываем индикатор:

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

Полный код [FractalsPainter](../api/StockSharp.Xaml.Charting.IndicatorPainters.FractalsPainter.html):

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
