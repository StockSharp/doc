# Portfolios

Für die Arbeit mit Portfolios bietet [S#](../../api.md) die folgenden grafischen Komponenten:

- ComboBox zur Portfolioauswahl - [PortfolioComboBox](xref:StockSharp.Xaml.PortfolioComboBox).
- ComboBox mit einer Schaltfläche, die ein Fenster mit einer Portfolioliste öffnet - [PortfolioEditor](xref:StockSharp.Xaml.PropertyGrid.PortfolioEditor).
- Fenster mit Portfolioliste - [PortfolioPickerWindow](xref:StockSharp.Xaml.PortfolioPickerWindow).
- Eine Tabelle, die Informationen zu Portfolios und Positionen anzeigt - [PortfolioGrid](xref:StockSharp.Xaml.PortfolioGrid).

> [!TIP]
> Beachten Sie, dass in allen Komponenten für die Portfolioliste (Eigenschaft Portfolios) die Klasse **ThreadSafeObservableCollection\<TItem\>** verwendet wird (befindet sich in Ecng.Xaml), die Threadsicherheit bietet.

## Empfohlene Inhalte

[Dropdown-Liste](portfolios/drop_down_list.md)

[Auswahl](portfolios/picker.md)

[Fenster zur Portfolioauswahl](portfolios/portfolio_picker_window.md)

[Tabelle](portfolios/table.md)
