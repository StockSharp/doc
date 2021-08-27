# Портфели

Для работы с портфелями [S\#](StockSharpAbout.md) предлагает следующие графические компоненты:

- Комбинированный список для выбора портфеля \- [PortfolioComboBox](xref:StockSharp.Xaml.PortfolioComboBox).
- Комбинированный список с кнопкой, которая открывает окно со списком портфелей \- [PortfolioEditor](xref:StockSharp.Xaml.PropertyGrid.PortfolioEditor).
- Окно со списком портфелей \- [PortfolioPickerWindow](xref:StockSharp.Xaml.PortfolioPickerWindow).
- Таблица, отображающая информацию о портфелях и позициях \- [PortfolioGrid](xref:StockSharp.Xaml.PortfolioGrid).

> [!TIP]
> Обратите внимание, что во всех компонентах для списка портфелей (свойство **Portfolios**) используется класс **ThreadSafeObservableCollection\<TItem\>** (находится в Ecng.Xaml), который обеспечивает потоковую безопасность. 

## См. также

[Выпадающий список портфелей](GuiPortfolioComboBox.md)

[Выбор портфеля](GuiPortfolioEditor.md)

[Окно выбора портфелей](GuiPortfolioPickerWindow.md)

[Таблица портфелей](GuiPortfolioGrid.md)
