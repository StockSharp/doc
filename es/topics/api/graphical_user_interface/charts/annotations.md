# Anotaciones

[S#](../../../api.md) proporciona la posibilidad de agregar anotaciones al gráfico en forma de texto, líneas, etc.

![Captura de Anotaciones](../../../../images/chartannotations.png)

Agregar anotaciones se realiza igual que agregar cualquier otra información al gráfico. Primero debe crear [ChartAnnotation](xref:StockSharp.Xaml.Charting.ChartAnnotation) y agregarlo al área del gráfico:

```cs
var _annotation = new ChartAnnotation { Type = ChartAnnotationTypes.BoxAnnotation };
Chart.AddElement(chartArea, _annotation);
		
```

Después, debe inicializar una nueva instancia de la clase [AnnotationData](xref:StockSharp.Xaml.Charting.ChartDrawData.AnnotationData), describir en ella la anotación y pasarla al método [IChart.Draw](xref:StockSharp.Charting.IThemeableChart.Draw(StockSharp.Charting.IChartDrawData))**(**datos [StockSharp.Charting.IChartDrawData](xref:StockSharp.Charting.IChartDrawData) **)** para dibujarla en el gráfico:

```cs
var data = new ChartDrawData.AnnotationData
{
	X1 = new DateTimeOffset(2017, 10, 02, 8, 30, 0, TimeSpan.FromHours(1)),
	X2 = new DateTimeOffset(2017, 10, 02, 10, 30, 0, TimeSpan.FromHours(1)),
	Y1 = 193.5m,
	Y2 = 194m,
	IsVisible = true,
	Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 255)),
	Thickness = new Thickness(3),
	Text = "Nueva anotación",
	HorizontalAlignment = HorizontalAlignment.Stretch,
	VerticalAlignment = VerticalAlignment.Stretch,
	LabelPlacement = LabelPlacement.Axis,
	ShowLabel = true,
	CoordinateMode = AnnotationCoordinateMode.Absolute,
};
var drawData = new ChartDrawData();
drawData.Add(_annotation, data);
Chart.Draw(drawData);
		
```
