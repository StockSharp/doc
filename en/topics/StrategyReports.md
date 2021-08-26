# Reports

The [S\#](StockSharpAbout.md) provides a reports generating mechanism for strategies created on the basis of [Strategy](../api/StockSharp.Algo.Strategies.Strategy.html). The report includes the instrument code ([Strategy.Security](../api/StockSharp.Algo.Strategies.Strategy.Security.html)), the total working time ([Strategy.TotalWorkingTime](../api/StockSharp.Algo.Strategies.Strategy.TotalWorkingTime.html)), slippage ([SlippageManager](../api/StockSharp.Algo.Connector.SlippageManager.html)), P&L ([Strategy.PnLManager](../api/StockSharp.Algo.Strategies.Strategy.PnLManager.html)) etc. Also, reports generation includes detailed statistics by all the orders ([Strategy.Orders](../api/StockSharp.Algo.Strategies.Strategy.Orders.html)) and trades ([Strategy.MyTrades](../api/StockSharp.Algo.Strategies.Strategy.MyTrades.html)), which have been registered during the strategy work. 

The reports framework is based on the [StrategyReport](../api/StockSharp.Algo.Strategies.Reporting.StrategyReport.html) class that normally has the following implementations: 

- [ExcelStrategyReport](../api/StockSharp.Algo.Strategies.Reporting.ExcelStrategyReport.html) \- report generation in the [Excel](https://en.wikipedia.org/wiki/Excel) format. With this generation, you can use the template file ([Template](../api/StockSharp.Algo.Strategies.Reporting.ExcelStrategyReport.Template.html)) which already contains the necessary formulas and charts based on the generated data. Data from the template before generating copied to the output file whose name is passed through the constructor. 

  By default, the [ExcelStrategyReport](../api/StockSharp.Algo.Strategies.Reporting.ExcelStrategyReport.html) uses an [Excel](https://en.wikipedia.org/wiki/Excel) worksheet called "report", where it records the data. If such a sheet in the template file is not present, it will be automatically created. 
- [XmlStrategyReport](../api/StockSharp.Algo.Strategies.Reporting.XmlStrategyReport.html)

   \- report generation in the 

  [Xml](https://en.wikipedia.org/wiki/XML)

   format. This format is useful when you need to pass strategy parameters from trading algorithm to another program. 
- [CsvStrategyReport](../api/StockSharp.Algo.Strategies.Reporting.CsvStrategyReport.html)

   \- report generation in the 

  [CSV](https://en.wikipedia.org/wiki/CSV)

   format. A simplified version of the 

  [Excel](https://en.wikipedia.org/wiki/Excel)

   report, supported by a large number of third\-party programs. 

### Prerequisites

[Event model](StrategyAction.md)

### Excel report generation adding to the SampleSMA

Excel report generation adding to the SampleSMA

1. You need to add the report generate button on the form:

   ```none
   \<Button x:Name\="Report" Grid.Column\="0" Width\="100" Grid.ColumnSpan\="2" Grid.Row\="11" IsEnabled\="False" Content\="Report" Click\="Report\_Click" \/\>
   ```
2. The generation code (starts by pressing a button):

   ```none
   private void Report\_Click(object sender, RoutedEventArgs e)
   {
   	new ExcelStrategyReport(\_strategy, "sma.xlsx").Generate();
   	\/\/ oppening the result file
   	Process.Start("sma.xlsx");
   }
   ```
3. As a result, the generated file will be located in the same folder as the SampleSMA.exe 

### Next Steps

[Logging](Logging.md)

## Recommended content
