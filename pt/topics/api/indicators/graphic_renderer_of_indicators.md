# Renderizador gráfico de indicadores

Alguns indicadores requerem um estilo de desenho especial, como duas linhas para o indicador [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands). Ou pontos para o indicador [Fractals](xref:StockSharp.Algo.Indicators.Fractals). Nestes casos, é necessário especificar explicitamente o renderizador gráfico de indicadores em [ChartIndicatorElement](xref:StockSharp.Xaml.Charting.ChartIndicatorElement).

```cs
var chartIndicatorElement = new ChartIndicatorElement()
{
	IndicatorPainter = new BollingerBandsPainter(),
};
```

Vamos ver como criar um IndicatorPainter personalizado usando o indicador [Fractals](xref:StockSharp.Algo.Indicators.Fractals) como exemplo.

Todos os IndicatorPainters devem herdar da classe base [BaseChartIndicatorPainter\<TIndicator\>](xref:StockSharp.Xaml.Charting.IndicatorPainters.BaseChartIndicatorPainter`1) ou implementar a interface [IChartIndicatorPainter](xref:StockSharp.Charting.IChartIndicatorPainter):

```cs
/// <summary>
/// Elemento gráfico para <see cref="Fractals"/>.
/// </summary>
[Indicator(typeof(Fractals))]
public class FractalsPainter : BaseChartIndicatorPainter<Fractals>
{
	...
}
```

Vamos definir os elementos de gráfico [ChartLineElement](xref:StockSharp.Xaml.Charting.ChartLineElement) que representarão os fractais superiores e inferiores:

```cs
/// <summary>
/// Elemento gráfico para <see cref="Fractals"/>.
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

No construtor de [FractalsPainter](xref:StockSharp.Xaml.Charting.IndicatorPainters.FractalsPainter), definimos os seus valores e propriedades principais, como cor, espessura e estilo de desenho. Depois disso, adicionamo-los como elementos filhos do gráfico:

```cs
...

/// <summary>
/// Criar instância.
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

Substitua o método [OnDraw](xref:StockSharp.Xaml.Charting.IndicatorPainters.BaseChartIndicatorPainter`1.OnDraw(`0,System.Collections.Generic.IDictionary{StockSharp.Algo.Indicators.IIndicator,System.Collections.Generic.IList{StockSharp.Xaml.Charting.ChartDrawData.IndicatorData}})), no qual desenhamos o indicador usando o método [DrawValues](xref:StockSharp.Xaml.Charting.IndicatorPainters.BaseChartIndicatorPainter`1.DrawValues(System.Collections.Generic.IList{StockSharp.Xaml.Charting.ChartDrawData.IndicatorData},StockSharp.Charting.IChartElement,System.Func{StockSharp.Xaml.Charting.ChartDrawData.IndicatorData,System.Double})):

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

Código completo de [FractalsPainter](xref:StockSharp.Xaml.Charting.IndicatorPainters.FractalsPainter):

```cs
/// <summary>
/// Elemento gráfico para <see cref="Fractals"/>.
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
	/// Criar instância.
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
