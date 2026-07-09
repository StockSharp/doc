# インジケーターのグラフィックレンダラー

一部のインジケーターでは、[BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands) インジケーターの 2 本線のように、特殊な描画スタイルが必要です。または、[Fractals](xref:StockSharp.Algo.Indicators.Fractals) インジケーターの点のような場合もあります。このような場合は、[ChartIndicatorElement](xref:StockSharp.Xaml.Charting.ChartIndicatorElement) でインジケーターのグラフィックレンダラーを明示的に指定する必要があります。

```cs
var chartIndicatorElement = new ChartIndicatorElement()
{
	IndicatorPainter = new BollingerBandsPainter(),
};
```

[Fractals](xref:StockSharp.Algo.Indicators.Fractals) インジケーターを例に、カスタム IndicatorPainter を作成する方法を見てみましょう。

すべての IndicatorPainter は、基底クラス [BaseChartIndicatorPainter\<TIndicator\>](xref:StockSharp.Xaml.Charting.IndicatorPainters.BaseChartIndicatorPainter`1) を継承するか、[IChartIndicatorPainter](xref:StockSharp.Charting.IChartIndicatorPainter) インターフェイスを実装する必要があります。

```cs
/// <summary>
/// <see cref="Fractals"/> 用のチャート要素。
/// </summary>
[Indicator(typeof(Fractals))]
public class FractalsPainter : BaseChartIndicatorPainter<Fractals>
{
	...
}
```

上側および下側フラクタルを表すチャート要素 [ChartLineElement](xref:StockSharp.Xaml.Charting.ChartLineElement) を定義します。

```cs
/// <summary>
/// <see cref="Fractals"/> 用のチャート要素。
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

[FractalsPainter](xref:StockSharp.Xaml.Charting.IndicatorPainters.FractalsPainter) のコンストラクターで、それらの値と、色、太さ、描画スタイルなどの主なプロパティを設定します。その後、それらをチャートの子要素として追加します。

```cs
...

/// <summary>
/// インスタンスを作成。
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

[OnDraw](xref:StockSharp.Xaml.Charting.IndicatorPainters.BaseChartIndicatorPainter`1.OnDraw(`0,System.Collections.Generic.IDictionary{StockSharp.Algo.Indicators.IIndicator,System.Collections.Generic.IList{StockSharp.Xaml.Charting.ChartDrawData.IndicatorData}})) メソッドをオーバーライドします。このメソッド内で、[DrawValues](xref:StockSharp.Xaml.Charting.IndicatorPainters.BaseChartIndicatorPainter`1.DrawValues(System.Collections.Generic.IList{StockSharp.Xaml.Charting.ChartDrawData.IndicatorData},StockSharp.Charting.IChartElement,System.Func{StockSharp.Xaml.Charting.ChartDrawData.IndicatorData,System.Double})) メソッドを使用してインジケーターを描画します。

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

[FractalsPainter](xref:StockSharp.Xaml.Charting.IndicatorPainters.FractalsPainter) の完全なコード:

```cs
/// <summary>
/// <see cref="Fractals"/> 用のチャート要素。
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
	/// インスタンスを作成。
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

