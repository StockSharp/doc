# 投资组合

要处理投资组合，[S#](../../api.md) 提供以下图形组件：

- 用于投资组合选择的组合框 - [PortfolioComboBox](xref:StockSharp.Xaml.PortfolioComboBox)。
- 组合框，带有一个按钮，打开一个包含投资组合列表的窗口 \- [PortfolioEditor](xref:StockSharp.Xaml.PropertyGrid.PortfolioEditor)。
- 投资组合列表窗口 - [PortfolioPickerWindow](xref:StockSharp.Xaml.PortfolioPickerWindow)。
- 一个显示有关投资组合和持仓信息的表格 - [PortfolioGrid](xref:StockSharp.Xaml.PortfolioGrid)。

> [!TIP]
> 请注意，在投资组合列表的所有组件中（Portfolios 属性），使用了 **ThreadSafeObservableCollection\<TItem\>** 类（位于 Ecng.Xaml 中），该类提供线程安全性。

## 推荐内容

[下拉列表](portfolios/drop_down_list.md)

[选择器](portfolios/picker.md)

[投资组合选择窗口](portfolios/portfolio_picker_window.md)

[表格](portfolios/table.md)
