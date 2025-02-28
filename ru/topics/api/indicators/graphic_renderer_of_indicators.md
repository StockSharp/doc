# Графический рендерер индикаторов 

Некоторые индикаторы требуют особого стиля прорисовки как, например, для индикатора [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands) это две линии. Или для индикатора [Fractals](xref:StockSharp.Algo.Indicators.Fractals) это точки. В этом случае необходимо явно указывать в [ChartIndicatorElement](xref:StockSharp.Xaml.Charting.ChartIndicatorElement) графический рендерер индикаторов.

```cs
var chartIndicatorElement = new ChartIndicatorElement()
{
	IndicatorPainter = new BollingerBandsPainter(),
};
		
```

Рассмотрим на примере индикатора [Fractals](xref:StockSharp.Algo.Indicators.Fractals) как создать собственный IndicatorPainter. 

Все IndicatorPainter должны быть унаследованны от базового класса [BaseChartIndicatorPainter\<TIndicator\>](xref:StockSharp.Xaml.Charting.IndicatorPainters.BaseChartIndicatorPainter`1) или интерфейса [IChartIndicatorPainter](xref:StockSharp.Charting.IChartIndicatorPainter):

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

Зададим элементы графика [ChartLineElement](xref:StockSharp.Xaml.Charting.ChartLineElement) которые будут представлять верхний и нижний фракталы:

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

В конструкторе [FractalsPainter](xref:StockSharp.Xaml.Charting.IndicatorPainters.FractalsPainter) зададим им значения и основные свойства цвет, толщину и стиль прорисовки. После чего добавляем их как дочерний элемент графика:

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

Переопределяем метод [OnDraw](xref:StockSharp.Xaml.Charting.IndicatorPainters.BaseChartIndicatorPainter`1.OnDraw(`0,System.Collections.Generic.IDictionary{StockSharp.Algo.Indicators.IIndicator,System.Collections.Generic.IList{StockSharp.Xaml.Charting.ChartDrawData.IndicatorData}})), в котором с помощью метода [DrawValues](xref:StockSharp.Xaml.Charting.IndicatorPainters.BaseChartIndicatorPainter`1.DrawValues(System.Collections.Generic.IList{StockSharp.Xaml.Charting.ChartDrawData.IndicatorData},StockSharp.Charting.IChartElement,System.Func{StockSharp.Xaml.Charting.ChartDrawData.IndicatorData,System.Double})) прорисовываем индикатор:

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

Полный код [FractalsPainter](xref:StockSharp.Xaml.Charting.IndicatorPainters.FractalsPainter):

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
