# Стакан

![GUI MarketDepthControl](../images/GUI_MarketDepthControl.png)

[MarketDepthControl](../api/StockSharp.Xaml.MarketDepthControl.html) \- графический компонент для отображения стакана. Компонент позволяет отображать котировки и собственные заявки. 

**Основные свойства и методы**

- [MaxDepth](../api/StockSharp.Xaml.MarketDepthControl.MaxDepth.html) \- глубина стакана.
- [IsBidsOnTop](../api/StockSharp.Xaml.MarketDepthControl.IsBidsOnTop.html) \- отображать покупки сверху.
- [UpdateFormat](../api/StockSharp.Xaml.MarketDepthControl.UpdateFormat.html) \- обновить формат отображения цен и объёмов при помощи инструмента.
- [ProcessOrder](../api/StockSharp.Xaml.MarketDepthControl.ProcessOrder.html) \- обработать заявку.
- [UpdateDepth](../api/StockSharp.Xaml.MarketDepthControl.UpdateDepth.html) \- обновить стакан.
- [UpdateDepth](../api/StockSharp.Xaml.MarketDepthControl.UpdateDepth.html) \- обновить стакан при помощи сообщения.

Ниже показаны фрагменты кода с его использованием. Пример кода взят из *Samples\/Common\/SampleConnection*. 

```xaml
<Window x:Class="SampleBarChart.QuotesWindow"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:xaml="http://schemas.stocksharp.com/xaml"
    Title="QuotesWindow" Height="600" Width="280">
	<xaml:MarketDepthControl x:Name="DepthCtrl" x:FieldModifier="public" />
</Window>
	  				
```
```cs
private void ConnectorOnMarketDepthsChanged(IEnumerable<MarketDepth> depths)
{
	foreach (var depth in depths)
	{
		var wnd = _quotesWindows.TryGetValue(depth.Security);
		if (wnd != null)
			wnd.DepthCtrl.UpdateDepth(depth);
	}
}
	  				
```
