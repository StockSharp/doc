# Выбор инструмента

Компонент [SecurityPicker](../api/StockSharp.Xaml.SecurityPicker.html) предназначен для поиска и выбора инструментов. Поддерживается как одиночный, так и множественный выбор. Компонент позволяет фильтровать список инструментов по их типу. Также этот компонент может использоваться для отображения финансовой информации (полей level1), как показано в разделе [SecurityGrid](GuiSecurityGrid.md). 

![GUI SecurityPicker2](~/images/GUI_SecurityPicker2.png)

[SecurityPicker](../api/StockSharp.Xaml.SecurityPicker.html) состоит из: 

1. Текстового поля, для ввода кода (или Id) инструмента. При вводе выполняется фильтрация списка по введенной подстроке.
2. Специального комбинированного списка [SecurityTypeComboBox](../api/StockSharp.Xaml.SecurityTypeComboBox.html) для фильтрации инструментов по их типу.
3. Таблицы [SecurityGrid](../api/StockSharp.Xaml.SecurityGrid.html) для отображения списка инструментов.

**Основные свойства**

- [SelectionMode](../api/StockSharp.Xaml.SecurityPicker.SelectionMode.html) \- режим выбора инструмента: одиночный, множественный.
- [ShowCommonStatColumns](../api/StockSharp.Xaml.SecurityPicker.ShowCommonStatColumns.html) \- отображать основные колонки.
- [ShowCommonOptionColumns](../api/StockSharp.Xaml.SecurityPicker.ShowCommonOptionColumns.html) \- отображать основные колонки для опционов.
- [Title](../api/StockSharp.Xaml.SecurityPicker.Title.html) \- заголовок, который отображается в верхней части компонента.
- [Securities](../api/StockSharp.Xaml.SecurityPicker.Securities.html) \- список инструментов.
- [SelectedSecurity](../api/StockSharp.Xaml.SecurityPicker.SelectedSecurity.html) \- выбранный инструмент.
- [SelectedSecurities](../api/StockSharp.Xaml.SecurityPicker.SelectedSecurities.html) \- список выбранных инструментов.
- [FilteredSecurities](../api/StockSharp.Xaml.SecurityPicker.FilteredSecurities.html) \- список отфильтрованных инструментов.
- [ExcludeSecurities](../api/StockSharp.Xaml.SecurityPicker.ExcludeSecurities.html) \- список скрытых в списке инструментов.
- [SelectedType](../api/StockSharp.Xaml.SecurityPicker.SelectedType.html) \- выбранный тип инструмента.
- [SecurityProvider](../api/StockSharp.Xaml.SecurityPicker.SecurityProvider.html) \- провайдер информации об инструментах.
- [MarketDataProvider](../api/StockSharp.Xaml.SecurityPicker.MarketDataProvider.html) \- провайдер рыночных данных.

Ниже показан фрагмент кода с его использованием, взятый из примера *Samples\/Common\/SampleConnection*: 

```xaml
\<Window x:Class\="Sample.SecuritiesWindow"
    xmlns\="http:\/\/schemas.microsoft.com\/winfx\/2006\/xaml\/presentation"
    xmlns:x\="http:\/\/schemas.microsoft.com\/winfx\/2006\/xaml"
    xmlns:loc\="clr\-namespace:StockSharp.Localization;assembly\=StockSharp.Localization"
    xmlns:xaml\="http:\/\/schemas.stocksharp.com\/xaml"
    Title\="{x:Static loc:LocalizedStrings.Securities}" Height\="415" Width\="1081"\>
	\<Grid\>
		\<Grid.RowDefinitions\>
			\<RowDefinition Height\="\*" \/\>
			\<RowDefinition Height\="Auto" \/\>
		\<\/Grid.RowDefinitions\>
		\<xaml:SecurityPicker x:Name\="SecurityPicker" x:FieldModifier\="public" SecuritySelected\="SecurityPicker\_OnSecuritySelected" ShowCommonStatColumns\="True" \/\>
	\<\/Grid\>
\<\/Window\>
	  	
```
```cs
private void ConnectClick(object sender, RoutedEventArgs e)
{
    ......................................
	\_connector.NewSecurity +\= security \=\> \_securitiesWindow.SecurityPicker.Securities.Add(security);
	\/\/ устанавливаем поставщик маркет\-данных
	\_securitiesWindow.SecurityPicker.MarketDataProvider \= \_connector;
	......................................
}
private void SecurityPicker\_OnSecuritySelected(Security security)
{
	NewStopOrder.IsEnabled \= NewOrder.IsEnabled \=
	Level1.IsEnabled \= Depth.IsEnabled \= security \!\= null;
}
```
