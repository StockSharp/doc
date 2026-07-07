# Interface gráfica de utilizador

## Componentes gráficos do S#

[S#](../api.md) inclui um grande número de componentes gráficos próprios. Os componentes estão colocados nos namespaces [StockSharp.Xaml](xref:StockSharp.Xaml), [StockSharp.Xaml.Charting](xref:StockSharp.Xaml.Charting) e [StockSharp.Xaml.Diagram](xref:StockSharp.Xaml.Diagram).

[S#](../api.md) tem uma variedade de controlos para:

- pesquisar e selecionar dados (instrumentos, portfólios, endereços);
- a criação de ordens;
- apresentar informação de bolsa e outra informação (negócios, ordens, transações, livros de ordens, logs, etc.);
- construção de gráficos.

Para aceder aos controlos gráficos [S#](../api.md) no código XAML, é necessário definir os aliases para o namespace correspondente e utilizar estes aliases no código XAML. O exemplo seguinte mostra como fazer isto:

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
