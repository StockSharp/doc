# Портфели

Для работы с портфелями [S\#](StockSharpAbout.md) предлагает следующие графические компоненты:

- Комбинированный список для выбора портфеля \- [PortfolioComboBox](../api/StockSharp.Xaml.PortfolioComboBox.html).
- Комбинированный список с кнопкой, которая открывает окно со списком портфелей \- [PortfolioEditor](../api/StockSharp.Xaml.PropertyGrid.PortfolioEditor.html).
- Окно со списком портфелей \- [PortfolioPickerWindow](../api/StockSharp.Xaml.PortfolioPickerWindow.html).
- Таблица, отображающая информацию о портфелях и позициях \- [PortfolioGrid](../api/StockSharp.Xaml.PortfolioGrid.html).

> [!TIP]
> Обратите внимание, что во всех компонентах для списка портфелей (свойство **Portfolios**) используется класс **ThreadSafeObservableCollection\<TItem\>** (находится в Ecng.Xaml), который обеспечивает потоковую безопасность. 

## См. также

[Выпадающий список портфелей](GuiPortfolioComboBox.md)

[Выбор портфеля](GuiPortfolioEditor.md)

[Окно выбора портфелей](GuiPortfolioPickerWindow.md)

[Таблица портфелей](GuiPortfolioGrid.md)
