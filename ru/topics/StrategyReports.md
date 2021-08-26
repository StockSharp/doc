# Отчеты

Для стратегий, построенных на базе [Strategy](../api/StockSharp.Algo.Strategies.Strategy.html), [S\#](StockSharpAbout.md) предоставляет механизм генерации отчетов. Отчет включает в себя такие вещи, как код инструмента ([Strategy.Security](../api/StockSharp.Algo.Strategies.Strategy.Security.html)), общее время работы ([Strategy.TotalWorkingTime](../api/StockSharp.Algo.Strategies.Strategy.TotalWorkingTime.html)), проскальзывание ([SlippageManager](../api/StockSharp.Algo.Connector.SlippageManager.html)), P&L ([Strategy.PnLManager](../api/StockSharp.Algo.Strategies.Strategy.PnLManager.html)) и т.д. Также, генерация отчетов включает детальную статистику по всем заявкам ([Strategy.Orders](../api/StockSharp.Algo.Strategies.Strategy.Orders.html)) и сделкам ([Strategy.MyTrades](../api/StockSharp.Algo.Strategies.Strategy.MyTrades.html)), которые были совершены в рамках работы стратегии. 

Механизм отчетов построен на базе класса [StrategyReport](../api/StockSharp.Algo.Strategies.Reporting.StrategyReport.html), который стандартно имеет следующие реализации: 

- [ExcelStrategyReport](../api/StockSharp.Algo.Strategies.Reporting.ExcelStrategyReport.html) \- генерация отчета в формат [Excel](https://ru.wikipedia.org/wiki/Excel). При данной генерации можно использовать файл\-шаблон ([Template](../api/StockSharp.Algo.Strategies.Reporting.ExcelStrategyReport.Template.html)), который уже содержит необходимые формулы и графики, опирающиеся на генерируемые данные. Данные из шаблона перед генерацией копируются в результирующий файл, имя которого передается через конструктор. 

  По умолчанию, [ExcelStrategyReport](../api/StockSharp.Algo.Strategies.Reporting.ExcelStrategyReport.html) использует лист [Excel](https://ru.wikipedia.org/wiki/Excel) с названием "отчет", куда записывает данные. Если такого листа в файле\-шаблоне нет, то он будет автоматически создан. 
- [XmlStrategyReport](../api/StockSharp.Algo.Strategies.Reporting.XmlStrategyReport.html)

   \- генерация отчета в формат 

  [Xml](https://ru.wikipedia.org/wiki/Xml)

  . Данный формат удобен, когда необходимо передать параметры стратегии из торгового робота в другую программу. 
- [CsvStrategyReport](../api/StockSharp.Algo.Strategies.Reporting.CsvStrategyReport.html)

   \- генерация отчета в формат 

  [CSV](https://ru.wikipedia.org/wiki/CSV)

  . Упрощенная версия 

  [Excel](https://ru.wikipedia.org/wiki/Excel)

   отчета, поддерживается большим количеством сторонних программ. 

### Предварительные условия

[Событийная модель](StrategyAction.md)

### Добавление в SampleSMA генерации Excel отчета

Добавление в SampleSMA генерации Excel отчета

1. На форму необходимо добавить кнопку генерации отчета:

   ```none
   \<Button x:Name\="Report" Grid.Column\="0" Width\="100" Grid.ColumnSpan\="2" Grid.Row\="11" IsEnabled\="False" Content\="Отчет" Click\="Report\_Click" \/\>
   ```
2. Сам код генерации (срабатывает при нажатии кнопки):

   ```none
   private void Report\_Click(object sender, RoutedEventArgs e)
   {
   	\/\/ сгерерировать отчет по прошедшему тестированию
   	new ExcelStrategyReport(\_strategy, "sma.xls").Generate();
   	\/\/ открыть отчет
   	Process.Start("sma.xls");
   }
   ```
3. В результате сгенерированный файл будет находится в папке рядом с SampleSMA.exe 

### Следующие шаги

[Логирование](Logging.md)

## См. также
