# Пользовательский интерфейс (GUI)

Данный топик предназначен для трейдеров, которые разрабатывают графические торговые программы с помощью [S\#](StockSharpAbout.md), но недостаточно знакомы с азами программирования графического интерфейса под [.NET](https://ru.wikipedia.org/wiki/.NET_Framework).

В [.NET](https://ru.wikipedia.org/wiki/.NET_Framework) существует специальная технология для построения графического интерфейса – [WPF](https://ru.wikipedia.org/wiki/Windows_Presentation_Foundation) (до этого была технология [WinForms](https://ru.wikipedia.org/wiki/WinForms), которая значительно уступает по графическим возможностям). В этой технологии для создания графических элементов используется специальный декларативный язык [XAML](https://msdn.microsoft.com/ru-ru/library/hh700354.aspx).

Основное ограничение визуального API под Windows состоит в том, что нельзя обращаться из другого потока к элементам окна. Это связанно с ограничениями архитектуры Windows (подробнее описано здесь [https:\/\/msdn.microsoft.com\/ru\-ru\/library\/ms741870.aspx](https://msdn.microsoft.com/ru-ru/library/ms741870.aspx)). Реализации шлюза [IConnector](../api/StockSharp.BusinessEntities.IConnector.html) в целях повышения производительности работают в многопоточном режиме. Поэтому, подписываясь на событие, например, [Connector.NewSecurity](../api/StockSharp.Algo.Connector.NewSecurity.html), нельзя напрямую выводить полученные данные в окно пользователя. Для этого нужно провести операцию синхронизации при помощи специального объекта [Dispatcher](https://msdn.microsoft.com/ru-ru/library/system.windows.threading.dispatcher(v=vs.110).aspx), который управляет очередью рабочих элементов потока. 

Вот простой пример, как это делается:

```cs
// обязательно нужно вызвать метод BeginInvoke,
// и уже в его обработчике можно обратиться к элементу окна 'Security' (это выпадающий список)
_connector.NewSecurity += security => this.Dispatcher.BeginInvoke((Action)(() => this.Security.ItemsSource = _connector.Securities));
```

[S\#](StockSharpAbout.md) уже содержит специальные методы, которые скрывают использование Dispatcher и упрощают написание кода: 

```cs
// обязательно нужно вызвать метод GuiSync, прежде чем обратиться к элементу окна 'Security' (это выпадающий список)
_connector.NewSecurity += security => this.GuiSync(() => this.Security.ItemsSource = _connector.Securities);
```

### Графические компоненты S\#

Графические компоненты S\#

В состав [S\#](StockSharpAbout.md) входит большое количество собственных графических компонент, которые размещены в пространствах имен [StockSharp.Xaml](../api/StockSharp.Xaml.html), [StockSharp.Xaml.Charting](../api/StockSharp.Xaml.Charting.html) и [StockSharp.Xaml.Diagram](../api/StockSharp.Xaml.Diagram.html). Некоторые специфические компоненты находятся в пространствах имен коннекторов, как например, комбинированный список [SmartComAddressComboBox](../api/StockSharp.SmartCom.Xaml.SmartComAddressComboBox.html) для выбора адреса сервера [SmartCOM](Smart.md). 

[S\#](StockSharpAbout.md) имеет различные контролы для: 

- поиска и выбора данных (инструментов, портфелей, адресов); 
- создания заявок; 
- отображения биржевой и другой информации (сделки, заявки, транзакции, стаканы, логи и т.д.);
- построения графиков.

Для доступа к графическим контролам [S\#](StockSharpAbout.md) в коде XAML необходимо определить псевдонимы для соответствующих пространств имен и использовать эти псевдонимы в коде XAML. Как это сделать показано в следующем примере: 

```xaml
<Window x:Class="SampleSmartSMA.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:loc="clr-namespace:StockSharp.Localization;assembly=StockSharp.Localization"
        xmlns:sx="clr-namespace:StockSharp.Xaml;assembly=StockSharp.Xaml"
        xmlns:ss="clr-namespace:StockSharp.SmartCom.Xaml;assembly=StockSharp.SmartCom"
        xmlns:charting="http://schemas.stocksharp.com/xaml"
        Title="{x:Static loc:LocalizedStrings.XamlStr570}" Height="700" Width="900">
    
    <Grid>
   </Grid>
</Window>
	
```
