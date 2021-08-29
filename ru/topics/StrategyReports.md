# Отчеты

Для стратегий, построенных на базе [Strategy](xref:StockSharp.Algo.Strategies.Strategy), [S\#](StockSharpAbout.md) предоставляет механизм генерации отчетов. Отчет включает в себя такие вещи, как код инструмента ([Strategy.Security](xref:StockSharp.Algo.Strategies.Strategy.Security)), общее время работы ([Strategy.TotalWorkingTime](xref:StockSharp.Algo.Strategies.Strategy.TotalWorkingTime)), проскальзывание ([SlippageManager](xref:StockSharp.Algo.Connector.SlippageManager)), P&L ([Strategy.PnLManager](xref:StockSharp.Algo.Strategies.Strategy.PnLManager)) и т.д. Также, генерация отчетов включает детальную статистику по всем заявкам ([Strategy.Orders](xref:StockSharp.Algo.Strategies.Strategy.Orders)) и сделкам ([Strategy.MyTrades](xref:StockSharp.Algo.Strategies.Strategy.MyTrades)), которые были совершены в рамках работы стратегии. 

Механизм отчетов построен на базе класса [StrategyReport](xref:StockSharp.Algo.Strategies.Reporting.StrategyReport), который стандартно имеет следующие реализации: 

- [ExcelStrategyReport](xref:StockSharp.Algo.Strategies.Reporting.ExcelStrategyReport) \- генерация отчета в формат [Excel](https://ru.wikipedia.org/wiki/Excel). При данной генерации можно использовать файл\-шаблон ([Template](xref:StockSharp.Algo.Strategies.Reporting.ExcelStrategyReport.Template)), который уже содержит необходимые формулы и графики, опирающиеся на генерируемые данные. Данные из шаблона перед генерацией копируются в результирующий файл, имя которого передается через конструктор. 

  По умолчанию, [ExcelStrategyReport](xref:StockSharp.Algo.Strategies.Reporting.ExcelStrategyReport) использует лист [Excel](https://ru.wikipedia.org/wiki/Excel) с названием "отчет", куда записывает данные. Если такого листа в файле\-шаблоне нет, то он будет автоматически создан. 
- [XmlStrategyReport](xref:StockSharp.Algo.Strategies.Reporting.XmlStrategyReport) \- генерация отчета в формат [Xml](https://ru.wikipedia.org/wiki/Xml). Данный формат удобен, когда необходимо передать параметры стратегии из торгового робота в другую программу. 
- [CsvStrategyReport](xref:StockSharp.Algo.Strategies.Reporting.CsvStrategyReport) \- генерация отчета в формат [CSV](https://ru.wikipedia.org/wiki/CSV). Упрощенная версия [Excel](https://ru.wikipedia.org/wiki/Excel) отчета, поддерживается большим количеством сторонних программ. 

## Предварительные условия

[Событийная модель](StrategyAction.md)

## Добавление в SampleSMA генерации Excel отчета

1. На форму необходимо добавить кнопку генерации отчета:

   ```none
   <Button x:Name="Report" Grid.Column="0" Width="100" Grid.ColumnSpan="2" Grid.Row="11" IsEnabled="False" Content="Отчет" Click="Report_Click" />
   ```
2. Сам код генерации (срабатывает при нажатии кнопки):

   ```none
   private void Report_Click(object sender, RoutedEventArgs e)
   {
   	// сгерерировать отчет по прошедшему тестированию
   	new ExcelStrategyReport(_strategy, "sma.xls").Generate();
   	// открыть отчет
   	Process.Start("sma.xls");
   }
   ```
3. В результате сгенерированный файл будет находится в папке рядом с SampleSMA.exe 

## Следующие шаги

[Логирование](Logging.md)
