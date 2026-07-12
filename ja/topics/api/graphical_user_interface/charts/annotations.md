# 注釈

[S#](../../../api.md) は、テキスト、線などの形式でチャートに注釈を追加する機能を提供します。

![注釈 のスクリーンショット](../../../../images/chartannotations.png)

注釈の追加は、チャートに他の情報を追加する場合と同じです。まず [ChartAnnotation](xref:StockSharp.Xaml.Charting.ChartAnnotation) を作成し、チャート領域に追加する必要があります。

```cs
var _annotation = new ChartAnnotation { Type = ChartAnnotationTypes.BoxAnnotation };
Chart.AddElement(chartArea, _annotation);
		
```

その後、[AnnotationData](xref:StockSharp.Xaml.Charting.ChartDrawData.AnnotationData) クラスの新しいインスタンスを初期化し、その中で注釈を記述して、チャートに描画するために [IChart.Draw](xref:StockSharp.Charting.IThemeableChart.Draw(StockSharp.Charting.IChartDrawData))**(**[StockSharp.Charting.IChartDrawData](xref:StockSharp.Charting.IChartDrawData) data **)** メソッドへ渡す必要があります。

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
	Text = "新しい注釈",
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
