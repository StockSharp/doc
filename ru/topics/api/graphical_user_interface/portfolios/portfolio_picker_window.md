# Окно выбора портфелей

[PortfolioPickerWindow](xref:StockSharp.Xaml.PortfolioPickerWindow) \- окно для выбора портфеля. В окне отображается список портфелей и информация о денежных позициях портфелей.

![GUI PortfolioPickerWindow](../../../../images/gui_portfoliopickerwindow.png)

**Основные свойства**

- [PortfolioPickerWindow.Portfolios](xref:StockSharp.Xaml.PortfolioPickerWindow.Portfolios) \- список портфелей.
- [PortfolioPickerWindow.SelectedPortfolio](xref:StockSharp.Xaml.PortfolioPickerWindow.SelectedPortfolio) \- выбранный портфель.

Ниже показан внешний вид компонента, а также фрагмент кода с его использованием.

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
