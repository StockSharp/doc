# Стакан

![GUI MarketDepthControl](../images/GUI_MarketDepthControl.png)

[MarketDepthControl](xref:StockSharp.Xaml.MarketDepthControl) \- графический компонент для отображения стакана. Компонент позволяет отображать котировки и собственные заявки. 

**Основные свойства и методы**

- [MaxDepth](xref:StockSharp.Xaml.MarketDepthControl.MaxDepth) \- глубина стакана.
- [IsBidsOnTop](xref:StockSharp.Xaml.MarketDepthControl.IsBidsOnTop) \- отображать покупки сверху.
- [UpdateFormat](xref:StockSharp.Xaml.MarketDepthControl.UpdateFormat(StockSharp.BusinessEntities.Security)) \- обновить формат отображения цен и объёмов при помощи инструмента.
- [ProcessOrder](xref:StockSharp.Xaml.MarketDepthControl.ProcessOrder(StockSharp.BusinessEntities.Order,System.Decimal,StockSharp.Messages.OrderStates)) \- обработать заявку.
- [UpdateDepth](xref:StockSharp.Xaml.MarketDepthControl.UpdateDepth(StockSharp.BusinessEntities.MarketDepth)) \- обновить стакан.
- [UpdateDepth](xref:StockSharp.Xaml.MarketDepthControl.UpdateDepth(StockSharp.Messages.QuoteChangeMessage)) \- обновить стакан при помощи сообщения.

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
