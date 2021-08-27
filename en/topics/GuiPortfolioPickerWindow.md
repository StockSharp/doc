# Portfolio picker window

[PortfolioPickerWindow](xref:StockSharp.Xaml.PortfolioPickerWindow) is the window for selecting a portfolio. The window displays a list of portfolios and information about the cash positions of portfolios.

![GUI PortfolioPickerWindow](../images/GUI_PortfolioPickerWindow.png)

**Main properties**

- [Portfolios](xref:StockSharp.Xaml.PortfolioPickerWindow.Portfolios) \- the list of portfolios.
- [SelectedPortfolio](xref:StockSharp.Xaml.PortfolioPickerWindow.SelectedPortfolio) \- the selected portfolio.

Below is the code snippet with its use. 

```cs
private void Button_Click(object sender, RoutedEventArgs e)
{
	var wnd = new PortfolioPickerWindow();
	if (Portfolios != null)
		wnd.Portfolios = Portfolios;
	if (wnd.ShowModal(this))
	{
		SelectedPortfolio = wnd.SelectedPortfolio;
	}
}
	  				
```
