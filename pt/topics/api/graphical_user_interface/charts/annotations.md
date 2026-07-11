# Anotações

[S#](../../../api.md) fornece a possibilidade de adicionar anotações ao gráfico na forma de texto, linhas, etc.

![Captura de tela de Anotações](../../../../images/chartannotations.png)

Adicionar anotações é igual a adicionar qualquer outra informação ao gráfico. Primeiro, é necessário criar [ChartAnnotation](xref:StockSharp.Xaml.Charting.ChartAnnotation) e adicioná-lo à área do gráfico:

```cs
var _annotation = new ChartAnnotation { Type = ChartAnnotationTypes.BoxAnnotation };
Chart.AddElement(chartArea, _annotation);
		
```

Depois disso, é necessário inicializar uma nova instância da classe [AnnotationData](xref:StockSharp.Xaml.Charting.ChartDrawData.AnnotationData), na qual se descreve a anotação, e passá-la para o método [IChart.Draw](xref:StockSharp.Charting.IThemeableChart.Draw(StockSharp.Charting.IChartDrawData))**(**[StockSharp.Charting.IChartDrawData](xref:StockSharp.Charting.IChartDrawData) data **)** para desenhar no gráfico:

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
	Text = "New annotation",
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
