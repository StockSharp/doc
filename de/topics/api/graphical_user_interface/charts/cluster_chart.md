# Cluster-Chart

ClusterChart ist ein spezieller Diagrammtyp zur Anzeige von Volumina als Cluster mit Balkendiagrammen. Um diesen Diagrammtyp zu verwenden, müssen Sie den speziellen Stil [IChartCandleElement.DrawStyle](xref:StockSharp.Charting.IChartCandleElement.DrawStyle) \= [ChartCandleDrawStyles.ClusterProfile](xref:StockSharp.Charting.ChartCandleDrawStyles.ClusterProfile) setzen. Dieses Diagramm verwendet die Informationen aus der Eigenschaft [PriceLevels](xref:StockSharp.Messages.CandleMessage.PriceLevels) als Quelldaten.

![GUI Clusterdiagramm](../../../../images/gui_clasterchart.png)

**Haupteigenschaften**

- [ChartCandleElement.ClusterLineColor](xref:StockSharp.Xaml.Charting.ChartCandleElement.ClusterLineColor) - grundlegende Farbe der Cluster-Linien.
- [ChartCandleElement.ClusterTextColor](xref:StockSharp.Xaml.Charting.ChartCandleElement.ClusterTextColor) - Farbe der Volumenwerte im Diagramm.
- [ChartCandleElement.ClusterColor](xref:StockSharp.Xaml.Charting.ChartCandleElement.ClusterColor) - Hauptfarbe der Balken in den Cluster-Histogrammen.
- [ChartCandleElement.ClusterMaxColor](xref:StockSharp.Xaml.Charting.ChartCandleElement.ClusterMaxColor) - Farbe des Balkens mit maximalem Volumen in den Cluster-Histogrammen.

Ein Beispiel für die Verwendung dieses Diagrammtyps befindet sich in *Samples/Common/SampleChart*.

> [!TIP]
> Um zwischen Diagrammtypen zu wechseln, verwenden Sie die Einstellungsschaltfläche (Zahnrad) in der oberen linken Ecke des Diagramms.
