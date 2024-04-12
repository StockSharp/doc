# Портфели

Для работы с портфелями [S\#](../../api.md) предлагает следующие графические компоненты:

- Комбинированный список для выбора портфеля \- [PortfolioComboBox](xref:StockSharp.Xaml.PortfolioComboBox).
- Комбинированный список с кнопкой, которая открывает окно со списком портфелей \- [PortfolioEditor](xref:StockSharp.Xaml.PropertyGrid.PortfolioEditor).
- Окно со списком портфелей \- [PortfolioPickerWindow](xref:StockSharp.Xaml.PortfolioPickerWindow).
- Таблица, отображающая информацию о портфелях и позициях \- [PortfolioGrid](xref:StockSharp.Xaml.PortfolioGrid).

> [!TIP]
> Обратите внимание, что во всех компонентах для списка портфелей (свойство **Portfolios**) используется класс **ThreadSafeObservableCollection\<TItem\>** (находится в Ecng.Xaml), который обеспечивает потоковую безопасность. 

## См. также

[Выпадающий список портфелей](portfolios/drop_down_list.md)

[Выбор портфеля](portfolios/picker.md)

[Окно выбора портфелей](portfolios/portfolio_picker_window.md)

[Таблица портфелей](portfolios/table.md)
