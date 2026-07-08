# Annotationen

[S#](../../../api.md) bietet die Möglichkeit, Annotationen in Form von Text, Linien usw. zum Diagramm hinzuzufügen.

![ChartAnnotations](../../../../images/chartannotations.png)

Das Hinzufügen von Annotationen erfolgt genauso wie das Hinzufügen anderer Informationen zum Diagramm. Zunächst müssen Sie eine [ChartAnnotation](xref:StockSharp.Xaml.Charting.ChartAnnotation) erstellen und dem Diagrammbereich hinzufügen:

```cs
var _annotation = new ChartAnnotation { Type = ChartAnnotationTypes.BoxAnnotation };
Chart.AddElement(chartArea, _annotation);

```

Danach müssen Sie eine neue Instanz der Klasse [AnnotationData](xref:StockSharp.Xaml.Charting.ChartDrawData.AnnotationData) initialisieren, in der die Annotation beschrieben wird, und sie zum Zeichnen an die Methode [IChart.Draw](xref:StockSharp.Charting.IThemeableChart.Draw(StockSharp.Charting.IChartDrawData))**(**[StockSharp.Charting.IChartDrawData](xref:StockSharp.Charting.IChartDrawData) data **)** übergeben:

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
