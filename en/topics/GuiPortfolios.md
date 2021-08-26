# Portfolios

To work with portfolios, [S\#](StockSharpAbout.md) offers the following graphical components:

- The combo box for portfolio selection \- [PortfolioComboBox](../api/StockSharp.Xaml.PortfolioComboBox.html).
- The combo box with a button that opens a window with a list of portfolios \- [PortfolioEditor](../api/StockSharp.Xaml.PropertyGrid.PortfolioEditor.html).
- Portfolio list window \- [PortfolioPickerWindow](../api/StockSharp.Xaml.PortfolioPickerWindow.html).
- A table that displays information about portfolios and positions \- [PortfolioGrid](../api/StockSharp.Xaml.PortfolioGrid.html).

> [!TIP]
> Note that in all components for the portfolios list (the Portfolios property), the **ThreadSafeObservableCollection\<TItem\>** class (located in Ecng.Xaml) is used, which provides thread safety. 

## Recommended content

[Drop down list](GuiPortfolioComboBox.md)

[Picker](GuiPortfolioEditor.md)

[Portfolio picker window](GuiPortfolioPickerWindow.md)

[Table](GuiPortfolioGrid.md)
