# Portfolios

To work with portfolios, [S\#](../../api.md) offers the following graphical components:

- The combo box for portfolio selection \- [PortfolioComboBox](xref:StockSharp.Xaml.PortfolioComboBox).
- The combo box with a button that opens a window with a list of portfolios \- [PortfolioEditor](xref:StockSharp.Xaml.PropertyGrid.PortfolioEditor).
- Portfolio list window \- [PortfolioPickerWindow](xref:StockSharp.Xaml.PortfolioPickerWindow).
- A table that displays information about portfolios and positions \- [PortfolioGrid](xref:StockSharp.Xaml.PortfolioGrid).

> [!TIP]
> Note that in all components for the portfolios list (the Portfolios property), the **ThreadSafeObservableCollection\<TItem\>** class (located in Ecng.Xaml) is used, which provides thread safety. 

## Recommended content

[Drop down list](portfolios/drop_down_list.md)

[Picker](portfolios/picker.md)

[Portfolio picker window](portfolios/portfolio_picker_window.md)

[Table](portfolios/table.md)
