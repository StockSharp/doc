# 图形用户界面

## S# 的图形组件

[S#](../api.md) 包含大量自己的图形组件。这些组件位于 [StockSharp.Xaml](xref:StockSharp.Xaml)、[StockSharp.Xaml.Charting](xref:StockSharp.Xaml.Charting) 和 [StockSharp.Xaml.Diagram](xref:StockSharp.Xaml.Diagram) 命名空间中。

[S#](../api.md) 提供了各种控件用于：

- 搜索并选择数据（交易品种、投资组合、地址）；
- 订单创建；
- 显示交易所及其他信息（交易、订单、交易记录、订单簿、日志等）；
- 图表绘制。

要在 XAML 代码中访问 [S#](../api.md) 图形控件，必须为相应的命名空间定义别名，并在 XAML 代码中使用这些别名。如何操作请参见以下示例：

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
