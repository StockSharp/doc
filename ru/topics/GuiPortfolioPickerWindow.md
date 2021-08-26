# Окно выбора портфелей

[PortfolioPickerWindow](../api/StockSharp.Xaml.PortfolioPickerWindow.html) \- окно для выбора портфеля. В окне отображается список портфелей и информация о денежных позициях портфелей.

![GUI PortfolioPickerWindow](~/images/GUI_PortfolioPickerWindow.png)

**Основные свойства**

- [Portfolios](../api/StockSharp.Xaml.PortfolioPickerWindow.Portfolios.html) \- список портфелей.
- [SelectedPortfolio](../api/StockSharp.Xaml.PortfolioPickerWindow.SelectedPortfolio.html) \- выбранный портфель.

Ниже показан внеший вид компонента, а также фрагмент кода с его использованием. 

```cs
private void Button\_Click(object sender, RoutedEventArgs e)
{
	var wnd \= new PortfolioPickerWindow();
	if (Portfolios \!\= null)
		wnd.Portfolios \= Portfolios;
	if (wnd.ShowModal(this))
	{
		SelectedPortfolio \= wnd.SelectedPortfolio;
	}
}
	  				
```
