# Grafischer Renderer für Indikatoren

Einige Indikatoren erfordern einen speziellen Zeichenstil, zum Beispiel zwei Linien für den Indikator [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands). Oder Punkte für den Indikator [Fractals](xref:StockSharp.Algo.Indicators.Fractals). In solchen Fällen müssen Sie den grafischen Renderer für Indikatoren explizit in [ChartIndicatorElement](xref:StockSharp.Xaml.Charting.ChartIndicatorElement) angeben.

```cs
var chartIndicatorElement = new ChartIndicatorElement()
{
	IndicatorPainter = new BollingerBandsPainter(),
};
```

Betrachten wir anhand des Indikators [Fractals](xref:StockSharp.Algo.Indicators.Fractals), wie ein benutzerdefinierter IndicatorPainter erstellt wird.

Alle IndicatorPainter müssen von der Basisklasse [BaseChartIndicatorPainter\<TIndicator\>](xref:StockSharp.Xaml.Charting.IndicatorPainters.BaseChartIndicatorPainter`1) erben oder das Interface [IChartIndicatorPainter](xref:StockSharp.Charting.IChartIndicatorPainter) implementieren:

```cs
/// <summary>
/// Das Chartelement für <see cref="Fractals"/>.
/// </summary>
[Indicator(typeof(Fractals))]
public class FractalsPainter : BaseChartIndicatorPainter<Fractals>
{
	...
}
```

Definieren wir die Chartelemente [ChartLineElement](xref:StockSharp.Xaml.Charting.ChartLineElement), die die oberen und unteren Fraktale darstellen:

```cs
/// <summary>
/// Das Chartelement für <see cref="Fractals"/>.
/// </summary>
[Indicator(typeof(Fractals))]
public class FractalsPainter : BaseChartIndicatorPainter<Fractals>
{
	/// <summary>
	/// Punktfarbe von <see cref="Fractals.Up"/>.
	/// </summary>
	[Display(
		ResourceType = typeof(LocalizedStrings),
		Name = LocalizedStrings.Str2035Key,
		Description = LocalizedStrings.Str2036Key)]
	public ChartLineElement Up { get; }

	/// <summary>
	/// Punktfarbe von <see cref="Fractals.Down"/>.
	/// </summary>
	[Display(
		ResourceType = typeof(LocalizedStrings),
		Name = LocalizedStrings.Str2037Key,
		Description = LocalizedStrings.Str2038Key)]
	public ChartLineElement Down { get; }
	...
}
```

Im Konstruktor von [FractalsPainter](xref:StockSharp.Xaml.Charting.IndicatorPainters.FractalsPainter) setzen wir deren Werte und wichtigste Eigenschaften wie Farbe, Dicke und Zeichenstil. Danach fügen wir sie als untergeordnete Elemente zum Chart hinzu:

```cs
...

/// <summary>
/// Instanz erstellen.
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

Überschreiben Sie die Methode [OnDraw](xref:StockSharp.Xaml.Charting.IndicatorPainters.BaseChartIndicatorPainter`1.OnDraw(`0,System.Collections.Generic.IDictionary{StockSharp.Algo.Indicators.IIndicator,System.Collections.Generic.IList{StockSharp.Xaml.Charting.ChartDrawData.IndicatorData}})), in der der Indikator mit der Methode [DrawValues](xref:StockSharp.Xaml.Charting.IndicatorPainters.BaseChartIndicatorPainter`1.DrawValues(System.Collections.Generic.IList{StockSharp.Xaml.Charting.ChartDrawData.IndicatorData},StockSharp.Charting.IChartElement,System.Func{StockSharp.Xaml.Charting.ChartDrawData.IndicatorData,System.Double})) gezeichnet wird:

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

Vollständiger Code von [FractalsPainter](xref:StockSharp.Xaml.Charting.IndicatorPainters.FractalsPainter):

```cs
/// <summary>
/// Das Chartelement für <see cref="Fractals"/>.
/// </summary>
[Indicator(typeof(Fractals))]
public class FractalsPainter : BaseChartIndicatorPainter<Fractals>
{
	/// <summary>
	/// Punktfarbe von <see cref="Fractals.Up"/>.
	/// </summary>
	[Display(
		ResourceType = typeof(LocalizedStrings),
		Name = LocalizedStrings.Str2035Key,
		Description = LocalizedStrings.Str2036Key)]
	public ChartLineElement Up { get; }
	/// <summary>
	/// Punktfarbe von <see cref="Fractals.Down"/>.
	/// </summary>
	[Display(
		ResourceType = typeof(LocalizedStrings),
		Name = LocalizedStrings.Str2037Key,
		Description = LocalizedStrings.Str2038Key)]
	public ChartLineElement Down { get; }
	/// <summary>
	/// Instanz erstellen.
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
