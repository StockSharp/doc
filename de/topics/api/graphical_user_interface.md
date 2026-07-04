# Grafische Benutzeroberfläche

## Grafische Komponenten von S#

[S#](../api.md) enthält eine große Anzahl eigener grafischer Komponenten. Die Komponenten befinden sich in den Namespaces [StockSharp.Xaml](xref:StockSharp.Xaml), [StockSharp.Xaml.Charting](xref:StockSharp.Xaml.Charting) und [StockSharp.Xaml.Diagram](xref:StockSharp.Xaml.Diagram). 

[S#](../api.md) bietet verschiedene Steuerelemente für: 

- Suche und Auswahl von Daten (Instrumente, Portfolios, Adressen);
- das Erstellen von Orders;
- Anzeige von Börsen- und anderen Informationen (Trades, Orders, Transaktionen, Orderbücher, Logs usw.);
- Zeichnen von Diagrammen.

Um im XAML-Code auf die grafischen Steuerelemente von [S#](../api.md) zuzugreifen, müssen Sie Aliase für den entsprechenden Namespace definieren und diese Aliase im XAML-Code verwenden. Wie das funktioniert, zeigt das folgende Beispiel:

```xaml
<Window x:Class="SampleSmartSMA.MainWindow"
		xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
		xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
		xmlns:loc="clr-namespace:StockSharp.Localization;assembly=StockSharp.Localization"
		xmlns:sx="clr-namespace:StockSharp.Xaml;assembly=StockSharp.Xaml"
		xmlns:charting="http://schemas.stocksharp.com/xaml"
		Title="{x:Static loc:LocalizedStrings.XamlStr570}" Height="700" Width="900">
	
	<Grid>
	</Grid>
</Window>
	
```
